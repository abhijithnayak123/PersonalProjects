using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class CheckException : ZeoException
    {

        public static string ProductCode = ((int)Helper.ProductCode.CheckProcessing).ToString();
        public static string ProviderCode = ((int)Helper.ProviderId.Alloy).ToString();

        public CheckException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, ProviderCode, alloyErrorCode, innerException)
        {
        }
        public CheckException(string alloyErrorCode)
            : base(ProductCode, ProviderCode, alloyErrorCode, null)
        {
        }


        public static readonly string CHECK_FRANK_TEMPLATE_NOT_FOUND = "6002";
        public static readonly string CHECK_SUBMIT_FAILED = "6003";
        public static readonly string CHECK_CANCEL_FAILED = "6004";
        public static readonly string CHECK_FRANK_UPDATE_FAILED = "6005";
        public static readonly string CHECK_RESUBMIT_FAILED = "6006";
        public static readonly string CHECK_COMMIT_FAILED = "6007";
        public static readonly string GET_CHECK_TYPE_FAILED = "6008";
        public static readonly string GET_FEE_FAILED = "6009";
        public static readonly string GET_CHECK_FRANK_FAILED = "6010";
        public static readonly string GET_TRANSACTION_FAILED = "6011";
        public static readonly string GET_CHECK_PROCESSOR_INFO_FAILED = "6012";
        public static readonly string GET_STATUS_FAILED = "6013";
        public static readonly string CPENGINE_EXCEPTION = "6014";
        public static readonly string GET_CHECK_LOGIN_FAILED = "6016";



        // Used in  bizlayer CPEngineServiceImpl for logic to do look up form MessageStore
        public static readonly string UNHANDLED_DECLINE_CODE = "6015";

        public static readonly string PROMOCODE_INVALID = "6016";
    }
}
