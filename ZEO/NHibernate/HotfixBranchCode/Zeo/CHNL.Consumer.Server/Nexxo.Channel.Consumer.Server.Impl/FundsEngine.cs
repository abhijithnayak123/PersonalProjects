using System.Collections.Generic;
using AutoMapper;
using Spring.Transaction.Interceptor;

using MGI.Channel.Consumer.Server.Contract;
using BizFundsEngineContract = MGI.Biz.FundsEngine.Contract;
using SharedData = MGI.Channel.Shared.Server.Data;
using BizCommon = MGI.Biz.Common.Data;
using BizFundsEngineData = MGI.Biz.FundsEngine.Data;
using MGI.Common.Util;

namespace MGI.Channel.Consumer.Server.Impl
{
    public partial class ConsumerEngine : IFundsProcessorService
    {
        #region Injected Services

        public BizFundsEngineContract.IFundsEngine BizFundsService { private get; set; }

        #endregion

        #region FundEngine Data Mapper

        public static void FundEngineConverter()
        {
            Mapper.CreateMap<SharedData.FundType, BizFundsEngineData.FundType>();
            Mapper.CreateMap<BizCommon.TransactionFee, SharedData.TransactionFee>();
            Mapper.CreateMap<BizFundsEngineData.FundsAccount, SharedData.FundsProcessorAccount>();
            Mapper.CreateMap<SharedData.Funds, BizFundsEngineData.Funds>();
        }

        #endregion

        #region IFundsProcessorService Impl

        [Transaction()]
		public SharedData.TransactionFee GetFundsFee(long customerSessionId, decimal amount, SharedData.FundType fundsType, MGIContext mgiContext)
        {
			return SharedEngine.GetFundsFee(customerSessionId, amount, fundsType, mgiContext);
        }

        [Transaction()]
		public long WithdrawFunds(long customerSessionId, SharedData.Funds funds, MGIContext mgiContext)
        {
			return SharedEngine.WithdrawFunds(customerSessionId, funds, mgiContext);
        }

        [Transaction()]
		public SharedData.FundsProcessorAccount LookupFundsAccount(long customerSessionId, MGIContext mgiContext)
        {
			return SharedEngine.LookupFundsAccount(customerSessionId, mgiContext);
        }

        #endregion
    }
}
