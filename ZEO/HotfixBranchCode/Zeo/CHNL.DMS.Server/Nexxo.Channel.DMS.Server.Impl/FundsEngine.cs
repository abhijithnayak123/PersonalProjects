using System;
using System.Linq;
using System.Collections.Generic;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;

using AutoMapper;

using BizFundsService = MGI.Biz.FundsEngine.Contract.IFundsEngine;
using BizFundsAccount = MGI.Biz.FundsEngine.Data.FundsAccount;
using BizFundsTrx = MGI.Biz.FundsEngine.Data.Funds;
using AgentSessionSvc = MGI.Biz.Partner.Contract.IAgentService;
using CustomerSessionSvc = MGI.Core.Partner.Contract.ICustomerSessionService;
using CoreCustomerSession = MGI.Core.Partner.Data.CustomerSession;
using BizFundType = MGI.Biz.FundsEngine.Data.FundType;
using BizCardMaintenanceInfo = MGI.Biz.FundsEngine.Data.CardMaintenanceInfo;
using BizCardShippingTypes = MGI.Biz.FundsEngine.Data.ShippingTypes;
using BizCommon = MGI.Biz.Common.Data;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public partial class DesktopEngine : IFundsProcessorService
	{
		public BizFundsService BizFundsService { private get; set; }
		public AgentSessionSvc BizAgentSessionSvc { private get; set; }
		public CustomerSessionSvc CustomerSessionSvc { private get; set; }

		#region IFundsProcessorService Impl

		#region Shared Methods

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Begin GPR withdraw")]
		public long Withdraw(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.WithdrawFunds(customerSessionId, funds, context);

			//BizFundsTrx bizFundsTrx = null;
			//context = _GetContext(customerSessionId);
			//bizFundsTrx = Mapper.Map<BizFundsTrx>(funds);
			//long cxeFundsTrxId = BizFundsService.Withdraw(customerSessionId, bizFundsTrx, context);
			//BIZShoppingCartService.AddFunds(customerSessionId, cxeFundsTrxId);
			//return cxeFundsTrxId;
		}

		//Moved to shared
		//[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		//[DMSMethodAttribute(DMSFunctionalArea.Funds, "Commit GPR transaction")]
		//public Receipt CommitFunds(long customerSessionId, long transactionId, MGIContext context, string cardNumber = "")
		//{
		//    context = _GetContext(customerSessionId);

		//    BizFundsService.Commit(customerSessionId, transactionId, context, cardNumber);
		//    Receipt fundReceipt = new Receipt();
		//    //TODO:sending empty receipt as receipts are not ready for fund transaction
		//    //fundReceipt.Lines = FundReceiptService.GetFundsReceipt(transactionId.ToString()).Lines;
		//    return fundReceipt;
		//}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "GPR lookup")]
		public FundsProcessorAccount LookupForPAN(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.LookupFundsAccount(customerSessionId, context);

			//context = _GetContext(customerSessionId);
			//try
			//{
			//    return Mapper.Map<FundsProcessorAccount>(BizFundsService.GetAccount(customerSessionId, context));
			//}
			//catch (System.Exception ex)
			//{
			//    MGI.Biz.FundsEngine.Contract.BizFundsException bizFundsex = ((MGI.Biz.FundsEngine.Contract.BizFundsException)ex);
			//    if (bizFundsex != null && bizFundsex.MajorCode == 1003 && (bizFundsex.MinorCode == 2019 || bizFundsex.MinorCode == 6003))
			//        //for the first time there can be a scenario where there will not be an account for funds
			//        return null;
			//    else
			//        throw ex;
			//}
		}

		[Transaction()]
		public TransactionFee GetFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetFundsFee(customerSessionId, amount, fundsType, context);

			//BizCommon.TransactionFee fee = BizFundsService.GetFee(customerSessionId, amount, Mapper.Map<BizFundType>(fundsType));
			//return Mapper.Map<TransactionFee>(fee);
		}

		#endregion

		#region DMS Methods

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Add GPR account")]
		public long Add(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsAccount bizFundsAccount = null;

			bizFundsAccount = Mapper.Map<BizFundsAccount>(fundsAccount);

			return BizFundsService.Add(customerSessionId, bizFundsAccount, context);
		}

		// is this needed. There is a authenticate in ICustomerSerice.
		// if needed - need to return CustomerSession.
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Authenticate GPR card+pin")]
		public bool AuthenticateCard(long customerSessionId, string cardNumber, string pin, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			//TODO : Remove the code
			//TODO: What is the encription key used for. We do not use the pin to authenticate and hence no encryption required
			return BizFundsService.AuthenticateCard(customerSessionId, cardNumber, pin, "", context);
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Begin GPR load")]
		public long Load(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsTrx bizFundsTrx = null;

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Funds>(customerSessionId, funds, "Load", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin Load - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			bizFundsTrx = Mapper.Map<BizFundsTrx>(funds);
			long cxeFundsTrxId = BizFundsService.Load(customerSessionId, bizFundsTrx, context);
			BIZShoppingCartService.AddFunds(customerSessionId, cxeFundsTrxId, context);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cxeFundsTrxId), "Load", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End Load - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			return cxeFundsTrxId;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Activate GPR card")]
		public long ActivateGPRCard(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsTrx bizFundsTrx = null;

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Funds>(customerSessionId, funds, "ActivateGPRCard", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin ActivateGPRCard - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			bizFundsTrx = Mapper.Map<BizFundsTrx>(funds);
			long cxeFundsTrxId = BizFundsService.Activate(customerSessionId, bizFundsTrx, context);
			BIZShoppingCartService.AddFunds(customerSessionId, cxeFundsTrxId, context);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cxeFundsTrxId), "ActivateGPRCard", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End ActivateGPRCard - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			return cxeFundsTrxId;
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Get GPR balance")]
		public Data.CardInfo GetBalance(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetBalance", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin GetBalance - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			Biz.FundsEngine.Data.CardInfo cardInfo = BizFundsService.GetBalance(customerSessionId, context);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Biz.FundsEngine.Data.CardInfo>(customerSessionId, cardInfo, "GetBalance", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End GetBalance - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			return Mapper.Map<Data.CardInfo>(cardInfo);
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Update GPR amount")]
		public long UpdateFundAmount(long customerSessionId, long cxeFundTrxId, decimal amount, FundType fundType, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return BizFundsService.UpdateAmount(cxeFundTrxId, amount, customerSessionId, Mapper.Map<BizFundType>(fundType), context);
		}

		public static void FundEngineConverter()
		{
			Mapper.CreateMap<Funds, BizFundsTrx>();
			Mapper.CreateMap<FundsProcessorAccount, BizFundsAccount>();
			Mapper.CreateMap<BizFundsAccount, FundsProcessorAccount>();
			Mapper.CreateMap<FundType, BizFundType>();
			Mapper.CreateMap<BizCommon.TransactionFee, TransactionFee>();

			Mapper.CreateMap<Data.TransactionHistory, MGI.Biz.FundsEngine.Data.TransactionHistory>();
			Mapper.CreateMap<Data.TransactionHistoryRequest, MGI.Biz.FundsEngine.Data.TransactionHistoryRequest>();
			Mapper.CreateMap<MGI.Biz.FundsEngine.Data.TransactionHistory, Data.CardTransactionHistory>();
			Mapper.CreateMap<MGI.Biz.FundsEngine.Data.CardInfo, Data.CardInfo>();
			Mapper.CreateMap<Data.CardMaintenanceInfo, BizCardMaintenanceInfo>();
			Mapper.CreateMap<Biz.FundsEngine.Data.ShippingTypes, ShippingTypes>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="initialLoad"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Get GPR load minimum")]
		public decimal GetMinimumLoadAmount(long customerSessionId, bool initialLoad, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return BizFundsService.GetMinimumLoadAmount(customerSessionId, initialLoad, context);

		}

		[Transaction()]
		public List<CardTransactionHistory> GetCardTransactionHistory(long customerSessionId, TransactionHistoryRequest request, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<TransactionHistoryRequest>(customerSessionId, request, "GetCardTransactionHistory", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin GetCardTransactionHistory - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion
			
			MGI.Biz.FundsEngine.Data.TransactionHistoryRequest bizTranasctionHistoryRequest = Mapper.Map<MGI.Biz.FundsEngine.Data.TransactionHistoryRequest>(request);
			List<MGI.Biz.FundsEngine.Data.TransactionHistory> transactionHistoryList = BizFundsService.GetTransactionHistory(customerSessionId, bizTranasctionHistoryRequest, context);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.ListInfo<MGI.Biz.FundsEngine.Data.TransactionHistory>(customerSessionId, transactionHistoryList, "GetCardTransactionHistory", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End GetCardTransactionHistory - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			return Mapper.Map<List<Data.CardTransactionHistory>>(transactionHistoryList);
		}

		[Transaction()]
		public bool CloseAccount(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return BizFundsService.CloseAccount(customerSessionId, context);
		}

		[Transaction()]
		public bool UpdateCardStatus(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<CardMaintenanceInfo>(customerSessionId, cardMaintenanceInfo, "UpdateCardStatus", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin UpdateCardStatus - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			BizCardMaintenanceInfo bizReplaceCard = Mapper.Map<BizCardMaintenanceInfo>(cardMaintenanceInfo);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<BizCardMaintenanceInfo>(customerSessionId, bizReplaceCard, "UpdateCardStatus", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End UpdateCardStatus - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			return BizFundsService.UpdateCardStatus(customerSessionId, bizReplaceCard, context);
		}

		[Transaction()]
		public bool ReplaceCard(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<CardMaintenanceInfo>(customerSessionId, cardMaintenanceInfo, "ReplaceCard", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin ReplaceCard - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			BizCardMaintenanceInfo bizReplaceCard = Mapper.Map<BizCardMaintenanceInfo>(cardMaintenanceInfo);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<BizCardMaintenanceInfo>(customerSessionId, bizReplaceCard, "ReplaceCard", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End ReplaceCard - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			return BizFundsService.ReplaceCard(customerSessionId, bizReplaceCard, context);
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Get Card Shipping Types")]
		public List<ShippingTypes> GetShippingTypes(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return Mapper.Map<List<ShippingTypes>>(BizFundsService.GetShippingTypes(context));
		}

		[Transaction()]
		public double GetShippingFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizCardMaintenanceInfo bizCardMaintenanceInfo = Mapper.Map<BizCardMaintenanceInfo>(cardMaintenanceInfo);
			return BizFundsService.GetShippingFee(bizCardMaintenanceInfo, context);
		}

		[Transaction()]
		public long AssociateCard(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsAccount bizFundsAccount = Mapper.Map<BizFundsAccount>(fundsAccount);
			return BizFundsService.AssociateCard(customerSessionId, bizFundsAccount, context);
		}

		[Transaction()]
		public double GetFundFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizCardMaintenanceInfo bizCardMaintenanceInfo = Mapper.Map<BizCardMaintenanceInfo>(cardMaintenanceInfo);
			return BizFundsService.GetFundFee(bizCardMaintenanceInfo, context);
		}

		[Transaction()]
		public long IssueAddOnCard(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsTrx bizFundsTrx = Mapper.Map<BizFundsTrx>(funds);
			long cxeFundsTrxId = BizFundsService.IssueAddOnCard(customerSessionId, bizFundsTrx, context);
			BIZShoppingCartService.AddFunds(customerSessionId, cxeFundsTrxId, context);
			return cxeFundsTrxId;
		}
		
		#endregion

		#endregion

	}
}
