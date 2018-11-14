using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Aop;
using MGI.Biz.MoneyTransfer.Contract;
using MGI.Biz.Compliance.Contract;
using MGI.Common.Sys;

namespace MGI.Biz.MoneyTransfer.Impl.Advice
{
    public class ThrowsBizMoneyTransferEngineExceptionAdvice : IThrowsAdvice
    {
        public void AfterThrowing(BizMoneyTransferException bizmtEx)
        {
            throw bizmtEx;
        }

		public void AfterThrowing(BizComplianceLimitException bizLimitEx)
		{
			throw bizLimitEx;
		}

		public void AfterThrowing(ProviderException providerEx)
		{
			throw providerEx;
		}
    }
}
