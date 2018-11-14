using System;

using MGI.Common.Sys;
using MGI.Biz.Customer.Data;
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

		public void AfterThrowing(AlloyException alloyEx)
		{
			throw alloyEx;
		}

		public void AfterThrowing(ProviderException providerEx)
        {
            throw providerEx;
        }

        public void AfterThrowing(Exception ex)
        {
            var bizEx = new BizCustomerException(BizCustomerException.CUSTOMER_EXCEPTION, ex);

			throw bizEx;
        }
	}
}
