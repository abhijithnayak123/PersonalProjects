using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class CheckException : ZeoException
    {
        public static string CheckProcessingProductCode = ((int)Helper.ProductCode.CheckProcessing).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public CheckException(string errorCode, Exception innerException)
            : base(CheckProcessingProductCode, AlloyCode, errorCode, innerException)
        {
        }

        public static string CREATE_CHECK_TRANSACTION_FAILED = "1000";
        public static string UPDATE_CHECK_TRANSACTION_FAILED = "1001";
        public static string GET_CHECK_TRANSACTION_FAILED = "1002";
        public static string SUBMIT_CHECK_TRANSACTION_FAILED = "1003";
        public static string GET_CHECK_TYPE_FAILED = "1004";
        public static string CANCEL_CHECK_TRANSACTION_FAILED = "1005";

    }
}
