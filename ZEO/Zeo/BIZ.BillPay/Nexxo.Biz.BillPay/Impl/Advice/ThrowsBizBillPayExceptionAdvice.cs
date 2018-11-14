using MGI.Biz.BillPay.Data;
using MGI.Biz.Compliance.Contract;
using MGI.Common.Sys;
using Spring.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.BillPay.Impl.Advice
{
	public class ThrowsBizBillPayExceptionAdvice : IThrowsAdvice
	{
		public void AfterThrowing(BizBillPayException bizBillPayEx)
		{
			throw bizBillPayEx;
		}
        public void AfterThrowing(BizComplianceLimitException bizLimitEx)
        {
            throw bizLimitEx;
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
            var bizEx = new BizBillPayException(BizBillPayException.BILLPAY_EXCEPTION, ex);
			throw bizEx;
        }
	}
}
