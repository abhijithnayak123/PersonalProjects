using System;

using MGI.Common.Sys;
using MGI.Biz.FundsEngine.Contract;

using MGI.Biz.Compliance.Contract;

using Spring.Aop;

namespace MGI.Biz.FundsEngine.Impl.Advice
{
    public class ThrowsBizFundsEngineExceptionAdvice : IThrowsAdvice
    {
        public void AfterThrowing(BizFundsException bizFundEx)
        {
            throw bizFundEx;
        }

		public void AfterThrowing(BizComplianceLimitException bizLimitEx)
		{
			throw bizLimitEx;
		}

        public void AfterThrowing(NexxoException nexEx)
        {
            var bizEx = new BizFundsException(nexEx.MinorCode, nexEx.Message, nexEx.InnerException);

            throw bizEx;
        }
    }
}
