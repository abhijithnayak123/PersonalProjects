using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class BillPayException : ZeoException
    {
        public static string BillPayProductCode = ((int)Helper.ProductCode.BillPay).ToString();
        public static string AlloyCode = ((int)ProviderId.Alloy).ToString();

        public BillPayException(string alloyErrorCode, Exception innerException)
            : base(BillPayProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string TRANSACTION_UPDATE_FAILED = "6003";
        public static readonly string FAVORITEBILLER_DELETE_FAILED = "6004";
        public static readonly string TRANSACTION_CANCEL_FAILED = "6005";
        public static readonly string PASTBILLER_ADD_FAILED = "6009";
        public static readonly string CARDINFO_GET_FAILED = "6010";
        public static readonly string TRANSACTION_GET_FAILED = "6012";
        public static readonly string FAVORITEBILLER_GET_FAILED = "6013";
        public static readonly string BILLERMESSAGE_GET_FAILED = "6015";
        public static readonly string BILLPAY_GETFEE_FAILED = "6016";
        public static readonly string BILLERLOCATION_GET_FAILED = "6017";
        public static readonly string TRANSACTION_COMMIT_FAILED = "6018";
        public static readonly string TRANSACTION_VALIDATE_FAILED = "6020";
        public static readonly string TRANSACTION_SUBMIT_FAILED = "6023";
        public static readonly string BILLERDETAILS_GET_FAILED = "6024";
        public static readonly string PROVIDERATTRIBUTES_GET_FAILED = "6025";
    }
}
