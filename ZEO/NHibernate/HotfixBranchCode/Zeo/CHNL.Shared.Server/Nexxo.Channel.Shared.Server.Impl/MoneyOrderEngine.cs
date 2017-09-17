using System;
using System.Collections.Generic;

using AutoMapper;
using MGI.Channel.Shared.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;
using BizMOData = MGI.Biz.MoneyOrderEngine.Data;
using MGI.Biz.MoneyOrderEngine.Contract;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.Shared.Server.Impl
{
    public partial class SharedEngine : IMoneyOrderService
    {
        #region Injected Services

        public IMoneyOrderEngineService MoneyOrderEngineService { private get; set; }
		public MGI.Common.Util.TLoggerCommon MongoDBLogger { private get; set; }

        #endregion

        #region MoneyOrderService Data Mapper

        internal static void MoneyOrderConverter()
        {
            Mapper.CreateMap<BizMOData.MoneyOrder, MoneyOrder>();
        }

        #endregion

        #region IMoneyOrderService Impl

        public MoneyOrder GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrderId:" + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "GetMoneyOrderStage", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "Begin GetMoneyOrderStage - MGI.Channel.Shared.Server.Impl.MoneyOrderEngine",
				mgiContext);
			#endregion

            var bizMoneyOrder = MoneyOrderEngineService.GetMoneyOrderStage(customerSessionId, moneyOrderId, mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<BizMOData.MoneyOrder>(customerSessionId, bizMoneyOrder, "GetMoneyOrderStage", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "End GetMoneyOrderStage - MGI.Channel.Shared.Server.Impl.MoneyOrderEngine",
				mgiContext);
			#endregion

            return Mapper.Map<MoneyOrder>(bizMoneyOrder);
        }

        #endregion
    }
}
