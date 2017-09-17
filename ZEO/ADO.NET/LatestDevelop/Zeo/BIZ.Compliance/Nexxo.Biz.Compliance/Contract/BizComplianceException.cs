using System;

using MGI.Common.Sys;

namespace MGI.Biz.Compliance.Contract
{
	public class BizComplianceException: AlloyException
    {
        const string COMPLIANCE_EXCEPTION_MAJOR_CODE = "1008";
        const string AlloyCode = "100";

        public BizComplianceException(string productCode, string alloyErrorCode, string Message, Exception innerException)
            : base(productCode, AlloyCode, alloyErrorCode, Message, innerException)
        {
        }

        public BizComplianceException(string errorCode)
            : this(COMPLIANCE_EXCEPTION_MAJOR_CODE, errorCode, string.Empty, null)
        {
        }

        public BizComplianceException(string alloyErrorCode, string Message, Exception innerException)
            : base(COMPLIANCE_EXCEPTION_MAJOR_CODE, alloyErrorCode, Message, innerException)
        {
        }

        static public string COMPLIANCE_PROGRAM_NOT_FOUND = "5004";
	}
}
