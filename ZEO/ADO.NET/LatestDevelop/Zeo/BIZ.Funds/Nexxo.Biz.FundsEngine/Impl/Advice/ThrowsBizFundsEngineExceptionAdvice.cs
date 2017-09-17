using System;

using MGI.Common.Sys;
using MGI.Biz.FundsEngine.Contract;

using MGI.Biz.Compliance.Contract;
using MGI.Biz.FundsEngine.Data;

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

		//public void AfterThrowing(ProviderException providerEx)
		//{
		//	throw providerEx;
		//}
		//public void AfterThrowing(Exception ex)
		//{
		//	var bizEx = new BizFundsException(BizFundsException.FUNDS_EXCEPTION, ex);
		//}
    }
}
