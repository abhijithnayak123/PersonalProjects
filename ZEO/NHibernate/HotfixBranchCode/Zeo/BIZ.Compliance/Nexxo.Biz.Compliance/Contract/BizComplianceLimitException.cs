using System;

using MGI.Common.Sys;

namespace MGI.Biz.Compliance.Contract
{
	public class BizComplianceLimitException : BizComplianceException
	{
		private decimal _limit;
		public decimal LimitValue
		{
			get { return _limit; }
		}
		public BizComplianceLimitException(int MinorCode, string Message)
			: this(MinorCode, Message, 0.00M)
		{
		}

		public BizComplianceLimitException(int MinorCode, string Message, decimal limit)
			: this(MinorCode, Message, null, limit)
		{
		}

		public BizComplianceLimitException(int MinorCode, decimal limit)
			: this(MinorCode, string.Empty, limit)
		{
		}

		public BizComplianceLimitException(int MinorCode, Exception innerException, decimal limit)
			: this(MinorCode, string.Empty, innerException, limit)
		{
		}

		public BizComplianceLimitException(int MinorCode, string Message, Exception innerException, decimal limit)
			: base(MinorCode, Message, innerException)
		{
			_limit = limit;
		}

		static public int BILL_PAY_LIMIT_EXCEEDED = 6000;
		static public int CASH_WITHDRAWAL_LIMIT_EXCEEDED = 6001;
		static public int CASH_LIMIT_EXCEEDED = 6002;
		static public int CHECK_LIMIT_EXCEEDED = 6003;
		static public int GPR_LOAD_LIMIT_EXCEEDED = 6004;
		static public int MONEY_ORDER_LIMIT_EXCEEDED = 6005;
		static public int MONEY_TRANSFER_LIMIT_EXCEEDED = 6006;

		static public int GPR_LOAD_MINIMUM_LIMIT_CHECK = 6007;
		static public int CHECK_MINIMUM_LIMIT_CHECK = 6008;
		static public int MONEY_TRANSFER_MINIMUM_LIMIT_CHECK = 6009;
		static public int MONEY_ORDER_MINIMUM_LIMIT_CHECK = 6010;
		static public int BILL_PAY_MINIMUM_LIMIT_CHECK = 6011;

		static public int GPR_DEBIT_LIMIT_EXCEEDED = 6012;

		static public int ALLOY_LIMIT_EXCEED = 6013;
	}
}
