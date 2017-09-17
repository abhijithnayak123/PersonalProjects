using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class MoneyTransferException : ZeoException
    {
        static string MoneyTransferProductCode = ((int)Helper.ProductCode.MoneyTransfer).ToString();
        static string AlloyCode = ((int)ProviderId.Alloy).ToString();

        public MoneyTransferException(string alloyErrorCode, Exception innerException)
            : base(MoneyTransferProductCode, AlloyCode, alloyErrorCode, string.Empty, innerException)
        {
        }

        public MoneyTransferException(string alloyErrorCode): base(MoneyTransferProductCode, AlloyCode, alloyErrorCode, null)
		{
        }

        public static readonly string MONEYTRANSFER_GET_FAILED = "1000";
        public static readonly string MONEYTRANSFER_CREATE_FAILED = "1001";
        public static readonly string MONEYTRANSFER_UPDATE_FAILED = "1002";
        public static readonly string MONEYTRANSFER_UPDATE_STATUS_FAILED = "1003";
        public static readonly string MONEYTRANSFER_CREATEMSMRSM_FAILED = "1005";
        public static readonly string MONEYTRANSFER_TRANSACTION_PAID_ALREADY = "1006";
        public static readonly string MONEYTRANSFER_TRANSACTION_ALREADY_ADDED_SHOPPINGCART = "1007";
    }
}
