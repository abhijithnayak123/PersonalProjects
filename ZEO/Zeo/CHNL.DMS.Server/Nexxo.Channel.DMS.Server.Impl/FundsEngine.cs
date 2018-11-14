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
		public Response Withdraw(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = SharedEngine.WithdrawFunds(customerSessionId, funds, context);
			return response;
		}


		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "GPR lookup")]
		public Response LookupForPAN(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = SharedEngine.LookupFundsAccount(customerSessionId, context);
			return response;
		}

		[Transaction()]
		public Response GetFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = SharedEngine.GetFundsFee(customerSessionId, amount, fundsType, context);
			return response;
		}

		#endregion

		#region DMS Methods

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Add GPR account")]
		public Response Add(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsAccount bizFundsAccount = null;

			bizFundsAccount = Mapper.Map<BizFundsAccount>(fundsAccount);
			Response response = new Response();
			response.Result = BizFundsService.Add(customerSessionId, bizFundsAccount, context);

			return response;
		}

		// is this needed. There is a authenticate in ICustomerSerice.
		// if needed - need to return CustomerSession.
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Authenticate GPR card+pin")]
		public Response AuthenticateCard(long customerSessionId, string cardNumber, string pin, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			//TODO : Remove the code
			//TODO: What is the encription key used for. We do not use the pin to authenticate and hence no encryption required
			response.Result = BizFundsService.AuthenticateCard(customerSessionId, cardNumber, pin, "", context);
			return response;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Begin GPR load")]
		public Response Load(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsTrx bizFundsTrx = null;

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Funds>(customerSessionId, funds, "Load", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin Load - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion
			Response response = new Response();

			bizFundsTrx = Mapper.Map<BizFundsTrx>(funds);
			long cxeFundsTrxId = BizFundsService.Load(customerSessionId, bizFundsTrx, context);
			BIZShoppingCartService.AddFunds(customerSessionId, cxeFundsTrxId, context);
			response.Result = cxeFundsTrxId;

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cxeFundsTrxId), "Load", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End Load - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			return response;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Activate GPR card")]
		public Response ActivateGPRCard(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsTrx bizFundsTrx = null;

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Funds>(customerSessionId, funds, "ActivateGPRCard", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin ActivateGPRCard - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion
			Response response = new Response();
			bizFundsTrx = Mapper.Map<BizFundsTrx>(funds);
			long cxeFundsTrxId = BizFundsService.Activate(customerSessionId, bizFundsTrx, context);
			BIZShoppingCartService.AddFunds(customerSessionId, cxeFundsTrxId, context);
			response.Result = cxeFundsTrxId;

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cxeFundsTrxId), "ActivateGPRCard", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End ActivateGPRCard - MGI.Channel.DMS.Server.Impl.FundsEngine",
				context);
			#endregion

			return response;
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Get GPR balance")]
		public Response GetBalance(long customerSessionId, MGIContext mgiContext)
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
			Response response = new Response();

			response.Result = Mapper.Map<Data.CardInfo>(cardInfo);
			return response;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Update GPR amount")]
		public Response UpdateFundAmount(long customerSessionId, long cxeFundTrxId, decimal amount, FundType fundType, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = BizFundsService.UpdateAmount(cxeFundTrxId, amount, customerSessionId, Mapper.Map<BizFundType>(fundType), context);
			return response;
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
		public Response GetMinimumLoadAmount(long customerSessionId, bool initialLoad, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = BizFundsService.GetMinimumLoadAmount(customerSessionId, initialLoad, context);
			return response;

		}

		[Transaction()]
		public Response GetCardTransactionHistory(long customerSessionId, TransactionHistoryRequest request, MGIContext mgiContext)
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
			Response response = new Response();

			response.Result = Mapper.Map<List<Data.CardTransactionHistory>>(transactionHistoryList);
			return response;
		}

		[Transaction()]
		public Response CloseAccount(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			response.Result = BizFundsService.CloseAccount(customerSessionId, context);
			return response;
		}

		[Transaction()]
		public Response UpdateCardStatus(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
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

			Response response = new Response();

			response.Result = BizFundsService.UpdateCardStatus(customerSessionId, bizReplaceCard, context);
			return response;
		}

		[Transaction()]
		public Response ReplaceCard(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
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

			Response response = new Response();

			response.Result = BizFundsService.ReplaceCard(customerSessionId, bizReplaceCard, context);
			return response;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Funds, "Get Card Shipping Types")]
		public Response GetShippingTypes(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = Mapper.Map<List<ShippingTypes>>(BizFundsService.GetShippingTypes(context));
			return response;
		}

		[Transaction()]
		public Response GetShippingFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizCardMaintenanceInfo bizCardMaintenanceInfo = Mapper.Map<BizCardMaintenanceInfo>(cardMaintenanceInfo);
			Response response = new Response();

			response.Result = BizFundsService.GetShippingFee(bizCardMaintenanceInfo, context);
			return response;
		}

		[Transaction()]
		public Response AssociateCard(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsAccount bizFundsAccount = Mapper.Map<BizFundsAccount>(fundsAccount);
			Response response = new Response();

			response.Result = BizFundsService.AssociateCard(customerSessionId, bizFundsAccount, context);
			return response;
		}

		[Transaction()]
		public Response GetFundFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizCardMaintenanceInfo bizCardMaintenanceInfo = Mapper.Map<BizCardMaintenanceInfo>(cardMaintenanceInfo);
			Response response = new Response();

			response.Result = BizFundsService.GetFundFee(bizCardMaintenanceInfo, context);
			return response;
		}

		[Transaction()]
		public Response IssueAddOnCard(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsTrx bizFundsTrx = Mapper.Map<BizFundsTrx>(funds);
			long cxeFundsTrxId = BizFundsService.IssueAddOnCard(customerSessionId, bizFundsTrx, context);
			BIZShoppingCartService.AddFunds(customerSessionId, cxeFundsTrxId, context);
			Response response = new Response();
			response.Result = cxeFundsTrxId;
			return response;
		}

		#endregion

		#endregion

	}
}
