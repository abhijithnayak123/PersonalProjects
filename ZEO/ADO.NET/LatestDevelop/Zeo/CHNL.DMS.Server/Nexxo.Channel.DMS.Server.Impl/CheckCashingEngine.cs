using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;

using MGI.Biz.CPEngine.Contract;
using BizData = MGI.Biz.CPEngine.Data;
using MGI.Biz.Partner.Contract;

using BizCommon = MGI.Biz.Common.Data;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : ICheckCashingService
	{
		public ICPEngineService CPEngineService { private get; set; }

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Check, "Get check status")]
		public Response GetCheckStatus(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

            #region AL-3371 Transactional Log User Story(Process check)
            string checkProcessId = "Check Id :" + Convert.ToString(checkId);

			MongoDBLogger.Info<string>(customerSessionId, checkProcessId, "GetCheckStatus", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "Begin GetCheckStatus - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

            var bizCheck = CPEngineService.GetStatus(customerSessionId, checkId, false, context);

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<BizData.Check>(customerSessionId, bizCheck, "GetCheckStatus", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "End GetCheckStatus - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion


			response.Result = Mapper.Map<Check>(bizCheck);
			return response;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Check, "Submit check")]
		public Response SubmitCheck(long customerSessionId, CheckSubmission check, MGIContext mgiContext)
		{
			var bizCheckSubmission = Mapper.Map<BizData.CheckSubmission>(check);
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<CheckSubmission>(customerSessionId, check, "SubmitCheck", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "Begin SubmitCheck - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			var bizCheck = CPEngineService.Submit(bizCheckSubmission, customerSessionId, context);
			if (bizCheck.Status != "Declined")
				BIZShoppingCartService.AddCheck(Convert.ToInt64(customerSessionId), Convert.ToInt64(bizCheck.Id), context);

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<BizData.Check>(customerSessionId, bizCheck, "SubmitCheck", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "End SubmitCheck - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			response.Result = Mapper.Map<Check>(bizCheck);
			return response;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Check, "CanResubmit")]
		public Response CanResubmit(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			response.Result = CPEngineService.CanResubmit(customerSessionId, checkId, context);
			return response;
		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		[DMSMethodAttribute(DMSFunctionalArea.Check, "Commit check")]
		public Response CommitCheck(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

            #region AL-3371 Transactional Log User Story(Process check)
            string checkProcessId = "Check Id :" + Convert.ToString(checkId);

			MongoDBLogger.Info<string>(customerSessionId, checkProcessId, "CommitCheck", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "Begin CommitCheck - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			CPEngineService.Commit(checkId, customerSessionId, context);
			Receipt checkReceipt = new Receipt();

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<string>(customerSessionId, checkProcessId, "CommitCheck", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "End CommitCheck - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			response.Result = checkReceipt;
			return response;
		}

        [Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
        [DMSMethodAttribute(DMSFunctionalArea.Check, "Cancel check")]
        public Response CancelCheck(long customerSessionId, string checkId, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
            MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

            #region AL-3371 Transactional Log User Story(Process check)
            string checkProcessId = "Check Id :" + Convert.ToString(checkId);

			MongoDBLogger.Info<string>(customerSessionId, checkProcessId, "CancelCheck", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "Begin CancelCheck - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

            CPEngineService.Cancel(customerSessionId, checkId, context);

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<string>(customerSessionId, checkProcessId, "CancelCheck", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "End CancelCheck - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			return new Response();
        }

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Check, "GetCheckTypes")]
		public Response GetCheckTypes(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			response.Result = CPEngineService.GetCheckTypes(customerSessionId, context);
			return response;
		}

		[Transaction()]
		public Response GetCheckFee(long customerSessionId, CheckSubmission checkSubmit, MGIContext mgiContext)
		{
			var bizCheckSubmission = Mapper.Map<BizData.CheckSubmission>(checkSubmit);
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<CheckSubmission>(customerSessionId, checkSubmit, "GetCheckFee", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "Begin GetCheckFee - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			BizCommon.TransactionFee fee = CPEngineService.GetFee(customerSessionId, bizCheckSubmission, context);

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<BizCommon.TransactionFee>(customerSessionId, fee, "GetCheckFee", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "End GetCheckFee - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			response.Result = Mapper.Map<TransactionFee>(fee);
			return response;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Check, "Get check details")]
		public Response GetCheckTranasactionDetails(long agentSessionId, long customerSessionId, string checkId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			Response response = new Response();
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			response.Result = Mapper.Map<CheckTransactionDetails>(CPEngineService.GetTransaction(agentSessionId, customerSessionId, checkId, context));
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response GetCheckFrankingData(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = CPEngineService.GetCheckFrankingData(customerSessionId, transactionId, context);
			return response;
		}

		[Transaction()]
		public Response UpdateTransactionFranked(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

            #region AL-3371 Transactional Log User Story(Process check)
            string tranId = "Transaction Id :" + Convert.ToString(transactionId);

			MongoDBLogger.Info<string>(customerSessionId, tranId, "UpdateTransactionFranked", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "Begin UpdateTransactionFranked - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			CPEngineService.UpdateTransactionFranked(customerSessionId, transactionId, context);

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<string>(customerSessionId, tranId, "UpdateTransactionFranked", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "End UpdateTransactionFranked - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			return new Response();
		}

		internal static void CheckCashingConverter()
		{
			Mapper.CreateMap<BizData.Check, SharedData.Check>()
				.ForMember(c => c.StatusMessage, d => d.MapFrom(c => c.DmsDeclineMessage))
				.ForMember(c => c.Fee, d => d.MapFrom(c => c.ValidatedFee));
			Mapper.CreateMap<CheckSubmission, BizData.CheckSubmission>();
			Mapper.CreateMap<BizData.CheckTransaction, CheckTransactionDetails>();
			Mapper.CreateMap<BizCommon.TransactionFee, TransactionFee>();
			Mapper.CreateMap<BizData.CheckProcessorInfo, Data.CheckProcessorInfo>();
			Mapper.CreateMap<BizData.ChexarLogin, CheckLogin>();
		}

		[Transaction()]
		public Response GetCheckProcessorInfo(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

            #region AL-3371 Transactional Log User Story(Process check)
            string Id = "AgentSession Id :" + Convert.ToString(agentSessionId);

			MongoDBLogger.Info<string>(0, Id, "GetCheckProcessorInfo", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "Begin GetCheckProcessorInfo - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			BizData.CheckProcessorInfo checkProcessorInfo = new BizData.CheckProcessorInfo();
			if (!string.IsNullOrEmpty(context.CheckUserName) && !string.IsNullOrEmpty(context.CheckPassword))
			{
				checkProcessorInfo = CPEngineService.GetCheckProcessorInfo(agentSessionId, context);
			}

            #region AL-3371 Transactional Log User Story(Process check)
            MongoDBLogger.Info<BizData.CheckProcessorInfo>(0, checkProcessorInfo, "GetCheckProcessorInfo", AlloyLayerName.SERVICE,
				ModuleName.ProcessCheck, "End GetCheckProcessorInfo - MGI.Channel.DMS.Server.Impl.CheckCashingEngine",
				context);
			#endregion

			response.Result = Mapper.Map<Data.CheckProcessorInfo>(checkProcessorInfo);
			return response;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Check, "Get check status")]
		public Response GetCheckSession(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			response.Result = Mapper.Map<CheckLogin>(CPEngineService.GetChexarSessions(context));
			return response;
		}
	}
}
