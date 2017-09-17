using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class BillpayException : ZeoException
    {
        public static string BillPayProductCode = ((int)Helper.ProductCode.BillPay).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public BillpayException(string alloyErrorCode, Exception innerException)
            : base(BillPayProductCode, AlloyCode, alloyErrorCode, string.Empty, innerException)
        {

        }

        public static string BILLPAY_CREATE_FAILED = "1000";
        public static string BILLPAY_UPDATE_FAILED = "1001";
        public static string FAVOURITEBILLER_CREATEORUPDATE_FAILED = "1004";
        public static string BILLERS_GET_FAILED = "1005";
        public static string FAVOURITEBILLER_GET_FAILED = "1006";
        public static string FAVOURITEBILLER_DELETE_FAILED = "1007";
        public static string BILLPAY_FETCH_CXN_ID_FAILED = "1008";
    }
}
