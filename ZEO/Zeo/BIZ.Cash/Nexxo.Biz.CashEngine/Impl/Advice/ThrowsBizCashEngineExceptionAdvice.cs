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
        public void AfterThrowing(AlloyException alloyEx)
        {
            throw alloyEx;
        }
        public void AfterThrowing(ProviderException providerEx)
        {
            throw providerEx;
        }
    }
}
