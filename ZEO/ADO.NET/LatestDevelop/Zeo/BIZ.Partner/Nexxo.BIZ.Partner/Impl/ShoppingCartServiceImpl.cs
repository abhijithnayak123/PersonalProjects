using AutoMapper;
using MGI.Biz.BillPay.Contract;
using MGI.Biz.CashEngine.Contract;
using MGI.Biz.CPEngine.Contract;
using MGI.Biz.FundsEngine.Contract;
using MGI.Biz.FundsEngine.Data;
using MGI.Biz.MoneyOrderEngine.Contract;
using MGI.Biz.MoneyTransfer.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using Spring.Transaction.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using BillPayDTO = MGI.Biz.Partner.Data.Transactions.BillPay;
using CashDTO = MGI.Biz.Partner.Data.Transactions.Cash;
using CashTransactionTypeDTO = MGI.Biz.CashEngine.Data.CashTransactionType;
using ChannelPartnerService = MGI.Biz.Partner.Contract.IChannelPartnerService;
using CheckDTO = MGI.Biz.Partner.Data.Transactions.Check;
using CXNFundsType = MGI.Cxn.Fund.Data.RequestType;
using FundsDTO = MGI.Biz.Partner.Data.Transactions.Funds;
using ModifyRequestDTO = MGI.Biz.MoneyTransfer.Data.ModifyRequest;
using MoneyOrderDTO = MGI.Biz.Partner.Data.Transactions.MoneyOrder;
using MoneyOrderEngineDTO = MGI.Biz.MoneyOrderEngine.Data.MoneyOrder;
using MoneyTransferDTO = MGI.Biz.Partner.Data.Transactions.MoneyTransfer;
using pBillPay = MGI.Core.Partner.Data.Transactions.BillPay;
using pCash = MGI.Core.Partner.Data.Transactions.Cash;
using pCheck = MGI.Core.Partner.Data.Transactions.Check;
using pFunds = MGI.Core.Partner.Data.Transactions.Funds;
using pMoneyOrder = MGI.Core.Partner.Data.Transactions.MoneyOrder;
using pMoneyTransfer = MGI.Core.Partner.Data.Transactions.MoneyTransfer;
using pShoppingCart = MGI.Core.Partner.Data.ShoppingCart;
using pTransaction = MGI.Core.Partner.Data.Transactions.Transaction;
using ReceiptDTO = MGI.Biz.Partner.Data.Receipt;
using CartFlush = MGI.Biz.Partner.Data.CartFlush;
using RequestTypeDTO = MGI.Biz.Partner.Data.RequestType;
using ShoppingCartCheckoutStatusDTO = MGI.Biz.Partner.Data.ShoppingCartCheckoutStatus;
using ShoppingCartDTO = MGI.Biz.Partner.Data.ShoppingCart;
using TransactionDTO = MGI.Biz.Partner.Data.Transactions.Transaction;
using TransactionSubTypeDTO = MGI.Biz.MoneyTransfer.Data.TransactionSubType;
using TransferTypeDTO = MGI.Biz.MoneyTransfer.Data.TransferType;
using BizTransactionRequest = MGI.Biz.MoneyTransfer.Data.TransactionRequest;
using CXNCustomerTransactionDetails = MGI.Cxn.Partner.TCF.Data.CustomerTransactionDetails;
using CXNTransaction = MGI.Cxn.Partner.TCF.Data.Transaction;
using ICXECustomerService = MGI.Core.CXE.Contract.ICustomerService;
using MoneyTransferTransactionDTO = MGI.Biz.MoneyTransfer.Data.MoneyTransferTransaction;
using BillPayTransactionDTO = MGI.Biz.BillPay.Data.BillPayTransaction;
using CheckTransactionDTO = MGI.Biz.CPEngine.Data.CheckTransaction;
using MoneyOrderTransactionDTO = MGI.Biz.MoneyOrderEngine.Data.MoneyOrder;
using FundsTransactionDTO = MGI.Biz.FundsEngine.Data.Funds;
using BizPartnerData = MGI.Biz.Partner.Data;
using CXNCustomer = MGI.Cxn.Partner.TCF.Data.Customer;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using CXEEmploymentDetails = MGI.Core.CXE.Data.CustomerEmploymentDetails;
using CxnFundsAccount = MGI.Cxn.Fund.Data.CardAccount;
using CxnCustomerProfile = MGI.Cxn.Customer.Data.CustomerProfile;
using MGI.Biz.Events.Contract;
using MGI.Core.Partner.Data.Fees;
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Common.Processor.Util;
using IPtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using PTNRChannelPartners = MGI.Biz.Partner.Data.ChannelPartners;
using MGI.Cxn.Check.Contract;
using MGI.Cxn.Check.Chexar.Data;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;
using MGI.Biz.MoneyOrderEngine.Data;
using MGI.Biz.Partner.Contract;


namespace MGI.Biz.Partner.Impl
{
	public class ShoppingCartServiceImpl : MGI.Biz.Partner.Contract.IShoppingCartService
	{
		public ChannelPartnerService ChannelPartnerService { get; set; }

		#region Depedencies

		public ICPEngineService CPEngineService { private get; set; }
		public IMoneyTransferEngine MoneyTransferEngine { private get; set; }
		public IFundsEngine FundsEngine { private get; set; }
		public ICashEngine CashEngine { private get; set; }
		public IMoneyOrderEngineService MoneyOrderEngineService { private get; set; }
		public IBillPayService BillPayService { private get; set; }
		public ICustomerSessionService CustomerSessionService { private get; set; }
		public ITransactionService<pCheck> CheckTransactionService { private get; set; }
		public ITransactionService<pFunds> FundsTransactionService { private get; set; }
		public ITransactionService<pBillPay> BillPayTransactionService { private get; set; }
		public ITransactionService<pMoneyOrder> MoneyOrderTransactionService { private get; set; }
		public ITransactionService<pMoneyTransfer> MoneyTransferTransactionService { private get; set; }
		public ITransactionService<pCash> CashTransactionService { private get; set; }
		public MGI.Core.Partner.Contract.IShoppingCartService ShoppingCartSvc { private get; set; }
		public ICustomerService CustomerService { private get; set; }
		public MGI.Core.Partner.Contract.IMessageCenter MessageCenterService { private get; set; }
		public MGI.Core.Partner.Contract.IManageUsers ManageUserService { private get; set; }
		public MGI.Core.Partner.Contract.INexxoDataStructuresService PTNRDataStructureService { private get; set; }
		public IProcessorRouter ProcessorRouter { private get; set; }
		public ICXECustomerService CXECustomerService { private get; set; }
		public INexxoBizEventPublisher EventPublisher { private get; set; }
		public MGI.Core.Partner.Contract.IChannelPartnerService CoreChannelPartnerService { private get; set; }
		public MGI.Core.Partner.Contract.IFeeAdjustmentService PTNRFeeAdjustmentService { get; set; }
		public MGI.Core.Partner.Contract.ICustomerFeeAdjustmentService CustomerFeeAdjustmentService { private get; set; }
		public MGI.Core.Partner.Contract.ITransactionService<MGI.Core.Partner.Data.Transactions.Funds> PtnrFundsSvc { private get; set; }
		public MGI.Cxn.Customer.Contract.IClientCustomerService CxnClientCustomerService { private get; set; }
		public IPtnrCustomerService PartnerCustomerService { private get; set; }
		public IProcessorRouter CustomerRouter { private get; set; }
		private MGI.Core.Partner.Contract.IChannelPartnerService _ptnrSvc;
		public MGI.Core.Partner.Contract.IChannelPartnerService PtnrSvc { set { _ptnrSvc = value; } } // duplicate
		public ProcessorRouter CheckProcessorRouter { private get; set; }
		//AL-594 Change
		public ILimitService LimitService { private get; set; }
		public MGI.Common.Util.TLoggerCommon MongoDBLogger { private get; set; }
		#endregion

		public ShoppingCartServiceImpl()
		{
			Mapper.CreateMap<pCheck, CheckDTO>()
				.ForMember(x => x.CustomerSessionId, opt => opt.MapFrom(c => c.CustomerSession.Id));
			Mapper.CreateMap<pFunds, FundsDTO>()
				.ForMember(x => x.TransactionType, opt => opt.MapFrom(c => Enum.Parse(typeof(MGI.Cxn.Fund.Data.RequestType), c.FundType.ToString())))
				.ForMember(x => x.CustomerSessionId, opt => opt.MapFrom(c => c.CustomerSession.Id));
			Mapper.CreateMap<pBillPay, BillPayDTO>()
				.ForMember(x => x.CustomerSessionId, opt => opt.MapFrom(c => c.CustomerSession.Id));
			Mapper.CreateMap<pMoneyOrder, MoneyOrderDTO>()
				.ForMember(x => x.CustomerSessionId, opt => opt.MapFrom(c => c.CustomerSession.Id));
			Mapper.CreateMap<pMoneyTransfer, MoneyTransferDTO>()
				.ForMember(x => x.CustomerSessionId, opt => opt.MapFrom(c => c.CustomerSession.Id));
			Mapper.CreateMap<CXNCustomer, CxnCustomerProfile>();
			Mapper.CreateMap<pCash, CashDTO>()
			.ForMember(x => x.TransactionType, opt => opt.MapFrom(c => Enum.Parse(typeof(MGI.Biz.CashEngine.Data.CashTransactionType), c.CashType.ToString())))
				.ForMember(x => x.CustomerSessionId, opt => opt.MapFrom(c => c.CustomerSession.Id));
		}

		#region Public Methods

		public ShoppingCartDTO Get(long customerSessionId, MGIContext mgiContext)
		{
			try
			{
			CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			if (!session.HasActiveShoppingCart())
				return null;

			return PopulateCart(customerSessionId, session.ActiveShoppingCart, mgiContext);
		}
			catch(Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.GET_SHOPPINGCART_FAILED, ex);
			}
		}

		public void Update(long customerSessionId, Data.ShoppingCartStatus status, MGIContext mgiContext)
		{
			ShoppingCartSvc.Update(customerSessionId, (ShoppingCartStatus)((int)status));
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added new method to update IsReferral flag in shoppingCart
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="isReferral"></param>
		/// <returns></returns>
		public void Update(long customerSessionId, MGIContext mgiContext)
		{
			ShoppingCartSvc.Update(customerSessionId, mgiContext.IsReferral);
		}

		public ShoppingCartDTO GetCartById(long customerSessionId, long shoppingCartId, MGIContext mgiContext)
		{
			try
			{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, Convert.ToString(shoppingCartId), "GetCartById", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "Begin GetCartById - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
			pShoppingCart cart = ShoppingCartSvc.Lookup(shoppingCartId);
			if (cart == null)
				return null;

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<pShoppingCart>(mgiContext.CustomerSessionId, cart, "GetCartById", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "End GetCartById - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
			return PopulateCart(customerSessionId, cart, mgiContext);
		}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, Convert.ToString(shoppingCartId), "GetCartById", AlloyLayerName.BIZ,
					ModuleName.ShoppingCart, "Error in GetCartById - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.GET_SHOPPINGCART_FAILED, ex);
			}
		}


		public void AddCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			try
			{
			#region AL-3371 Transactional Log User Story(Process check)
			string id = Convert.ToString(checkId);

			MongoDBLogger.Info<string>(customerSessionId, id, "AddCheck", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin AddCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pCheck check = CheckTransactionService.Lookup(checkId);
			AddTransaction(customerSessionId, check);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<pCheck>(customerSessionId, check, "AddCheck", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End AddCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch(Exception ex)
			{
				#region AL-3371 Transactional Log User Story(Process check)
				string id = Convert.ToString(checkId);

				MongoDBLogger.Info<string>(customerSessionId, id, "AddCheck", AlloyLayerName.BIZ,
					ModuleName.ProcessCheck, "Error in AddCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.ADD_CHECK_SHOPPINGCART_FAILED, ex);
			}
		}

		public void AddFunds(long customerSessionId, long fundsId, MGIContext mgiContext)
		{
			try
			{
			#region AL-3372 transaction information for GPR cards.
			string id = Convert.ToString(fundsId);

			MongoDBLogger.Info<string>(customerSessionId, id, "AddFunds", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin AddFunds - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pFunds funds = FundsTransactionService.Lookup(fundsId);
			AddTransaction(customerSessionId, funds);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<pFunds>(customerSessionId, funds, "AddFunds", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End AddFunds - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-3372 transaction information for GPR cards.
				string id = Convert.ToString(fundsId);

				MongoDBLogger.Info<string>(customerSessionId, id, "AddFunds", AlloyLayerName.BIZ,
					ModuleName.ProcessCheck, "Error in AddFunds - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.ADD_FUNDS_SHOPPINGCART_FAILED, ex);
			}
		}

		public void AddBillPay(long customerSessionId, long billPayId, MGIContext mgiContext)
		{
			try
			{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(billPayId), "AddBillPay", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "Begin AddBillPay - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
			pBillPay billPay = BillPayTransactionService.Lookup(billPayId);
			AddTransaction(customerSessionId, billPay);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<pBillPay>(customerSessionId, billPay, "AddBillPay", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "End AddBillPay - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(billPayId), "AddBillPay", AlloyLayerName.BIZ,
					ModuleName.BillPayment, "Error in AddBillPay - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.ADD_BILLPAY_SHOPPINGCART_FAILED, ex);
			}
		}

		public void AddMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			try
			{
			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrder Id:" + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "AddMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "Begin AddMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pMoneyOrder moneyOrder = MoneyOrderTransactionService.Lookup(moneyOrderId);
			AddTransaction(customerSessionId, moneyOrder);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<pMoneyOrder>(customerSessionId, moneyOrder, "AddMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "End AddMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-1071 Transactional Log class for MO flow
				string id = "MoneyOrder Id:" + Convert.ToString(moneyOrderId);

				MongoDBLogger.Info<string>(customerSessionId, id, "AddMoneyOrder", AlloyLayerName.BIZ,
					ModuleName.MoneyOrder, "Error in AddMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.ADD_MONEYORDER_SHOPPINGCART_FAILED, ex);
			}
		}

		public void AddMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext)
		{
			try
			{
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(moneyTransferId), "AddMoneyTransfer", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
									  "Begin AddMoneyTransfer-MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
			pMoneyTransfer moneyTransfer = MoneyTransferTransactionService.Lookup(moneyTransferId);

			AddTransaction(customerSessionId, moneyTransfer);

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<pMoneyTransfer>(customerSessionId, moneyTransfer, "AddMoneyTransfer", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
									  "End AddMoneyTransfer-MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(moneyTransferId), "AddMoneyTransfer", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
										  "Error in AddMoneyTransfer-MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.ADD_MONEYTRANSFER_SHOPPINGCART_FAILED, ex);
			}
		}

		public void AddCash(long customerSessionId, long cashTxnId, MGIContext mgiContext)
		{
			try
			{
			#region AL-1071 Transactional Log class for MO flow
			string id = "CashTran Id:" + Convert.ToString(cashTxnId);

			MongoDBLogger.Info<string>(customerSessionId, id, "AddCash", AlloyLayerName.BIZ,
				ModuleName.CashOut, "Begin AddCash - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pCash cash = CashTransactionService.Lookup(cashTxnId);
			AddTransaction(customerSessionId, cash);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<pCash>(customerSessionId, cash, "AddCash", AlloyLayerName.BIZ,
				ModuleName.CashOut, "End AddCash - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-1071 Transactional Log class for MO flow
				string id = "CashTran Id:" + Convert.ToString(cashTxnId);

				MongoDBLogger.Info<string>(customerSessionId, id, "AddCash", AlloyLayerName.BIZ,
					ModuleName.CashOut, "Error in AddCash - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.ADD_FUNDS_SHOPPINGCART_FAILED, ex);
			}
		}

		public void RemoveCashIn(long customerSessionId, long cashTrxId, MGIContext mgiContext)
		{
			try
			{
			#region AL-1071 Transactional Log class for MO flow
			string id = "CashTran Id:" + Convert.ToString(cashTrxId);

			MongoDBLogger.Info<string>(customerSessionId, id, "RemoveCashIn", AlloyLayerName.BIZ,
				ModuleName.CashIn, "Begin RemoveCashIn - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pCash cash = CashTransactionService.Lookup(cashTrxId);

			RemoveTransaction(customerSessionId, cash);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<pCash>(customerSessionId, cash, "RemoveCashIn", AlloyLayerName.BIZ,
				ModuleName.CashIn, "End RemoveCashIn - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-1071 Transactional Log class for MO flow
				string id = "CashTran Id:" + Convert.ToString(cashTrxId);

				MongoDBLogger.Info<string>(customerSessionId, id, "RemoveCashIn", AlloyLayerName.BIZ,
					ModuleName.CashIn, "Error in RemoveCashIn - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.REMOVE_CASH_SHOPPINGCART_FAILED, ex);
			}
		}

		public bool RemoveCheck(long customerSessionId, long checkId, bool isParkedTransaction, MGIContext mgiContext)
		{
			try
			{
			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Check Id:" + Convert.ToString(checkId));
			details.Add("isParkedTransaction:" + Convert.ToString(isParkedTransaction));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveCheck", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin RemoveCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pCheck check = CheckTransactionService.Lookup(checkId);
			RemoveTransaction(customerSessionId, check, isParkedTransaction);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<pCheck>(customerSessionId, check, "RemoveCheck", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End RemoveCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			return MessageCenterService.Delete(check);
		}
			catch (Exception ex)
			{
				#region AL-3371 Transactional Log User Story(Process check)
				List<string> details = new List<string>();
				details.Add("Check Id:" + Convert.ToString(checkId));
				details.Add("isParkedTransaction:" + Convert.ToString(isParkedTransaction));

				MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveCheck", AlloyLayerName.BIZ,
					ModuleName.ProcessCheck, "Error in RemoveCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.REMOVE_CHECK_SHOPPINGCART_FAILED, ex);
			}
		}

		public void RemoveFunds(long customerSessionId, long fundsId, bool isParkedTransaction, MGIContext mgiContext)
		{
			try
			{
			#region AL-3372 transaction information for GPR cards.
			List<string> details = new List<string>();
			details.Add("Check Id:" + Convert.ToString(fundsId));
			details.Add("isParkedTransaction:" + Convert.ToString(isParkedTransaction));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveFunds", AlloyLayerName.BIZ,
				ModuleName.Funds, "Begin RemoveFunds - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pFunds funds = FundsTransactionService.Lookup(fundsId);
			RemoveTransaction(customerSessionId, funds, isParkedTransaction);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<pFunds>(customerSessionId, funds, "RemoveFunds", AlloyLayerName.BIZ,
				ModuleName.Funds, "End RemoveFunds - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-3372 transaction information for GPR cards.
				List<string> details = new List<string>();
				details.Add("Check Id:" + Convert.ToString(fundsId));
				details.Add("isParkedTransaction:" + Convert.ToString(isParkedTransaction));

				MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveFunds", AlloyLayerName.BIZ,
					ModuleName.Funds, "Error in RemoveFunds - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.REMOVE_FUNDS_SHOPPINGCART_FAILED, ex);
			}
		}

		public void RemoveBillPay(long customerSessionId, long billPayId, bool isParkedTransaction, MGIContext mgiContext)
		{
			try
			{
			#region AL-1014 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Transaction Id:" + Convert.ToString(billPayId));
			details.Add("Is Parked:" + Convert.ToString(isParkedTransaction));
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveBillPay", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "Begin RemoveBillPay - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
			pBillPay billPay = BillPayTransactionService.Lookup(billPayId);
			RemoveTransaction(customerSessionId, billPay, isParkedTransaction);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<pBillPay>(customerSessionId, billPay, "RemoveBillPay", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "End RemoveBillPay - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(billPayId));
				details.Add("Is Parked:" + Convert.ToString(isParkedTransaction));
				MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveBillPay", AlloyLayerName.BIZ,
					ModuleName.BillPayment, "Error in RemoveBillPay - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.REMOVE_BILLPAY_SHOPPINGCART_FAILED, ex);
			}
		}

		public void RemoveMoneyOrder(long customerSessionId, long moneyOrderId, bool isParkedTransaction, MGIContext mgiContext)
		{
			try
			{
			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrder Id:" + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "RemoveMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "Begin RemoveMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pMoneyOrder moneyOrder = MoneyOrderTransactionService.Lookup(moneyOrderId);
			RemoveTransaction(customerSessionId, moneyOrder, isParkedTransaction);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<pMoneyOrder>(customerSessionId, moneyOrder, "RemoveMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "End RemoveMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-1071 Transactional Log class for MO flow
				string id = "MoneyOrder Id:" + Convert.ToString(moneyOrderId);

				MongoDBLogger.Info<string>(customerSessionId, id, "RemoveMoneyOrder", AlloyLayerName.BIZ,
					ModuleName.MoneyOrder, "Error in RemoveMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.REMOVE_MONEYORDER_SHOPPINGCART_FAILED, ex);
			}
		}

		public void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, bool isParkedTransaction, MGIContext mgiContext)
		{
			try
			{
			#region AL-3370 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Transaction Id:" + Convert.ToString(moneyTransferId));
			details.Add("Parked Transaction:" + Convert.ToString(isParkedTransaction));
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveMoneyTransfer", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
									  "Begin RemoveMoneyTransfer-MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
			pMoneyTransfer moneyTransfer = MoneyTransferTransactionService.Lookup(moneyTransferId);
			RemoveTransaction(customerSessionId, moneyTransfer, isParkedTransaction);
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "RemoveMoneyTransfer", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
									  "End RemoveMoneyTransfer-MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-3370 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(moneyTransferId));
				details.Add("Parked Transaction:" + Convert.ToString(isParkedTransaction));
				MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveMoneyTransfer", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
										  "Error in RemoveMoneyTransfer-MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.REMOVE_MONEYTRANSFER_SHOPPINGCART_FAILED, ex);
			}
		}

		public void CloseShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
			try
			{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "CloseShoppingCart", AlloyLayerName.BIZ, ModuleName.ShoppingCart,
				"Begin CloseShoppingCart - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
			CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
			session.ActiveShoppingCart.CloseShoppingCart();

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "CloseShoppingCart", AlloyLayerName.BIZ, ModuleName.ShoppingCart,
				"End CloseShoppingCart - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, string.Empty, "CloseShoppingCart", AlloyLayerName.BIZ, ModuleName.ShoppingCart,
					"Error in CloseShoppingCart - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.CLOSE_SHOPPINGCART_FAILED, ex);
			}
		}


		public void ParkCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			try
			{
			#region AL-3371 Transactional Log User Story(Process check)
			string id = Convert.ToString(checkId);

			MongoDBLogger.Info<string>(customerSessionId, id, "ParkCheck", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin ParkCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pCheck check = CheckTransactionService.Lookup(checkId);
			ParkTransaction(customerSessionId, check);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<pCheck>(customerSessionId, check, "ParkCheck", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End ParkCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-3371 Transactional Log User Story(Process check)
				string id = Convert.ToString(checkId);

				MongoDBLogger.Info<string>(customerSessionId, id, "ParkCheck", AlloyLayerName.BIZ,
					ModuleName.ProcessCheck, "Error in ParkCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.PARK_CHECK_SHOPPINGCART_FAILED, ex);
			}
		}

		public void ParkFunds(long customerSessionId, long fundsId, MGIContext mgiContext)
		{
			try
			{
			#region AL-3372 transaction information for GPR cards.
			string id = Convert.ToString(fundsId);

			MongoDBLogger.Info<string>(customerSessionId, id, "ParkFunds", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "Begin ParkFunds - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pFunds funds = FundsTransactionService.Lookup(fundsId);
			ParkTransaction(customerSessionId, funds);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<pFunds>(customerSessionId, funds, "ParkFunds", AlloyLayerName.BIZ,
				ModuleName.ProcessCheck, "End ParkFunds - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-3372 transaction information for GPR cards.
				string id = Convert.ToString(fundsId);

				MongoDBLogger.Info<string>(customerSessionId, id, "ParkFunds", AlloyLayerName.BIZ,
					ModuleName.ProcessCheck, "Error in ParkFunds - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.PARK_FUNDS_SHOPPINGCART_FAILED, ex);
			}
		}

		public void ParkBillPay(long customerSessionId, long billPayId, MGIContext mgiContext)
		{
			try
			{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(billPayId), "ParkBillPay", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "Begin ParkBillPay - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
			pBillPay billPay = BillPayTransactionService.Lookup(billPayId);
			ParkTransaction(customerSessionId, billPay);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<pBillPay>(customerSessionId, billPay, "ParkBillPay", AlloyLayerName.BIZ,
				ModuleName.BillPayment, "End ParkBillPay - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(billPayId), "ParkBillPay", AlloyLayerName.BIZ,
					ModuleName.BillPayment, "Error in ParkBillPay - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.PARK_BILLPAY_SHOPPINGCART_FAILED, ex);
			}
		}

		public void ParkMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext)
		{
			try
			{
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(moneyTransferId), "ParkMoneyTransfer", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
									  "Begin ParkMoneyTransfer-MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
			pMoneyTransfer moneyTransfer = MoneyTransferTransactionService.Lookup(moneyTransferId);
			ParkTransaction(customerSessionId, moneyTransfer);
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "ParkMoneyTransfer", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
									  "End ParkMoneyTransfer-MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(moneyTransferId), "ParkMoneyTransfer", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
										  "Error in ParkMoneyTransfer-MGI.Biz.Partner.Impl.ShoppingCartServiceImpl", mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.PARK_MONEYTRANSFER_SHOPPINGCART_FAILED, ex);
			}
		}

		public void ParkMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			try
			{
			#region AL-1071 Transactional Log class for MO flow
			string id = Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "ParkMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "Begin ParkMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			pMoneyOrder moneyOrder = MoneyOrderTransactionService.Lookup(moneyOrderId);
			ParkTransaction(customerSessionId, moneyOrder);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<pMoneyOrder>(customerSessionId, moneyOrder, "ParkMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.MoneyOrder, "End ParkMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}
			catch (Exception ex)
			{
				#region AL-1071 Transactional Log class for MO flow
				string id = Convert.ToString(moneyOrderId);

				MongoDBLogger.Info<string>(customerSessionId, id, "ParkMoneyOrder", AlloyLayerName.BIZ,
					ModuleName.MoneyOrder, "Error in ParkMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
					mgiContext);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizShoppingCartException(BizShoppingCartException.PARK_MONEYORDER_SHOPPINGCART_FAILED, ex);
			}
		}

		public ShoppingCartCheckoutStatusDTO Checkout(long customerSessionId, decimal cashToCustomer, string cardNumber, ShoppingCartCheckoutStatusDTO shoppingCartstatus, MGIContext mgiContext)
		{
			//try
			//{
			#region AL-1071 Transactional Log class for MO flow
			List<string> details = new List<string>();
			details.Add("CashToCustomer :" + Convert.ToString(cashToCustomer));
			details.Add("CardNumber :" + Convert.ToString(cardNumber));
			details.Add("ShoppingCartstatus :" + Convert.ToString(shoppingCartstatus));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "Checkout", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "Begin Shopping Cart Checkout - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			ShoppingCartDTO shoppingCart = Get(customerSessionId, mgiContext);

			Update(shoppingCart.Id, (Biz.Partner.Data.ShoppingCartStatus)(int)shoppingCartstatus, mgiContext);

			//handling with first or default as there may be case where there is no load or withdraw transactions
			FundsDTO activate = shoppingCart.Funds.Where(x => x.TransactionType == CXNFundsType.None.ToString() && x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized).FirstOrDefault();
			FundsDTO load = shoppingCart.Funds.Where(x => x.TransactionType == CXNFundsType.Credit.ToString() && x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized).FirstOrDefault();
			FundsDTO withdraw = shoppingCart.Funds.Where(x => x.TransactionType == CXNFundsType.Debit.ToString() && x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized).FirstOrDefault();
			FundsDTO addOnCard = shoppingCart.Funds.Where(x => x.TransactionType == CXNFundsType.AddOnCard.ToString() && x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized).FirstOrDefault();
			//AL-2729 user story for updating cash in transaction - Change this to list of cashDTO
			List<CashDTO> cashIn = shoppingCart.CashInTransactions.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized).ToList();

			mgiContext.ShouldIncludeShoppingCartItems = false;

			PreShoppingCartFlushCheck(customerSessionId, cashToCustomer, mgiContext);

			// Check out the funds generating transactions.
			CheckoutFundsGenerating(customerSessionId, shoppingCart, mgiContext);

			// Withdraw checkout 
			// Check for CashOverCounter true, then process the fund transaction and return the status

			if (withdraw != null && withdraw.Amount > 0)
				CommitFunds(customerSessionId, withdraw.Id, mgiContext, cardNumber);

			if ((withdraw != null && withdraw.Amount > 0) && IsCashOverCounter(customerSessionId, mgiContext) && ShoppingCartCheckoutStatusDTO.CashCollected != shoppingCartstatus)
			{
				CashOut(customerSessionId, withdraw.Amount, mgiContext);
				Update(shoppingCart.Id, Biz.Partner.Data.ShoppingCartStatus.CashOverCounter, mgiContext);
				return ShoppingCartCheckoutStatusDTO.CashOverCounter;
			}
			// Withdraw checkout completes
			CheckoutFundsDepleting(customerSessionId, shoppingCart, mgiContext);

			Update(shoppingCart.Id, mgiContext);  //cartCheckOut.IsReferral

			if (shoppingCart.MoneyOrders != null && shoppingCart.MoneyOrders.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized).Any())
			{
				return ShoppingCartCheckoutStatusDTO.MOPrinting;
			}

			if (activate != null)
				CommitFunds(customerSessionId, activate.Id, mgiContext, cardNumber);

			if (addOnCard != null)
			{
				CommitFunds(customerSessionId, addOnCard.Id, mgiContext);
			}

			if (load != null && load.Amount > 0)
				CommitFunds(customerSessionId, load.Id, mgiContext);

			//AL-2729 user story for updating cash in transaction
			//if (cashIn.Count > 0 && (shoppingCart.IsShoppingCartItemExists || cashToCustomer > 0 ))
			if (cashIn.Count > 0)
			{
				foreach (var trn in cashIn)
					CommitCash(customerSessionId, trn.CXEId, mgiContext);
			}


			if (cashToCustomer > 0)
				CashOut(customerSessionId, cashToCustomer, mgiContext);

			Update(shoppingCart.Id, Biz.Partner.Data.ShoppingCartStatus.Completed, mgiContext);

			//US1800 Referral Referee Promotions
			addUpdateCustomerFeeAdjustment(customerSessionId, mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<ShoppingCartDTO>(customerSessionId, shoppingCart, "Checkout", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "End Shopping Cart Checkout - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			return ShoppingCartCheckoutStatusDTO.Completed;
			//}
			//catch (Exception ex)
			//{
			//	#region AL-1071 Transactional Log class for MO flow
			//	List<string> details = new List<string>();
			//	details.Add("CashToCustomer :" + Convert.ToString(cashToCustomer));
			//	details.Add("CardNumber :" + Convert.ToString(cardNumber));
			//	details.Add("ShoppingCartstatus :" + Convert.ToString(shoppingCartstatus));

			//	MongoDBLogger.ListInfo<string>(customerSessionId, details, "Checkout", AlloyLayerName.BIZ,
			//		ModuleName.ShoppingCart, "Error in Shopping Cart Checkout - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
			//		mgiContext);
			//	#endregion
			//	if (ExceptionHelper.IsExceptionHandled(ex)) throw;
			//	throw new BizShoppingCartException(BizShoppingCartException.CHECKOUT_SHOPPINGCART_FAILED, ex);
			//}
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
		/// Method will insert referral promotions once new customer registered and done one transaction
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="mgiContext"></param>
		private void addUpdateCustomerFeeAdjustment(long customerSessionId, MGIContext mgiContext)
		{

			// check is this first checkout and having referral code, referal promotion in system

			CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);

			ChannelPartner channelPartner = CoreChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

			FeeAdjustment feeAdj = getReferralPromotion(channelPartner);



			var shoppingCarts = ShoppingCartSvc.LookupForCustomer(customerSession.Customer.CXEId).Where(x => x.ShoppingCartTransactions.Any(y => (TransactionType)y.Transaction.Type != TransactionType.Cash
								&& (y.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Committed)
								&& (y.Transaction.CXNState == (int)MGI.Core.CXE.Data.TransactionStates.Committed))).ToList();

			if (shoppingCarts.Count == 0 && feeAdj != null && customerSession.Customer.ReferralCode != string.Empty)
			{
				var customer = CXECustomerService.Lookup(customerSession.Customer.ReferralCode);

				if (customer != null)
				{
					CustomerFeeAdjustments customerFeeAdjustment = new CustomerFeeAdjustments()
					{
						feeAdjustment = feeAdj,
						IsAvailed = false,
						CustomerID = customer.Id,
						DTServerCreate = DateTime.Now,
						DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone)
					};
					CustomerFeeAdjustmentService.Create(customerFeeAdjustment);
				}
			}

		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
		/// A method to get IsReferralApplicable Section for the Channelpartner
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="mgiContext"></param>
		/// <returns></returns>
		public bool IsReferralApplicable(long customerSessionId, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "IsReferralApplicable", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "Begin IsReferralApplicable - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
			CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
			// This code has to be changed to get channelpartner id from context to support MVA

			ChannelPartner channelPartner = CoreChannelPartnerService.ChannelPartnerConfig(customerSession.AgentSession.Terminal.ChannelPartner.Id);

			FeeAdjustment feeAdjustment = getReferralPromotion(channelPartner);


			if (feeAdjustment != null && channelPartner.ChannelPartnerConfig.IsReferralSectionEnable)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, string.Empty, "IsReferralApplicable", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "End IsReferralApplicable - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
				#endregion
				return channelPartner.ChannelPartnerConfig.IsReferralSectionEnable;
			}

			else
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(customerSessionId, string.Empty, "IsReferralApplicable", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "End IsReferralApplicable - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
				#endregion
				return false;

			}

		}

		public void PreShoppingCartFlushCheck(long customerSessionId, decimal cashToCustomer, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "PreShoppingCartFlushCheck", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "Begin PreShoppingCartFlushCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
			string channelPartnerName = string.Empty;

			CXNCustomerTransactionDetails customerDetails = PrepareCXNCustomerTransactionDetails(customerSessionId, cashToCustomer, CartFlush.PreFlush, mgiContext);
			if (customerDetails != null)
				channelPartnerName = customerDetails.Customer.ChannelPartnerName;

			mgiContext.CxnAccountId = GetCXNCustomerAccount(customerSessionId, mgiContext);
			//Get the transaction count. "CashIn" and "Cashout" transactions are not counted as these two transaction's status will always be "Committed".
			//This check is applied for "Fund/GPR" transaction as this will call "CheckOut" method multiple times in case of withdrawal.
			int tranCount = customerDetails.Transactions.Where(t => t.Type.ToLower() != "cash").Count();

			bool istrue = IsPreFlushCallSent(customerSessionId);

			if (tranCount > 0 && !istrue)
			{
				PublishEvent(channelPartnerName, new BizPartnerData.PreFlushEvent()
				{
					CustomerTransactionDetails = customerDetails,
					mgiContext = mgiContext,
				});
			}

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "PreShoppingCartFlushCheck", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "End PreShoppingCartFlushCheck - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}

		public ShoppingCartDTO PopulateCart(long customerSessionId, pShoppingCart cart, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<pShoppingCart>(customerSessionId, cart, "PopulateCart", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "Begin PopulateCart - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
			ShoppingCartDTO cartDTO = new ShoppingCartDTO(cart.Id);

			AddIfAny<CheckDTO, pCheck>(cartDTO.Checks, cart.ShoppingCartTransactions, TransactionType.Check);
			AddIfAny<FundsDTO, pFunds>(cartDTO.Funds, cart.ShoppingCartTransactions, TransactionType.Funds);
			AddIfAny<BillPayDTO, pBillPay>(cartDTO.BillPays, cart.ShoppingCartTransactions, TransactionType.BillPay);
			AddIfAny<MoneyOrderDTO, pMoneyOrder>(cartDTO.MoneyOrders, cart.ShoppingCartTransactions, TransactionType.MoneyOrder);
			AddIfAny<MoneyTransferDTO, pMoneyTransfer>(cartDTO.MoneyTransfers, cart.ShoppingCartTransactions, TransactionType.MoneyTransfer);
			AddIfAny<CashDTO, pCash>(cartDTO.CashInTransactions, cart.ShoppingCartTransactions, TransactionType.Cash);

			cartDTO.CheckTotal = cartDTO.Checks.Sum(d => d.Amount);
			cartDTO.BillTotal = cartDTO.BillPays.Sum(d => d.Amount) + cartDTO.BillPays.Sum(d => d.Fee);
			cartDTO.MoneyTransferTotal = cartDTO.MoneyTransfers.Sum(d => d.Amount) + cartDTO.MoneyTransfers.Sum(d => d.Fee);
			cartDTO.FundsTotal = cartDTO.Funds.Sum(d => d.Amount) + cartDTO.Funds.Sum(d => d.Fee);
			cartDTO.CashTotal = cartDTO.CashInTransactions.Where(x => x.TransactionType.ToLower() == CashTransactionTypeDTO.CashIn.ToString().ToLower()).Sum(d => d.Amount);
			cartDTO.MoneyOrderTotal = (cartDTO.MoneyOrders == null) ? 0 : cartDTO.MoneyOrders.Sum(d => d.Amount);
			cartDTO.IsReferral = cart.IsReferral;



			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<ShoppingCartDTO>(customerSessionId, cartDTO, "PopulateCart", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "End PopulateCart - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
			return cartDTO;
		}

		public void PostFlush(long customerSessionId, MGIContext mgiContext)
		{

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, null, "PostFlush", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "Begin PostFlush - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
			string channelPartnerName = mgiContext.ChannelPartnerName;
			//TDB added to build to solution
			long accountId = GetCXNCustomerAccount(customerSessionId, mgiContext);

			CXNCustomerTransactionDetails customerDetails = PrepareCXNCustomerTransactionDetails(customerSessionId, 0, CartFlush.PostFlush, mgiContext);

			//if (customerDetails != null)
			//    channelPartnerName = customerDetails.Customer.ChannelPartnerName;

			mgiContext.CxnAccountId = accountId;

			////Get the transaction count. "CashIn" and "Cashout" transactions are not counted as these two transaction's status will always be "Committed".
			////This check is applied for "Fund/GPR" transaction as this will call "CheckOut" method multiple times in case of withdrawal.
			var tranCount = customerDetails.Transactions.Where(t => t.Type.ToLower() != "cash").Count();

			if (tranCount > 0)
			{
				PublishEvent(channelPartnerName, new BizPartnerData.PostFlushEvent()
				{
					CustomerTransactionDetails = customerDetails,
					mgiContext = mgiContext
				});
			}

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "PostFlush", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "End PostFlush - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}

		public List<MGI.Biz.Partner.Data.ParkedTransaction> GetAllParkedShoppingCartTransactions()
		{
			var parkedTransactions = new List<MGI.Biz.Partner.Data.ParkedTransaction>();
			var parkedShoppingCart = ShoppingCartSvc.GetAllParkedShoppingCarts();
			foreach (var cart in parkedShoppingCart)
			{
				var parkedTrxs = cart.ShoppingCartTransactions;
				foreach (var trx in parkedTrxs)
				{
					parkedTransactions.Add(new BizPartnerData.ParkedTransaction()
					{
						CustomerSessionId = trx.Transaction.CustomerSession.Id,
						TransactionId = trx.Transaction.Id,
						TransactionType = ((TransactionTypes)trx.Transaction.Type).ToString()
					});
				}
			}
			return parkedTransactions;
		}
		#endregion

		#region Private Methods

		private CXNCustomerTransactionDetails PrepareCXNCustomerTransactionDetails(long customerSessionId, decimal cashToCustomer, CartFlush shoppingcartFlush, MGIContext mgiContext)
		{
			CXNCustomerTransactionDetails shoppingCart = new CXNCustomerTransactionDetails();

			pShoppingCart cart = GetShoppingCartsByCustomerSessionID(customerSessionId);

			cart.ShoppingCartTransactions = cart.ShoppingCartTransactions.Where(c => c.CartItemStatus == ShoppingCartItemStatus.Added).ToList();

			foreach (var shoppingCartTran in cart.ShoppingCartTransactions)
			{
				long tranID = 0;
				long cxeId = 0;
				if (shoppingCartTran.Transaction != null)
				{
					tranID = shoppingCartTran.Transaction.Id;
					cxeId = shoppingCartTran.Transaction.CXEId;
				}

				TransactionType type = (TransactionType)shoppingCartTran.Transaction.Type;

				//If its of Transaction Type "Money Transfer" and while doing preflush make sure the transaction is in Authorised state.
				if (type == TransactionType.MoneyTransfer && (((shoppingCartTran.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized && shoppingcartFlush == CartFlush.PreFlush))
						|| shoppingcartFlush == CartFlush.PostFlush) && string.IsNullOrWhiteSpace(((MGI.Core.Partner.Data.Transactions.MoneyTransfer)shoppingCartTran.Transaction).TransactionSubType))
				{
					string tranTypeName = string.Empty;

					int tranType = ((MGI.Core.Partner.Data.Transactions.MoneyTransfer)shoppingCartTran.Transaction).TransferType;

					if (tranType == (int)TransferTypeDTO.SendMoney)
						tranTypeName = TransferTypeDTO.SendMoney.ToString();
					else
						tranTypeName = TransferTypeDTO.ReceiveMoney.ToString();

					BizTransactionRequest request = new BizTransactionRequest()
					{
						PTNRTransactionId = tranID,
						CXNTransactionId = shoppingCartTran.Transaction.CXNId
					};

					MoneyTransferTransactionDTO moneyTranferTran = MoneyTransferEngine.Get(customerSessionId, request, mgiContext);

					CXNTransaction tran = PopulateMoneyTransfer(moneyTranferTran, tranTypeName, customerSessionId, cxeId);

					shoppingCart.Transactions.Add(tran);

				}
				//If its of Transaction Type "Bill Pay" then call Biz.BillPay and get Transaction Details.
				else if (type == TransactionType.BillPay && (((shoppingCartTran.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized && shoppingcartFlush == CartFlush.PreFlush))
						|| shoppingcartFlush == CartFlush.PostFlush))
				{
					BillPayTransactionDTO billPayTran = BillPayService.GetTransaction(customerSessionId, tranID, mgiContext);

					CXNTransaction tran = PopulateBillPay(billPayTran, cxeId);

					shoppingCart.Transactions.Add(tran);

				}
				//If its of Transaction Type "CheckProcessing" then call Biz.CPEngine and get Transaction Details.
				else if (type == TransactionType.Check && (((shoppingCartTran.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized && shoppingcartFlush == CartFlush.PreFlush))
						|| shoppingcartFlush == CartFlush.PostFlush))
				{
					CheckTransactionDTO cpTran = CPEngineService.GetTransaction(mgiContext.AgentSessionId, customerSessionId, tranID.ToString(), mgiContext);

					CXNTransaction tran = PopulateCheck(cpTran, cxeId);

					shoppingCart.Transactions.Add(tran);

				}
				//If its of Transaction Type "MoneyOrder" then call Biz.MoneyOrderEngine and get Transaction Details.
				else if (type == TransactionType.MoneyOrder && (((shoppingCartTran.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized && shoppingcartFlush == CartFlush.PreFlush))
						|| shoppingcartFlush == CartFlush.PostFlush))
				{
					MoneyOrderTransactionDTO moTran = MoneyOrderEngineService.GetMoneyOrderStage(customerSessionId, tranID, mgiContext);

					CXNTransaction tran = PopulateMoneyOrder(moTran, cxeId);

					shoppingCart.Transactions.Add(tran);

				}
				//If its of Transaction Type "Funds" then call Biz.FundsEngine and get Transaction Details.
				else if (type == TransactionType.Funds && (((shoppingCartTran.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized && shoppingcartFlush == CartFlush.PreFlush))
						|| shoppingcartFlush == CartFlush.PostFlush))
				{
					bool isActivate = false;
					int fundType = ((MGI.Core.Partner.Data.Transactions.Funds)shoppingCartTran.Transaction).FundType;

					string fundTypeName = string.Empty;

					if (fundType == (int)CXNFundsType.Credit || fundType == (int)CXNFundsType.None || fundType == (int)CXNFundsType.AddOnCard)
					{
						fundTypeName = "PrePaidLoad";
						if (fundType == (int)CXNFundsType.None)
						{
							isActivate = true;
						}
					}
					else if (fundType == (int)CXNFundsType.Debit)
					{
						fundTypeName = "PrePaidWithdraw";
					}

					FundsTransactionDTO feTran = FundsEngine.Get(customerSessionId, tranID, mgiContext);

					CXNTransaction tran = PopulateFunds(feTran, fundTypeName, cxeId, customerSessionId, isActivate, mgiContext);

					shoppingCart.Transactions.Add(tran);

				}
				//No condition check for cash transaction as "cash" transaction is always be in committed state.
				else if (type == TransactionType.Cash)
				{
					string cashTypeName = string.Empty;

					int cashType = ((MGI.Core.Partner.Data.Transactions.Cash)shoppingCartTran.Transaction).CashType;

					if (cashType == (int)CashTransactionTypeDTO.CashIn)
						cashTypeName = CashTransactionTypeDTO.CashIn.ToString();
					else
						cashTypeName = CashTransactionTypeDTO.CashOut.ToString();

					CXNTransaction tran = PopulateCash(shoppingCartTran, cashTypeName, cxeId);

					shoppingCart.Transactions.Add(tran);

				}
			}

			//AL-4058 CashOut transaction is missing in Precart message for PrepaidWithdrawal, Receive Money and Check Processing transactions
			//To Add Cash out Transaction
			//This fix will be replaced by proper user story implementation
			if (cashToCustomer > 0)
			{
				string cashTypeName = cashTypeName = CashTransactionTypeDTO.CashOut.ToString();

				ShoppingCartTransaction transaction = new ShoppingCartTransaction();
				transaction.Transaction = new MGI.Core.Partner.Data.Transactions.Cash();
				transaction.Transaction.Id = 20000000;
				transaction.Transaction.Amount = cashToCustomer;
				transaction.Transaction.Fee = 0;

				CXNTransaction tran = PopulateCash(transaction, cashTypeName, 20000000);

				shoppingCart.Transactions.Add(tran);
			}

			shoppingCart.Customer = PopulateCustomerSession(customerSessionId, mgiContext);

			if (shoppingcartFlush == CartFlush.PreFlush && cart.ShoppingCartTransactions.Where(c => (TransactionType)c.Transaction.Type != TransactionType.Cash && (c.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Committed || c.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Processing)).Any())
			{
				// In Post Flush we have to make "TcfCustInd" to "Y" to signify this customer has already have commited transaction.
				shoppingCart.Customer.CustInd = true;
			}

			return shoppingCart;
		}

		private CXNCustomer PopulateCustomerSession(long customerSessionId, MGIContext mgiContext)
		{
			CXNCustomer cust = new CXNCustomer();

			CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
			CXECustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
			NexxoIdType idType = null;
			if (cxeCustomer.GovernmentId != null)
				idType = PTNRDataStructureService.Find(Convert.ToInt32(cxeCustomer.ChannelPartnerId), cxeCustomer.GovernmentId.IdTypeId);

			long accountID = GetCXNCustomerAccount(customerSessionId, mgiContext);

			bool isVal = CxnClientCustomerService.GetCustInd(accountID, mgiContext);

			if (cxeCustomer != null)
			{
				cust.ChannelPartnerName = ChannelPartnerService.ChannelPartnerConfig(Convert.ToInt32(cxeCustomer.ChannelPartnerId), mgiContext).Name;
				cust.FirstName = cxeCustomer.FirstName;
				cust.MiddleName = cxeCustomer.MiddleName;
				cust.LastName = cxeCustomer.LastName;
				cust.SecondLastName = cxeCustomer.LastName2;
				//SecondLastName =
				cust.Address1 = cxeCustomer.Address1;
				cust.Address2 = cxeCustomer.Address2;
				cust.City = cxeCustomer.City;
				cust.State = cxeCustomer.State;
				//Country =
				cust.CountryofBirth = cxeCustomer.CountryOfBirth;
				cust.Zip = cxeCustomer.ZipCode;
				cust.Phone1 = cxeCustomer.Phone1;
				cust.Ph1Type1 = cxeCustomer.Phone1Type;
				cust.Phone2 = cxeCustomer.Phone2;
				cust.Ph2Type2 = cxeCustomer.Phone2Type;
				cust.Ph2Prov = cxeCustomer.Phone2Provider;
				cust.Email = cxeCustomer.Email;
				cust.DateOfBirth = Convert.ToString(cxeCustomer.DateOfBirth);
				cust.Occupation = cxeCustomer.EmploymentDetails != null ? cxeCustomer.EmploymentDetails.Occupation : string.Empty;
				cust.SSN = cxeCustomer.SSN;
				//cust.TaxCd = cxeCustomer.TaxpayerId
				cust.AlloyID = cxeCustomer.Id;
				cust.ClientCustId = cxeCustomer.ClientID;
				cust.Gender = cxeCustomer.Gender;
				cust.Identification = cxeCustomer.GovernmentId != null ? cxeCustomer.GovernmentId.Identification : string.Empty;
				cust.IssueDate = cxeCustomer.GovernmentId != null ? cxeCustomer.GovernmentId.IssueDate : null;
				cust.ExpirationDate = cxeCustomer.GovernmentId != null ? cxeCustomer.GovernmentId.ExpirationDate : null;
				cust.IdType = idType != null ? idType.Name : string.Empty;
				//cust.ma = cxeCustomer.GovernmentId.IdTypeId;
				cust.ClientID = cxeCustomer.ClientID;
				cust.LegalCode = cxeCustomer.LegalCode;
				cust.PrimaryCountryCitizenship = cxeCustomer.PrimaryCountryCitizenShip;
				cust.SecondaryCountryCitizenship = cxeCustomer.SecondaryCountryCitizenShip;
				cust.Maiden = cxeCustomer.MothersMaidenName;
				cust.IDCode = cxeCustomer.IDCode;
				if (idType != null)
				{
					cust.IdIssuer = idType.StateId != null ? Convert.ToString(idType.StateId.Abbr) : string.Empty;
					cust.IdIssuerCountry = idType.Country;
					cust.IdIssuerCountryCode = idType.CountryId != null ? idType.CountryId.Abbr2 : null;
				}
				cust.OccupationDescription = cxeCustomer.EmploymentDetails != null ? cxeCustomer.EmploymentDetails.OccupationDescription : string.Empty;
				cust.EmployerName = cxeCustomer.EmploymentDetails != null ? cxeCustomer.EmploymentDetails.Employer : string.Empty;
				cust.EmployerPhoneNum = cxeCustomer.EmploymentDetails != null ? cxeCustomer.EmploymentDetails.EmployerPhone : string.Empty;
				cust.CustInd = isVal;
				cust.CustomerSessionId = customerSessionId;
			}

			return cust;
		}

		private CXNTransaction PopulateMoneyTransfer(MoneyTransferTransactionDTO moneyTranferTran, string tranTypeName, long customerSessionId, long tranID)
		{
			CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
			CXECustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);


			CXNTransaction tran = new CXNTransaction()
			{
				ID = tranID.ToString(),
				Type = TransactionType.MoneyTransfer.ToString(),
				TransferType = tranTypeName,
				MTCN = moneyTranferTran.ConfirmationNumber,
				//Amount = moneyTranferTran.TransactionAmount,
				Amount = tranTypeName == TransferTypeDTO.SendMoney.ToString() ? moneyTranferTran.TransactionAmount : moneyTranferTran.AmountToReceiver,
				Fee = tranTypeName == TransferTypeDTO.SendMoney.ToString() ? moneyTranferTran.Fee : 0,
				//GrossTotalAmount = tranTypeName == TransferTypeDTO.SendMoney.ToString() ? moneyTranferTran.GrossTotalAmount : moneyTranferTran.TransactionAmount,
				GrossTotalAmount = tranTypeName == TransferTypeDTO.SendMoney.ToString() ? moneyTranferTran.GrossTotalAmount : moneyTranferTran.AmountToReceiver,
				ToFirstName = moneyTranferTran.Receiver.FirstName,
				ToMiddleName = moneyTranferTran.Receiver.MiddleName,
				ToLastName = moneyTranferTran.Receiver.LastName,
				ToSecondLastName = moneyTranferTran.Receiver.SecondLastName,
				ToAddress = moneyTranferTran.Receiver.Address,
				ToCity = moneyTranferTran.Receiver.City,
				ToState_Province = moneyTranferTran.Receiver.State_Province,
				ToZipCode = moneyTranferTran.Receiver.ZipCode,
				ToPhoneNumber = moneyTranferTran.Receiver.PhoneNumber,
				ToPickUpCountry = moneyTranferTran.DestinationCountryCode,
				ToPickUpState_Province = string.IsNullOrWhiteSpace(moneyTranferTran.Receiver.PickupState_Province) ? moneyTranferTran.ExpectedPayoutStateCode : moneyTranferTran.Receiver.PickupState_Province,
				ToPickUpCity = moneyTranferTran.Receiver.PickupCity,
				ToDeliveryMethod = moneyTranferTran.DeliveryServiceName,
				ToDeliveryOption = moneyTranferTran.DeliveryServiceName,
			};

			return tran;
		}

		private CXNTransaction PopulateBillPay(BillPayTransactionDTO billPayTran, long tranID)
		{
			CXNTransaction tran = new CXNTransaction()
			{
				ID = tranID.ToString(),
				Type = TransactionType.BillPay.ToString(),
				AccountNumber = billPayTran.AccountNumber,
				Payee = billPayTran.BillerName,
				MTCN = Convert.ToString(billPayTran.MetaData["MTCN"]),
				Amount = billPayTran.Amount,
				Fee = billPayTran.Fee,
				GrossTotalAmount = billPayTran.Amount + billPayTran.Fee
			};

			return tran;
		}

		private CXNTransaction PopulateCheck(CheckTransactionDTO cpTran, long tranID)
		{
			CXNTransaction tran = new CXNTransaction()
			{
				ID = tranID.ToString(),
				Type = TransactionType.Check.ToString(),
				CheckType = cpTran.CheckType,
				//TCFABA =
				//TCFAccount = 
				CheckNumber = cpTran.CheckNumber,
				ConfirmationNumber = cpTran.ConfirmationNumber,
				Amount = cpTran.Amount,
				Fee = cpTran.Fee,
				GrossTotalAmount = cpTran.Amount - cpTran.Fee
			};

			return tran;
		}

		private CXNTransaction PopulateMoneyOrder(MoneyOrderTransactionDTO moTran, long tranID)
		{
			CXNTransaction tran = new CXNTransaction()
			{
				ID = tranID.ToString(),
				Type = TransactionType.MoneyOrder.ToString(),
				CheckNumber = moTran.CheckNumber,
				Amount = moTran.Amount,
				Fee = moTran.Fee,
				GrossTotalAmount = moTran.Amount + moTran.Fee,
				Status = moTran.Status
			};

			return tran;
		}

		private CXNTransaction PopulateFunds(FundsTransactionDTO fundTrx, string fundTypeName, long tranID, long customerSessionId, bool isActivate, MGIContext mgiContext)
		{
			CXNTransaction tran = new CXNTransaction();

			pFunds trx = PtnrFundsSvc.Lookup(tranID);

			//ShoppingCartDTO PTNRshoppingCart = Get(customerSessionId);

			//CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
			//ChannelPartner channelPartner = CoreChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

			if (string.Compare(fundTypeName, "PrePaidLoad", true) == 0)
			{
				//FundsDTO load = PTNRshoppingCart.Funds.Where(x => x.TransactionType == CXNFundsType.Credit.ToString() && x.CXEState == (int)Core.CXE.Data.TransactionStates.Authorized).FirstOrDefault();

				tran.ID = Convert.ToString(tranID);
				tran.Type = TransactionType.Funds.ToString();
				tran.TransferType = fundTypeName;
				tran.InitialPurchase = isActivate ? "Y" : "N";
				tran.PurchaseFee = fundTrx.BaseFee;
				tran.NewCardBalance = FundsEngine.GetBalance(customerSessionId, mgiContext).Balance;
				tran.CardNumber = isActivate ? fundTrx.Account.FullCardNumber : fundTrx.Account.FullCardNumber.Substring(fundTrx.Account.FullCardNumber.Length - 6);
				tran.AliasId = fundTrx.Account.AccountNumber;
				tran.ConfirmationNumber = trx.ConfirmationNumber;
				tran.LoadAmount = trx.Amount;
				tran.Fee = trx.Fee;
				tran.GrossTotalAmount = trx.Amount - trx.Fee;
			}
			else if (string.Compare(fundTypeName, "PrePaidWithdraw", true) == 0)
			{
				//FundsDTO withdraw = PTNRshoppingCart.Funds.Where(x => x.TransactionType == CXNFundsType.Debit.ToString() && x.CXEState == (int)Core.CXE.Data.TransactionStates.Authorized).FirstOrDefault();

				//	CXNtranID = Convert.ToString(withdraw.CXNId);
				//}
				tran.ID = Convert.ToString(tranID); //GetTransactionID(customerSessionId, fundTypeName, feTran, shoppingcartFlush, isActivate);
				tran.TransferType = fundTypeName;
				tran.Type = TransactionType.Funds.ToString();
				tran.NewCardBalance = FundsEngine.GetBalance(customerSessionId, mgiContext).Balance;
				tran.CardNumber = isActivate ? fundTrx.Account.FullCardNumber : fundTrx.Account.FullCardNumber.Substring(fundTrx.Account.FullCardNumber.Length - 6);
				tran.AliasId = fundTrx.Account.AccountNumber;
				tran.ConfirmationNumber = trx.ConfirmationNumber;
				tran.WithdrawAmount = trx.Amount;
				tran.Fee = trx.Fee;
				tran.GrossTotalAmount = trx.Amount - trx.Fee;
			}

			return tran;
		}

		//No need to call the Transaction method since we get all values in ShoppingCartTransaction object itself.
		private CXNTransaction PopulateCash(ShoppingCartTransaction cashTran, string cashTypeName, long tranID)
		{
			CXNTransaction tran = new CXNTransaction();

			if (cashTran.Transaction != null)
			{
				tran.ID = tranID.ToString();
				tran.Type = TransactionType.Cash.ToString();
				tran.CashType = cashTypeName;
				tran.Amount = cashTran.Transaction.Amount;
				tran.Fee = cashTran.Transaction.Fee;
				tran.GrossTotalAmount = cashTran.Transaction.Amount - cashTran.Transaction.Fee;
			}

			return tran;
		}


		private pShoppingCart GetShoppingCartsByCustomerSessionID(long customerSessionId)
		{
			CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			if (session == null)
				return null;

			return session.ShoppingCarts.Where(s => s.IsParked == false).OrderByDescending(s => s.DTTerminalCreate).First();
		}


		private void PublishEvent(string channelPartner, BizPartnerData.PreFlushEvent preFlushEvent)
		{
			EventPublisher.Publish(channelPartner, preFlushEvent);
		}

		private void PublishEvent(string channelPartner, BizPartnerData.PostFlushEvent postFlushEvent)
		{
			EventPublisher.Publish(channelPartner, postFlushEvent);
		}

		private ReceiptDTO CheckoutFundsGenerating(long customerSessionId, ShoppingCartDTO cart, MGIContext mgiContext)
		{
			ReceiptDTO receipts = new ReceiptDTO();
			foreach (CheckDTO each in cart.Checks)
			{
				if (each.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized)
				{
					//Author : Abhijith
					//User Story : AL-3371
					//Description : As Tier 1 or Tier 2 support personnel, I need step-by-step transaction information for Check Processing.
					MongoDBLogger.Info<CheckDTO>(customerSessionId, each, "CheckoutFundsGenerating", AlloyLayerName.BIZ,
						ModuleName.ShoppingCart, "Begin CheckoutFundsGenerating - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
						mgiContext);
					//Ends Here

					//AL-594 changes : Checking for limits during checkout
					CheckProductsLimits(customerSessionId, each.Amount, TransactionTypes.Check, mgiContext);
					ICheckProcessor checkProcessor = _GetProcessor(mgiContext.ChannelPartnerName);
					Cxn.Check.Data.CheckLogin cxnCheckLogin = checkProcessor.GetCheckSessions(mgiContext);
					if (cxnCheckLogin != null)
					{
						mgiContext.URL = cxnCheckLogin.URL;
						mgiContext.IngoBranchId = cxnCheckLogin.BranchId;
						mgiContext.CompanyToken = cxnCheckLogin.CompanyToken;
						mgiContext.EmployeeId = cxnCheckLogin.EmployeeId;
					}
					receipts = CommitCheck(customerSessionId, each.Id.ToString(), mgiContext);

					//Author : Abhijith
					//User Story : AL-3371
					//Description : As Tier 1 or Tier 2 support personnel, I need step-by-step transaction information for Check Processing.
					MongoDBLogger.Info<CheckDTO>(customerSessionId, each, "CheckoutFundsGenerating", AlloyLayerName.BIZ,
						ModuleName.ShoppingCart, "End CheckoutFundsGenerating - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
						mgiContext);
					//Ends Here
				}
			}
			foreach (MoneyTransferDTO each in cart.MoneyTransfers)
			{
				if (each.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized &&
							(each.TransferType == ((int)TransferTypeDTO.ReceiveMoney) || each.TransferType == ((int)TransferTypeDTO.Refund))
					)
					CommitXfr(customerSessionId, each.Id, mgiContext);
			}
			return receipts;
		}

		private ReceiptDTO CommitCheck(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			CPEngineService.Commit(checkId, customerSessionId, mgiContext);
			//Biz.Partner.Data.Receipt receipt = CheckReceiptService.GetCheckReceipt(checkId);
			ReceiptDTO checkReceipt = new ReceiptDTO();
			//checkReceipt.Lines = receipt.Lines;
			return checkReceipt;
		}

		private int CommitXfr(long customerSessionId, long ptnrTransactionId, MGIContext mgiContext)
		{
			return MoneyTransferEngine.Commit(customerSessionId, ptnrTransactionId, mgiContext);
		}

		private ReceiptDTO CommitFunds(long customerSessionId, long transactionId, MGIContext mgiContext, string cardNumber = "")
		{
			//TBD remove after code changes
			FundsEngine.Commit(customerSessionId, transactionId, mgiContext, cardNumber);
			ReceiptDTO fundReceipt = new ReceiptDTO();
			//TODO:sending empty receipt as receipts are not ready for fund transaction
			//fundReceipt.Lines = FundReceiptService.GetFundsReceipt(transactionId.ToString()).Lines;
			return fundReceipt;
		}

		private bool IsCashOverCounter(long customerSessionId, MGIContext mgiContext)
		{
			// chk for channel partner config for CashOverCounter
			//MGI.Core.Partner.Data.CustomerSession customerSession = CustomerSessionSvc.Lookup(customerSessionId);
			//Biz.Partner.Data.ChannelPartner channelpartner = ChannelPartnerService.ChannelPartnerConfig(customerSession.AgentSession.Terminal.Location.ChannelPartnerId);

			Biz.Partner.Data.ChannelPartner channelpartner = ChannelPartnerService.ChannelPartnerConfig(Convert.ToInt32(mgiContext.ChannelPartnerId), mgiContext);

			return channelpartner.CashOverCounter;
		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		private long CashOut(long customerSessionId, decimal amount, MGIContext mgiContext)
		{
			long cxeCashTrxId = CashEngine.CashOut(customerSessionId, amount, mgiContext);
			//Commented by Bijo as a quick fix Cash to Customer issue in UAT which is same as DE2816
			AddCash(customerSessionId, cxeCashTrxId, mgiContext);
			int cashTrxnId;
			if (cxeCashTrxId > 0)
			{
				cashTrxnId = CommitCash(customerSessionId, cxeCashTrxId, mgiContext);
			}
			return cxeCashTrxId;
		}

		private int CommitCash(long customerSessionId, long cxeTxnId, MGIContext mgiContext)
		{
			return CashEngine.Commit(customerSessionId, cxeTxnId, mgiContext);
		}

		private ReceiptDTO CheckoutFundsDepleting(long customerSessionId, ShoppingCartDTO cart, MGIContext mgiContext)
		{

			if (string.IsNullOrEmpty(mgiContext.RequestType))
				mgiContext.RequestType = RequestTypeDTO.RELEASE.ToString();

			ReceiptDTO receipts = new ReceiptDTO();
			foreach (BillPayDTO each in cart.BillPays)
			{
				if (each.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized)
				{
					//AL-594 changes : Checking for limits during checkout
					CheckProductsLimits(customerSessionId, each.Amount, TransactionTypes.BillPay, mgiContext);
					CommitBillPayment(customerSessionId, each.Id, mgiContext);
				}
			}

			foreach (MoneyTransferDTO each in cart.MoneyTransfers)
			{
				if (each.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized && each.TransferType == ((int)TransferTypeDTO.SendMoney)
					&& string.IsNullOrEmpty(each.TransactionSubType))
				{
					//AL-594 changes : Checking for limits during checkout for sendmoney
					CheckProductsLimits(customerSessionId, each.Amount, TransactionTypes.MoneyTransfer, mgiContext);
					CommitXfr(customerSessionId, each.Id, mgiContext);
				}

				if (each.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized && each.TransferType == ((int)TransferTypeDTO.SendMoney)
					&& each.TransactionSubType == ((int)TransactionSubTypeDTO.Modify).ToString())
				{
					ModifyRequestDTO moneyTransferModify = new ModifyRequestDTO();

					moneyTransferModify.ModifyTransactionId = each.Id;

					if (cart.MoneyTransfers.Exists(x => x.TransactionSubType == ((int)TransactionSubTypeDTO.Cancel).ToString() && x.OriginalTransactionId == each.OriginalTransactionId))
					{
						moneyTransferModify.CancelTransactionId = cart.MoneyTransfers.FirstOrDefault(x => x.TransactionSubType == ((int)TransactionSubTypeDTO.Cancel).ToString() && x.OriginalTransactionId == each.OriginalTransactionId).Id;
					}
					//AL-594 changes : Checking for limits during checkout for sendmoney
					CheckProductsLimits(customerSessionId, each.Amount, TransactionTypes.MoneyTransfer, mgiContext);
					CommitXfrModify(customerSessionId, moneyTransferModify, mgiContext);
				}
			}

			CommitMoneyOrder(customerSessionId, cart, mgiContext);
			//	//TODO:sending empty receipts as receipts are not ready for the bill pay and money transfer
			return receipts;
		}

		private void CommitBillPayment(long channelPartnerId, long transactionId, MGIContext mgiContext)
		{
			BillPayService.Commit(channelPartnerId, transactionId, mgiContext);
		}


		public int CommitXfrModify(long customerSessionId, ModifyRequestDTO moneyTransferModify, MGIContext mgiContext)
		{
			return MoneyTransferEngine.Modify(customerSessionId, moneyTransferModify, mgiContext);
		}

		private void CommitMoneyOrder(long customerSessionId, ShoppingCartDTO shoppingCart, MGIContext context)
		{
			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<ShoppingCartDTO>(customerSessionId, shoppingCart, "CommitMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "Begin CommitMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				context);
			#endregion

			//AL-594 changes : Checking for limits during checkout for moneyorder
			if (shoppingCart.MoneyOrders.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized).Any())
			{
				Biz.Partner.Data.Transactions.MoneyOrder bMoneyOrder = shoppingCart.MoneyOrders.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Authorized).OrderBy(mo => mo.Id).First();
				CheckProductsLimits(customerSessionId, bMoneyOrder.Amount, TransactionTypes.MoneyOrder, context);
			}

			if (shoppingCart.MoneyOrders.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Processing).Any())
			{
				Biz.Partner.Data.Transactions.MoneyOrder bizMoneyOrder =
					shoppingCart.MoneyOrders.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Processing).First();

				if (bizMoneyOrder != null && bizMoneyOrder.Id > 0)
				{
					MoneyOrderEngineDTO cxeMoneyOrder = GetMoneyOrderStage(customerSessionId, bizMoneyOrder.Id, context);

					if (!string.IsNullOrEmpty(cxeMoneyOrder.CheckNumber))
					{
						CommitMoneyOrder(customerSessionId, bizMoneyOrder.Id, context);
						bizMoneyOrder.CXEState = (int)MGI.Core.CXE.Data.TransactionStates.Committed;
					}
				}
			}

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<ShoppingCartDTO>(customerSessionId, shoppingCart, "CommitMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "End CommitMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				context);
			#endregion
		}

		private void CommitMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrder Id : " + moneyOrderId;

			MongoDBLogger.Info<string>(customerSessionId, id, "CommitMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "Begin CommitMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion

			MoneyOrderEngineService.Commit(customerSessionId, moneyOrderId, mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<string>(customerSessionId, id, "CommitMoneyOrder", AlloyLayerName.BIZ,
				ModuleName.ShoppingCart, "End CommitMoneyOrder - MGI.Biz.Partner.Impl.ShoppingCartServiceImpl",
				mgiContext);
			#endregion
		}

		private MoneyOrderEngineDTO GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			var bizMoneyOrder = MoneyOrderEngineService.GetMoneyOrderStage(customerSessionId, moneyOrderId, mgiContext);

			return Mapper.Map<MoneyOrderEngineDTO>(bizMoneyOrder);
		}

		private void AddIfAny<T, V>(List<T> dto, IEnumerable<ShoppingCartTransaction> pTxns, TransactionType txnType)
			where T : TransactionDTO
			where V : pTransaction
		{
			Mapper.CreateMap<pTransaction, T>();
			IList<V> txns = null;
			if (pTxns.Any<ShoppingCartTransaction>(t => t.Transaction.Type == (int)txnType))
			{
				txns = pTxns.Where<ShoppingCartTransaction>(t => t.Transaction.Type == (int)txnType && t.CartItemStatus == ShoppingCartItemStatus.Added).ToList().ConvertAll<V>(t => (V)t.Transaction);
				dto.AddRange(Mapper.Map<IList<V>, IList<T>>(txns));
			}
		}

		private void AddTransaction(long customerSessionId, Transaction transaction)
		{
			CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
			string timezone = session.AgentSession.Terminal.Location.TimezoneID;

			if (!session.HasActiveShoppingCart())
				session.AddShoppingCart(timezone);

			if (!session.ActiveShoppingCart.ShoppingCartTransactions.Any(x => x.Transaction == transaction))
			{
				session.ActiveShoppingCart.AddTransaction(transaction);
			}
		}

		private void RemoveTransaction(long customerSessionId, Transaction transaction, bool isParkedTransaction = false)
		{
			CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
			if (isParkedTransaction)
				session.ParkingShoppingCart.RemoveTransaction(transaction);
			else
				session.ActiveShoppingCart.RemoveTransaction(transaction);
		}

		//US1488 Parking Transaction Changes
		private void ParkTransaction(long customerSessionId, Transaction transaction)
		{
			CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			string timezone = session.AgentSession.Terminal.Location.TimezoneID;

			if (session.ParkingShoppingCart == null)
				session.AddParkingShoppingCart(timezone);

			session.ActiveShoppingCart.ParkTransaction(transaction, session.ParkingShoppingCart);
		}

		private string GetOccupation(CXEEmploymentDetails custEmpDetails)
		{
			string strOccupation = custEmpDetails.Occupation;
			List<Occupation> occupations = PTNRDataStructureService.GetOccupations();

			if (custEmpDetails != null)
			{
				var occupation = occupations.SingleOrDefault(a => a.Code == custEmpDetails.Occupation);

				if (occupation != null)
				{
					strOccupation = occupation.Name;
				}
			}

			return strOccupation;
		}

		private long GetCXNCustomerAccount(long customerSessionId, MGIContext mgiContext)
		{
			long accountID = 0;

			CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);

			CXECustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);

			MGI.Core.Partner.Data.Customer customer = PartnerCustomerService.Lookup(customerSession.Customer.Id);

			string providername = CustomerRouter.GetProvider(mgiContext.ChannelPartnerName);

			if (!string.IsNullOrWhiteSpace(providername) && customer != null)
			{
				ProviderIds provider = (ProviderIds)Enum.Parse(typeof(ProviderIds), providername);
				var account = customer.GetAccount(Convert.ToInt32(provider));
				if (account != null)
					accountID = account.CXNId;
			}
			return accountID;
		}
		private ICheckProcessor _GetProcessor(string channelPartner)
		{
			// get the fund processor for the channel partner.
			return (ICheckProcessor)CheckProcessorRouter.GetProcessor(channelPartner);
		}

		private void CheckProductsLimits(long customerSessionId, decimal transactionAmount, TransactionTypes transactionType, MGIContext mgiContext)
		{
			CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
			decimal maximumAmount = LimitService.CalculateTransactionMaximumLimit(customerSessionId, session.Customer.ChannelPartner.ComplianceProgramName, transactionType, mgiContext);
			int productCode = GetProductCode(transactionType);
			if (transactionAmount > maximumAmount)
			{
				if (mgiContext.ChannelPartnerId != 0)
					mgiContext.ChannelPartnerId = 0;

				throw new BizComplianceLimitException(productCode.ToString(), BizComplianceLimitException.MAXIMUM_LIMIT_FAILED, transactionType.ToString(), transactionAmount);
			}
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
		/// Method to GetReferralPromotion for the Channelpartner
		/// </summary>
		/// <param name="channelpartner"></param>
		/// <returns></returns>
		private FeeAdjustment getReferralPromotion(MGI.Core.Partner.Data.ChannelPartner channelpartner)
		{
			List<FeeAdjustment> feeAdjustments = PTNRFeeAdjustmentService.Lookup(channelpartner);
			FeeAdjustment feeAdjustment = null;

			if (feeAdjustments != null && feeAdjustments.Count > 0)
			{
				feeAdjustment = feeAdjustments.Find(x => x.PromotionType != null && x.PromotionType.ToLower() == PromotionType.Referral.ToString().ToLower());
			}

			return feeAdjustment;
		}

		private bool IsPreFlushCallSent(long customerSessionId)
		{
			pShoppingCart cart = GetShoppingCartsByCustomerSessionID(customerSessionId);

			bool istrue = cart.ShoppingCartTransactions.Where(x => x.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Processing && x.Transaction.Type == (int)TransactionType.MoneyOrder).Any();
			return istrue;
		}

		private int GetProductCode(TransactionTypes transactionType)
		{
			int productCode = 1;
			switch (transactionType)
			{
				case TransactionTypes.BillPay:
					productCode = Convert.ToInt32(Biz.BillPay.Data.BizBillPayException.BillPayProductCode);
					break;
				case TransactionTypes.MoneyOrder:
					productCode = Convert.ToInt32(BizMoneyOrderEngineException.MOProductCode);
					break;
				case TransactionTypes.MoneyTransfer:
					productCode = Convert.ToInt32(BizMoneyTransferException.MoneyTransferProductCode);
					break;
				case TransactionTypes.Funds:
					productCode = Convert.ToInt32(BizFundsException.FundsProductCode);
					break;
			}
			return productCode;
		}

		#endregion
	}
}
