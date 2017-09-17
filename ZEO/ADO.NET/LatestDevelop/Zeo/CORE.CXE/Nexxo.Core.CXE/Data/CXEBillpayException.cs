using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.CXE.Contract
{
    public class CXEBillpayException : AlloyException
    {
        const string BillPayProductCode = "1004";
        const string AlloyCode = "100";

        public CXEBillpayException(string alloyErrorCode, Exception innerException)
            : base(BillPayProductCode, AlloyCode, alloyErrorCode, string.Empty, innerException)
        {

        }

        public static string BILLPAY_STAGE_FAILED  = "1000";
        public static string BILLPAY_UPDATE_FAILED = "1001";
        public static string BILLPAY_COMMIT_FAILED = "1002";
        public static string BILLPAY_GET_FAILED    = "1003";

    }
}
