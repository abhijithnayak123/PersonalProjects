using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;
using MGI.Biz.MoneyOrderEngine.Contract;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.MoneyOrderEngine.Data;

using Spring.Aop;
namespace MGI.Biz.MoneyOrderEngine.Impl.Advice
{
    public class ThrowsBizMoneyOrderEngineExceptionAdvice : IThrowsAdvice
    {
        public void AfterThrowing(BizMoneyOrderEngineException bizMoneyOrderEx)
        {
            throw bizMoneyOrderEx;
        }

        public void AfterThrowing(BizComplianceLimitException bizLimitEx)
        {
            throw bizLimitEx;
        }

        public void AfterThrowing(AlloyException alloyEx)
        {
            throw alloyEx;
        }

		public void AfterThrowing(Exception ex)
		{
			var bizEx = new BizMoneyOrderEngineException(BizMoneyOrderEngineException.MONEYORDER_EXCEPTION);
            throw bizEx;
		}
    }
}
