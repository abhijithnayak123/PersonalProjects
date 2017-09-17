using MGI.Biz.BillPay.Contract;
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
		public void AfterThrowing( BizBillPayException bizBillPayEx )
		{
			throw bizBillPayEx;
		}
        public void AfterThrowing(BizComplianceLimitException bizLimitEx)
        {
            throw bizLimitEx;
        }
		public void AfterThrowing( NexxoException nexEx )
		{
			var bizEx = new BizBillPayException( nexEx.MinorCode, nexEx.Message, nexEx.InnerException );

			throw bizEx;
		}
	
	}
}
