using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;
using MGI.Biz.MoneyOrderEngine.Contract;
using MGI.Biz.Compliance.Contract;

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

        public void AfterThrowing(NexxoException nexEx)
        {
            var bizEx = new BizMoneyOrderEngineException(nexEx.MinorCode, nexEx.Message, nexEx.InnerException);

            throw bizEx;
        }

    }
}
