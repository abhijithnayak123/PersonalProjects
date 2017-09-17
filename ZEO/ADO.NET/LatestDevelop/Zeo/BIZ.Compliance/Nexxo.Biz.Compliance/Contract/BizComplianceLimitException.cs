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

        public BizComplianceLimitException(string productCode, string alloyErrorCode, string Message)
            : this(productCode, alloyErrorCode, Message, 0.00M)
		{
		}

        public BizComplianceLimitException(string productCode, string alloyErrorCode, string Message, decimal limit)
            : this(productCode, alloyErrorCode, Message, null, limit)
		{
		}

        public BizComplianceLimitException(string productCode, string alloyErrorCode, decimal limit)
            : this(productCode, alloyErrorCode, string.Empty, limit)
		{
		}

        public BizComplianceLimitException(string productCode, string alloyErrorCode, Exception innerException, decimal limit)
            : this(productCode, alloyErrorCode, string.Empty, innerException, limit)
		{
		}

        public BizComplianceLimitException(string productCode, string alloyErrorCode, string Message, Exception innerException, decimal limit)
            : base(productCode, alloyErrorCode, Message, innerException)
		{
			_limit = limit;
		}

		static public string MAXIMUM_LIMIT_FAILED           = "6000";
        static public string MINIMUM_LIMIT_FAILED           = "6001";
	}
}
