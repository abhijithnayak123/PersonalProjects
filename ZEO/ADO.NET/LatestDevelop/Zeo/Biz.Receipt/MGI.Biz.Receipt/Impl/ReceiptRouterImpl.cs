using AutoMapper;
using MGI.Core.Partner.Contract;
using MGI.Cxn.Common.Processor.Util;
using System.Collections.Generic;
using System.Linq;
using CoreCXEContract = MGI.Core.CXE.Contract;
using CXNCheckContract = MGI.Cxn.Check.Contract;
using PTNRData = MGI.Core.Partner.Data;
using BizReceiptContract = MGI.Biz.Receipt.Contract;
using BizFundsData = MGI.Biz.FundsEngine.Data;
using BizPtrnData = MGI.Biz.Partner.Data;
using MGI.Common.Util;
using MGI.Biz.Receipt.Data;

namespace MGI.Biz.Receipt.Impl
{
	public class ReceiptRouterImpl : BizReceiptContract.IReceiptService
	{
		#region Dependencies

		public BizReceiptContract.IReceiptService BaseReceiptService { private get; set; }
		public IProcessorRouter ReceiptProcessorRouter { private get; set; }

		private IChannelPartnerService _channelPartnerSvc;
		public IChannelPartnerService ChannelPartnerSvc
		{
			get { return _channelPartnerSvc; }
			set { _channelPartnerSvc = value; }
		}

		private ITransactionService<PTNRData.Transactions.Check> _ptnrCheckSvc;
		public ITransactionService<PTNRData.Transactions.Check> PtnrCheckSvc
		{
			get { return _ptnrCheckSvc; }
			set { _ptnrCheckSvc = value; }
		}

		private ITransactionService<PTNRData.Transactions.Funds> _ptnrFundsSvc;
		public ITransactionService<PTNRData.Transactions.Funds> PtnrFundsSvc
		{
			get { return _ptnrFundsSvc; }
			set { _ptnrFundsSvc = value; }
		}

		private ITransactionService<PTNRData.Transactions.BillPay> _ptnrBillPaySvc;
		public ITransactionService<PTNRData.Transactions.BillPay> PtnrBillPaySvc
		{
			get { return _ptnrBillPaySvc; }
			set { _ptnrBillPaySvc = value; }
		}

		private ITransactionService<PTNRData.Transactions.MoneyTransfer> _ptnrXferSvc;
		public ITransactionService<PTNRData.Transactions.MoneyTransfer> PtnrXferSvc
		{
			get { return _ptnrXferSvc; }
			set { _ptnrXferSvc = value; }
		}

		private ITransactionService<PTNRData.Transactions.MoneyOrder> _ptnrMOSvc;
		public ITransactionService<PTNRData.Transactions.MoneyOrder> PtnrMOSvc
		{
			get { return _ptnrMOSvc; }
			set { _ptnrMOSvc = value; }
		}

		#endregion

		public ReceiptRouterImpl()
		{
			Mapper.CreateMap<PTNRData.Terminal, BizPtrnData.Terminal>();
		}

		#region IReceiptService Public Members

		public List<Data.Receipt> GetCheckReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{

			try
			{
				PTNRData.Transactions.Check trx = _ptnrCheckSvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(trx.CustomerSession.AgentSession.Terminal);

				BizReceiptContract.IReceiptService processor = GetReceiptProcessor(GetChannelPartnerName(terminal), ((ProviderIds)trx.Account.ProviderId).ToString());

				return processor.GetCheckReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);

			}
			catch (System.Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.CHECK_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}
		}

		public List<Data.Receipt> GetFundsReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{

			try
			{
				PTNRData.Transactions.Funds trx = _ptnrFundsSvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(trx.CustomerSession.AgentSession.Terminal);

				BizReceiptContract.IReceiptService processor = GetReceiptProcessor(GetChannelPartnerName(terminal), ((ProviderIds)trx.Account.ProviderId).ToString());

				return processor.GetFundsReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
			}
			catch (System.Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.FUNDS_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}
		}

		public List<Data.Receipt> GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{

			try
			{
				PTNRData.Transactions.BillPay trx = _ptnrBillPaySvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(trx.CustomerSession.AgentSession.Terminal);

				BizReceiptContract.IReceiptService processor = GetReceiptProcessor(GetChannelPartnerName(terminal), ((ProviderIds)trx.Account.ProviderId).ToString());

				return processor.GetBillPayReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
			}
			catch (System.Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.BILLPAY_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}
		}

		public List<Data.Receipt> GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			try
			{
				PTNRData.Transactions.MoneyTransfer trx = _ptnrXferSvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(trx.CustomerSession.AgentSession.Terminal);

				BizReceiptContract.IReceiptService processor = GetReceiptProcessor(GetChannelPartnerName(terminal), ((ProviderIds)trx.Account.ProviderId).ToString());

				return processor.GetMoneyTransferReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
			}
			catch (System.Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.MT_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}

		}

		public List<Data.Receipt> GetMoneyOrderReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{

			try
			{
				PTNRData.Transactions.MoneyOrder trx = _ptnrMOSvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(trx.CustomerSession.AgentSession.Terminal);

				BizReceiptContract.IReceiptService processor = GetReceiptProcessor(GetChannelPartnerName(terminal), ((ProviderIds)trx.Account.ProviderId).ToString());

				return processor.GetMoneyOrderReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
			}
			catch (System.Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.MONEYORDER_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}
		}

		public List<Data.Receipt> GetDoddFrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			try
			{
				PTNRData.Transactions.MoneyTransfer trx = _ptnrXferSvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(trx.CustomerSession.AgentSession.Terminal);

				BizReceiptContract.IReceiptService processor = GetReceiptProcessor(GetChannelPartnerName(terminal), ((ProviderIds)trx.Account.ProviderId).ToString());

				return processor.GetDoddFrankReceipt(agentSessionId, customerSessionId, transactionId, mgiContext);
			}
			catch (System.Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.DODFRANK_RECEIPT_RETRIVEL_FAILED, ex);
			}
		}

		public List<Data.Receipt> GetCheckDeclinedReceiptData(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{

			try
			{
				PTNRData.Transactions.Check trx = _ptnrCheckSvc.Lookup(transactionId);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(trx.CustomerSession.AgentSession.Terminal);

				BizReceiptContract.IReceiptService processor = GetReceiptProcessor(GetChannelPartnerName(terminal), ((ProviderIds)trx.Account.ProviderId).ToString());

				return processor.GetCheckDeclinedReceiptData(agentSessionId, customerSessionId, transactionId, mgiContext);
			}
			catch (System.Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.CHECKDECLINED_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}

		}

		public List<Data.Receipt> GetFundsActivationReceipt(long agentSessionId, long customerSessionId, List<BizPtrnData.Transactions.Funds> fundTransactions, MGIContext mgiContext)
		{

			try
			{
				PTNRData.Transactions.Funds trx = _ptnrFundsSvc.Lookup(fundTransactions.Where(x => x.TransactionType == BizFundsData.FundType.None.ToString()).FirstOrDefault().Id);

				BizPtrnData.Terminal terminal = Mapper.Map<BizPtrnData.Terminal>(trx.CustomerSession.AgentSession.Terminal);

				BizReceiptContract.IReceiptService processor = GetReceiptProcessor(GetChannelPartnerName(terminal), ((ProviderIds)trx.Account.ProviderId).ToString());

				return processor.GetFundsActivationReceipt(agentSessionId, customerSessionId, fundTransactions, mgiContext);
			}
			catch (System.Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizReceiptException(BizReceiptException.FUNDS_RECEIPT_TEMPLATE_RETRIVEL_FAILED, ex);
			}

		}

		public List<Data.Receipt> GetSummaryReceipt(BizPtrnData.ShoppingCart cart, long customerSessionId, MGIContext mgiContext)
		{
			return BaseReceiptService.GetSummaryReceipt(cart, customerSessionId, mgiContext);
		}

		public List<Data.Receipt> GetSummaryReceipt(long agentSessionId, long customerSessionId, long transactionId, string transactiontype, MGIContext mgiContext)
		{
			return BaseReceiptService.GetSummaryReceipt(agentSessionId, customerSessionId, transactionId, transactiontype, mgiContext);
		}

		public List<Data.Receipt> GetCouponReceipt(long customerSessionId, MGIContext mgiContext)
		{
			return BaseReceiptService.GetCouponReceipt(customerSessionId, mgiContext);
		}

		#endregion

		#region private methods

		private BizReceiptContract.IReceiptService GetReceiptProcessor(string channelPartnerName, string providerName)
		{
			return (BizReceiptContract.IReceiptService)ReceiptProcessorRouter.GetProcessor(channelPartnerName, providerName) ?? BaseReceiptService;
		}

		private string GetChannelPartnerName(BizPtrnData.Terminal terminal)
		{
			PTNRData.ChannelPartner p = _channelPartnerSvc.ChannelPartnerConfig(terminal.Location.ChannelPartnerId);
			return p.Name;
		}

		#endregion




	}
}