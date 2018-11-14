using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Biz.Common
{
    public class MoneyOrderException : ZeoException
    {
        public static string MOProductCode = ((int)Helper.ProductCode.MoneyOrder).ToString();
        public static string MOProviderCode = ((int)ProviderId.Alloy).ToString();

        public MoneyOrderException(string alloyErrorCode, Exception innerException): base(MOProductCode, MOProviderCode, alloyErrorCode, innerException)
		{
        }
        public MoneyOrderException(string alloyErrorCode): base(MOProductCode, MOProviderCode, alloyErrorCode, null)
		{
        }

        public static readonly string CHECKPRINT_TEMPLATE_NOT_FOUND = "6002";
        public static readonly string MONEYORDER_COMMIT_ALREADY_EXIST = "6003";
        public static readonly string RESUBMIT_MONEYORDER_FAILED = "6004";
        public static readonly string MONEYOREDER_ADD_FAILED = "6006";
        public static readonly string UPDATE_MONEYORDER_FAILED = "6007";
        public static readonly string COMMIT_MONEYORDER_FAILED = "6008";
        public static readonly string GET_MONEYORDER_FAILED = "6009";
        public static readonly string MONEYORDER_CHECK_PRINT_FAILED = "6010";
        public static readonly string MONEYORDER_DIAGONOSTIC_FAILED = "6011";
        public static readonly string UPDATE_MONEYORDER_STATUS_EXCEPTION = "6013";
        public static readonly string MONEYORDER_GETFEE_FAILED = "6014";
        public static readonly string MONEYORDER_PROMOCODE_INVALID = "6015";

    }
}
