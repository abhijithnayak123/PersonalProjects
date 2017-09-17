using AutoMapper;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;
using MGI.Biz.Events.Contract;
using MGI.Biz.FundsEngine.Contract;
using MGI.Biz.FundsEngine.Data;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.Util;
using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Cxn.Common.Processor.Util;
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.Data;
using MGI.TimeStamp;
using Spring.Transaction.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using BizCommon = MGI.Biz.Common.Data;
using BizFunds = MGI.Biz.FundsEngine.Data.Funds;
using BizFundsAccount = MGI.Biz.FundsEngine.Data.FundsAccount;
using CxeAccount = MGI.Core.CXE.Data.Account;
using CxeCustomer = MGI.Core.CXE.Data.Customer;
using CxeFunds = MGI.Core.CXE.Data.Transactions.Stage.Funds;
using CxnFundsAccount = MGI.Cxn.Fund.Data.CardAccount;
using PTNRAccount = MGI.Core.Partner.Data.Account;
using PTNRChannelPartnerService = MGI.Core.Partner.Contract.IChannelPartnerService;
using PTNRCustomer = MGI.Core.Partner.Data.Customer;
using PTNRCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using PTNRFee = MGI.Core.Partner.Data.TransactionFee;
using PTNRFunds = MGI.Core.Partner.Data.Transactions.Funds;
using PTNRFundsService = MGI.Core.Partner.Contract.ITransactionService<MGI.Core.Partner.Data.Transactions.Funds>;

namespace MGI.Biz.FundsEngine.Impl
{
	/// <summary>
	/// 
	/// </summary>
	public class FundsEngineImpl : IFundsEngine
	{
		#region Injected Services

		public IFundsService CXEFundsService { private get; set; }
		public MGI.Core.CXE.Contract.ICustomerService CXECustomerService { private get; set; }
		public IAccountService CXEAccountService { private get; set; }
		public PTNRCustomerService PTNRCustomerService { private get; set; }
		public PTNRFundsService PTNRFundsService { private get; set; }
		public ICustomerSessionService CustomerSessionService { private get; set; }
		public IFeeService FeeService { private get; set; }
		public ITerminalService TerminalService { private get; set; } // Not Using
		public INexxoDataStructuresService NexxoIdTypeService { private get; set; }
		public PTNRChannelPartnerService PTNRChannelPartnerService { private get; set; }
		public ILimitService LimitService { private get; set; }
		public IProcessorRouter ProcessorRouter { private get; set; }
		public INexxoBizEventPublisher EventPublisher { private get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public FundsEngineImpl()
		{
			Mapper.CreateMap<CxnFundsAccount, BizFundsAccount>();
			Mapper.CreateMap<TransactionFee, BizCommon.TransactionFee>();
			Mapper.CreateMap<BizFundsAccount, CxnFundsAccount>();
			Mapper.CreateMap<CxeFunds, BizFunds>()
				.ForMember(d => d.Amount, s => s.MapFrom(c => c.Amount))
				.ForMember(d => d.Fee, s => s.MapFrom(c => c.Fee));
			Mapper.CreateMap<MGI.Cxn.Fund.Data.FundTrx, BizFunds>()
				.ForMember(d => d.FundDescription, s => s.MapFrom(c => c.TransactionDescription))
				.AfterMap((s, d) =>
				{
					if (s.TransactionType != null)
					{
						Biz.FundsEngine.Data.FundType fundType;
						if (s.TransactionType.ToString() == MGI.Cxn.Fund.TSys.Data.TSysTransactionType.Credit.ToString())
							fundType = FundType.Credit;
						else if (s.TransactionType.ToString() == MGI.Cxn.Fund.TSys.Data.TSysTransactionType.Debit.ToString())
							fundType = FundType.Debit;
						else if (s.TransactionType.ToString() == MGI.Cxn.Fund.TSys.Data.TSysTransactionType.AddOnCard.ToString())
							fundType = FundType.AddOnCard;
						else
							fundType = FundType.None;

						d.FundsType = fundType;
					}
					else
						d.FundsType = FundType.None;
				});
			Mapper.CreateMap<MGI.Cxn.Fund.Data.TransactionHistory, Data.TransactionHistory>();
			Mapper.CreateMap<Data.TransactionHistoryRequest, Cxn.Fund.Data.TransactionHistoryRequest>();
			Mapper.CreateMap<Cxn.Fund.Data.CardInfo, Data.CardInfo>();
			Mapper.CreateMap<Data.CardMaintenanceInfo, Cxn.Fund.Data.CardMaintenanceInfo>();
			Mapper.CreateMap<MGI.Cxn.Fund.Data.ShippingTypes, MGI.Biz.FundsEngine.Data.ShippingTypes>();

		}

		protected void PublishEvent(string channelPartner, GPRAddEvent gprAddEvent, string AccountNumber)
		{
			if (!gprAddEvent.mgiContext.Context.ContainsKey("AccountNumber"))
			{
				gprAddEvent.mgiContext.Context.Add("AccountNumber", AccountNumber);
			}
			EventPublisher.Publish(channelPartner, gprAddEvent);
		}

		#region Interface Implementation
		public long Add(long customerSessionId, BizFundsAccount fundsAccount, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<BizFundsAccount>(customerSessionId, fundsAccount, "Add", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin Add - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			CustomerSession customerSession = null;
			CxeCustomer cxeCustomer = null;

			if (customerSessionId == -1)
			{
				cxeCustomer = CXECustomerService.Lookup(mgiContext.CXECustomerId);
			}
			else
			{
				customerSession = GetCustomerSession(customerSessionId);
				// now add the account to CXE.
				cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
			}

			CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(a => a.Type == (int)AccountTypes.Funds);

			if (cxeAccount == null)
				cxeAccount = CXEAccountService.AddCustomerFundsAccount(cxeCustomer);
			fundsAccount = MapToFundsAccount(fundsAccount, cxeCustomer, mgiContext);

			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

			// add the account to CXN.
            //PTNRAccount ptnrAccount = PTNRCustomerService.Lookup(cxeCustomer.Id).Accounts.FirstOrDefault(a => a.CXEId == cxeAccount.Id);

            int providerId = (int)Enum.Parse(typeof(ProviderIds), _GetFundProvider(mgiContext.ChannelPartnerName));

            PTNRAccount ptnrAccount = PTNRCustomerService.Lookup(cxeCustomer.Id).Accounts.FirstOrDefault(a => a.CXEId == cxeAccount.Id & a.ProviderId == providerId);

			long cxnFundId = 0;
            if (ptnrAccount == null)
            {
                cxnFundId = _AddCXNAccount(fundsAccount, cxnFundsProcessor, mgiContext);
                // add the account to PTNR considering Providerid
                PTNRCustomer ptnrCustomer = PTNRCustomerService.Lookup(cxeCustomer.Id);
                ptnrCustomer.AddAccount(providerId, cxeAccount.Id, cxnFundId);
            }
            else
            {
                CxnFundsAccount cxnFundsAccount = Mapper.Map<BizFundsAccount, CxnFundsAccount>(fundsAccount);
                cxnFundsAccount.Id = ptnrAccount.CXNId;
                cxnFundsProcessor.UpdateRegistrationDetails(cxnFundsAccount, mgiContext);
                cxnFundId = ptnrAccount.CXNId;
            }

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<PTNRAccount>(customerSessionId, ptnrAccount, "Add", AlloyLayerName.BIZ,
				ModuleName.Funds, "End Add - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return cxeAccount.Id;
		}

		public bool AuthenticateCard(long customerSessionId, string cardNumber, string authenticationInfo, string encryptionKey, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			List<string> details = new List<string>();
			details.Add("Card Number : " + cardNumber);
			details.Add("Authentication Info : " + authenticationInfo);
			details.Add("Encryption Key : " + encryptionKey);

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "AuthenticateCard", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin AuthenticateCard - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			// how do I know, if it is Cxn authentication? and which 1.
			ProcessorResult processorResult;

			CustomerSession customerSession = GetCustomerSession(customerSessionId);

			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

			long cxnAccountId = cxnFundsProcessor.Authenticate(cardNumber, mgiContext, out processorResult);
			_HandleProcessorResult(processorResult);

			#region AL-3372 transaction information for GPR cards.
			string id = Convert.ToString(cxnAccountId);

			MongoDBLogger.Info<string>(customerSessionId, id, "AuthenticateCard", AlloyLayerName.BIZ,
				ModuleName.Funds, "End AuthenticateCard - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			if (cxnAccountId > 0)
				return true;
			else
				return false;
		}

		public long Withdraw(long customerSessionId, BizFunds bizFunds, MGIContext mgiContext)
		{
			return Transact(customerSessionId, bizFunds, RequestType.Debit, mgiContext);
		}

		public long Activate(long customerSessionId, BizFunds bizFunds, MGIContext mgiContext)
		{
			return Transact(customerSessionId, bizFunds, RequestType.None, mgiContext);
		}

		public long Load(long customerSessionId, BizFunds bizFunds, MGIContext mgiContext)
		{
			return Transact(customerSessionId, bizFunds, RequestType.Credit, mgiContext);
		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public int Commit(long customerSessionId, long cxeTrxId, MGIContext mgiContext, string cardNumber = "")
		{
			#region AL-3372 transaction information for GPR cards.
			List<string> details = new List<string>();
			details.Add("Card Number : " + cardNumber);
			details.Add("cxeTrxId : " + cxeTrxId);

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "Commit", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin Commit - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			ProcessorResult processorResult;
			PTNRFunds ptnrTrx = PTNRFundsService.Lookup(cxeTrxId);

			if (ptnrTrx.CXNId <= 0)
				throw new BizFundsException(BizFundsException.INVALID_TRANSACTION_REQUEST);

			CustomerSession customerSession = GetCustomerSession(customerSessionId);
			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
			//TBD remove the code 
			if (ptnrTrx.AddOnCustomerId > 0)
			{
				CxeCustomer cxeCustomer = CXECustomerService.Lookup(ptnrTrx.AddOnCustomerId);

				BizFundsAccount bizFundsAccount = MapToFundsAccount(new BizFundsAccount(), cxeCustomer, mgiContext);

				CxnFundsAccount cxnFundsAccount = Mapper.Map<BizFundsAccount, CxnFundsAccount>(bizFundsAccount);
				ChannelPartner channelPartner;
				GetAccountInfo(customerSessionId, mgiContext, out channelPartner);

				var visaProvider = channelPartner.Providers.FirstOrDefault(x => x.ProductProcessor.Code == (long)ProviderIds.Visa);

				if (visaProvider != null)
				{
					mgiContext.CardExpiryPeriod = visaProvider.CardExpiryPeriod;
				}
				if (mgiContext.Context == null)
					mgiContext.Context = new Dictionary<string, object>();

				mgiContext.Context.Add("FundsAccount", cxnFundsAccount);
			}
			cxnFundsProcessor.Commit(ptnrTrx.CXNId, mgiContext, out processorResult, cardNumber);

			MGI.Cxn.Fund.Data.FundTrx cxnFunds = cxnFundsProcessor.Get(ptnrTrx.CXNId, mgiContext);

			if (cxnFunds.TransactionType == "0000" || cxnFunds.TransactionType.ToLower() == "activation")
			{
				// publish the GPRadd event. 
				PTNRAccount ptnraccout = customerSession.Customer.Accounts.Where(x => x.ProviderId == (int)ProviderIds.FIS).FirstOrDefault();
				long cxnid = ptnraccout != null ? ptnraccout.CXNId : 0;
				if (cxnid > 0)
				{
					mgiContext.Context.Add("CXNId", cxnid);
				}
				PublishEvent(mgiContext.ChannelPartnerName, new GPRAddEvent() { Gpr = Mapper.Map<CxnFundsAccount, BizFundsAccount>(cxnFunds.Account), mgiContext = mgiContext }, cxnFunds.Account.AccountNumber);
			}

			if (cxnFunds.TransactionType.ToLower() == "addoncard")
			{
				CardAccount cardAccount = (CardAccount)mgiContext.Context["FundsAccount"];
				MGI.Core.Partner.Data.Customer customer = PTNRCustomerService.LookupByCxeId(ptnrTrx.AddOnCustomerId);
				long cxnid = 0;
				if (customer != null)
				{
					PTNRAccount ptnraccount = customer.Accounts.Where(x => x.ProviderId == (int)ProviderIds.FIS).FirstOrDefault();
					cxnid = ptnraccount != null ? ptnraccount.CXNId : 0;
				}
				if (cxnid > 0)
				{
					mgiContext.Context.Add("CXNId", cxnid);
				}
				PublishEvent(mgiContext.ChannelPartnerName, new GPRAddEvent() { Gpr = Mapper.Map<CxnFundsAccount, BizFundsAccount>(cardAccount), mgiContext = mgiContext }, cxnFunds.Account.AccountNumber);
			}
			//timestamp changes
			CXEFundsService.Commit(cxeTrxId, mgiContext.TimeZone);

			CXEFundsService.Update(cxeTrxId, TransactionStates.Committed, mgiContext.TimeZone);

			_HandleProcessorResult(processorResult);

			//Modified by Yashasvi to handle the confirmation number. 06/19/2013			
			PTNRFundsService.UpdateTransactionDetails(cxeTrxId, (int)MGI.Core.CXE.Data.TransactionStates.Committed,
				(int)MGI.Core.CXE.Data.TransactionStates.Committed, processorResult.ConfirmationNumber, ptnrTrx.FundType);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<PTNRFunds>(customerSessionId, ptnrTrx, "Commit", AlloyLayerName.BIZ,
				ModuleName.Funds, "End Commit - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return (int)MGI.Core.CXE.Data.TransactionStates.Committed;
		}

		public Data.CardInfo GetBalance(long customerSessionId, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetBalance", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin GetBalance - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			long cxnAccountID = getCXNAccountId(customerSessionId, mgiContext);

			//if (cxnAccountID <= 0)
			//    throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			ProcessorResult processorResult = new ProcessorResult();

			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

			MGI.Cxn.Fund.Data.CardInfo cardInfo = cxnFundsProcessor.GetBalance(cxnAccountID, mgiContext, out processorResult);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<MGI.Cxn.Fund.Data.CardInfo>(customerSessionId, cardInfo, "GetBalance", AlloyLayerName.BIZ,
				ModuleName.Funds, "End GetBalance - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return Mapper.Map<Data.CardInfo>(cardInfo);
		}

		private long getCXNAccountId(long customerSessionId, MGIContext mgiContext)
		{
			CustomerSession customerSession = GetCustomerSession(customerSessionId);

			CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
			CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(a => a.Type == (int)AccountTypes.Funds);
			long cxeAccountID = 0;
			if (cxeAccount != null)
				cxeAccountID = cxeAccount.Id;

			if (cxeAccountID <= 0)
				throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			//long cxeAccountID = cxeAccount.Id;

			//Begin Changes - AL-2059
			// Commented by Karun
			//long cxnAccountID = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccountID).CXNId;
			//In case of multiple accounts for a product type, then fetch the cxn id based on the processor instead of assuming it will be 1-1 always. 
			//Since the processor mapping to channel partner and product is 1-1 at this time.
			// Where else do we have to change??

			int providerId = (int)Enum.Parse(typeof(ProviderIds), _GetFundProvider(mgiContext.ChannelPartnerName));
			PTNRCustomer ptnrcustomer = PTNRCustomerService.Lookup(cxeCustomer.Id);

			long cxnAccountID = 0;
			PTNRAccount ptnrAccount = ptnrcustomer.Accounts.FirstOrDefault(x => x.ProviderId == providerId);
			if (ptnrAccount != null)
				cxnAccountID = ptnrAccount.CXNId;

			if (cxnAccountID <= 0)
				throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			// End Changes AL-2059

			return cxnAccountID;
		}

		public BizFundsAccount GetAccount(long customerSessionId, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetAccount", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin GetAccount - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			//CustomerSession customerSession = GetCustomerSession(customerSessionId);

			//CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);

			//CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(a => a.Type == (int)AccountTypes.Funds);

			//if (cxeAccount == null)
			//{
			//    return null;
			//}


			// Why are we doing this twice??
			ProcessorResult processorResult = new ProcessorResult();
			//processorResult = new ProcessorResult();

			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
			MGI.Cxn.Fund.Data.CardInfo cardInfo = new Cxn.Fund.Data.CardInfo();
			long cxnAccountID = 0;

			try
			{
				cxnAccountID = getCXNAccountId(customerSessionId, mgiContext);
				cardInfo = cxnFundsProcessor.GetBalance(cxnAccountID, mgiContext, out processorResult);
			}
			catch (FundException ex)
			{
				//AL-3372 transaction information for GPR cards.
				//MongoDBLogger.Error<CxeAccount>(cxeAccount, "GetAccount", AlloyLayerName.BIZ,
				//	ModuleName.Funds, "Error in GetAccount - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				//	ex.Message, ex.StackTrace);

				if (ex.MajorCode == 1003 && ex.MinorCode == 2107)
				{
					cardInfo.Balance = decimal.MinValue;
				}
				else
				{
					throw new FundException(ex.MinorCode, ex.Message);
				}
			}
			CardAccount cardAccount = cxnFundsProcessor.Lookup(cxnAccountID);

			if (cardAccount == null)
				throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<CardAccount>(customerSessionId, cardAccount, "GetAccount", AlloyLayerName.BIZ,
				ModuleName.Funds, "End GetAccount - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return new BizFundsAccount()
			{
				CardNumber = cardAccount.CardNumber,
				AccountNumber = cardAccount.AccountNumber,
				CardBalance = cardInfo.Balance,
				FirstName = cardAccount.FirstName,
				LastName = cardAccount.LastName
			};
		}

		public BizFunds Get(long customerSessionId, long Id, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			string trnId = "Id :" + Convert.ToString(Id);

			MongoDBLogger.Info<string>(customerSessionId, trnId, "Get", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin Get - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			CustomerSession customerSession = GetCustomerSession(customerSessionId);
			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
			PTNRFunds ptnrTransaction = PTNRFundsService.Lookup(Id);
			MGI.Cxn.Fund.Data.FundTrx fundTrx = cxnFundsProcessor.Get(ptnrTransaction.CXNId, mgiContext);
			BizFunds bizFundTrx = Mapper.Map<BizFunds>(fundTrx);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<MGI.Cxn.Fund.Data.FundTrx>(customerSessionId, fundTrx, "Get", AlloyLayerName.BIZ,
				ModuleName.Funds, "End Get - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return bizFundTrx;
		}

		public BizCommon.TransactionFee GetFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			List<string> details = new List<string>();
			details.Add("Amount : " + amount);
			details.Add("Funds Type : " + Convert.ToString(fundsType));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetFee", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin GetFee - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			CustomerSession customerSession = GetCustomerSession(customerSessionId);
			List<PTNRFunds> transactions = PTNRFundsService.GetAllForCustomer(customerSession.Customer.Id);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.ListInfo<PTNRFunds>(customerSessionId, transactions, "GetFee", AlloyLayerName.BIZ,
				ModuleName.Funds, "End GetFee - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return Mapper.Map<BizCommon.TransactionFee>(FeeService.GetFundsFee(customerSession, transactions, amount, (int)fundsType, mgiContext));
		}

		public long UpdateAmount(long cxeFundTrxId, decimal amount, long customerSessionId, FundType fundType, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			List<string> details = new List<string>();
			details.Add("Amount : " + amount);
			details.Add("Funds Type : " + Convert.ToString(fundType));
			details.Add("Cxe Fund TrxId : " + Convert.ToString(cxeFundTrxId));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "UpdateAmount", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin UpdateAmount - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			CustomerSession customerSession = GetCustomerSession(customerSessionId);

			CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);

			CxeAccount _cxeAccount = cxeCustomer.Accounts.FirstOrDefault(a => a.Type == (int)AccountTypes.Funds);
			RequestType requestType = RequestType.None;

			if (fundType == FundType.Credit)
				requestType = RequestType.Credit;
			else if (fundType == FundType.Debit)
				requestType = RequestType.Debit;

			_ValidateLimits(customerSession, cxeCustomer, _cxeAccount, requestType, mgiContext, amount);

			//retrieve the cxn space transaction
			PTNRFunds ptnrFunds = PTNRFundsService.Lookup(cxeFundTrxId);

			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

			//update the amount of the transaction in cxe
			CXEFundsService.UpdateAmount(ptnrFunds.CXEId, ptnrFunds.FundType == (int)FundType.Credit ? amount - ptnrFunds.Fee : amount + ptnrFunds.Fee, customerSession.AgentSession.Terminal.Location.TimezoneID);//timestamp
			if (requestType == RequestType.None || requestType == RequestType.Credit || requestType== RequestType.Debit )
			{
				FundRequest fundRequest = new FundRequest()
				{
					RequestType = GetRequestType(Convert.ToString(requestType)),
					Amount = ptnrFunds.FundType == (int)FundType.Credit ? amount - ptnrFunds.Fee : amount + ptnrFunds.Fee,
					PromoCode = mgiContext.GPRPromoCode
				};

				//update the amount of the transaction in cxn
				cxnFundsProcessor.UpdateAmount(ptnrFunds.CXNId, fundRequest, mgiContext.TimeZone);
			}

			//update the amount of the transaction in partner
			PTNRFundsService.UpdateAmount(cxeFundTrxId, amount);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<PTNRFunds>(customerSessionId, ptnrFunds, "UpdateAmount", AlloyLayerName.BIZ,
				ModuleName.Funds, "End UpdateAmount - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return cxeFundTrxId;
		}
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="isActivation"></param>
		/// <returns></returns>
		public decimal GetMinimumLoadAmount(long customerSessionId, bool initialLoad, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			string load = "initialLoad : " + Convert.ToString(initialLoad);

			MongoDBLogger.Info<string>(customerSessionId, load, "GetMinimumLoadAmount", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin GetMinimumLoadAmount - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			//Get the ComplainceProgram from channelpartner id 
			ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(mgiContext.ChannelPartnerId);

			decimal minLoadAmt = 0.00M;
			// pass the complaince program 	
			if (initialLoad)
			{
				minLoadAmt = LimitService.GetProductMinimum(channelPartner.ComplianceProgramName, TransactionTypes.ActivateGPR, mgiContext);
			}
			else
			{
				minLoadAmt = LimitService.GetProductMinimum(channelPartner.ComplianceProgramName, TransactionTypes.LoadToGPR, mgiContext);
			}

			#region AL-3372 transaction information for GPR cards.
			string loadAmt = "initialLoad : " + Convert.ToString(minLoadAmt);

			MongoDBLogger.Info<string>(customerSessionId, loadAmt, "GetMinimumLoadAmount", AlloyLayerName.BIZ,
				ModuleName.Funds, "End GetMinimumLoadAmount - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return minLoadAmt;
		}

		public void Cancel(long customerSessionId, long fundsId, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			string id = "Funds Id : " + Convert.ToString(fundsId);

			MongoDBLogger.Info<string>(customerSessionId, id, "Cancel", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin Cancel - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			PTNRFunds ptnrFund = PTNRFundsService.Lookup(fundsId);

			CustomerSession customerSession = GetCustomerSession(customerSessionId);

			CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);

			if (ptnrFund.FundType == (int)FundType.None)
			{
				long cxeAccountID = cxeCustomer.Accounts.First(a => a.Type == (int)AccountTypes.Funds).Id;

                //AL-5904 
                //long cxnAccountID = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccountID).CXNId;
                long cxnAccountID = getCXNAccountId(customerSessionId, mgiContext);

				if (cxnAccountID <= 0)
					throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

				IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

				cxnFundsProcessor.Cancel(cxnAccountID, mgiContext);
			}

			// Update CXE Space
			CXEFundsService.Update(ptnrFund.CXEId, TransactionStates.Canceled, mgiContext.TimeZone);

			// Update Partner Space
			PTNRFundsService.UpdateStates(ptnrFund.Id, (int)TransactionStates.Canceled, (int)TransactionStates.Canceled);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<PTNRFunds>(customerSessionId, ptnrFund, "Cancel", AlloyLayerName.BIZ,
				ModuleName.Funds, "End Cancel - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion
		}

		public List<Data.TransactionHistory> GetTransactionHistory(long customerSessionId, Data.TransactionHistoryRequest request, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Data.TransactionHistoryRequest>(customerSessionId, request, "GetTransactionHistory", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin GetTransactionHistory - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion


			ChannelPartner channelPartner;
			long cxnAccountID;
			cxnAccountID = GetAccountInfo(customerSessionId, mgiContext, out channelPartner);

			if (cxnAccountID <= 0)
				throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			IFundProcessor cxnFundsProcessor = _GetProcessor(channelPartner.Name);

			MGI.Cxn.Fund.Data.TransactionHistoryRequest cxnTransactionHistoryRequest = Mapper.Map<MGI.Cxn.Fund.Data.TransactionHistoryRequest>(request);

			List<MGI.Cxn.Fund.Data.TransactionHistory> transactionHistoryList = cxnFundsProcessor.GetTransactionHistory(cxnAccountID, cxnTransactionHistoryRequest, mgiContext);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.ListInfo<MGI.Cxn.Fund.Data.TransactionHistory>(customerSessionId, transactionHistoryList, "GetTransactionHistory", AlloyLayerName.BIZ,
				ModuleName.Funds, "End GetTransactionHistory - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return Mapper.Map<List<Data.TransactionHistory>>(transactionHistoryList);
		}

		public bool CloseAccount(long customerSessionId, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "CloseAccount", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin CloseAccount - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			ChannelPartner channelPartner;
			long cxnAccountID;
			cxnAccountID = GetAccountInfo(customerSessionId, mgiContext, out channelPartner);

			if (cxnAccountID <= 0)
				throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			IFundProcessor cxnFundsProcessor = _GetProcessor(channelPartner.Name);

			#region AL-3372 transaction information for GPR cards.
			string acntId = "Account Id :" + Convert.ToString(cxnAccountID);

			MongoDBLogger.Info<string>(customerSessionId, acntId, "CloseAccount", AlloyLayerName.BIZ,
				ModuleName.Funds, "End CloseAccount - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return cxnFundsProcessor.CloseAccount(cxnAccountID, mgiContext);
		}

		public bool UpdateCardStatus(long customerSessionId, Data.CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Data.CardMaintenanceInfo>(customerSessionId, cardMaintenanceInfo, "UpdateCardStatus", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin UpdateCardStatus - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			ChannelPartner channelPartner;
			long cxnAccountID;
			cxnAccountID = GetAccountInfo(customerSessionId, mgiContext, out channelPartner);

			if (cxnAccountID <= 0)
				throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			IFundProcessor cxnFundsProcessor = _GetProcessor(channelPartner.Name);
			Cxn.Fund.Data.CardMaintenanceInfo cxnCardMaintenanceInfo = Mapper.Map<Cxn.Fund.Data.CardMaintenanceInfo>(cardMaintenanceInfo);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Cxn.Fund.Data.CardMaintenanceInfo>(customerSessionId, cxnCardMaintenanceInfo, "UpdateCardStatus", AlloyLayerName.BIZ,
				ModuleName.Funds, "End UpdateCardStatus - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return cxnFundsProcessor.UpdateCardStatus(cxnAccountID, cxnCardMaintenanceInfo, mgiContext);
		}

		public bool ReplaceCard(long customerSessionId, Data.CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Data.CardMaintenanceInfo>(customerSessionId, cardMaintenanceInfo, "ReplaceCard", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin ReplaceCard - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			ChannelPartner channelPartner;
			long cxnAccountID = GetAccountInfo(customerSessionId, mgiContext, out channelPartner);

			var visaProvider = channelPartner.Providers.FirstOrDefault(x => x.ProductProcessor.Code == (long)ProviderIds.Visa);

			if (visaProvider != null)
			{
				mgiContext.CardExpiryPeriod = visaProvider.CardExpiryPeriod;
			}

			if (cxnAccountID <= 0)
				throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			IFundProcessor cxnFundsProcessor = _GetProcessor(channelPartner.Name);
			Cxn.Fund.Data.CardMaintenanceInfo cxnCardMaintenanceInfo = Mapper.Map<Cxn.Fund.Data.CardMaintenanceInfo>(cardMaintenanceInfo);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Cxn.Fund.Data.CardMaintenanceInfo>(customerSessionId, cxnCardMaintenanceInfo, "ReplaceCard", AlloyLayerName.BIZ,
				ModuleName.Funds, "End ReplaceCard - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			return cxnFundsProcessor.ReplaceCard(cxnAccountID, cxnCardMaintenanceInfo, mgiContext);
		}

		public List<MGI.Biz.FundsEngine.Data.ShippingTypes> GetShippingTypes(MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(0, string.Empty, "GetShippingTypes", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin GetShippingTypes - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion

			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

			List<MGI.Cxn.Fund.Data.ShippingTypes> cardShippingTypes = cxnFundsProcessor.GetShippingTypes((long)mgiContext.ChannelPartnerId);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.ListInfo<MGI.Cxn.Fund.Data.ShippingTypes>(0, cardShippingTypes, "GetShippingTypes", AlloyLayerName.BIZ,
				ModuleName.Funds, "End GetShippingTypes - MGI.Biz.FundsEngine.Impl.FundsEngineImpl",
				mgiContext);
			#endregion
			return Mapper.Map<List<MGI.Biz.FundsEngine.Data.ShippingTypes>>(cardShippingTypes);

		}

		public double GetShippingFee(Data.CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
			Cxn.Fund.Data.CardMaintenanceInfo cxnCardMaintenanceInfo = Mapper.Map<Cxn.Fund.Data.CardMaintenanceInfo>(cardMaintenanceInfo);
			double shippingfee = cxnFundsProcessor.GetShippingFee(cxnCardMaintenanceInfo, mgiContext);

			return shippingfee;
		}

        public long AssociateCard(long customerSessionId, BizFundsAccount fundsAccount, MGIContext mgiContext)
        {
            CustomerSession customerSession = GetCustomerSession(customerSessionId);
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);

            CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(a => a.Type == (int)AccountTypes.Funds);

            if (cxeAccount == null)
                cxeAccount = CXEAccountService.AddCustomerFundsAccount(cxeCustomer);

            fundsAccount = MapToFundsAccount(fundsAccount, cxeCustomer, mgiContext);

            IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);

            int providerId = (int)Enum.Parse(typeof(ProviderIds), _GetFundProvider(mgiContext.ChannelPartnerName));

            PTNRAccount ptnrAccount = PTNRCustomerService.Lookup(cxeCustomer.Id).Accounts.FirstOrDefault(a => a.CXEId == cxeAccount.Id & a.ProviderId == providerId);
            
            long cxnFundId = 0;

            CxnFundsAccount cxnFundsAccount = Mapper.Map<BizFundsAccount, CxnFundsAccount>(fundsAccount);
            if (ptnrAccount == null)
            {
                cxnFundId = cxnFundsProcessor.AssociateCard(cxnFundsAccount, mgiContext);
                // add the account to PTNR considering Providerid
                PTNRCustomer ptnrCustomer = PTNRCustomerService.Lookup(cxeCustomer.Id);
                var account = ptnrCustomer.AddAccount(providerId, cxeAccount.Id, cxnFundId);
                cxnFundId = account.Id;
            }
            else
            {
                cxnFundsAccount.Id = ptnrAccount.CXNId;
                cxnFundsProcessor.AssociateCard(cxnFundsAccount, mgiContext);
                cxnFundId = ptnrAccount.CXNId;
            }
            return cxnFundId;
        }

		public double GetFundFee(Data.CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
			Cxn.Fund.Data.CardMaintenanceInfo cxnCardMaintenanceInfo = Mapper.Map<Cxn.Fund.Data.CardMaintenanceInfo>(cardMaintenanceInfo);
			double fundfee = cxnFundsProcessor.GetFundFee(cxnCardMaintenanceInfo, mgiContext);

			return fundfee;
		}

		public long IssueAddOnCard(long customerSessionId, BizFunds funds, MGIContext mgiContext)
		{
			return Transact(customerSessionId, funds, RequestType.AddOnCard, mgiContext);
		}

		#region Private Methods
		//This method should exist in some common area so that every module can use it.
		private CustomerSession GetCustomerSession(long customerSessionId)
		{
			try
			{
				CustomerSession session = CustomerSessionService.Lookup(customerSessionId); // TODO - don't ned the heavy weight object lookup. Must be a way to check session validity based on time. (Important)

				if (session == null)
				{
					throw new BizFundsException(BizFundsException.INVALID_SESSION);
				}

				return session;
			}
			catch (Exception ex)
			{ // need to add appropriate exception here...
				throw new BizFundsException(BizFundsException.INVALID_SESSION, ex);
			}
		}

		private IFundProcessor _GetProcessor(string channelPartner)
		{
			// get the fund processor for the channel partner.
			return (IFundProcessor)ProcessorRouter.GetProcessor(channelPartner);
		}

		//A Method to Get Fund Provider based on ChannelPartner
		private string _GetFundProvider(string channelPartner)
		{
			// get the fund provider for the channel partner.
			return ProcessorRouter.GetProvider(channelPartner);
		}

		private long GetAccountInfo(long customerSessionId, MGIContext mgiContext, out ChannelPartner channelPartner)
		{
			long cxnAccountID;
			CustomerSession customerSession = GetCustomerSession(customerSessionId);

			CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);

			channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

            //AL-5094
			//long cxeAccountID = cxeCustomer.Accounts.First(a => a.Type == (int)AccountTypes.Funds).Id;

            
            //return cxnAccountID = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccountID).CXNId;
            return cxnAccountID = getCXNAccountId(customerSessionId, mgiContext);
		}

		private long _AddCXNAccount(BizFundsAccount fundsAccount, IFundProcessor cxnFundsProcessor,
				MGIContext mgiContext)
		{
			CxnFundsAccount cxnFundsAccount = Mapper.Map<BizFundsAccount, CxnFundsAccount>(fundsAccount);

			ProcessorResult result;

			long accountNumber = cxnFundsProcessor.Register(cxnFundsAccount, mgiContext, out result);

			_HandleProcessorResult(result);

			return accountNumber;
		}

		private static void _HandleProcessorResult(ProcessorResult result)
		{
			if (!result.IsSuccess)
			{
				if (result.Exception != null)
					throw new BizFundsException(BizFundsException.PROVIDER_ERROR, result.Exception);
				else
					throw new BizFundsException(BizFundsException.UNKNOWN, result.Exception);
			}
		}

		private long Transact(long customerSessionId, BizFunds bizFunds,
				RequestType requestType, MGIContext mgiContext)
		{
			_ValidateFundsRequest(bizFunds, requestType);

			CustomerSession customerSession = GetCustomerSession(customerSessionId);

			CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);

			CxeAccount _cxeAccount = cxeCustomer.Accounts.FirstOrDefault(a => a.Type == (int)AccountTypes.Funds);

			_ValidateLimits(customerSession, cxeCustomer, _cxeAccount, requestType, mgiContext, bizFunds.Amount);

			long cxeAccountID = 0;
			if (_cxeAccount != null)
				cxeAccountID = _cxeAccount.Id;

            //AL-5904, AL-2059 
            //long cxnAccountID = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccountID).CXNId;
            long cxnAccountID = getCXNAccountId(customerSessionId, mgiContext);

			if (cxnAccountID <= 0)
				throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			// ---------------------
			// just added this md --
			IFundProcessor cxnFundsProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
			CxnFundsAccount cxnAccount = cxnAccount = cxnFundsProcessor.Lookup(cxnAccountID);
			if (cxnAccount != null && (requestType == RequestType.Credit || requestType == RequestType.Debit))
			{
				ProcessorResult processorResult;
				MGI.Cxn.Fund.Data.CardInfo cardInfo = cxnFundsProcessor.GetBalance(cxnAccountID, mgiContext, out processorResult);
			}

			List<PTNRFunds> transactions = PTNRFundsService.GetAllForCustomer(customerSession.Customer.Id);

			PTNRFee fee = FeeService.GetFundsFee(customerSession, transactions, bizFunds.Amount, (int)requestType, mgiContext);

			if (fee.NetFee != bizFunds.Fee)
				throw new BizFundsException(BizFundsException.FEE_CHANGED);
			long cxeFundsId = 0;

			// first create the CXE trx.
			if (mgiContext.TrxId == 0)
			{
				// first create the CXE trx.
				CxeAccount cxeAccount;
				cxeFundsId = _WriteCXEFunds(bizFunds, customerSession, out cxeAccount);

				if (cxeFundsId <= 0)
					throw new BizFundsException(BizFundsException.STAGE_TRANSACTION_FAILED);

				long cxnFundsId = _SendCxnFunds(cxnFundsProcessor, cxnAccountID, bizFunds, requestType, mgiContext, cxeFundsId);

				if (cxnFundsId <= 0)
					throw new BizFundsException(BizFundsException.STAGE_TRANSACTION_FAILED);

				// now write to PTNR.
				_WritePTNRFunds(bizFunds, fee, customerSession, cxeAccount, cxeFundsId, cxnFundsId, requestType, mgiContext);
			}
			else
			{
				FundType fundType = FundType.None;
				mgiContext.GPRPromoCode = bizFunds.PromoCode;
				cxeFundsId = UpdateAmount(mgiContext.TrxId, bizFunds.Amount, customerSessionId, fundType, mgiContext);
			}
			return cxeFundsId;
		}

		private void _ValidateFundsRequest(BizFunds bizFunds, RequestType requestType)
		{
			if (bizFunds.Amount <= 0 && (requestType == RequestType.Credit || requestType == RequestType.Debit))
				throw new BizFundsException(BizFundsException.INVALID_TRANSACTION_REQUEST);
			if (bizFunds.Amount <= bizFunds.Fee && (requestType == RequestType.Credit || requestType == RequestType.Debit))
				throw new BizFundsException(BizFundsException.INVALID_TRANSACTION_REQUEST);
		}

		private void _ValidateLimits(CustomerSession _customerSession, CxeCustomer _cxeCustomer, CxeAccount _cxeAccount,
			RequestType requestType, MGIContext mgiContext, decimal amount)
		{
			long cxeAccountID = 0;
			if (_cxeAccount != null)
				cxeAccountID = _cxeAccount.Id;

			//AL-5904
			//long cxnAccountID = PTNRCustomerService.Lookup(_cxeCustomer.Id).FindAccountByCXEId(cxeAccountID).CXNId;
            long cxnAccountID = getCXNAccountId(_customerSession.Id, mgiContext);

			if (cxnAccountID <= 0)
				throw new BizFundsException(BizFundsException.ACCOUNT_NOT_FOUND);

			// ---------------------
			// just added this md --
			ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(_customerSession.Customer.ChannelPartnerId);
			IFundProcessor cxnFundsProcessor = _GetProcessor(channelPartner.Name);

			CxnFundsAccount cxnAccount = cxnFundsProcessor.Lookup(cxnAccountID);

			decimal maximumAmount = 0.00M;
			decimal minimumAmount = 0.00M;
			MGI.Cxn.Fund.Data.CardInfo cardInfo;
			var maxException = BizComplianceLimitException.GPR_DEBIT_LIMIT_EXCEEDED;
			TransactionTypes transactionType = TransactionTypes.DebitGPR;
			if ((requestType == RequestType.Credit || requestType == RequestType.Debit))
			{
				ProcessorResult processorResult;

				if (cxnAccount != null)
					cardInfo = cxnFundsProcessor.GetBalance(cxnAccountID, mgiContext, out processorResult);

				if (requestType == RequestType.Credit)
				{
					transactionType = TransactionTypes.LoadToGPR;
					maxException = BizComplianceLimitException.GPR_LOAD_LIMIT_EXCEEDED;
				}
				maximumAmount = LimitService.CalculateTransactionMaximumLimit(_customerSession.Id, channelPartner.ComplianceProgramName, transactionType, mgiContext);

				minimumAmount = LimitService.GetProductMinimum(channelPartner.ComplianceProgramName, transactionType, mgiContext);

				if (amount > maximumAmount)
				{
					throw new BizComplianceLimitException(maxException, maximumAmount);
				}
			}
		}

		private void _WritePTNRFunds(BizFunds bizFunds, PTNRFee fee, CustomerSession customerSession, CxeAccount cxeAccount, long cxeFundsId, long cxnFundsId, RequestType requesttype, MGIContext mgiContext)
		{
            int providerId = (int)Enum.Parse(typeof(ProviderIds), _GetFundProvider(mgiContext.ChannelPartnerName));

            PTNRFunds ptnrFunds = new PTNRFunds
            {
                Id = cxeFundsId,
                CXEId = cxeFundsId,
                CXNId = cxnFundsId,
                Amount = bizFunds.Amount,
                BaseFee = fee.BaseFee,
                DiscountApplied = fee.DiscountApplied,
                AdditionalFee = fee.AdditionalFee,
                Fee = fee.NetFee,
                CustomerSession = customerSession,
                CXEState = (int)TransactionStates.Authorized,
                CXNState = (int)TransactionStates.Authorized,
                //timestamp changes
                DTTerminalCreate = Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                DTServerCreate = DateTime.Now,
                // AL-6023, AL-5904
                //Account = customerSession.Customer.FindAccountByCXEId(cxeAccount.Id),
                Account = customerSession.Customer.FindAccountByCXEId(cxeAccount.Id, providerId),
                FundType = (int)requesttype,
                AddOnCustomerId = mgiContext.AddOnCustomerId
            };

			ptnrFunds.AddFeeAdjustments(fee.Adjustments);

			PTNRFundsService.Create(ptnrFunds);
		}

		private long _SendCxnFunds(IFundProcessor cxnFundsProcessor, long accountId, BizFunds bizFunds, RequestType requestType, MGIContext mgiContext, long cxeFundsId)
		{
			FundRequest cxnFunds = new FundRequest
			{
				Amount = bizFunds.Amount,
				PromoCode = bizFunds.PromoCode
			};

			ProcessorResult processorResult = new ProcessorResult();
			long cxnTransactionId = 0L;

			if (requestType == RequestType.Debit)
			{
				cxnFunds.Amount += bizFunds.Fee;
				cxnTransactionId = cxnFundsProcessor.Withdraw(accountId, cxnFunds, mgiContext, out processorResult);
			}
			else if (requestType == RequestType.Credit)
			{
				cxnFunds.Amount -= bizFunds.Fee;
				cxnTransactionId = cxnFundsProcessor.Load(accountId, cxnFunds, mgiContext, out processorResult);
			}
			else if (requestType == RequestType.None)
			{
				cxnFunds.RequestType = "Activation";
				cxnTransactionId = cxnFundsProcessor.Activate(accountId, cxnFunds, mgiContext, out processorResult);
			}
			else
			{
				cxnFunds.RequestType = ((RequestType)requestType).ToString();
				cxnTransactionId = cxnFundsProcessor.Activate(accountId, cxnFunds, mgiContext, out processorResult);
			}
			_HandleProcessorResult(processorResult);

			return cxnTransactionId;
		}

		private long _WriteCXEFunds(BizFunds bizFunds, CustomerSession customerSession, out CxeAccount cxeAccount)
		{
			CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
			cxeAccount = cxeCustomer.Accounts.Where(x => x.Type == (int)AccountTypes.Funds).First<CxeAccount>();

			CxeFunds cxeFunds = new CxeFunds
			{
				Account = cxeAccount,
				rowguid = Guid.NewGuid(),
				Amount = bizFunds.Amount,
				Fee = bizFunds.Fee,
				//timestamp changes
				DTTerminalCreate = Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
				DTServerCreate = DateTime.Now,
				DTTerminalLastModified = Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
				DTServerLastModified = DateTime.Now,
				Type = (int)RequestType.Debit,
				Status = (int)TransactionStates.Authorized
			};

			return CXEFundsService.Create(cxeFunds);
		}

		private BizFundsAccount MapToFundsAccount(BizFundsAccount fundsAccount, CxeCustomer cxeCustomer, MGIContext mgiContext)
		{
			NexxoIdType idType = NexxoIdTypeService.Find(mgiContext.ChannelPartnerId, cxeCustomer.GovernmentId.IdTypeId);
			fundsAccount.Address1 = cxeCustomer.Address1;
			fundsAccount.Address2 = cxeCustomer.Address2;
			fundsAccount.City = cxeCustomer.City;
			fundsAccount.CountryCode = string.Empty; //cxeCustomer.Profile does not contain country code.
			fundsAccount.DateOfBirth = (DateTime)cxeCustomer.DateOfBirth;
			fundsAccount.FirstName = cxeCustomer.FirstName;
			fundsAccount.MiddleName = cxeCustomer.MiddleName;
			fundsAccount.LastName = cxeCustomer.LastName;
			fundsAccount.GovtIDCountry = idType.CountryId.Abbr2;
			fundsAccount.GovtIDExpiryDate = cxeCustomer.GovernmentId.ExpirationDate == null ? DateTime.MinValue : (DateTime)cxeCustomer.GovernmentId.ExpirationDate;
			fundsAccount.GovtIDIssueDate = cxeCustomer.GovernmentId.IssueDate == null ? DateTime.MinValue : (DateTime)cxeCustomer.GovernmentId.IssueDate;
			fundsAccount.GovtIDIssueState = idType.StateId == null ? string.Empty : idType.StateId.Abbr;
			fundsAccount.GovernmentId = cxeCustomer.GovernmentId.Identification;
			fundsAccount.GovtIDType = idType.Name;
			fundsAccount.SSN = cxeCustomer.SSN;
			fundsAccount.MailingAddress1 = cxeCustomer.MailingAddress1;
			fundsAccount.MailingAddress2 = cxeCustomer.MailingAddress2;
			fundsAccount.MailingCity = cxeCustomer.MailingCity;
			fundsAccount.MailingState = cxeCustomer.MailingState;
			fundsAccount.MailingZipCode = cxeCustomer.MailingZipCode;
			fundsAccount.Phone = cxeCustomer.Phone1;
			fundsAccount.State = cxeCustomer.State;
			fundsAccount.ZipCode = cxeCustomer.ZipCode;
			fundsAccount.IdTypeId = (int)cxeCustomer.GovernmentId.IdTypeId;
			fundsAccount.IDCode = cxeCustomer.SSN == null ? string.Empty : cxeCustomer.IDCode;
			//AL-2999 Changes
			fundsAccount.Email = cxeCustomer.Email;
			//AL-3054
			fundsAccount.MothersMaidenName = cxeCustomer.MothersMaidenName == null ? string.Empty : cxeCustomer.MothersMaidenName;

			return fundsAccount;
		}

		private string GetRequestType(string requestType)
		{
			switch(requestType)
			{
				case "None" :
					requestType= "Activation";
					break;
				case "Debit":
					requestType = Convert.ToString(RequestType.Debit);
					break;
				case "Credit":
					requestType = Convert.ToString(RequestType.Credit);
					break;
			}
			return requestType;
		}

		#endregion
	}
}
