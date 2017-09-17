using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class TransactionHistoryException : ZeoException
    {
        public static string TransactionHistoryProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public TransactionHistoryException(string alloyErrorCode, Exception innerException)
            : base(TransactionHistoryProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }
        public static readonly string GET_CUSTOMER_TRANSACTION_HISTORY_FAILED = "4900";
        public static readonly string GET_AGENT_TRANSACTION_HISTORY_FAILED = "4901";
        public static readonly string GET_TRANSACTION_LOCATION_FAILED = "4902";
    }
}
