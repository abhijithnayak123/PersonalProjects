using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Receipt.Contract;
using MGI.Common.Sys;

using Spring.Aop;

namespace MGI.Biz.Receipt.Impl.Advice
{
    public class ThrowsBizReceiptExceptionAdvice : IThrowsAdvice
    {

        public void AfterThrowing(BizReceiptException bizReceiptEx)
        {
            throw bizReceiptEx;
        }

        public void AfterThrowing(NexxoException nexEx)
        {
            var bizEx = new BizReceiptException(nexEx.MinorCode, nexEx.Message, nexEx.InnerException);

            throw bizEx;
        }
    }
}
