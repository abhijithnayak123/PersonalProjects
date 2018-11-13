using System;
using System.Collections.Generic;

using AutoMapper;

using MGI.TimeStamp;

using CXEContract = MGI.Core.CXE.Contract;
using CXEData = MGI.Core.CXE.Data;

using CXNContract = MGI.Cxn.Check.Contract;
using CXNData = MGI.Cxn.Check.Data;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Fees;

using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;

using BizCommon = MGI.Biz.Common.Data;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;

using MGI.Biz.CPEngine.Contract;
using MGI.Biz.CPEngine.Data;
using MGI.Common.Util;
using System.Diagnostics;
using Spring.Transaction.Interceptor;
using PTNRTrans = MGI.Core.Partner.Data.Transactions;
using MGI.Cxn.Check.Contract;
using MGI.Cxn.Common.Processor.Util;
using MGI.Core.Partner.Contract;
using MGI.Common.TransactionalLogging.Data;


namespace MGI.Biz.CPEngine.Impl
{
	public class CPEngineServiceImpl : ICPEngineService
	{
		#region Dependencies
		private PTNRContract.ICustomerSessionService _sessionSvc;
		public PTNRContract.ICustomerSessionService SessionSvc { set { _sessionSvc = value; } }

		private CXEContract.ICustomerService _cxeCustSvc;
		public CXEContract.ICustomerService CxeCustSvc { set { _cxeCustSvc = value; } }

		private CXEContract.IAccountService _acctSvc;
		public CXEContract.IAccountService AcctSvc { set { _acctSvc = value; } }

		private CXEContract.ICheckService _cxeCheckSvc;
		public CXEContract.ICheckService CxeCheckSvc { set { _cxeCheckSvc = value; } }

		private PTNRContract.ITransactionService<PTNRData.Transactions.Check> _ptnrCheckSvc;
		public PTNRContract.ITransactionService<PTNRData.Transactions.Check> PTNRCheckSvc { set { _ptnrCheckSvc = value; } }

		private PTNRContract.IFeeService _feeSvc;
		public PTNRContract.IFeeService FeeSvc { set { _feeSvc = value; } }

		private PTNRContract.ICustomerService _ptnrCustomerSvc;
		public PTNRContract.ICustomerService CustomerService { set { _ptnrCustomerSvc = value; } }

		private PTNRContract.IAgentSessionService _ptnrAgentSvc;
		public PTNRContract.IAgentSessionService AgentService { set { _ptnrAgentSvc = value; } }

		private PTNRContract.IChannelPartnerService _ptnrSvc;
		public PTNRContract.IChannelPartnerService PtnrSvc { set { _ptnrSvc = value; } }

		private ILimitService _limitService;
		public ILimitService LimitService { set { _limitService = value; } }

		private PTNRContract.INexxoDataStructuresService _ptnrIDTypeSvc;
		public PTNRContract.INexxoDataStructuresService NexxoIdTypeService { set { _ptnrIDTypeSvc = value; } }

		public CheckFrankTemplateRepo CheckFrankRepo { private get; set; }

		public PTNRContract.IManageUsers ManageUserService { private get; set; }

		public PTNRContract.IMessageCenter MessageCenterService { private get; set; }
		public PTNRContract.IMessageStore MessageStore { private get; set; }
		//US1800 Referral & Referree Promotions
		public PTNRContract.ICustomerFeeAdjustmentService CustomerFeeAdjustmentService { private get; set; }

		// AL-591: This is introduced to update IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
		// Developed by: Sunil Shetty || 03/07/2015
		public PTNRContract.IFeeAdjustmentService FeeAdjustmentService { private get; set; }

		public TLoggerCommon MongoDBLogger { get; set; }

		public IProcessorRouter CheckProcessorRouter { private get; set; }

		#endregion

		public CPEngineServiceImpl()
		{
			Mapper.CreateMap<TransactionFee, BizCommon.TransactionFee>();
			Mapper.CreateMap<CXNData.CheckProcessorInfo, CheckProcessorInfo>();
			Mapper.CreateMap<CXNData.CheckLogin, ChexarLogin>();
		}

		#region ICPEngineService methods

		public Check Submit(CheckSubmission checkSubmission, long customerSessionId, MGIContext mgiContext)
        {
            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<CheckSubmission>(customerSessionId, checkSubmission, "Submit", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin Submit - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
            #endregion


			// retrieve session and customer account info, which contains agent, location and channel partner info
			CustomerSession session = _sessionSvc.Lookup(customerSessionId);

			// Karun@10/14/2013: get the channelpartner pk from session, instead of making another service call. 
			var channelPartner = _ptnrSvc.ChannelPartnerConfig(mgiContext.ChannelPartnerRowGuid);

			// get the check account
			// if multiple check processors, would need to lookup the check processor for the partner
			CXEData.Account checkAccount = GetCheckAccount(session.Customer, mgiContext);

			CXEData.Customer cxeCustomer = _cxeCustSvc.Lookup(session.Customer.CXEId);

			long cxnAccountID = _ptnrCustomerSvc.LookupByCxeId(cxeCustomer.Id).FindAccountByCXEId(checkAccount.Id).CXNId;

			// stage the check in CXE
			CXEData.Transactions.Stage.Check cxeCheck = new CXEData.Transactions.Stage.Check
			{
				Amount = checkSubmission.Amount,
				Fee = checkSubmission.Fee,
				MICR = checkSubmission.MICR,
				Account = checkAccount,
				DTTerminalCreate = Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
				DTServerCreate = DateTime.Now,
				Status = (int)CXEData.TransactionStates.Pending,
				IssueDate = checkSubmission.IssueDate,
				CheckType = (int)_ptnrSvc.GetCheckType(checkSubmission.CheckType).Id
			};

			cxeCheck.AddImages(checkSubmission.FrontImage, checkSubmission.BackImage, checkSubmission.ImageFormat);
			long cxeCheckId = _cxeCheckSvc.Create(cxeCheck, mgiContext.TimeZone);

			// submit the check to the CXN processor
			CXNData.CheckInfo cxnCheckSubmission = new CXNData.CheckInfo
			{
				Amount = checkSubmission.Amount,
				Micr = checkSubmission.MICR,
				IssueDate = checkSubmission.IssueDate,
				Type = (CXNData.CheckType)_ptnrSvc.GetCheckType(checkSubmission.CheckType).Id,
				FrontImage = checkSubmission.FrontImage,
				BackImage = checkSubmission.BackImage,
				ImageFormat = checkSubmission.ImageFormat,
				FrontImageTIF = checkSubmission.FrontImageTIFF,
				BackImageTIF = checkSubmission.BackImageTIFF,
				AccountNumber = checkSubmission.AccountNumber,
				RoutingNumber = checkSubmission.RoutingNumber,
				CheckNumber = checkSubmission.CheckNumber,
				MicrEntryType = checkSubmission.MicrEntryType
			};

			ICheckProcessor checkProcessor = _GetProcessor(channelPartner.Name);

			CXNData.CheckStatus cxnStatus = checkProcessor.Submit(cxeCheckId, cxnAccountID, cxnCheckSubmission, mgiContext);

			// get the cxn check 
			CXNData.CheckTrx cxnCheck = checkProcessor.Get(cxeCheckId);

			// create the Partner transaction record
			PTNRData.Transactions.Check ptnrCheck = new PTNRData.Transactions.Check
			{
				Id = cxeCheckId,
				CXEId = cxeCheckId,
				CXNId = cxeCheckId,
				Amount = cxeCheck.Amount,
				Fee = cxeCheck.Fee,
				CustomerSession = session,
				CXEState = (int)CXEData.TransactionStates.Pending,
				CXNState = (int)CXEData.TransactionStates.Pending,
				DTTerminalCreate = Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
				DTServerCreate = DateTime.Now,
				Account = session.Customer.FindAccountByCXEId(checkAccount.Id),
				Description = cxnCheck.ReturnType.ToString(),
				ConfirmationNumber = cxnCheck.ConfirmationNumber,
				DiscountName = checkSubmission.PromoCode,
				IsSystemApplied = checkSubmission.IsSystemApplied

			};
			//US1799 Promotions - context added to consider manual entry promotions
			mgiContext.PromotionCode = checkSubmission.PromoCode;
			mgiContext.IsSystemApplied = checkSubmission.IsSystemApplied;

			// if instantly approved, get the fee before saving the record
            if ( cxnStatus == CXNData.CheckStatus.Approved || cxnStatus == CXNData.CheckStatus.Pending )
			{
				updatePartnerCheckFee(session, cxnCheck.ReturnType, ref ptnrCheck, mgiContext);

				// update cxe record
				_cxeCheckSvc.Update(cxeCheckId, (int)cxnCheck.ReturnType, ptnrCheck.Fee, mgiContext.TimeZone);

                if ( cxnStatus == CXNData.CheckStatus.Approved)
				    ptnrCheck.CXNState = ptnrCheck.CXEState = (int)CXEData.TransactionStates.Authorized;

			}
			else if (cxnStatus == CXNData.CheckStatus.Declined)
			{
				ptnrCheck.CXNState = ptnrCheck.CXEState = (int)CXEData.TransactionStates.Declined;
				MGI.Core.Partner.Data.Language lang = MGI.Core.Partner.Data.Language.EN;
				BizCPEngineException bizcpexception = new BizCPEngineException(cxnCheck.DeclineCode);
				string key = Convert.ToString(bizcpexception.MajorCode) + "." + Convert.ToString(bizcpexception.MinorCode);
				MGI.Core.Partner.Data.Message message = MessageStore.Lookup(session.AgentSession.Terminal.ChannelPartner.Id, key, lang);
				if (message != null)
				{
					cxnCheck.DeclineMessage = message.Content;
				}
			}
			else if (cxnStatus == CXNData.CheckStatus.Failed)
				ptnrCheck.CXNState = ptnrCheck.CXEState = (int)CXEData.TransactionStates.Failed;

			_ptnrCheckSvc.Create(ptnrCheck);

			// update cxe status if needed
			if (ptnrCheck.CXEState != (int)CXEData.TransactionStates.Pending)
				_cxeCheckSvc.Update(cxeCheckId, ptnrCheck.CXEState, mgiContext.TimeZone);

			// return the partner check Id, which will be stored in the shopping cart
			Check check = new Check
			{
				Id = ptnrCheck.Id.ToString(),
				Amount = ptnrCheck.Amount,
				SelectedType = checkSubmission.CheckType,
				//ValidatedType=_ptnrSvc.GetCheckType((int)cxnCheck.ReturnType).Name,
				SelectedFee = checkSubmission.Fee,
				Status = cxnStatus.ToString(),
				SubmissionDate = ptnrCheck.DTTerminalCreate,
				StatusMessage = cxnCheck.DeclineMessage,
				StatusDescription = cxnCheck.WaitTime
			};

			if (cxnStatus == CXNData.CheckStatus.Approved)
				updateBizCheckApprovalDetails(ptnrCheck, cxnCheck.ReturnType, ref check);
			if (cxnStatus == CXNData.CheckStatus.Pending)
			{
				MessageCenterService.Create(new MGI.Core.Partner.Data.AgentMessage()
				{
					Agent = ManageUserService.GetUser((int)session.AgentSession.Agent.Id),
					IsParked = false,
					IsActive = true,
					Transaction = ptnrCheck
				}, session.AgentSession.Terminal.Location.TimezoneID);
			}
			if (cxnStatus == CXNData.CheckStatus.Declined)
			{
				var status = GetStatus(customerSessionId, ptnrCheck.Id.ToString(), false, mgiContext);
				return status;
            }

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<Check>(customerSessionId, check, "Submit", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End Submit - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
            #endregion

            return check;
		}

		public Check GetStatus(long customerSessionId, string checkId, bool includeImage, MGIContext mgiContext)
        {

            #region AL-3371 Transactional Log User Story(Process check)
            List<string> details = new List<string>();
			details.Add("Check Id : " + checkId);
			details.Add("Include Image : " + (includeImage == true ? "Yes" : "No"));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetStatus", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin GetStatus - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
            #endregion
            // retrieve session and customer account info, which contains agent, location and channel partner info
			CustomerSession session = _sessionSvc.Lookup(customerSessionId);

			long Id = long.Parse(checkId);

			ICheckProcessor checkProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

			// lookup the current cxn status
			CXNData.CheckStatus checkStatus = checkProcessor.Status(Id, mgiContext.TimeZone, mgiContext);

			// get the ptnr check transaction
			PTNRData.Transactions.Check ptnrCheck = _ptnrCheckSvc.Lookup(Id);

			decimal selectedFee = ptnrCheck.Fee;

			// need to get the cxn check for returning some of the necessary info
			CXNData.CheckTrx cxnCheck = checkProcessor.Get(Id);

			// update the cxn status in the ptnr check if necessary
			if (checkStatus != (CXNData.CheckStatus)ptnrCheck.CXNState)
			{
				// if status change to approved, retrieve check fee and store in CXE
                if ( checkStatus == CXNData.CheckStatus.Approved || checkStatus == CXNData.CheckStatus.Pending )
				{
					//US1799 Promotions - context added to consider manual promotions
					mgiContext.PromotionCode = ptnrCheck.DiscountName;
					mgiContext.IsSystemApplied = ptnrCheck.IsSystemApplied;

					//AL101 changes update check amount

					if (cxnCheck.Amount != cxnCheck.ReturnAmount)
					{
						ptnrCheck.Amount = cxnCheck.ReturnAmount;
					}
					//-----End
					updatePartnerCheckFee(session, cxnCheck.ReturnType, ref ptnrCheck, mgiContext);

					//AL-101

					_cxeCheckSvc.Update(Id, (int)cxnCheck.ReturnType, ptnrCheck.Amount, ptnrCheck.Fee, mgiContext.TimeZone);

					selectedFee = ptnrCheck.Fee;
					//---End
                    if (checkStatus == CXNData.CheckStatus.Approved)
					    ptnrCheck.CXNState = ptnrCheck.CXEState = (int)CXEData.TransactionStates.Authorized;
				}
				else if (checkStatus == CXNData.CheckStatus.Declined)
				{
					ptnrCheck.CXNState = ptnrCheck.CXEState = (int)CXEData.TransactionStates.Declined;
					MGI.Core.Partner.Data.Language lang = MGI.Core.Partner.Data.Language.EN;
					BizCPEngineException bizcpexception = new BizCPEngineException(cxnCheck.DeclineCode);
					MGI.Core.Partner.Data.Message _message = MessageStore.Lookup(session.AgentSession.Terminal.ChannelPartner.Id, Convert.ToString(bizcpexception.MajorCode) + "." + Convert.ToString(bizcpexception.MinorCode), lang);
					if (_message == null)
					{
						_message = MessageStore.Lookup(session.AgentSession.Terminal.ChannelPartner.Id, Convert.ToString(bizcpexception.MajorCode) + "." + Convert.ToString(BizCPEngineException.UNHANDLED_DECLINE_CODE), lang);
					}

					if (_message != null)
						cxnCheck.DeclineMessage = _message.Content;
					
					checkProcessor.Update(cxnCheck, mgiContext);
				}

				// inefficient, but update again
				_cxeCheckSvc.Update(Id, ptnrCheck.CXEState, mgiContext.TimeZone);

				_ptnrCheckSvc.Update(ptnrCheck);
			}

			// setup return check object
			Check check = new Check
			{
				Id = checkId,
				Amount = ptnrCheck.Amount,
				// Amount=ptnrCheck.Amount!=cxnCheck.Amount?cxnCheck.Amount:ptnrCheck.Amount,
				SelectedType = _ptnrSvc.GetCheckType((int)cxnCheck.SubmitType).Name,
				SelectedFee = selectedFee,
				Status = checkStatus.ToString(),
				StatusMessage = ptnrCheck.Description,
				SubmissionDate = ptnrCheck.DTTerminalCreate,
				StatusDescription = cxnCheck.WaitTime,
				DeclineCode = cxnCheck.DeclineCode,
				DmsDeclineMessage = cxnCheck.DeclineMessage
			};

			if (checkStatus == CXNData.CheckStatus.Approved)
				updateBizCheckApprovalDetails(ptnrCheck, cxnCheck.ReturnType, ref check);


            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<Check>(customerSessionId, check, "GetStatus", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End GetStatus - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion

			return check;
		}

		public bool Cancel(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			long Id = long.Parse(checkId);

            #region AL-3371 Transactional Log User Story(Process check)
            string chkId = "Check Id : " + checkId;

			MongoDBLogger.Info<string>(customerSessionId, chkId, "Cancel", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin Cancel - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
            #endregion

            CXEData.TransactionStates cxeStatus;
			CXEData.TransactionStates ptnrCxnStatus;

			ICheckProcessor checkProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
  
            //AL-2012 getting chexar login details for "parked Approved check" to cancel.
            ChexarLogin chexarLogin = GetChexarSessions(mgiContext);
            if (mgiContext.URL == null && chexarLogin != null)
            {
                mgiContext.URL = chexarLogin.URL;
                mgiContext.IngoBranchId = chexarLogin.BranchId;
                mgiContext.CompanyToken = chexarLogin.CompanyToken;
                mgiContext.EmployeeId = chexarLogin.EmployeeId;
            }
			
            // retrieve cxn status - should probably change Cancel to return this
			CXNData.CheckStatus cxnStatus = checkProcessor.Status(Id, mgiContext.TimeZone, mgiContext);

			ptnrCxnStatus = cxeStatus = cxnStatus == CXNData.CheckStatus.Declined ? CXEData.TransactionStates.Declined : CXEData.TransactionStates.Canceled;

            //AL-2012,Scenario#4,if check is in Failed state, then remove from cart, no change in Transaction Status of AgentTransactionHistory, Keeping Failed state as it is.
            if(cxnStatus==CXNData.CheckStatus.Failed)
            {
                ptnrCxnStatus =cxeStatus= CXEData.TransactionStates.Failed;
            }


			bool isCheckCancelled = checkProcessor.Cancel(Id, mgiContext.TimeZone, mgiContext);


			// set to final status in cxe
			//changes for timestamp
			if (isCheckCancelled)
			{
				_cxeCheckSvc.Update(Id, (int)cxeStatus, mgiContext.TimeZone);

				// update ptnr record
				_ptnrCheckSvc.UpdateStates(Id, (int)cxeStatus, (int)ptnrCxnStatus);
            }

            #region AL-3371 Transactional Log User Story(Process check)
            string isCancelled = "Check Cancelled : " + (isCheckCancelled ? "Yes" : "No");

			MongoDBLogger.Info<string>(customerSessionId, isCancelled, "Cancel", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End Cancel - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
            #endregion

            return isCheckCancelled;
		}

		public void UpdateStatusOnRemoval(long customerSessionId, long checkId)
		{
			var location = _sessionSvc.Lookup(customerSessionId).AgentSession.Terminal.Location;
			_cxeCheckSvc.Update(checkId, (int)CXEData.TransactionStates.CommittedReversed, location.TimezoneID);
			// update ptnr record
			_ptnrCheckSvc.UpdateStates(checkId, (int)CXEData.TransactionStates.CommittedReversed, (int)CXEData.TransactionStates.CommittedReversed);
		}

		public bool CanResubmit(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			long Id = long.Parse(checkId);
			// need to add CanResubmit method in cxn.ICheckService
			return false;
		}


		public bool Resubmit(long customerSessionId, long checkId, MGIContext mgiContext)
        {
            #region AL-3371 Transactional Log User Story(Process check)
            string id = "Check Id : " + checkId;

			MongoDBLogger.Info<string>(customerSessionId, id, "Resubmit", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin Resubmit - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
            #endregion
            //US2030 
			CustomerSession session = _sessionSvc.Lookup(customerSessionId);

			PTNRData.Transactions.Check ptnrCheck = _ptnrCheckSvc.Lookup(checkId);

			CXNData.CheckTrx cxnCheck = _GetProcessor(mgiContext.ChannelPartnerName).Get(ptnrCheck.CXNId);

			updatePartnerNextCheckFee(session, cxnCheck.ReturnType, ref ptnrCheck, mgiContext);

			// update cxe record
			_cxeCheckSvc.Update(ptnrCheck.CXEId, (int)cxnCheck.ReturnType, ptnrCheck.Fee, mgiContext.TimeZone);

            #region AL-3371 Transactional Log User Story(Process check)

            MongoDBLogger.Info<PTNRData.Transactions.Check>(customerSessionId, ptnrCheck, "Resubmit", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End Resubmit - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
            #endregion
            return true;
		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void Commit(string checkId, long customerSessionId, MGIContext mgiContext)
        {
            #region AL-3371 Transactional Log User Story(Process check)

            string id = "Check Id : " + checkId;

			MongoDBLogger.Info<string>(customerSessionId, id, "Commit", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin Commit - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion

			//US1800 Referral & REferree Promotions 
			var customerSession = _sessionSvc.Lookup(customerSessionId);

			//var location = _sessionSvc.Lookup(customerSessionId).AgentSession.Terminal.Location;

			long Id = long.Parse(checkId);

			ICheckProcessor checkProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

			// commit check with processor
			checkProcessor.Commit(Id, mgiContext.TimeZone, mgiContext);

			// update stage check
			_cxeCheckSvc.Update(Id, (int)CXEData.TransactionStates.Committed, mgiContext.TimeZone);

			// commit check in cxe
			_cxeCheckSvc.Commit(Id);

			//update the partner transaction
			CXNData.CheckStatus cxnStatus = checkProcessor.Status(Id, mgiContext.TimeZone, mgiContext);
			_ptnrCheckSvc.UpdateStates(Id, (int)CXEData.TransactionStates.Committed, (int)cxnStatus);
			PTNRData.Transactions.Check ptnrCheck = _ptnrCheckSvc.Lookup(Id);
			MessageCenterService.Delete(ptnrCheck);

			//US1800 Referral Referee Promotions
			addUpdateCustomerFeeAdjustment(customerSession, ptnrCheck, mgiContext);

            #region AL-3371 Transactional Log User Story(Process check)

            MongoDBLogger.Info<PTNRData.Transactions.Check>(customerSessionId, ptnrCheck, "Commit", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End Commit - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion
		}

		public List<string> GetCheckTypes(long customerSessionId, MGIContext mgiContext)
		{
			return _ptnrSvc.GetCheckTypes();
		}

		public BizCommon.TransactionFee GetFee(long customerSessionId, CheckSubmission checkSubmission, MGIContext mgiContext)
        {
            #region AL-3371 Transactional Log User Story(Process check)

            MongoDBLogger.Info<CheckSubmission>(customerSessionId, checkSubmission, "GetFee", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin GetFee - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion
			
			// retrieve session and customer account info, which contains agent, location and channel partner info
			CustomerSession session = _sessionSvc.Lookup(customerSessionId);
			// check against limit
			ChannelPartner channelPartner = _ptnrSvc.ChannelPartnerConfig(session.Customer.ChannelPartnerId);

			decimal minimumAmount = _limitService.GetProductMinimum(channelPartner.ComplianceProgramName, TransactionTypes.Check, mgiContext);
			decimal maximumAmount = _limitService.CalculateTransactionMaximumLimit(customerSessionId, channelPartner.ComplianceProgramName, TransactionTypes.Check, mgiContext);

			if (checkSubmission.Amount < minimumAmount)
			{
				throw new BizComplianceLimitException(BizComplianceLimitException.CHECK_MINIMUM_LIMIT_CHECK, minimumAmount);
			}
			if (checkSubmission.Amount > maximumAmount)
			{
				throw new BizComplianceLimitException(BizComplianceLimitException.CHECK_LIMIT_EXCEEDED, maximumAmount);
			}

			List<PTNRData.Transactions.Check> transactions = _ptnrCheckSvc.GetAllForCustomer(session.Customer.Id);
			//US2030
			removeCancelParkedTransactions(session, ref transactions);

			//US1799 Promotions for manual entry promotionCodes
			mgiContext.PromotionCode =  checkSubmission.PromoCode;
			mgiContext.IsSystemApplied =  checkSubmission.IsSystemApplied;

            #region AL-3371 Transactional Log User Story(Process check)

            MongoDBLogger.ListInfo<PTNRData.Transactions.Check>(customerSessionId, transactions, "GetFee", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End GetFee - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion

			return Mapper.Map<BizCommon.TransactionFee>(_feeSvc.GetCheckFee(session, transactions, checkSubmission.Amount, (int)_ptnrSvc.GetCheckType(checkSubmission.CheckType).Id, mgiContext));
		}

		public CheckTransaction GetTransaction(long agentSessionId, long customerSessionId, string checkId, MGIContext mgiContext)
        {
            #region AL-3371 Transactional Log User Story(Process check)

            string id = "Check Id : " + checkId;

			MongoDBLogger.Info<string>(customerSessionId, id, "GetTransaction", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin GetTransaction - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion

			long Id = long.Parse(checkId);
			// get the ptnr check transaction
			PTNRData.Transactions.Check ptnrCheck = _ptnrCheckSvc.Lookup(Id);

			ICheckProcessor checkProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

			// need to get the cxn check for returning some of the necessary info
			CXNData.CheckTrx cxnCheck = checkProcessor.Get(ptnrCheck.CXNId);
			string _declineMessage = string.Empty;
			MGI.Core.Partner.Data.Language lang = MGI.Core.Partner.Data.Language.EN;
			if (cxnCheck.Status == CXNData.CheckStatus.Declined)
			{
				long ChannelPartnerId = ptnrCheck.CustomerSession.AgentSession.Terminal.ChannelPartner.Id;
				//BizCustomerException
				BizCPEngineException bizcpexception = new BizCPEngineException(cxnCheck.DeclineCode);
				MGI.Core.Partner.Data.Message _message = MessageStore.Lookup(ChannelPartnerId, Convert.ToString(bizcpexception.MajorCode) + "." + Convert.ToString(bizcpexception.MinorCode), lang);

				if (_message == null)
				{
					_message = MessageStore.Lookup(ChannelPartnerId, Convert.ToString(bizcpexception.MajorCode) + "." + Convert.ToString(BizCPEngineException.UNHANDLED_DECLINE_CODE), lang);					
				}

				if(_message != null)
					_declineMessage = _message.Content;

				cxnCheck.DeclineMessage = _declineMessage;

				checkProcessor.Update(cxnCheck, mgiContext);

			}
			CXEData.CheckImages images = _cxeCheckSvc.GetImages(Id);

            #region AL-3371 Transactional Log User Story(Process check)

            MongoDBLogger.Info<CXNData.CheckTrx>(customerSessionId, cxnCheck, "GetTransaction", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End GetTransaction - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion

			return new CheckTransaction
			{
				Id = checkId,
				CheckNumber = cxnCheck.CheckNumber,
				Amount = ptnrCheck.Amount,
				Fee = ptnrCheck.Fee,
				ConfirmationNumber = ptnrCheck.ConfirmationNumber,
				Type = _ptnrSvc.GetCheckType((int)cxnCheck.ReturnType).Name,
				ImageFront = images.Front,
				ImageBack = images.Back,
				BaseFee = ptnrCheck.BaseFee,
				DiscountApplied = ptnrCheck.DiscountApplied,
				DiscountName = ptnrCheck.DiscountName,
				DiscountDescription = ptnrCheck.DiscountDescription,
				DeclineMessage = cxnCheck.DeclineMessage,
				CheckType = cxnCheck.ReturnType.ToString(),
				ProviderId = ptnrCheck.Account.ProviderId,
				Status = cxnCheck.Status.ToString(),
				DmsDeclineMessage = _declineMessage,
				DeclineErrorCode = cxnCheck.DeclineCode
			};
		}

		public string GetCheckFrankingData(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            #region AL-3371 Transactional Log User Story(Process check)

            string id = "Transaction Id : " + transactionId;

			MongoDBLogger.Info<string>(customerSessionId, id, "GetCheckFrankingData", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin GetCheckFrankingData - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion

			PTNRData.Transactions.Check check = _ptnrCheckSvc.Lookup(transactionId);

			MGI.Core.Partner.Data.ChannelPartner channelpartner = _ptnrSvc.ChannelPartnerConfig(mgiContext.ChannelPartnerId);
			//Get Provider for Check
			var provider = (ProviderIds)Enum.Parse(typeof(ProviderIds), _GetCheckProvider(channelpartner.Name));

			string checkPrintContents = CheckFrankRepo.GetCheckFrankingTemplate(channelpartner.Name, PTNRData.Transactions.TransactionType.Check, provider, string.Empty);

			checkPrintContents = checkPrintContents.Replace("{frankdata}", GetFrankText(check, channelpartner, mgiContext));

            #region AL-3371 Transactional Log User Story(Process check)

            MongoDBLogger.Info<string>(customerSessionId, checkPrintContents, "GetCheckFrankingData", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End GetCheckFrankingData - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion
			
			return checkPrintContents;
		}


		private string GetFrankText(PTNRData.Transactions.Check transaction, ChannelPartner channelPartner, MGIContext mgiContext)
		{
			//Dictionary context name has to be changed to generic name
			string frankText = channelPartner.ChannelPartnerConfig.FrankData;

			if (!string.IsNullOrWhiteSpace(frankText))
			{

				if (transaction != null && transaction.CustomerSession != null && transaction.CustomerSession.AgentSession != null && transaction.CustomerSession.AgentSession.Terminal.Location != null)
				{
					string BankID = transaction.CustomerSession.AgentSession.Terminal.Location.BankID;
					string BranchID = transaction.CustomerSession.AgentSession.Terminal.Location.BranchID;
					string LocationIdentifier = mgiContext.CheckUserName;
					string LocationName = transaction.CustomerSession.AgentSession.Terminal.Location.LocationName;
					string TerminalName = transaction.CustomerSession.AgentSession.Terminal.Name;
					string TellerName = ManageUserService.GetUser((int)transaction.CustomerSession.AgentSession.Agent.Id).FullName;
					string TellerID = transaction.CustomerSession.AgentSession.Agent.Id.ToString();
					frankText = frankText.Replace("BankID", BankID);
					frankText = frankText.Replace("BranchID", BranchID);
					frankText = frankText.Replace("LocationIdentifier", LocationIdentifier);
					frankText = frankText.Replace("LocationName", LocationName);
					frankText = frankText.Replace("TerminalName", TerminalName);
					frankText = frankText.Replace("TellerName", TellerName);
					frankText = frankText.Replace("TellerID", TellerID);
				}

				if (transaction != null)
				{
					string TransactionID = transaction.Id == 0 ? "" : transaction.Id.ToString();
					string CheckNumber = String.IsNullOrEmpty(transaction.ConfirmationNumber) ? "" : transaction.ConfirmationNumber;
					string TransactionDate = transaction.DTTerminalCreate == null ? "" : transaction.DTTerminalCreate.ToShortDateString();
					string TransactionTime = transaction.DTTerminalCreate == null ? "" : transaction.DTTerminalCreate.ToShortDateString();
					string CheckAmount = transaction.Amount == 0 ? "" : transaction.Amount.ToString("0.00");
					string FormatedTransactionDate = transaction.DTTerminalCreate == null ? "" : transaction.DTTerminalCreate.ToString("yyyyMMdd");
					frankText = frankText.Replace("FormatedTransactionDate", FormatedTransactionDate);
					frankText = frankText.Replace("TransactionDate", TransactionDate);
					frankText = frankText.Replace("TransactionTime", TransactionTime);
					frankText = frankText.Replace("CheckAmount", CheckAmount);
					frankText = frankText.Replace("CheckNumber", CheckNumber);
					frankText = frankText.Replace("TransactionID", TransactionID);
					frankText = frankText.Replace("SequenceNo", Convert.ToString(transaction.Account.Customer.CXEId));
				}
				frankText = frankText.Replace("|", "");
			}
			return frankText;
		}

		public CheckProcessorInfo GetCheckProcessorInfo(long agentSessionId, MGIContext mgiContext)
		{
			AgentSession session = _ptnrAgentSvc.Lookup(agentSessionId);
			//Dictionary context name has to be changed to generic name
			string branchUserName = mgiContext.CheckUserName;

			if (session != null && session.Terminal != null && session.Terminal.Location != null)
			{
				string locationId = string.Format("{0}-{1}", session.Terminal.ChannelPartner.Id, branchUserName);
				ICheckProcessor checkProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
				if (checkProcessor != null)
				{
					CXNData.CheckProcessorInfo checkProcessorInfo = checkProcessor.GetCheckProcessorInfo(locationId);

					return Mapper.Map<CheckProcessorInfo>(checkProcessorInfo);
				}
				else
					return null;
			}
			else
			{
				return null;
			}
		}

		public void UpdateTransactionFranked(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            #region AL-3371 Transactional Log User Story(Process check)

            string id = "Transaction Id : " + transactionId;

			MongoDBLogger.Info<string>(customerSessionId, id, "UpdateTransactionFranked", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin UpdateTransactionFranked - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion

			PTNRData.Transactions.Check ptnrCheck = _ptnrCheckSvc.Lookup(transactionId);

			_GetProcessor(mgiContext.ChannelPartnerName).UpdateTransactionFranked(ptnrCheck.CXNId);

            #region AL-3371 Transactional Log User Story(Process check)

            MongoDBLogger.Info<PTNRData.Transactions.Check>(customerSessionId, ptnrCheck, "UpdateTransactionFranked", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End UpdateTransactionFranked - MGI.Biz.CPEngine.Impl.CPEngineServiceImpl",
				mgiContext);
			#endregion
		}

		public ChexarLogin GetChexarSessions(MGIContext mgiContext)
		{
			ICheckProcessor checkProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
			return Mapper.Map<ChexarLogin>(checkProcessor.GetCheckSessions(mgiContext));
		}

		#endregion

		#region private methods

		private ICheckProcessor _GetProcessor(string channelPartner)
		{
			// get the fund processor for the channel partner.
			return (ICheckProcessor)CheckProcessorRouter.GetProcessor(channelPartner);
		}

		//A Method to Get Fund Provider based on ChannelPartner
		private string _GetCheckProvider(string channelPartner)
		{
			// get the fund provider for the channel partner.
			return CheckProcessorRouter.GetProvider(channelPartner);
		}

		private CXEData.Account GetCheckAccount(PTNRData.Customer ptnrCustomer, MGIContext mgiContext)
		{
			//get the CXE customer
			CXEData.Customer customer = _cxeCustSvc.Lookup(ptnrCustomer.CXEId);

			//Get Provider for Check
			int provider = (int)Enum.Parse(typeof(ProviderIds), _GetCheckProvider(mgiContext.ChannelPartnerName));

			PTNRData.Account ptnrCheckAcct = ptnrCustomer.GetAccount(provider);

			CXEData.Account cxeCheckAccount;
			if (ptnrCheckAcct == null)
			{
				// create CXE account
				cxeCheckAccount = _acctSvc.AddCustomerCheckAccount(customer);

				// create PTNR account
				ptnrCheckAcct = ptnrCustomer.AddAccount(provider, cxeCheckAccount.Id);


				NexxoIdType idType = null;
				if (customer.GovernmentId != null)
					idType = _ptnrIDTypeSvc.Find(customer.ChannelPartnerId, customer.GovernmentId.IdTypeId);

				CXNData.CheckAccount cxnAccount = CheckMapper.ToCxnAccount(customer, idType);

				// create CXN account
				cxnAccount.Id = _GetProcessor(mgiContext.ChannelPartnerName).Register(cxnAccount, mgiContext, mgiContext.TimeZone);

				// update PTNR account
				ptnrCheckAcct.CXNId = cxnAccount.Id;

				// not sure if needed (to update the ptnrCheckAcct.CXNId
				_ptnrCustomerSvc.Update(ptnrCustomer);
			}
			else
				cxeCheckAccount = customer.GetAccount(ptnrCheckAcct.CXEId);

			return cxeCheckAccount;
		}

		private void updateBizCheckApprovalDetails(PTNRData.Transactions.Check ptnrCheck, CXNData.CheckType validatedType, ref Check check)
		{
			check.ValidatedType = _ptnrSvc.GetCheckType((int)validatedType).Name;
			check.ValidatedFee = ptnrCheck.Fee;
			check.BaseFee = ptnrCheck.BaseFee;
			check.DiscountApplied = ptnrCheck.DiscountApplied;
			check.DiscountName = ptnrCheck.DiscountName;
		}

		private void updatePartnerCheckFee(CustomerSession session, CXNData.CheckType validatedType, ref PTNRData.Transactions.Check ptnrCheck, MGIContext mgiContext)
		{
			List<PTNRData.Transactions.Check> transactions = _ptnrCheckSvc.GetAllForCustomer(session.Customer.Id);

			//US2030		
			removeCancelParkedTransactions(session, ref transactions);

			// don't include this check in fee calculation
			transactions.Remove(ptnrCheck);

			TransactionFee fee = _feeSvc.GetCheckFee(session, transactions, ptnrCheck.Amount, (int)validatedType, mgiContext);

			ptnrCheck.BaseFee = fee.BaseFee;
			ptnrCheck.DiscountApplied = fee.DiscountApplied;
			ptnrCheck.Fee = fee.NetFee;
			ptnrCheck.AdditionalFee = fee.AdditionalFee;
            // AL-4604: handled duplicate fee adjustments.
            FeeAdjustmentService.DeleteFeeAdjustments(ptnrCheck.rowguid);
			ptnrCheck.AddFeeAdjustments(fee.Adjustments);
			ptnrCheck.Description = validatedType.ToString();
			//US1799 Manual Entry Promotions
			ptnrCheck.DiscountName = fee.DiscountName;
			ptnrCheck.DiscountDescription = fee.DiscountDescription;
			ptnrCheck.IsSystemApplied = fee.IsSystemApplied;
		}

		private void updatePartnerNextCheckFee(CustomerSession session, CXNData.CheckType validatedType, ref PTNRData.Transactions.Check ptnrCheck, MGIContext mgiContext)
		{
			List<PTNRData.Transactions.Check> transactions = _ptnrCheckSvc.GetAllForCustomer(session.Customer.Id);

			removeCancelParkedTransactions(session, ref transactions);
			// dont't include the upcoming transactions 
			// don't include this check in fee calculation
			var id = ptnrCheck.Id;
			transactions.RemoveAll(x => x.Id >= id && x.CXEState != (int)PTNRTransactionStates.Committed);
			mgiContext.IsSystemApplied = ptnrCheck.IsSystemApplied;
			if (ptnrCheck.DiscountName != null)
			{
				mgiContext.PromotionCode = ptnrCheck.DiscountName.Trim();
			}
			TransactionFee fee = _feeSvc.GetCheckFee(session, transactions, ptnrCheck.Amount, (int)validatedType, mgiContext);

			ptnrCheck.BaseFee = fee.BaseFee;
			ptnrCheck.DiscountApplied = fee.DiscountApplied;
			ptnrCheck.AdditionalFee = fee.AdditionalFee; //DE3352 For Surcharge
			ptnrCheck.Fee = fee.NetFee;

			// AL-591: This is introduced to update IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
			// Developed by: Sunil Shetty || 03/07/2015
			FeeAdjustmentService.DeleteFeeAdjustments(ptnrCheck.rowguid);

			ptnrCheck.AddFeeAdjustments(fee.Adjustments);
			ptnrCheck.Description = validatedType.ToString();
			//US1799 Manual Entry Promotions	
			ptnrCheck.DiscountName = fee.DiscountName;
			ptnrCheck.DiscountDescription = fee.DiscountDescription;
			ptnrCheck.IsSystemApplied = fee.IsSystemApplied;
		}

		private void removeCancelParkedTransactions(CustomerSession session, ref List<PTNRData.Transactions.Check> transactions)
		{
			var ShoppingCart = session.ParkingShoppingCart;

			if (ShoppingCart != null)
			{
				foreach (var transaction in ShoppingCart.ShoppingCartTransactions)
				{
					transactions.RemoveAll(x => x.rowguid == transaction.Transaction.rowguid);
				}
			}
			transactions.RemoveAll(x => x.CXEState == (int)PTNRTransactionStates.Canceled || x.CXEState == (int)PTNRTransactionStates.Declined);
		}


		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
		/// Method to update Existing Customer Referral promotions.
		/// </summary>
		/// <param name="session"></param>
		/// <param name="ptnrCheck"></param>
		/// <param name="context"></param>
		private void addUpdateCustomerFeeAdjustment(CustomerSession session, PTNRData.Transactions.Check ptnrCheck, MGIContext mgiContext)
		{
			foreach (PTNRTrans.TransactionFeeAdjustment adj in ptnrCheck.FeeAdjustments)
			{
				if (adj.feeAdjustment.PromotionType != null && adj.feeAdjustment.PromotionType.ToLower() == PromotionType.Referral.ToString().ToLower())
				{
					var customerFeeAdjustments = CustomerFeeAdjustmentService.lookup(session.Customer.CXEId, adj.feeAdjustment);
					if (customerFeeAdjustments != null)
					{
						customerFeeAdjustments.IsAvailed = true;
						customerFeeAdjustments.DTServerLastModified = DateTime.Now;
						customerFeeAdjustments.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
						CustomerFeeAdjustmentService.Update(customerFeeAdjustments);
					}
				}
			}
		}
		#endregion
	}
}
