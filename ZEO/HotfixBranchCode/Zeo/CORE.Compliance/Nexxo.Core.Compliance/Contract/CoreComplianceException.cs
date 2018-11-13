using System;

using MGI.Common.Sys;

namespace MGI.Core.Compliance.Contract
{
    public class CoreComplianceException : NexxoException
    {
        const int COMPLIANCE_EXCEPTION_MAJOR_CODE = 1008;

        public CoreComplianceException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public CoreComplianceException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public CoreComplianceException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public CoreComplianceException(int MinorCode, string Message, Exception innerException)
            : base(COMPLIANCE_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }

        //static public int CUSTOMER_ON_OFAC_BLACKLIST = 5000;
        //static public int OFAC_BRIDGER_FAILURE = 5001;
        //static public int OFAC_BATCH_FILE_IO_FAILURE = 5002;
        //static public int OFAC_BRIDGER_COMMAND_LINE_FAILURE = 5003;
        static public int COMPLIANCE_PROGRAM_NOT_FOUND = 5004;
    }
}