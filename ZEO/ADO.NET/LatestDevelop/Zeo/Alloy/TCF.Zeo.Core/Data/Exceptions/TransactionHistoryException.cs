using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class TransactionHistoryException : ZeoException
    {
        public static string TransactionServiceProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public TransactionHistoryException(string errorCode, Exception innerException)
            : base(TransactionServiceProductCode, AlloyCode, errorCode, innerException)
        {
        }

        public static readonly string GET_AGENT_TRANSACTION_FAILED = "3902";
        public static readonly string GET_CUSTOMER_TRANSACTION_FAILED = "3903";
        public static readonly string GET_CUSTOMER_TRANSACTION_LOCATIONS_FAILED = "3904";
    }
}
