using System;

using MGI.Common.Sys;
using MGI.Biz.Synovus.Contract;
using MGI.Cxn.Customer.Contract;
using Spring.Aop;
using MGI.Cxn.Customer.Data;


namespace MGI.Biz.Synovus.Impl.Advice
{
	public class ThrowsBizSynovusExceptionAdvice : IThrowsAdvice
	{
        public void AfterThrowing(AlloyException ex)
        {
            throw ex;
        }

		public void AfterThrowing(ProviderException providerEx)
		{
			throw providerEx;
		}

	}
}
