using System;

using MGI.Common.Sys;

namespace MGI.Core.CXE.Contract
{
    public class CXECashException : AlloyException 
    {
        const string CASH_PRODUCT_CODE = "1007";
        const string ALLOY_CODE = "100";

		public CXECashException(string errorCode)
            : base(CASH_PRODUCT_CODE, ALLOY_CODE, errorCode, null)
		{
		}

        public CXECashException(string errorCode, Exception ex)
            : base(CASH_PRODUCT_CODE, ALLOY_CODE, errorCode, ex)
        {
        }

		public static string CASH_NOT_FOUND        = "1000";		
		public static string CASH_UPDATE_FAILED    = "1001";
		public static string CASH_COMMIT_FAILED    = "1002";
        public static string CASH_CREATE_FAILED    = "1003";
    }
}
