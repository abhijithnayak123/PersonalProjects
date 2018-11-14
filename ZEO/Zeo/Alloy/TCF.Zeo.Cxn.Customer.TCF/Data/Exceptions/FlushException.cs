using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Cxn.Customer.TCF.Data.Exceptions
{
    public class FlushException : ZeoException
    {
        static string ProductCode = ((int)Helper.ProductCode.Partner).ToString();
        static string ProviderCode = ((int)Helper.ProviderId.Alloy).ToString();

        public FlushException(string errorCode)
            : base(ProductCode, ProviderCode, errorCode, null)
        {
        }

        public FlushException(string errorCode, Exception innerException)
            : base(ProductCode, ProviderCode, errorCode, innerException)
        {
        }

        public static string POST_FLUSH_FAILED = "2100";
        public static string PRE_FLUSH_FAILED = "2101";


        //For Final Commit calls.
        public static string TELLER_MAINFRAME_COMMIT_FAILED = "2102";
        public static string TELLER_MIDDLETIER_COMMIT_FAILED = "2103";
        public static string RCIF_COMMIT_FAILED = "2104";
        public static string TELLER_FINAL_COMMIT_FAILED = "2105";
    }
}
