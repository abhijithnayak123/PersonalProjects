using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.BillPay.Contract
{
	public class BizBillPayException : NexxoException
	{
		const int MAJOR_CODE = 1004;
		public BizBillPayException(int MinorCode, string Message)
			: this(MinorCode, Message, null)
		{
		}

		public BizBillPayException(int MinorCode)
			: this(MinorCode, string.Empty)
		{
		}

		public BizBillPayException(int MinorCode, Exception innerException)
			: this(MinorCode, string.Empty, innerException)
		{
		}

		public BizBillPayException(int MinorCode, string Message, Exception innerException)
			: base(MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

		public static readonly int LOCATION_NOT_SET  = 2450;
		public static readonly int PROCESSOR_NOT_SET = 2451;
		public static readonly int ACCOUNT_NOT_FOUND = 2452;
	}
}
