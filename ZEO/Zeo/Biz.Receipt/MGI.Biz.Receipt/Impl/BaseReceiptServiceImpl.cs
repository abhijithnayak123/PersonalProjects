using AutoMapper;
using MGI.Biz.Receipt.Contract;
using MGI.Common.Util;
using MGI.Core.Partner.Contract;
using MGI.Cxn.Common.Processor.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CoreCXEContract = MGI.Core.CXE.Contract;
using CoreCXEData = MGI.Core.CXE.Data;
using CorePtnrData = MGI.Core.Partner.Data;
using CXNCheckContract = MGI.Cxn.Check.Contract;
using CXNFundsContract = MGI.Cxn.Fund.Contract;
using CXNCheckData = MGI.Cxn.Check.Data;
using CXNFundData = MGI.Cxn.Fund.Data;
using CXNMoneyTransferContract = MGI.Cxn.MoneyTransfer.Contract;
using CXNMoneyTransferData = MGI.Cxn.MoneyTransfer.Data;
using CXNBillPayContract = MGI.Cxn.BillPay.Contract;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;
using BizFundsData = MGI.Biz.FundsEngine.Data;
using BizPtrnData = MGI.Biz.Partner.Data;
using BizPtrnContract = MGI.Biz.Partner.Contract;
using MGI.Core.Partner.Data.Fees;
using MGI.Core.Partner.Data;
using MGI.Cxn.Check.Contract;
using MGI.Common.TransactionalLogging.Data;
using MGI.Biz.Receipt.Data;

namespace MGI.Biz.Receipt.Impl
{
	public class BaseReceiptServiceImpl : IReceiptService
	{
		#region Dependencies
		protected PTNRContract.IChannelPartnerService _channelPartnerSvc;
		public PTNRContract.IChannelPartnerService ChannelPartnerSvc
		{
			get { return _channelPartnerSvc; }
			set { _channelPartnerSvc = value; }
		}

		private PTNRContract.ITransactionService<PTNRData.Transactions.Check> _ptnrCheckSvc;
		public PTNRContract.ITransactionService<PTNRData.Transactions.Check> PtnrCheckSvc
		{
			get { return _ptnrCheckSvc; }
			set { _ptnrCheckSvc = value; }
		}

		protected PTNRContract.ITransactionService<PTNRData.Transactions.BillPay> _ptnrBillPaySvc;
		public PTNRContract.ITransactionService<PTNRData.Transactions.BillPay> PtnrBillPaySvc
		{
			get { return _ptnrBillPaySvc; }
			set { _ptnrBillPaySvc = value; }
		}

		private PTNRContract.ITransactionService<PTNRData.Transactions.Funds> _ptnrFundsSvc;
		public PTNRContract.ITransactionService<PTNRData.Transactions.Funds> PtnrFundsSvc
		{
			get { return _ptnrFundsSvc; }
			set { _ptnrFundsSvc = value; }
		}

		protected PTNRContract.ITransactionService<PTNRData.Transactions.MoneyTransfer> _ptnrXferSvc;
		public PTNRContract.ITransactionService<PTNRData.Transactions.MoneyTransfer> PtnrXferSvc
		{
			get { return _ptnrXferSvc; }
			set { _ptnrXferSvc = value; }
		}

		private PTNRContract.ITransactionService<PTNRData.Transactions.MoneyOrder> _ptnrMOSvc;
		public PTNRContract.ITransactionService<PTNRData.Transactions.MoneyOrder> PtnrMOSvc
		{
			get { return _ptnrMOSvc; }
			set { _ptnrMOSvc = value; }
		}

		private PTNRContract.ITransactionService<PTNRData.Transactions.Cash> _ptnrCashSvc;
		public PTNRContract.ITransactionService<PTNRData.Transactions.Cash> PtnrCashSvc
		{
			get { return _ptnrCashSvc; }
			set { _ptnrCashSvc = value; }
		}

		public ICustomerSessionService CustomerSessionService { private get; set; }
		public MGI.Biz.Partner.Contract.IManageUsers ManageUserService { private get; set; }

		protected string removeLine = "{RemoveLine}";

		protected ReceiptTemplateRepo _receiptRepo;
		public ReceiptTemplateRepo ReceiptRepo
		{
			get { return _receiptRepo; }
			set { _receiptRepo = value; }
		}

		private CoreCXEContract.IMoneyOrderService _cxeMoneyOrderSvc;
		public CoreCXEContract.IMoneyOrderService CxeMoneyOrderSvc
		{
			get { return _cxeMoneyOrderSvc; }
			set { _cxeMoneyOrderSvc = value; }
		}

		public CoreCXEContract.ICustomerService CxeCustomerSvc { protected get; set; }

		public IProcessorRouter ProcessorSvc { private get; set; }

		public IProcessorRouter MoneyTransferProcessorSvc { private get; set; }

		public IProcessorRouter BillPayProcessorRouter { private get; set; }

		public Dictionary<string, string> TimeZones { get; set; }

		public MGI.Biz.Partner.Contract.IShoppingCartService PtnrShoppingCartSvc { get; set; }

		public MGI.Core.Partner.Contract.IFeeAdjustmentService PTNRFeeAdjustmentService { get; set; }

		public IProcessorRouter CheckProcessorRouter { private get; set; }

		public MGI.Common.Util.TLoggerCommon MongoDBLogger { protected get; set; }
		#endregion

		public BaseReceiptServiceImpl()
		{
			Mapper.CreateMap<PTNRData.Terminal, BizPtrnData.Terminal>();
			Mapper.CreateMap<BizPtrnData.Terminal, PTNRData.Terminal>();
		}

		#region IReceiptService Members

		public virtual List<Data.Receipt> GetCheckReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			try
			{
				#region AL-3371 Transactional Log User Story(Process check)
				List<string> details = new List<string>();
				details.Add("AgentSession Id:" + Convert.ToString(agentSessionId));
				details.Add("Transaction Id:" + Convert.ToString(transactionId));
				details.Add("Is Reprint :" + (isReprint ? "Yes" : "No"));
				MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetCheckReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "Begin GetCheckReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion

				PTNRData.Transactions.Check check = _ptnrCheckSvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(check.CustomerSession.AgentSession.Terminal);


				ChannelPartner channelpartner = ChannelPartnerSvc.ChannelPartnerConfig(terminal.Location.ChannelPartnerId);

				//Get Provider for Check
				ProviderIds provider = (ProviderIds)Enum.Parse(typeof(ProviderIds), _GetCheckProvider(channelpartner.Name));

				string receiptContents = _receiptRepo.GetReceiptTemplate(channelpartner.Name, PTNRData.Transactions.TransactionType.Check, provider, string.Empty);

				string receiptContentstags = "";
				// replace the common transaction tags
				receiptContentstags = GetPartnerTags(agentSessionId, check, check.CustomerSession, mgiContext);

				// replace other transaction-specific receipt tags
				CXNCheckData.CheckTrx cxnCheck = _GetCheckProcessor(channelpartner.Name).Get(check.CXNId);

				StringBuilder transactionTags = new StringBuilder(receiptContentstags);

				transactionTags.Append("|{NetAmount}|" + (check.Amount - check.Fee).ToString("0.00"));
				transactionTags.Append("|{ConfirmationNo}|" + NexxoUtil.TrimString(cxnCheck.ConfirmationNumber, 6));
				transactionTags.Append("|{CheckType}|" + NexxoUtil.TrimString(cxnCheck.ReturnType.ToString(), 20));
				transactionTags.Append("|{Discount}|" + (check.DiscountApplied != 0 ? check.DiscountApplied.ToString("0.00") : removeLine));
				transactionTags.Append("|{DiscountName}|" + (check.DiscountApplied != 0 ? "(" + check.DiscountName + " Check Discount)" : removeLine));
				transactionTags.Append("|{Fee}|" + check.BaseFee.ToString("0.00"));

				#region AL-3371 Transactional Log User Story(Process check)
				MongoDBLogger.Info<PTNRData.Transactions.Check>(customerSessionId, check, "GetCheckReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "End GetCheckReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion

				return GetReceipt(receiptContents, transactionTags.ToString(), "Process Check Receipt");
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetCheckReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetCheckReceipt-MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.CHECK_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);

			}
		}

		public virtual List<Data.Receipt> GetFundsReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			try
			{
				#region AL-3372 transaction information for GPR cards.
				List<string> details = new List<string>();
				details.Add("AgentSession Id:" + Convert.ToString(agentSessionId));
				details.Add("Transaction Id:" + Convert.ToString(transactionId));
				details.Add("Is Reprint :" + (isReprint ? "Yes" : "No"));
				MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetFundsReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "Begin GetFundsReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion

				PTNRData.Transactions.Funds fundtrx = _ptnrFundsSvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(fundtrx.CustomerSession.AgentSession.Terminal);

				string receiptContents = _receiptRepo.GetFundsReceiptTemplate(getChannelPartnerName(terminal),
					(BizFundsData.FundType)fundtrx.FundType, (ProviderIds)fundtrx.Account.ProviderId, string.Empty);

				string transactionTags = appendFundsTags(agentSessionId, fundtrx, mgiContext);

				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<PTNRData.Transactions.Funds>(customerSessionId, fundtrx, "GetFundsReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "End GetFundsReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion


				return GetReceipt(receiptContents, transactionTags, "Funds Receipt");
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetFundsReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetFundsReceipt-MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.FUNDS_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}

		}
		//TODO Change Message
		public virtual List<Data.Receipt> GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
            throw new BizReceiptException(BizReceiptException.RECEIPT_EXCEPTION);
		}
		//TODO Change Message
		public virtual List<Data.Receipt> GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
            throw new BizReceiptException(BizReceiptException.RECEIPT_EXCEPTION);
		}

		public virtual List<Data.Receipt> GetMoneyOrderReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			try
			{
				#region AL-1071 Transactional Log class for MO flow
				List<string> details = new List<string>();
				details.Add("AgentSession Id:" + Convert.ToString(agentSessionId));
				details.Add("Transaction Id:" + Convert.ToString(transactionId));
				details.Add("Is Reprint :" + (isReprint ? "Yes" : "No"));
				MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetMoneyOrderReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "Begin GetMoneyOrderReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion

				PTNRData.Transactions.MoneyOrder moneyOrder = _ptnrMOSvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(moneyOrder.CustomerSession.AgentSession.Terminal);

				string receiptContents = _receiptRepo.GetReceiptTemplate(getChannelPartnerName(terminal), PTNRData.Transactions.TransactionType.MoneyOrder,
					(ProviderIds)moneyOrder.Account.ProviderId, string.Empty);

				string receiptContentstags = GetPartnerTags(agentSessionId, moneyOrder, moneyOrder.CustomerSession, mgiContext);
				// Append other transaction-specific receipt tags
				CoreCXEData.Transactions.Commit.MoneyOrder cxeMoneyOrder = _cxeMoneyOrderSvc.Get(moneyOrder.Id);

				StringBuilder transactionTags = new StringBuilder(receiptContentstags);

				transactionTags.Append("|{MONumber}|" + cxeMoneyOrder.MoneyOrderCheckNumber);
				transactionTags.Append("|{Amount}|" + cxeMoneyOrder.Amount.ToString("0.00"));

				transactionTags.Append("|{Discount}|" + (Math.Abs(moneyOrder.DiscountApplied) > 0 ? Math.Abs(moneyOrder.DiscountApplied).ToString("0.00") : removeLine));
				transactionTags.Append("|{DiscountName}|" + (Math.Abs(moneyOrder.DiscountApplied) > 0 ? "(" + moneyOrder.DiscountName + " Check Discount)" : removeLine));
				transactionTags.Append("|{Fee}|" + moneyOrder.BaseFee.ToString("0.00"));

				transactionTags.Append("|{NetAmount}|" + (cxeMoneyOrder.Amount + cxeMoneyOrder.Fee).ToString("0.00"));

				#region AL-1071 Transactional Log class for MO flow
				MongoDBLogger.Info<PTNRData.Transactions.MoneyOrder>(customerSessionId, moneyOrder, "GetMoneyOrderReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "End GetMoneyOrderReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion


				return GetReceipt(receiptContents, transactionTags.ToString(), "Money order Receipt");
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetMoneyOrderReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetMoneyOrderReceipt-MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.MONEYORDER_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);

			}

		}

		//TODO NEED TO WORK
		public virtual List<Data.Receipt> GetFundsActivationReceipt(long agentSessionId, long customerSessionId, List<BizPtrnData.Transactions.Funds> fundTransactions, MGIContext mgiContext)
		{
			try
			{

				////AL-6023 
				int provider = (int)Enum.Parse(typeof(ProviderIds), _GetFundProvider(mgiContext.ChannelPartnerName));

				PTNRData.Transactions.Funds fundactivatetrx = _ptnrFundsSvc.Lookup(fundTransactions.Where(x => x.TransactionType == BizFundsData.FundType.None.ToString()).FirstOrDefault().Id);
				PTNRData.Transactions.Funds fundLoadtrx = null;

				if (fundTransactions.Any(x => x.TransactionType == BizFundsData.FundType.Credit.ToString()))
					fundLoadtrx = _ptnrFundsSvc.Lookup(fundTransactions.Where(x => x.TransactionType == BizFundsData.FundType.Credit.ToString()).FirstOrDefault().Id);

				// get the load or activate transaction. If load trx is not available consider activate trx.
				PTNRData.Transactions.Funds fundtrx = fundLoadtrx == null ? fundactivatetrx : fundLoadtrx;

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(fundtrx.CustomerSession.AgentSession.Terminal);

				decimal netAmount = fundactivatetrx.Fee + (fundLoadtrx == null ? 0 : fundLoadtrx.Amount);

				// set the confirmation number based on either activate trx or load trx.

				//Code to get Providerid for GPR based on ChannelPartner
				string channelPartnerName = _channelPartnerSvc.ChannelPartnerConfig(fundtrx.CustomerSession.Customer.ChannelPartnerId).Name;

				ProviderIds providerId = (ProviderIds)Enum.Parse(typeof(ProviderIds), _GetFundProvider(channelPartnerName));

				// assumption is activate trx will be there, when we print activation receipt.
				string receiptContents = _receiptRepo.GetFundsReceiptTemplate(getChannelPartnerName(terminal), (BizFundsData.FundType)fundactivatetrx.FundType, providerId, string.Empty);

				// use the fundtrx object, which will have either fund activation trx or fund load trx.

				//AL-6023
				//CXNFundData.CardAccount cardAccount = _GetProcessor(channelPartnerName).Lookup(fundtrx.CustomerSession.Customer.FindAccountByCXEId(fundtrx.Account.CXEId).CXNId);

				CXNFundData.CardAccount cardAccount = _GetProcessor(channelPartnerName).Lookup(fundtrx.CustomerSession.Customer.FindAccountByCXEId(fundtrx.Account.CXEId, provider).CXNId);

				CXNFundData.ProcessorResult processorResult = null;

				decimal latestCardBalance = _GetProcessor(channelPartnerName).GetBalance(cardAccount.Id, mgiContext, out processorResult).Balance;

				string fundsreceiptContents = GetPartnerTags(agentSessionId, fundtrx, fundtrx.CustomerSession, mgiContext);

				// get the activation fee from fund activate trx
				string CardNumber = string.Empty;
				if (!string.IsNullOrEmpty(cardAccount.CardNumber))
					CardNumber = cardAccount.CardNumber.Length > 4 ? cardAccount.CardNumber.Substring(cardAccount.CardNumber.Length - 4) : cardAccount.CardNumber;
				StringBuilder transactionTags = new StringBuilder(fundsreceiptContents);
				transactionTags.Append("|{ActivationFee}|" + fundactivatetrx.Fee.ToString("0.00"));
				transactionTags.Append("|{CardNumber}|" + CardNumber);
				transactionTags.Append("|{CurrentBalance}|" + latestCardBalance.ToString("0.00"));
				transactionTags.Append("|{NetAmount}|" + (netAmount.ToString("0.00")));
				transactionTags.Append("|{ConfirmationNo}|" + fundLoadtrx.ConfirmationNumber);
				transactionTags.Append("|{Amount}|" + fundactivatetrx.Amount.ToString("0.00"));

				return GetReceipt(receiptContents, transactionTags.ToString(), "Fund Activation Receipt");
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetFundsActivationReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetFundsActivationReceipt-MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.FUNDS_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);

			}

		}
		// Get Fund Provider based on ChannelPartner
		private string _GetFundProvider(string channelPartnerName)
		{
			// get the fund provider for the channel partner.
			return ProcessorSvc.GetProvider(channelPartnerName);
		}


		public virtual List<Data.Receipt> GetSummaryReceipt(BizPtrnData.ShoppingCart cart, long customerSessionId, MGIContext mgiContext)
		{
			try
			{
				return GetSummaryreceipt(cart, customerSessionId, mgiContext);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetSummaryReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetSummaryReceipt-MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.SUMMARY_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);

			}
		}

		public virtual List<Data.Receipt> GetSummaryReceipt(long agentSessionId, long customerSessionId, long transactionId, string transactiontype, MGIContext mgiContext)
		{
			try
			{
				#region AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(transactionId));
				details.Add("Transaction Type:" + transactiontype);
				MongoDBLogger.ListInfo<string>(mgiContext.CustomerSessionId, details, "GetSummaryReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "Begin GetSummaryReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion
				PTNRData.Transactions.Transaction trx = null;

				if (transactiontype.ToLower().Contains("check"))
				{
					trx = PtnrCheckSvc.Lookup(transactionId);
				}
				else if (transactiontype.ToLower().StartsWith("gpr") || transactiontype.ToLower().StartsWith("prepaid"))
				{
					trx = PtnrFundsSvc.Lookup(transactionId);
				}
				else if (transactiontype.ToLower().Contains("send") || transactiontype.ToLower().Contains("receive"))
				{
					trx = PtnrXferSvc.Lookup(transactionId);
				}
				else if (transactiontype.ToLower().Contains("moneyorder"))
				{
					trx = PtnrMOSvc.Lookup(transactionId);
				}
				else if (transactiontype.ToLower().Contains("billpay"))
				{
					trx = PtnrBillPaySvc.Lookup(transactionId);
				}
				else if (transactiontype.ToLower().Contains("cash"))
				{
					trx = PtnrCashSvc.Lookup(transactionId);
				}

				BizPtrnData.ShoppingCart cart = PtnrShoppingCartSvc.PopulateCart(trx.CustomerSession.AgentSession.Id, trx.CustomerSession.ShoppingCarts.Where(c => (c.ShoppingCartTransactions.Where(d => d.Transaction.Id == transactionId).Count() == 1)).First(), mgiContext);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, string.Empty, "GetSummaryReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "End GetSummaryReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion
				return GetSummaryreceipt(cart, trx.CustomerSession.Id, mgiContext);

			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetSummaryReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetSummaryReceipt-MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.SUMMARY_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);

			}
		}

		public virtual List<Data.Receipt> GetDoddFrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			throw new BizReceiptException(BizReceiptException.RECEIPT_EXCEPTION);
		}

		public List<Data.Receipt> GetCheckDeclinedReceiptData(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{

			try
			{
				#region AL-3371 Transactional Log User Story(Process check)
				List<string> details = new List<string>();
				details.Add("AgentSession Id:" + Convert.ToString(agentSessionId));
				details.Add("Transaction Id:" + Convert.ToString(transactionId));
				MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetCheckDeclinedReceiptData", AlloyLayerName.BIZ,
					ModuleName.Receipt, "Begin GetCheckDeclinedReceiptData - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion

				PTNRData.Transactions.Check check = _ptnrCheckSvc.Lookup(transactionId);
				//Get Provider for Check
				ProviderIds provider = (ProviderIds)Enum.Parse(typeof(ProviderIds), _GetCheckProvider(mgiContext.ChannelPartnerName));

				string receiptContents = _receiptRepo.GetDeclinedReceiptTemplate(mgiContext.ChannelPartnerName, PTNRData.Transactions.TransactionType.Check, provider, string.Empty);

				string receiptContentstags = GetPartnerTags(agentSessionId, check, check.CustomerSession, mgiContext);

				// replace other transaction-specific receipt tags
				var checkProcessor = _GetCheckProcessor(mgiContext.ChannelPartnerName);
				CXNCheckData.CheckTrx cxnCheck = checkProcessor.Get(check.Id);

				StringBuilder transactionTags = new StringBuilder(receiptContentstags);
				transactionTags.Append("|{CheckNo}|" + NexxoUtil.GetDictionaryValueIfExists(cxnCheck.MetaData, "CheckNo"));
				transactionTags.Append("|{CertegyUID}|" + NexxoUtil.GetDictionaryValueIfExists(cxnCheck.MetaData, "CertegyUID"));
				transactionTags.Append("|{AccountNo}|" + NexxoUtil.GetDictionaryValueIfExists(cxnCheck.MetaData, "AccountNumber"));

				#region AL-3371 Transactional Log User Story(Process check)
				MongoDBLogger.Info<PTNRData.Transactions.Check>(customerSessionId, check, "GetCheckDeclinedReceiptData", AlloyLayerName.BIZ,
					ModuleName.Receipt, "End GetCheckDeclinedReceiptData - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion


				return GetReceipt(receiptContents, transactionTags.ToString(), "Check declined Receipt");
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetCheckDeclinedReceiptData", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetCheckDeclinedReceiptData-MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.CHECKDECLINED_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);

			}
		}


		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added new method to Get CouponReceipt for the ChannelPartner
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="mgiContext"></param>
		/// <returns></returns>
		public virtual List<Data.Receipt> GetCouponReceipt(long customerSessionId, MGIContext mgiContext)
		{
			try
			{

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, string.Empty, "GetCouponReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "Begin GetCouponReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion
				string couponCode = string.Empty;
				PTNRData.CustomerSession CustomerSession = GetCustomerSession(customerSessionId);

				if (CustomerSession.Customer.CXEId != 0)
					couponCode = CustomerSession.Customer.CXEId.ToString().Substring(CustomerSession.Customer.CXEId.ToString().Length - 8);

				ChannelPartner channelpartner = _channelPartnerSvc.ChannelPartnerConfig(CustomerSession.Customer.ChannelPartnerId);

				FeeAdjustment feeAdjustment = getReferralPromotion(channelpartner);

				string receiptContents = _receiptRepo.GetCouponCodeReceiptTemplate(channelpartner.Name);

				string receiptContentstags = "";
				receiptContentstags = GetPartnerTags(long.Parse(CustomerSession.AgentSession.AgentId), null, CustomerSession, mgiContext);

				StringBuilder appendreceipttags = new StringBuilder(receiptContentstags);
				appendreceipttags.Append("|{CouponCode}|" + couponCode);

				string promoName = feeAdjustment != null ? feeAdjustment.Name : string.Empty;
				string promoDesc = feeAdjustment != null ? feeAdjustment.Description : string.Empty;

				appendreceipttags.Append("|{PromoName}|" + promoName);
				appendreceipttags.Append("|{PromoDescriptionOfReferee}|" + promoDesc);
				appendreceipttags.Append("|{PromoDescriptionOfReferrer}|" + promoDesc);

				appendreceipttags.Append("|{data}|" + receiptContents);
				string tags = appendreceipttags.ToString();
				if (tags.Substring(0, 1) == "|")
					tags = tags.Remove(0, 1);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, string.Empty, "GetCouponReceipt", AlloyLayerName.BIZ,
					ModuleName.Receipt, "End GetCouponReceipt - MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl",
					mgiContext);
				#endregion
				return GetReceipt(receiptContents, tags, "Coupon Receipt");
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetCouponReceipt", AlloyLayerName.BIZ, ModuleName.Receipt,
									  "GetCouponReceipt-MGI.Biz.Receipt.Impl.BaseReceiptServiceImpl", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.COUPON_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}
		}

		#endregion

		#region protected methods-Common across child Implementation

		protected string GetChannelPartnerName(Guid channelPartnerId)
		{
			return _channelPartnerSvc.ChannelPartnerConfig(channelPartnerId).Name;
		}

		/// <summary>
		/// Getting PartnerTags -Same Method is Using for All methods 
		/// </summary>
		protected string GetPartnerTags(long agentSessionId, PTNRData.Transactions.Transaction trx, PTNRData.CustomerSession customerSession, MGIContext mgiContext)
		{
			PTNRData.ChannelPartner partner = _channelPartnerSvc.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

			BizPtrnData.UserDetails agent = ManageUserService.GetUser(agentSessionId, (int)customerSession.AgentSession.Agent.Id, mgiContext);

			string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;
			string timezoneFmt = TimeZones.Where(y => y.Key == timezone).FirstOrDefault().Value;

			DateTime trxDate;
			if (trx == null)
			{
				trxDate = GetTransactionSummaryDate(customerSession);
			}
			else
			{
				trxDate = trx.DTTerminalLastModified != null ? trx.DTTerminalLastModified.Value : trx.DTTerminalCreate;
			}

			CultureInfo cultureinfo = new CultureInfo("es-ES");

			StringBuilder appendreceipttags = new StringBuilder();
			appendreceipttags.Append("|{ClientName}|" + partner.Name);
			appendreceipttags.Append("|{LogoUrl}|" + partner.LogoFileName);
			appendreceipttags.Append("|{LocationAddress}|" + string.Format("{0} {1}", customerSession.AgentSession.Terminal.Location.Address1, customerSession.AgentSession.Terminal.Location.Address2));
			appendreceipttags.Append("|{City}|" + customerSession.AgentSession.Terminal.Location.City);
			appendreceipttags.Append("|{Phonenumber}|" + partner.PhoneNumber);
			appendreceipttags.Append("|{State}|" + customerSession.AgentSession.Terminal.Location.State);
			appendreceipttags.Append("|{Zip}|" + customerSession.AgentSession.Terminal.Location.ZipCode);
			appendreceipttags.Append("|{BranchId}|" + customerSession.AgentSession.Terminal.Location.BranchID);
			appendreceipttags.Append("|{BankId}|" + customerSession.AgentSession.Terminal.Location.BankID);
			appendreceipttags.Append("|{TerminalID}|" + customerSession.AgentSession.Terminal.Id.ToString());
			appendreceipttags.Append("|{TellerName}|" + NexxoUtil.TrimString(agent.UserName, 5));
			appendreceipttags.Append("|{SessionlID}|" + customerSession.Id.ToString());
			appendreceipttags.Append("|{TerminalDate}|" + customerSession.DTStart.ToLongDateString());
			appendreceipttags.Append("|{Amount}|" + (trx == null ? string.Empty : trx.Amount.ToString("0.00")));
			if (!(trx != null && (trx.Type == 2 || trx.Type == 3 || trx.Type == 5)))
			{
				appendreceipttags.Append("|{Fee}|" + (trx == null ? string.Empty : trx.Fee.ToString("0.00")));
			}
			appendreceipttags.Append("|{ConfirmationId}|" + (trx == null || string.IsNullOrEmpty(trx.ConfirmationNumber) ? string.Empty : trx.ConfirmationNumber.ToString()));
			appendreceipttags.Append("|{TransactionId}|" + (trx == null ? string.Empty : trx.Id.ToString()));

			// appendreceipttags.Append("|{ReceiptDate}|" + (trx == null ? (dtCreate.ToString("MMMM dd yyyy") + " / " + string.Format("{0} {1}", dtCreate.ToString("hh:mm tt"), timezoneFmt)) : (trx.DTCreate.ToString("MMMM dd yyyy") + " / " + string.Format("{0}",  timezoneFmt))));
			appendreceipttags.Append("|{ReceiptDate}|" + (trxDate.ToString("MMMM dd, yyyy")));
			appendreceipttags.Append("|{Currency}|" + "$");

			appendreceipttags.Append("|{TellerNumber}|" + agent.ClientAgentIdentifier);

			//appendreceipttags.Append("|{TxrDate}|" + (trxDate.ToString("MMMM dd yyyy") + string.Format(" / {0}", cultureinfo.TextInfo.ToTitleCase(trxDate.ToString("MMMM dd yyyy", cultureinfo)))));
			//appendreceipttags.Append("|{TxrTime}|" + string.Format("{0} {1}", trxDate.ToString("hh:mm tt"), timezoneFmt));

			CoreCXEData.Customer customer = CxeCustomerSvc.Lookup(customerSession.Customer.CXEId);
			appendreceipttags.Append("|{CustomerName}|" + string.Format("{0} {1}", customer.FirstName, customer.LastName));
			appendreceipttags.Append("|{LocationName}|" + customerSession.AgentSession.Terminal.Location.LocationName);
			appendreceipttags.Append("|{LocationPhoneNumber}|" + customerSession.AgentSession.Terminal.Location.PhoneNumber);
			if (appendreceipttags.ToString().Substring(0, 1) == "|")
				appendreceipttags.Remove(0, 1).ToString();
			return appendreceipttags.ToString();
		}


		/// <summary>
		/// Author : Abhijith
		/// Description : Added this method for the defect - "Summary Receipt printing current date instead of transaction date."
		/// </summary>
		/// <param name="customerSession"></param>
		/// <returns></returns>
		private DateTime GetTransactionSummaryDate(PTNRData.CustomerSession customerSession)
		{
			string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;
			string timezoneFmt = TimeZones.Where(y => y.Key == timezone).FirstOrDefault().Value;
			DateTime summaryDate = DateTime.Now;

			var transactionDetails = customerSession.ShoppingCarts.Where(y => y.IsParked == false).Last().ShoppingCartTransactions.Where(x => x.Transaction.CXEState == (int)PTNRTransactionStates.Committed).ToList();

			if (transactionDetails != null)
			{
				var trx = transactionDetails.OrderByDescending(x => x.Transaction.DTTerminalCreate).FirstOrDefault();
				summaryDate = trx.Transaction.DTTerminalLastModified != null ? trx.Transaction.DTTerminalLastModified.Value : trx.Transaction.DTTerminalCreate;

			}

			return summaryDate;
		}

		protected string getChannelPartnerName(BizPtrnData.Terminal terminal)
		{
			//extra lookupsneeded to work a bad NHibernate class setup
			//PTNRData.Terminal terminal = _terminalSvc.Lookup(terminalPK);
			PTNRData.ChannelPartner p = _channelPartnerSvc.ChannelPartnerConfig(terminal.Location.ChannelPartnerId);
			return p.Name;
		}

		protected string FormatMTCN(string mtcn)
		{
			return string.IsNullOrWhiteSpace(mtcn) ? string.Empty : mtcn.Insert(3, "-").Insert(7, "-");
		}
		protected CXNMoneyTransferContract.IMoneyTransfer _GetMoneyTransferProcessor(string channelPartner)
		{
			// get the moneytransfer processor for the channel partner.
			return (CXNMoneyTransferContract.IMoneyTransfer)MoneyTransferProcessorSvc.GetProcessor(channelPartner);
		}

		protected CXNBillPayContract.IBillPayProcessor _GetBillPayProcessor(string channelPartnerName, ProviderIds providerId)
		{
			return (CXNBillPayContract.IBillPayProcessor)BillPayProcessorRouter.GetProcessor(channelPartnerName, providerId.ToString());
		}

		protected virtual string GetPayOutCountry(CXNMoneyTransferContract.IMoneyTransfer moneyTransferProcessor, MGI.Cxn.MoneyTransfer.Data.Transaction cxntran)
		{
			string payOutCountry = string.Empty;
			var receiveCountry = moneyTransferProcessor.GetCountries().FirstOrDefault(c => c.Code.ToLower() == cxntran.DestinationCountryCode.ToLower());
			if (receiveCountry != null)
			{
				payOutCountry = receiveCountry.Name;
			}
			return payOutCountry;
		}

		protected List<Data.Receipt> GetReceipt(string receiptContent, string transactionTags, string receiptName)
		{
			Data.Receipt receiptData = new Data.Receipt();
			if (!string.IsNullOrEmpty(receiptContent))
			{
				receiptContent = transactionTags + "|{data}|" + receiptContent;

				if (receiptContent.Substring(0, 1) == "|")
					receiptContent = receiptContent.Remove(0, 1);

				receiptData.PrintData = receiptContent;
			}
			else
				receiptData.PrintData = string.Empty;

			receiptData.Name = receiptName;

			receiptData.NumberOfCopies = 1;

			return new List<Data.Receipt>() { receiptData };
		}

		#endregion

		#region private methods

		private ICheckProcessor _GetCheckProcessor(string channelPartner)
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

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added method to Get IsReferralApplicable for the ChannelPartner
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		private FeeAdjustment getReferralPromotion(ChannelPartner channelpartner)
		{
			List<FeeAdjustment> feeAdjustments = PTNRFeeAdjustmentService.Lookup(channelpartner);
			FeeAdjustment feeAdjustment = new FeeAdjustment();

			if (feeAdjustments != null && feeAdjustments.Count > 0)
			{
				feeAdjustment = feeAdjustments.Find(x => x.PromotionType != null && x.PromotionType.ToLower() == PromotionType.Referral.ToString().ToLower());
			}

			return feeAdjustment;
		}

		private CXNFundsContract.IFundProcessor _GetProcessor(string channelPartner)
		{
			// get the fund processor for the channel partner.
			return (CXNFundsContract.IFundProcessor)ProcessorSvc.GetProcessor(channelPartner);
		}

		private string appendFundsTags(long agentSessionId, PTNRData.Transactions.Funds trx, MGIContext mgiContext)
		{
			//AL-6023 
			int providerId = (int)Enum.Parse(typeof(ProviderIds), _GetFundProvider(mgiContext.ChannelPartnerName));

			//AL-6023
			//long cxnId = trx.CustomerSession.Customer.FindAccountByCXEId(trx.Account.CXEId).CXNId;

			long cxnId = trx.CustomerSession.Customer.FindAccountByCXEId(trx.Account.CXEId, providerId).CXNId;

			CXNFundData.CardAccount cardAccount = _GetProcessor(mgiContext.ChannelPartnerName).Lookup(cxnId);
			//CXNFundData.ProcessorResult processorResult = null;

			//AL-6023
			//decimal latestCardBalance = _GetProcessor(mgiContext.ChannelPartnerName).GetBalance(cxnId, mgiContext, out processorResult).Balance;
			decimal latestCardBalance = 0;

			string fundsreceiptContents = GetPartnerTags(agentSessionId, trx, trx.CustomerSession, mgiContext);

			BizFundsData.FundType trxType = (BizFundsData.FundType)trx.FundType;

			decimal netamount = 0;

			//TODO: Previous Card Balance From where ??? 
			decimal? previousCardBalance = _GetProcessor(mgiContext.ChannelPartnerName).Get(trx.CXNId, mgiContext).PreviousCardBalance;
			previousCardBalance = previousCardBalance == null ? 0 : previousCardBalance;

			CoreCXEData.Customer addOnCustomer = new CoreCXEData.Customer();
			if (trxType == BizFundsData.FundType.AddOnCard && trx.AddOnCustomerId != 0)
				addOnCustomer = CxeCustomerSvc.Lookup(trx.AddOnCustomerId);

			StringBuilder appendreceipttags = new StringBuilder(fundsreceiptContents);
			switch (trxType)
			{
				case MGI.Biz.FundsEngine.Data.FundType.Debit:
					latestCardBalance = (decimal)(previousCardBalance - trx.Amount);
					appendreceipttags.Append("|{DiscountName}|" + (trx.DiscountApplied > 0 ? "(" + trx.DiscountName + " Withdraw Discount)" : removeLine));
					netamount = trx.Amount;
					break;
				case MGI.Biz.FundsEngine.Data.FundType.Credit:
					latestCardBalance = (decimal)(previousCardBalance + trx.Amount);
					appendreceipttags.Append("|{DiscountName}|" + (trx.DiscountApplied > 0 ? "(" + trx.DiscountName + " Load Discount)" : removeLine));
					netamount = trx.Amount;
					break;
				case MGI.Biz.FundsEngine.Data.FundType.None:
					appendreceipttags.Append("|{DiscountName}|" + (trx.DiscountApplied > 0 ? "(" + trx.DiscountName + " Activate Discount)" : removeLine));
					netamount = trx.Amount + trx.Fee;
					break;
			}

			appendreceipttags.Append("|{PrevCardBalance}|" + Convert.ToDecimal(previousCardBalance).ToString("0.00"));
			appendreceipttags.Append("|{Amount}|" + trx.Amount.ToString("0.00"));
			appendreceipttags.Append("|{CurrentBalance}|" + latestCardBalance.ToString("0.00"));

			if (trxType != BizFundsData.FundType.None)
				appendreceipttags.Append("|{Fee}|" + trx.Fee.ToString("0.00"));
			else
				appendreceipttags.Append("|{ActivationFee}|" + trx.Fee.ToString("0.00"));

			appendreceipttags.Append("|{Discount}|" + (trx.DiscountApplied > 0 ? trx.DiscountApplied.ToString("0.00") : removeLine));
			appendreceipttags.Append("|{Fee}|" + trx.BaseFee.ToString("0.00"));

			appendreceipttags.Append("|{NetAmount}|" + netamount.ToString("0.00"));
			appendreceipttags.Append("|{CustomerName}|" + string.Format("{0} {1}", cardAccount.FirstName, cardAccount.LastName));
			appendreceipttags.Append("|{CardNumber}|" + (cardAccount.CardNumber.Length > 4 ? cardAccount.CardNumber.Substring(cardAccount.CardNumber.Length - 4) : cardAccount.CardNumber));
			appendreceipttags.Append("|{ConfirmationNo}|" + trx.ConfirmationNumber);
			if (addOnCustomer != null)
				appendreceipttags.Append("|{CompanionName}|" + string.Format("{0} {1}", addOnCustomer.FirstName, addOnCustomer.LastName));

			return appendreceipttags.ToString();
		}

		private MGI.Core.Partner.Data.CustomerSession GetCustomerSession(long customerSessionId)
		{
			try
			{
				MGI.Core.Partner.Data.CustomerSession session = CustomerSessionService.Lookup(customerSessionId); // TODO - don't ned the heavy weight object lookup. Must be a way to check session validity based on time. (Important)

				if (session == null)
				{
                    throw new MGI.Biz.Partner.Data.BizCustomerException(MGI.Biz.Partner.Data.BizCustomerException.CUSTOMERSESSION_NOT_FOUND);
				}

				return session;
			}
			catch
			{ // need to add appropriate exception here...
                throw new MGI.Biz.Partner.Data.BizCustomerException(MGI.Biz.Partner.Data.BizCustomerException.CUSTOMERSESSION_NOT_FOUND);
			}
		}

		private List<Data.Receipt> GetSummaryreceipt(BizPtrnData.ShoppingCart cart, long customerSessionId, MGIContext mgiContext)
		{
			string gpr = "NonGpr";
			PTNRData.Account gprAccount = null;

			PTNRData.CustomerSession CustomerSession = GetCustomerSession(customerSessionId);

			BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(CustomerSession.AgentSession.Terminal);

			string provider = _GetFundProvider(terminal.ChannelPartner.Name);

			if (!string.IsNullOrEmpty(provider))
			{
				//Get ProviderId based on ChannelPartner for Fund
				ProviderIds providerId = (ProviderIds)Enum.Parse(typeof(ProviderIds), provider);

				gprAccount = CustomerSession.Customer.GetAccount((int)providerId);

				gpr = gprAccount == null ? "NonGpr" : "Gpr";
			}

			string receiptContents = _receiptRepo.GetShoppingCartSummaryReceiptTemplate(getChannelPartnerName(terminal), gpr, string.Empty);

			// cash generating txn
			decimal processedCheckTotal = cart.CheckTotal - cart.Checks.Sum(x => x.Fee);
			decimal moneyTransferReceiver = 0;
			decimal prepaidWithDraw = cart.Funds.Where(c => c.TransactionType == BizFundsData.FundType.Debit.ToString()).Sum(c => c.Amount);

			decimal moneyTransferRefund = cart.MoneyTransfers.Where(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Refund).Sum(c => c.Amount) + cart.MoneyTransfers.Where(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Refund).Sum(c => c.Fee);

			// cash depleting txn
			decimal billpayTotal = cart.BillTotal;
			decimal moneyOrder = (cart.MoneyOrders == null) ? 0 : cart.MoneyOrderTotal + cart.MoneyOrders.Sum(x => x.Fee);

			decimal moneyTransferSend = cart.MoneyTransfers.Where(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send && string.IsNullOrEmpty(c.TransactionSubType)).Sum(c => c.Amount) + cart.MoneyTransfers.Where(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send && string.IsNullOrEmpty(c.TransactionSubType)).Sum(c => c.Fee);

			decimal moneyTransferModify = cart.MoneyTransfers.Where(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send && c.TransactionSubType == ((int)CXNMoneyTransferData.TransactionSubType.Modify).ToString()).Sum(c => c.Amount) + cart.MoneyTransfers.Where(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send && c.TransactionSubType == ((int)CXNMoneyTransferData.TransactionSubType.Modify).ToString()).Sum(c => c.Fee);
			decimal moneyTransferCancel = 0;

			if (cart.MoneyTransfers.Exists(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send && c.TransactionSubType == ((int)CXNMoneyTransferData.TransactionSubType.Modify).ToString()))
			{
				foreach (var moneytransfer in cart.MoneyTransfers.Where(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send && c.TransactionSubType == ((int)CXNMoneyTransferData.TransactionSubType.Modify).ToString()))
				{
					MGI.Biz.Partner.Data.Transactions.MoneyTransfer cancelMoneyTransfer = cart.MoneyTransfers.Where(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send && c.TransactionSubType == ((int)CXNMoneyTransferData.TransactionSubType.Cancel).ToString() && c.OriginalTransactionId == moneytransfer.OriginalTransactionId).First();
					moneyTransferCancel = +(cancelMoneyTransfer.Amount + cancelMoneyTransfer.Fee);
				}
			}

			decimal moneyTransferReceive = cart.MoneyTransfers.Where(c => c.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Receive).Sum(c => c.Amount); // Fees has to be considered ?
			decimal prepaidLoad = cart.Funds.Where(c => c.TransactionType == BizFundsData.FundType.Credit.ToString()).Sum(c => c.Amount);
			decimal prepaidActivate = cart.Funds.Where(c => c.TransactionType == BizFundsData.FundType.None.ToString()).Sum(c => (c.Fee + c.Amount));
			bool IsAddOn = cart.Funds.Any(c => c.TransactionType == BizFundsData.FundType.AddOnCard.ToString());


			decimal fundsGeneratingTotalAmount = processedCheckTotal + moneyTransferReceiver + prepaidWithDraw + moneyTransferReceive + moneyTransferCancel + moneyTransferRefund;
			decimal fundsDepletingTotalAmount = billpayTotal + moneyTransferSend + moneyOrder + prepaidLoad + prepaidActivate + moneyTransferModify;

			decimal TotalAmountDue = fundsGeneratingTotalAmount - fundsDepletingTotalAmount;
			decimal cashPaymentReceived = cart.CashTotal;
			decimal cashToCustomer = cashPaymentReceived + TotalAmountDue;

			string summaryreceiptContents = GetPartnerTags(long.Parse(CustomerSession.AgentSession.AgentId), null, CustomerSession, mgiContext);
			string channelPartnerName = _channelPartnerSvc.ChannelPartnerConfig(CustomerSession.Customer.ChannelPartnerId).Name;
			StringBuilder transactionTags = new StringBuilder(summaryreceiptContents);
			// funds generating txn
			transactionTags.Append("|{CheckCount}|" + cart.Checks.Count.ToString());

			transactionTags.Append("|{MOCount}|" + cart.MoneyOrders.Count.ToString());
			transactionTags.Append("|{BPCount}|" + cart.BillPays.Count.ToString());
			transactionTags.Append("|{SMCount}|" + cart.MoneyTransfers.Where(i => i.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Send).Count().ToString());
			transactionTags.Append("|{RMCount}|" + cart.MoneyTransfers.Where(i => i.TransferType == (int)CXNMoneyTransferData.MoneyTransferType.Receive).Count().ToString());


			transactionTags.Append("|{CheckTotal}|" + (cart.Checks.Count > 0 ? processedCheckTotal.ToString("0.00") : removeLine));
			transactionTags.Append("|{GPRWithDraw}|" + (prepaidWithDraw > 0 ? prepaidWithDraw.ToString("0.00") : removeLine));
			transactionTags.Append("|{FundsGeneratingTotal}|" + fundsGeneratingTotalAmount.ToString("0.00"));

			//funds depleting txn
			transactionTags.Append("|{GPRLoad}|" + (prepaidLoad > 0 ? prepaidLoad.ToString("-0.00") : removeLine));
			transactionTags.Append("|{GPRActivate}|" + (prepaidActivate > 0 ? prepaidActivate.ToString("-0.00") : removeLine));
			transactionTags.Append("|{MoneyOrder}|" + (moneyOrder > 0 ? moneyOrder.ToString("-0.00") : removeLine));
			transactionTags.Append("|{FundsDepletingTotal}|" + fundsDepletingTotalAmount.ToString("-0.00"));
			transactionTags.Append("|{GPRCompanion}|" + (IsAddOn ? "0.00" : removeLine));

			transactionTags.Append("|{NetAmount}|" + Math.Abs(TotalAmountDue).ToString("0.00"));

			transactionTags.Append("|{TotalMsg}|" + (TotalAmountDue > 0 ? "TOTAL DUE TO CUSTOMER" : "TOTAL AMOUNT DUE "));

			transactionTags.Append("|{CashCollected}|" + cashPaymentReceived.ToString("0.00"));
			transactionTags.Append("|{CashToCustomer}|" + cashToCustomer.ToString("0.00"));

			transactionTags.Append("|{Currency}|" + "$");
			string cardNumber = string.Empty;
			if (gprAccount != null)
			{
				CXNFundData.CardAccount cardAccount = _GetProcessor(channelPartnerName).Lookup(gprAccount.CXNId);
				if (cardAccount != null && !string.IsNullOrWhiteSpace(cardAccount.CardNumber))
				{
					cardNumber = cardAccount.CardNumber.Length > 4 ? cardAccount.CardNumber.Substring(cardAccount.CardNumber.Length - 4) : cardAccount.CardNumber;

				}
			}
			transactionTags.Append("|{CardNumber}|" + cardNumber);

			transactionTags.Append("|{MoneyTransferSend}|" + (moneyTransferSend > 0 ? moneyTransferSend.ToString("-0.00") : removeLine));
			transactionTags.Append("|{BillPay}|" + (billpayTotal > 0 ? billpayTotal.ToString("-0.00") : removeLine));
			transactionTags.Append("|{MoneyTransferReceive}|" + (moneyTransferReceive > 0 ? moneyTransferReceive.ToString("0.00") : removeLine));
			transactionTags.Append("|{MoneyTransferModified}|" + (moneyTransferModify > 0 ? moneyTransferModify.ToString("-0.00") : removeLine));
			transactionTags.Append("|{MoneyTransferCancelled}|" + (moneyTransferCancel > 0 ? moneyTransferCancel.ToString("0.00") : removeLine));
			transactionTags.Append("|{MoneyTransferRefund}|" + (moneyTransferRefund > 0 ? moneyTransferRefund.ToString("0.00") : removeLine));

			return GetReceipt(receiptContents, transactionTags.ToString(), "");
		}

		#endregion



	}
}
