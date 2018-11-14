using System;

using MGI.Common.Sys;

namespace MGI.Core.Compliance.Contract
{
    public class CoreComplianceException : AlloyException
    {
        const string COMPLIANCE_CODE = "1008";
        const string AlloyCode = "100";

        public CoreComplianceException(string ErrorCode)
            : base(COMPLIANCE_CODE, AlloyCode, ErrorCode, null)
        {
        }
        public CoreComplianceException(string ErrorCode, Exception ex)
            : base(COMPLIANCE_CODE, AlloyCode, ErrorCode, ex)
        {
        }

        static public string COMPLIANCE_PROGRAM_NOT_FOUND = "5004";
    }
}