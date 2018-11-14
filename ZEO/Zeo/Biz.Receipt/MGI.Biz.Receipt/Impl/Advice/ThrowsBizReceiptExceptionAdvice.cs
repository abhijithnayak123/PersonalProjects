using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Receipt.Data;
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

		public void AfterThrowing(Exception nexEx)
		{
			var bizReceiptEx = new BizReceiptException(BizReceiptException.RECEIPT_EXCEPTION);

			throw bizReceiptEx;
		}
	}
}
