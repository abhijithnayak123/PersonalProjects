using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class FundsException : ZeoException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Funds).ToString();
        public static string ProviderCode = ((int)Helper.ProviderId.Alloy).ToString();

        public FundsException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, ProviderCode, alloyErrorCode, innerException)
        {
        }

        static public string TRANSACTION_CREATE_FAILED = "3600";
        static public string TRANSACTION_UPDATE_FAILED = "3601";
        static public string TRANSACTION_COMMIT_FAILED = "3602";
        static public string TRANSACTION_UPDATE_AMOUNT_FAILED = "3603";
        static public string TRANSACTION_GET_FAILED = "3604";
    }
}
