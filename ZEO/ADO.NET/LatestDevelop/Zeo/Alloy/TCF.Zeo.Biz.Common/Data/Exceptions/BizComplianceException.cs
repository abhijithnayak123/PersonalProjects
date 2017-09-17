using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class BizComplianceException : ZeoException
    {
        static string COMPLIANCE_EXCEPTION_MAJOR_CODE = ((int)Helper.ProductCode.Compliance).ToString();
        static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public BizComplianceException(string productCode, string alloyErrorCode, string Message, Exception innerException)
            : base(productCode, AlloyCode, alloyErrorCode, Message, innerException)
        {
        }

        public BizComplianceException(string alloyErrorCode, Exception innerException)
            : base(COMPLIANCE_EXCEPTION_MAJOR_CODE, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public static string GET_TRANSACTION_LIMIT_FAILED = "5005";
    }

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

        static public string MAXIMUM_LIMIT_FAILED = "6000";
        static public string MINIMUM_LIMIT_FAILED = "6001";
    }
}
