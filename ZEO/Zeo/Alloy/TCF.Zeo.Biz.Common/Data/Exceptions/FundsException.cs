
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class FundsException : ZeoException
    {
        public static string FundsProductCode = ((int)Helper.ProductCode.Funds).ToString();
        public static string FundsProviderCode = ((int)Helper.ProviderId.Alloy).ToString();

        public FundsException(string alloyErrorCode, Exception innerException)
            : base(FundsProductCode, FundsProviderCode, alloyErrorCode, innerException)
        {
        }

        public FundsException(string alloyErrorCode)
            : base(FundsProductCode, FundsProviderCode, alloyErrorCode, null)
        {
        }
        
        public static readonly string ADD_FUNDS_FAILED = "6012";
        public static readonly string FUNDS_BALANCE_RETRIVEL_FAILED = "6014";
        public static readonly string FUNDS_TRANSACTION_RETRIVEL_FAILED = "6015";
        public static readonly string FUNDS_UPDATE_FAILED = "6017";
        public static readonly string FUNDS_GETMINIMUM_FAILED = "6018";
        public static readonly string FUNDS_CANCEL_TRANSACTION_FAILED = "6019";
        public static readonly string FUNDS_TRANSACTION_HISTORY_RETRIVEL_FAILED = "6020";
        public static readonly string FUNDS_ACCOUNT_CLOSURE_FAILED = "6021";
        public static readonly string FUNDS_UPDATE_STATUS_FAILED = "6022";
        public static readonly string FUNDS_CARD_REPLACE_FAILED = "6023";
        public static readonly string FUNDS_SHIPPING_TYPE_RETRIVEL_FAILED = "6024";
        public static readonly string FUNDS_SHIPPING_FEE_RETRIVEL_FAILED = "6025";
        public static readonly string FUNDS_CARD_ASSOCIATION_FAILED = "6026";
        public static readonly string FUNDS_CARDMAINTENANCE_FEE_RETRIVEL_FAILED = "6027";
        public static readonly string FUNDS_WITHDRAW_FAILED = "6028";
        public static readonly string FUNDS_ACTIVATE_FAILED = "6029";
        public static readonly string FUNDS_ORDER_COMPANION_FAILED = "6031";
        public static readonly string COMMIT_FAILED = "6032";
        public static readonly string FUNDS_CARD_RETRIVEL_FAILED = "6033";
        public static readonly string FUNDS_ACCOUNT_NOT_FOUND = "6034";
        public static readonly string FUNDS_TRANSACTION_INVALID = "6035";
    }
}
