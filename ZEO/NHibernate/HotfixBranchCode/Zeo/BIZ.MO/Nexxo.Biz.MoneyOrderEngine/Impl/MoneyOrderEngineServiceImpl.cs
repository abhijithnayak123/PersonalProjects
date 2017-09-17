using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Engine;

using MGI.Common.Util;

using MGI.Biz.MoneyOrderEngine.Contract;
using MGI.Biz.MoneyOrderEngine.Data;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Fees;
using MGI.Core.Partner.Contract;
using CXEContract = MGI.Core.CXE.Contract;
using CXEData = MGI.Core.CXE.Data;

using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;

using BizCommon = MGI.Biz.Common.Data;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;

using AutoMapper;
using Spring.Transaction.Interceptor;
using MGI.Core.CXE.Data;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Biz.MoneyOrderEngine.Impl
{
	public class MoneyOrderEngineServiceImpl : IMoneyOrderEngineService
	{
		#region Dependencies

		private PTNRContract.IChannelPartnerService _channelPartnerSvc;
		public PTNRContract.IChannelPartnerService ChannelPartnerSvc
		{
			get { return _channelPartnerSvc; }
			set { _channelPartnerSvc = value; }
		}

		private PTNRContract.ICustomerSessionService _sessionSvc;
		public PTNRContract.ICustomerSessionService SessionSvc
		{
			get { return _sessionSvc; }
			set { _sessionSvc = value; }
		}

		private CXEContract.ICustomerService _cxeCustSvc;
		public CXEContract.ICustomerService CxeCustSvc
		{
			get { return _cxeCustSvc; }
			set { _cxeCustSvc = value; }
		}

		private CXEContract.IAccountService _acctSvc;
		public CXEContract.IAccountService AcctSvc
		{
			get { return _acctSvc; }
			set { _acctSvc = value; }
		}

		private CXEContract.IMoneyOrderService _cxeMoneyOrderSvc;
		public CXEContract.IMoneyOrderService CxeMoneyOrderSvc
		{
			get { return _cxeMoneyOrderSvc; }
			set { _cxeMoneyOrderSvc = value; }
		}

		private PTNRContract.ITransactionService<PTNRData.Transactions.MoneyOrder> _ptnrMoneyOrderSvc;
		public PTNRContract.ITransactionService<PTNRData.Transactions.MoneyOrder> PTNRMoneyOrderSvc
		{
			get { return _ptnrMoneyOrderSvc; }
			set { _ptnrMoneyOrderSvc = value; }
		}

		private PTNRContract.IFeeService _feeSvc;
		public PTNRContract.IFeeService FeeSvc
		{
			get { return _feeSvc; }
			set { _feeSvc = value; }
		}

		public PTNRContract.IMoneyOrderImage MoneyOrderImageSvc { private get; set; }

		public PTNRContract.ICustomerService CustomerService { private get; set; }

		private ILimitService _limitService;
		public ILimitService LimitService { set { _limitService = value; } }

		public PTNRContract.INexxoDataStructuresService NexxoIdTypeService { private get; set; }

		private MoneyOrderCheckPrintTemplateRepo _checkPrintRepo;
		public MoneyOrderCheckPrintTemplateRepo CheckPrintRepo
		{
			get { return _checkPrintRepo; }
			set { _checkPrintRepo = value; }
		}

		public PTNRContract.ICustomerFeeAdjustmentService CustomerFeeAdjustmentService { private get; set; }

		// AL-591: This is introduced to update IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
		// Developed by: Sunil Shetty || 03/07/2015
		public PTNRContract.IFeeAdjustmentService FeeAdjustmentService { private get; set; }

        public bool AllowDuplicateMoneyOrder { get; set; }

		public TLoggerCommon MongoDBLogger { get; set; }
		#endregion

		public MoneyOrderEngineServiceImpl()
		{
			Mapper.CreateMap<TransactionFee, BizCommon.TransactionFee>();
			Mapper.CreateMap<PTNRData.Transactions.MoneyOrder, MoneyOrder>()
				.ForMember(r => r.Id, o => o.MapFrom(s => s.Id.ToString()))
				.ForMember(r => r.PurchaseDate, o => o.MapFrom(s => s.DTTerminalCreate))
				.ForMember(r => r.Status, o => o.MapFrom(s => s.CXEState.ToString()));
		}

		#region IMoneyOrderEngineService methods
		public BizCommon.TransactionFee GetMoneyOrderFee(long customerSessionId, MoneyOrderData moneyOrder, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<MoneyOrderData>(customerSessionId, moneyOrder, "GetMoneyOrderFee", AlloyLayerName.BIZ,
					ModuleName.MoneyOrder, "Begin GetMoneyOrderFee - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl", 
					mgiContext);
			#endregion

			CustomerSession customerSession = _sessionSvc.Lookup(customerSessionId);
			List<PTNRData.Transactions.MoneyOrder> transactions = _ptnrMoneyOrderSvc.GetAllForCustomer(customerSession.Customer.Id);

			//US2030
			removeCancelParkedTransactions(customerSession, ref transactions);
			//US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
			//added context for PromotionCode,IsSystemApplied
			mgiContext.PromotionCode = moneyOrder.PromotionCode;
			mgiContext.IsSystemApplied =  moneyOrder.IsSystemApplied;


			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.ListInfo<PTNRData.Transactions.MoneyOrder>(customerSessionId, transactions, "GetMoneyOrderFee", AlloyLayerName.BIZ,
					ModuleName.MoneyOrder, "End GetMoneyOrderFee - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
					mgiContext);
			#endregion

			return Mapper.Map<BizCommon.TransactionFee>(_feeSvc.GetMoneyOrderFee(customerSession, transactions, moneyOrder.Amount, mgiContext));
		}

		public MoneyOrder Add(long customerSessionId, MoneyOrderPurchase moneyOrderPurchase, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<MoneyOrderPurchase>(customerSessionId, moneyOrderPurchase, "Add", AlloyLayerName.BIZ,
					ModuleName.MoneyOrder, "Begin MoneyOrder Add - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
					mgiContext);
			#endregion
			
			// retrieve session and customer account info, which contains agent, location and channel partner info
			PTNRData.CustomerSession session = _sessionSvc.Lookup(customerSessionId);

			// get the moneyOrder account
			CXEData.Account moneyOrderAccount = GetMoneyOrderAccount(session.Customer);

			// stage the moneyOrder in CXE
			CXEData.Transactions.Stage.MoneyOrder cxeMoneyOrder = new CXEData.Transactions.Stage.MoneyOrder
			{
				Amount = moneyOrderPurchase.Amount,
				Fee = moneyOrderPurchase.Fee,
				Account = moneyOrderAccount,
				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(session.AgentSession.Terminal.Location.TimezoneID),
				DTServerCreate = DateTime.Now,
				Status = (int)CXEData.TransactionStates.Initiated,
				PurchaseDate = DateTime.Now,

			};
			//Add into CXE and Get CXEID
			long cxeMoneyOrderId = _cxeMoneyOrderSvc.Create(cxeMoneyOrder, mgiContext.TimeZone);

			ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig(session.Customer.ChannelPartnerId);

			long providerId = GetProviderId(channelPartner.Id);

			decimal minimumAmount = _limitService.GetProductMinimum(channelPartner.ComplianceProgramName, TransactionTypes.MoneyOrder, mgiContext);
			decimal maximumAmount = _limitService.CalculateTransactionMaximumLimit(customerSessionId, channelPartner.ComplianceProgramName, TransactionTypes.MoneyOrder, mgiContext);

			if (moneyOrderPurchase.Amount < minimumAmount)
			{
				throw new BizComplianceLimitException(BizComplianceLimitException.MONEY_ORDER_MINIMUM_LIMIT_CHECK, minimumAmount);
			}
			if (moneyOrderPurchase.Amount > maximumAmount)
			{
				throw new BizComplianceLimitException(BizComplianceLimitException.MONEY_ORDER_LIMIT_EXCEEDED, maximumAmount);
			}

			//If Amount limit Check pass update with Authorized Status
			_cxeMoneyOrderSvc.Update(cxeMoneyOrderId, (int)CXEData.TransactionStates.Authorized, mgiContext.TimeZone);

			List<PTNRData.Transactions.MoneyOrder> transactions = _ptnrMoneyOrderSvc.GetAllForCustomer(session.Customer.Id);

			//US2030
			removeCancelParkedTransactions(session, ref transactions);
			//US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
			//added context for PromotionCode,IsSystemApplied
			mgiContext.PromotionCode = moneyOrderPurchase.PromotionCode;
			mgiContext.IsSystemApplied = moneyOrderPurchase.IsSystemApplied;
			PTNRData.TransactionFee fee = _feeSvc.GetMoneyOrderFee(session, transactions, moneyOrderPurchase.Amount, mgiContext);

			//if (fee.NetFee != moneyOrderPurchase.Fee)
			//    throw new BizMoneyOrderEngineException(BizMoneyOrderEngineException.FEE_CHANGED);

			//Adding Data to Transaction
			PTNRData.Transactions.MoneyOrder ptnrMoneyOrder = new PTNRData.Transactions.MoneyOrder
			{
				Id = cxeMoneyOrderId,
				CXEId = cxeMoneyOrderId,
				Amount = moneyOrderPurchase.Amount,
				BaseFee = fee.BaseFee,
				DiscountApplied = fee.DiscountApplied,
				AdditionalFee = fee.AdditionalFee,
                Fee = fee.NetFee,
                CustomerSession = session,
                CXEState = (int)CXEData.TransactionStates.Authorized,
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(session.AgentSession.Terminal.Location.TimezoneID),
                DTServerCreate = DateTime.Now,
                Account = session.Customer.FindAccountByCXEId(moneyOrderAccount.Id),
				DiscountName = fee.DiscountName ,
				DiscountDescription = fee.DiscountDescription,
				IsSystemApplied = fee.IsSystemApplied
			};

			// add any fee adjustments that should be tracked with this transaction
			ptnrMoneyOrder.AddFeeAdjustments(fee.Adjustments);

			_ptnrMoneyOrderSvc.Create(ptnrMoneyOrder);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<PTNRData.Transactions.MoneyOrder>(customerSessionId, ptnrMoneyOrder, "Add", AlloyLayerName.BIZ,
					ModuleName.MoneyOrder, "End MoneyOrder Add - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
					mgiContext);
			#endregion

			return Mapper.Map<MoneyOrder>(ptnrMoneyOrder);
		}

		public void UpdateMoneyOrderStatus(long customerSessionId, long moneyOrderId, int transactionStatus, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			List<string> details = new List<string>();
			details.Add("Money Order Id: " + Convert.ToString(moneyOrderId));
			details.Add("Transaction Status: " + Convert.ToString(transactionStatus));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "UpdateMoneyOrderStatus", AlloyLayerName.BIZ,
					ModuleName.MoneyOrder, "Begin UpdateMoneyOrderStatus - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
					mgiContext);
			#endregion
			
			CXEData.TransactionStates cxeStatus = (CXEData.TransactionStates)transactionStatus;

			// set to final status in cxe
			_cxeMoneyOrderSvc.Update(moneyOrderId, (int)cxeStatus, mgiContext.TimeZone);

			// update ptnr record
			_ptnrMoneyOrderSvc.UpdateCXEStatus(moneyOrderId, (int)cxeStatus);

			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrderId : " + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "UpdateMoneyOrderStatus", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "End UpdateMoneyOrderStatus - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion
		}

		public void UpdateMoneyOrder(long customerSessionId, MoneyOrder moneyOrder, long moneyOrderId, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<MoneyOrder>(customerSessionId, moneyOrder, "UpdateMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "Begin UpdateMoneyOrder - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion
			
			_cxeMoneyOrderSvc.Update(moneyOrderId, moneyOrder.CheckNumber, moneyOrder.AccountNumber, moneyOrder.RoutingNumber, moneyOrder.MICR, mgiContext.TimeZone);

			PTNRData.Transactions.MoneyOrder ptnrMoneyOrder = _ptnrMoneyOrderSvc.Lookup(moneyOrderId);
			ptnrMoneyOrder.CheckNumber = moneyOrder.CheckNumber;
			ptnrMoneyOrder.AccountNumber = moneyOrder.AccountNumber;
			ptnrMoneyOrder.RoutingNumber = moneyOrder.RoutingNumber;

			IsMoneyOrderAlreadyCommited(customerSessionId, moneyOrder.MICR, mgiContext);//AL-722

			_ptnrMoneyOrderSvc.Update(ptnrMoneyOrder);

			MoneyOrderImage moneyOrderImage = MoneyOrderImageSvc.FindMoneyOrderByTxnId(ptnrMoneyOrder.rowguid);

			// set to Money Order check images in ptnr
			if (moneyOrderImage != null)
			{
				moneyOrderImage.CheckFrontImage = moneyOrder.FrontImage;
				moneyOrderImage.CheckBackImage = moneyOrder.BackImage;

				MoneyOrderImageSvc.Update(moneyOrderImage, mgiContext.TimeZone);
			}
			else
			{
				MoneyOrderImage moneyOrderImg = new MoneyOrderImage()
				{
					CheckFrontImage = moneyOrder.FrontImage,
					CheckBackImage = moneyOrder.BackImage,
					TrxId = ptnrMoneyOrder.rowguid
				};

				MoneyOrderImageSvc.Create(moneyOrderImg, mgiContext.TimeZone);
			}

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<PTNRData.Transactions.MoneyOrder>(customerSessionId, ptnrMoneyOrder, "UpdateMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "End UpdateMoneyOrder - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion
		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void Commit(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrderId : " + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "Commit", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "Begin Commit MoneyOrder - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion

			// update stage moneyOrder
			_cxeMoneyOrderSvc.Update(moneyOrderId, (int)CXEData.TransactionStates.Committed, mgiContext.TimeZone);

			// commit moneyOrder in cxe
			_cxeMoneyOrderSvc.Commit(moneyOrderId);

			CXEData.Transactions.Stage.MoneyOrder cxeMoneyOrder = _cxeMoneyOrderSvc.GetStage(moneyOrderId);

			string description = string.Format("MoneyOrder {0}", cxeMoneyOrder.MoneyOrderCheckNumber);

			// update the partner transaction
			_ptnrMoneyOrderSvc.UpdateStates(moneyOrderId, (int)CXEData.TransactionStates.Committed, 0, description);

			//US1800 Referral Referee Promotions			

			addUpdateCustomerFeeAdjustment(moneyOrderId, mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<CXEData.Transactions.Stage.MoneyOrder>(customerSessionId, cxeMoneyOrder, "Commit", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "End Commit MoneyOrder - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion

		}

		public MoneyOrderCheckPrint GetMoneyOrderCheck(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrderId : " + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "GetMoneyOrderCheck", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "Begin GetMoneyOrderCheck - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion

			PTNRData.Transactions.MoneyOrder moneyOrder = _ptnrMoneyOrderSvc.Lookup(moneyOrderId);

			Terminal terminal = moneyOrder.CustomerSession.AgentSession.Terminal;

			string checkPrintContents = _checkPrintRepo.GetCheckPrintTemplate(GetChannelPartner(terminal), PTNRData.Transactions.TransactionType.MoneyOrder, ProviderIds.Continental, string.Empty);

			checkPrintContents = ReplaceTags(GetTrxMoneyOrderTags(moneyOrder), checkPrintContents);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<PTNRData.Transactions.MoneyOrder>(customerSessionId, moneyOrder, "GetMoneyOrderCheck", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "End GetMoneyOrderCheck - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion

			return GetcheckPrint(checkPrintContents);
		}

        public MoneyOrderCheckPrint GetMoneyOrderDiagnostics(long agentSessionId, MGIContext mgiContext)
		{
			string moneyOrderPrintContents = _checkPrintRepo.GetMoneyOrderDiagnosticsTemplate();

			Dictionary<string, string> tags = new Dictionary<string, string>()
	        {
				{"{USDollars}", ("***USDOLLARS***")},
				{"{TransactionDate}", DateTime.Now.ToString("MM/dd/yyyy")},
				{"{TransactionAmount}", "$ 0.00" },
				{"{TransactionAmountInWords}", "THIS IS A PRINT TEST VOID CHECK"}
	        };

			moneyOrderPrintContents = ReplaceTags(tags, moneyOrderPrintContents);

			return GetcheckPrint(moneyOrderPrintContents);
		}

		public MoneyOrder GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrderId : " + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "GetMoneyOrderStage", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "Begin GetMoneyOrderStage - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion
			
			CXEData.Transactions.Stage.MoneyOrder moneyOrderStage = _cxeMoneyOrderSvc.GetStage(moneyOrderId);

			MoneyOrder moneyOrder = new MoneyOrder
			{
				Id = Convert.ToString(moneyOrderStage.Id),
				Amount = moneyOrderStage.Amount,
				CheckNumber = moneyOrderStage.MoneyOrderCheckNumber,
				AccountNumber = moneyOrderStage.AccountNumber,
				RoutingNumber = moneyOrderStage.RoutingNumber,
				Fee = moneyOrderStage.Fee,
				Status = Convert.ToString(moneyOrderStage.Status),
			};

			var ptnrMO = _ptnrMoneyOrderSvc.Lookup(moneyOrderId);

			moneyOrder.BaseFee = ptnrMO.BaseFee;
			moneyOrder.DiscountName = ptnrMO.DiscountName;
			moneyOrder.DiscountApplied = ptnrMO.DiscountApplied;

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<CXEData.Transactions.Stage.MoneyOrder>(customerSessionId, moneyOrderStage, "GetMoneyOrderStage", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "End GetMoneyOrderStage - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion

			return moneyOrder;
		}
			
		public bool Resubmit(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			string id = "Check Id : " + Convert.ToString(checkId);

			MongoDBLogger.Info<string>(customerSessionId, id, "Resubmit", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "Begin MoneyOrder Resubmit - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion
			
			//US2030 
			CustomerSession session = _sessionSvc.Lookup(customerSessionId);
			var location = session.AgentSession.Terminal.Location;

			PTNRData.Transactions.MoneyOrder ptnrMO = _ptnrMoneyOrderSvc.Lookup(checkId);

			updatePartnerNextMOFee(session, ref ptnrMO, mgiContext);

			// update cxe record
			_cxeMoneyOrderSvc.Update(ptnrMO.CXEId, ptnrMO.Fee, location.TimezoneID);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<PTNRData.Transactions.MoneyOrder>(customerSessionId, ptnrMO, "Resubmit", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "End MoneyOrder Resubmit - MGI.Biz.MoneyOrderEngine.Impl.MoneyOrderEngineServiceImpl",
				mgiContext);
			#endregion

			return true;
		}

		#endregion

		#region Private methods

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
		/// Updating Customer Promotions if customer availed Referal promotions
		/// </summary>
		/// <param name="moneyOrderId"></param>
		/// <param name="context"></param>
		private void addUpdateCustomerFeeAdjustment(long moneyOrderId, MGIContext mgiContext)
		{
			MGI.Core.Partner.Data.Transactions.MoneyOrder ptnrMO = _ptnrMoneyOrderSvc.Lookup(moneyOrderId);

			CustomerSession customerSession = ptnrMO.CustomerSession;

			foreach (MGI.Core.Partner.Data.Transactions.TransactionFeeAdjustment adj in ptnrMO.FeeAdjustments)
			{
				if (adj.feeAdjustment.PromotionType != null && adj.feeAdjustment.PromotionType.ToLower() == PromotionType.Referral.ToString().ToLower())
				{
					var customerFeeAdjustments = CustomerFeeAdjustmentService.lookup(customerSession.Customer.CXEId, adj.feeAdjustment);
					if (customerFeeAdjustments != null)
					{
						customerFeeAdjustments.IsAvailed = true;
						customerFeeAdjustments.DTServerLastModified = DateTime.Now;
						customerFeeAdjustments.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
						CustomerFeeAdjustmentService.Update(customerFeeAdjustments);
					}
				}
			}
		}

		private void updatePartnerNextMOFee(CustomerSession session, ref PTNRData.Transactions.MoneyOrder ptnrMO, MGIContext mgiContext)
		{
			List<PTNRData.Transactions.MoneyOrder> transactions = _ptnrMoneyOrderSvc.GetAllForCustomer(session.Customer.Id);

			removeCancelParkedTransactions(session, ref transactions);
			// dont't include the upcoming transactions 
			// don't include this check in fee calculation
			var id = ptnrMO.Id;
			transactions.RemoveAll(x => x.Id >= id && x.CXEState != (int)PTNRTransactionStates.Committed);

			mgiContext.IsSystemApplied = ptnrMO.IsSystemApplied;

			//US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
			//added context for PromotionCode,IsSystemApplied
			if (ptnrMO.DiscountName != null)
			{
				mgiContext.PromotionCode = ptnrMO.DiscountName;
			}
			
			TransactionFee fee = _feeSvc.GetMoneyOrderFee(session, transactions, ptnrMO.Amount, mgiContext);

			ptnrMO.BaseFee = fee.BaseFee;
			ptnrMO.DiscountApplied = fee.DiscountApplied;
			ptnrMO.AdditionalFee = fee.AdditionalFee; //DE3352 For Surcharge
			ptnrMO.Fee = fee.NetFee;
			// AL-591: This is introduced to update IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
			// Developed by: Sunil Shetty || 03/07/2015
			FeeAdjustmentService.DeleteFeeAdjustments(ptnrMO.rowguid);
			ptnrMO.AddFeeAdjustments(fee.Adjustments);

			ptnrMO.DiscountName = fee.DiscountName;
			ptnrMO.DiscountDescription = fee.DiscountDescription;
			ptnrMO.IsSystemApplied = fee.IsSystemApplied;
		}

		private void removeCancelParkedTransactions(CustomerSession session, ref List<PTNRData.Transactions.MoneyOrder> transactions)
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

		private CXEData.Account GetMoneyOrderAccount(PTNRData.Customer ptnrCustomer)
		{
			//get the CXE customer
			CXEData.Customer customer = _cxeCustSvc.Lookup(ptnrCustomer.CXEId);

			ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig(ptnrCustomer.ChannelPartnerId);

			int providerId = GetProviderId(channelPartner.Id);
			PTNRData.Account ptnrMoneyOrderAcct = ptnrCustomer.GetAccount(providerId);

			CXEData.Account cxeMoneyOrderAccount;
			if (ptnrMoneyOrderAcct == null)
			{
				// create CXE account
				cxeMoneyOrderAccount = _acctSvc.AddCustomerMoneyOrderAccount(customer);

				// create PTNR account
				ptnrMoneyOrderAcct = ptnrCustomer.AddAccount(providerId, cxeMoneyOrderAccount.Id);


				NexxoIdType idType = null;
				if (customer.GovernmentId != null)
					idType = NexxoIdTypeService.Find(channelPartner.Id, customer.GovernmentId.IdTypeId);

				// not sure if needed (to update the ptnrCheckAcct.CXNId
				CustomerService.Update(ptnrCustomer);
			}
			else
				cxeMoneyOrderAccount = customer.GetAccount(ptnrMoneyOrderAcct.CXEId);

			return cxeMoneyOrderAccount;
		}

		private string GetChannelPartner(Terminal terminal)
		{
			//extra lookupsneeded to work a bad NHibernate class setup
			PTNRData.ChannelPartner p = _channelPartnerSvc.ChannelPartnerConfig(terminal.Location.ChannelPartnerId);
			return p.Name;
		}

		private MoneyOrderCheckPrint GetcheckPrint(string checkPrintContent)
		{
			List<string> checkPrintContents = new List<string>(checkPrintContent.Split(new char[] { '\n' }));
			checkPrintContents = checkPrintContents.Where(x => (x.Length > 0)).ToList<string>();
			return new MoneyOrderCheckPrint
			{
				Lines = checkPrintContents
			};
		}

		private string ReplaceTags(Dictionary<string, string> tags, string checkPrintContents)
		{
			foreach (KeyValuePair<string, string> tag in tags)
			{
				checkPrintContents = checkPrintContents.Replace(tag.Key, tag.Value);
			}
			return checkPrintContents;
		}

		private Dictionary<string, string> GetTrxMoneyOrderTags(PTNRData.Transactions.MoneyOrder moneyOrder)
		{
			CXEData.Transactions.Stage.MoneyOrder cxeMoneyOrder = _cxeMoneyOrderSvc.GetStage(moneyOrder.Id);

			var account = moneyOrder.CXEId.ToString();
			string customerId = moneyOrder.CustomerSession.Customer.Id.ToString();
			string alloyId = (customerId.Length > 4 ? customerId.Substring(customerId.Length - 4) : customerId);
			string tellerNumber = Convert.ToString(moneyOrder.CustomerSession.AgentSession.Agent.ClientAgentIdentifier);
			string referenceNumber = string.Format("******{0} {1}", alloyId, tellerNumber);

			Dictionary<string, string> tags = new Dictionary<string, string>()
	        {
                {"{Amount}",GetFormattedAmount(moneyOrder.Amount) },
	            {"{AmountInWords}", GetAmountInWord(moneyOrder.Amount)},
	            {"{Date}", moneyOrder.DTTerminalCreate.ToString("MM/dd/yyyy") },
                {"{TransactionId}",Convert.ToString(moneyOrder.Id)},
                {"{MoneyOrderCheckNumber}",Convert.ToString(cxeMoneyOrder.MoneyOrderCheckNumber)},
				{"{MoneyOerderAccountNumber}", Convert.ToString(cxeMoneyOrder.AccountNumber)},
				{"{MoneyOrderRoutingNumber}", Convert.ToString(cxeMoneyOrder.RoutingNumber)},

				{"{USDollars}", ("***USDOLLARS***")},
				{"{TransactionReferenceNo}", referenceNumber},
				{"{TransactionDate}", moneyOrder.DTTerminalCreate.ToString("MMMM dd yyyy")},
				{"{TransactionAmount}", GetFormattedAmountForTCF(moneyOrder.Amount) },
				{"{TransactionAmountInWords}", GetAmountInWordForTCF(moneyOrder.Amount)}
	        };
			return tags;
		}

		private string GetFormattedAmountForTCF(decimal amount)
		{
			return (amount.ToString("N2").PadLeft(13, '$')) + "******";
		}

		private string GetAmountInWordForTCF(decimal amount)
		{
			return NexxoUtil.AmountToStringForTCF((double)amount);
		}

		private string GetAmountInWord(decimal Amount)
		{
			return NexxoUtil.AmountToString((double)Amount, "dollar", "cent");
		}

		private string GetFormattedAmount(decimal Amount)
		{
			string[] amountParts = Amount.ToString("0.00").Split('.');

			string formattedAmount = "$" + amountParts[0] + "." + amountParts[1];

			return formattedAmount;
		}

		private int GetProviderId(long channelPartnerId)
		{
			int providerId = 0;

			//TODO: This logic to be replaced once Channel partner VS Provider mapping is implemented 
			switch (channelPartnerId)
			{
				case 34://TCF
					providerId = (int)ProviderIds.TCF;
					break;
				case 1://MGI
					providerId = (int)ProviderIds.MGIMoneyOrder;
					break;
				default:
					providerId = (int)ProviderIds.Continental;
					break;
			}
			return providerId;
		}

        private void IsMoneyOrderAlreadyCommited(long customerSessionId, string MICR, MGIContext mgiContext)
        {
            if (AllowDuplicateMoneyOrder)
                return;

            IList<CXEData.Transactions.Commit.MoneyOrder> moneyOrderCommits = _cxeMoneyOrderSvc.GetMOByMICR(MICR);

            if (moneyOrderCommits.Count() > 0)
            {
                if(moneyOrderCommits.Any(x=>x.Account.Customer.ChannelPartnerId == mgiContext.ChannelPartnerId))
                {
                    throw new BizMoneyOrderEngineException(BizMoneyOrderEngineException.MONEYORDER_COMMIT_ALREADY_EXIST); 
                }
            }
        }
		#endregion
	}
}
