using System;

using MGI.Common.Sys;
using MGI.Biz.Synovus.Contract;
using MGI.Cxn.Customer.Contract;
using Spring.Aop;


namespace MGI.Biz.Synovus.Impl.Advice
{
	public class ThrowsBizSynovusExceptionAdvice : IThrowsAdvice
	{
        public void AfterThrowing(ClientCustomerException ex)
        {
            throw ex;
        }

        public void AfterThrowing(NexxoException nexEx)
        {
            var bizEx = new BizSynovusCustomerException(nexEx.MinorCode, nexEx.Message, nexEx.InnerException);

            throw bizEx;
        }      
	}
}
