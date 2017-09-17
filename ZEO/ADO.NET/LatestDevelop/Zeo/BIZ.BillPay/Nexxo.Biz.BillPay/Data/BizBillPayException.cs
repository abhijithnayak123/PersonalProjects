using MGI.Common.Sys;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.BillPay.Data
{
	public class BizBillPayException : AlloyException
	{
        public static string BillPayProductCode = "1004";
        public static string AlloyCode = ((int)ProviderId.Alloy).ToString();

        public BizBillPayException(string alloyErrorCode, Exception innerException)
            : base(BillPayProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string TRANSACTION_UPDATE_FAILED     = "6003";
        public static readonly string FAVORITEBILLER_DELETE_FAILED  = "6004";
        public static readonly string TRANSACTION_CANCEL_FAILED     = "6005";
        public static readonly string BILLERLASTTRANSACTION_GET_FAILED = "6006";
        public static readonly string FAVORITEBILLER_UPDATE_FAILED  = "6007";
        public static readonly string FAVORITEBILLER_ADD_FAILED     = "6008";
        public static readonly string PASTBILLER_ADD_FAILED         = "6009";
        public static readonly string CARDINFO_GET_FAILED           = "6010";
        public static readonly string BILLPAYFEE_GET_FAILED         = "6011";
        public static readonly string TRANSACTION_GET_FAILED        = "6012";
        public static readonly string FAVORITEBILLER_GET_FAILED     = "6013";
        public static readonly string BILLERFIELDS_GET_FAILED       = "6014";
        public static readonly string BILLERMESSAGE_GET_FAILED      = "6015";
        public static readonly string BILLPAY_GETFEE_FAILED         = "6016";
        public static readonly string BILLERLOCATION_GET_FAILED     = "6017";
        public static readonly string TRANSACTION_COMMIT_FAILED     = "6018";
        public static readonly string TRANSACTION_ADD_FAILED        = "6019";
        public static readonly string TRANSACTION_VALIDATE_FAILED   = "6020";
        public static readonly string ACCOUNT_UPDATE_FAILED         = "6021";
        public static readonly string BILLPAY_EXCEPTION             = "6022";
	}
}
