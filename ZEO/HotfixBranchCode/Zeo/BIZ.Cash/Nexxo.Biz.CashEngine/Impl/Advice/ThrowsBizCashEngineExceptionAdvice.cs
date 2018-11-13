using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Aop;
using MGI.Biz.CashEngine.Contract;
using MGI.Common.Sys;

namespace MGI.Biz.CashEngine.Impl.Advice
{
    public class ThrowsBizCashEngineExceptionAdvice : IThrowsAdvice
    {
        public void AfterThrowing(BizCashEngineException bizcashEx)
        {
            throw bizcashEx;
        }

        public void AfterThrowing(NexxoException nexEx)
        {
            var bizEx = new BizCashEngineException(nexEx.MinorCode, nexEx.Message, nexEx.InnerException);

            throw bizEx;
        }
    }
}
