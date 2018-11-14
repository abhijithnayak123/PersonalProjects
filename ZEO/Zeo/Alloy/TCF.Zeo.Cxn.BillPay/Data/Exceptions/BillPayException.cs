using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Cxn.BillPay.Data.Exceptions
{
    public class BillPayException : ZeoException
    {
        static string BillPayProductCode = ((int)Helper.ProductCode.BillPay).ToString();
        static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public BillPayException(string errorCode)
            : base(BillPayProductCode, AlloyCode, errorCode, null)
        {
        }

        public BillPayException(string errorCode, Exception innerException)
            : base(BillPayProductCode, AlloyCode, errorCode, innerException)
        {
        }

        public static string BILLPAY_VALIDATE_FAILED = "2101";
        public static string BILLPAY_COMMIT_FAILED = "2102";
        public static string LOCATION_RETRIEVAL_FAILED = "2103";
        public static string DELIVERY_METHODS_RETRIEVAL_FAILED = "2104";
        public static string BILLER_MESSAGE_RETRIEVAL_FAILED = "2105";
        public static string BILLER_FIELDS_RETRIEVAL_FAILED = "2106";
        public static string BILLER_RETRIEVAL_FAILED = "2107";
        public static string ACCOUNT_CREATE_FAILED = "2109";
        public static string PROVIDER_IMPORT_FAILED = "2114";
        public static string PROVIDER_ERROR = "2115";
        public static string COUNTERID_NOT_FOUND = "2116";
        public static string GOLD_CARD_POINTS_UPDATE_FAILED = "2117";
        public static string ACCOUNT_GET_FAILED = "2118";
        public static string TRANSACTION_GET_FAILED = "2119";
        public static string BILLPAY_GETFEE_FAILED = "2120";
        public static string BILLERMESSAGE_GET_FAILED = "2121";
        public static string BILLERFIELDS_GET_FAILED = "2122";
        public static string ACCOUNT_UPDATE_FAILED = "2123";
        public static string CARDINFO_GET_FAILED = "2124";
    }
}
