using System;

using MGI.Common.Sys;
using MGI.Biz.Customer.Contract;
using MGI.Biz.Compliance.Contract;
using MGI.Cxn.Customer.Contract;

using Spring.Aop;

namespace MGI.Biz.Customer.Impl.Advice
{
	public class ThrowsBizCustomerExceptionAdvice : IThrowsAdvice
	{
		public void AfterThrowing( BizCustomerException bizCustEx )
		{
			throw bizCustEx;
		}

		public void AfterThrowing( BizComplianceException bizCmplEx )
		{
			throw bizCmplEx;
		}

        public void AfterThrowing(ClientCustomerException ex)
        {
            throw ex;
        }

		public void AfterThrowing( NexxoException nexEx )
		{
			var bizEx = new BizCustomerException( nexEx.MinorCode, nexEx.Message, nexEx.InnerException );

			throw bizEx;
		}


	}
}
