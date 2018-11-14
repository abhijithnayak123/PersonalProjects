using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class ReceiptException : ZeoException
    {
        public static string ReceiptProductCode = "1000";
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public ReceiptException(string alloyErrorCode, Exception innerException)
           : base(ReceiptProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string GET_BILLPAY_RECEIPT_DATA_FAILED               = "3700";
        public static readonly string GET_CHECK_RECEIPT_DATA_FAILED                 = "3701";
        public static readonly string GET_COUPON_RECEIPT_DATA_FAILED                = "3702";
        public static readonly string GET_FUND_RECEIPT_DATA_FAILED                  = "3703";
        public static readonly string GET_MONEYORDER_RECEIPT_DATA_FAILED            = "3704";
        public static readonly string GET_MONEYTRANSFER_RECEIPT_DATA_FAILED         = "3705";
        public static readonly string GET_SHOPPING_CART_RECEIPT_DATA_FAILED         = "3706";
        public static readonly string GET_CASHDRAWER_RECEIPT_DATA_FAILED            = "3707";
    }
}
