using System;
using TCF.Zeo.Common.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class MoneyorderException : ZeoException
    {
        public static string MOProductCode = ((int)Helper.ProductCode.MoneyOrder).ToString();
        public static string MOProviderCode = ((int)ProviderId.Alloy).ToString();

        public MoneyorderException(string alloyErrorCode, Exception innerException): base(MOProductCode, MOProviderCode, alloyErrorCode, string.Empty, innerException)
        {
        }

        public static string MONEYORDER_CREATE_FAILED = "1000";
        public static string MONEYORDER_UPDATE_FAILED = "1001";
        public static string MONEYORDER_GET_FAILED = "1003";
        public static string MONEYORDER_STATUS_UPDATE_FAILED = "1004";
        public static string MONEYORDER_FEE_UPDATE_FAILED = "1005";


    }
}
