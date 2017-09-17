using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class ReceiptException : ZeoException
    {
        public const string ReceiptProductCode = "1000";
        public static string AlloyCode = ((int)ProviderId.Alloy).ToString();

        public ReceiptException(string alloyErrorCode)
            : base(ReceiptProductCode, AlloyCode, alloyErrorCode, null)
        {
        }

        public ReceiptException(string alloyErrorCode, Exception innerException)
            : base(ReceiptProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        //4100-4199
        public static string RECEIPT_TEMPLATE_NOT_FOUND                         = "4700";
        public static string CHECK_RECEIPT_TEMPLATE_RETRIVEL_FAILED             = "4703";
        public static string MT_RECEIPT_TEMPLATE_RETRIVEL_FAILED                = "4704";
        public static string FUNDS_RECEIPT_TEMPLATE_RETRIVEL_FAILED             = "4705";
        public static string BILLPAY_RECEIPT_TEMPLATE_RETRIVEL_FAILED           = "4706";
        public static string MONEYORDER_RECEIPT_TEMPLATE_RETRIVEL_FAILED        = "4707";
        public static string SUMMARY_RECEIPT_TEMPLATE_RETRIVEL_FAILED           = "4708";
        //public static string CHECKDECLINED_RECEIPT_TEMPLATE_RETRIVEL_FAILED     = "4709";
        public static string COUPON_RECEIPT_TEMPLATE_RETRIVEL_FAILED            = "4710";
        public static string DODFRANK_RECEIPT_RETRIVEL_FAILED                   = "4711";
        //public static string RECEIPT_EXCEPTION                                  = "4712";
        public static string CASHDRAWERRECEIPT_FAILED                           = "4713";
    }
}
