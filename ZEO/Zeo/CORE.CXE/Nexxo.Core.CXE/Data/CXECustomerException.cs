using System;

using MGI.Common.Sys;

namespace MGI.Core.CXE.Data
{
    public class CXECustomerException : AlloyException
	{
        const string ProductCode = "1001";
        const string AlloyCode = "100";

        public CXECustomerException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, AlloyCode, alloyErrorCode, string.Empty, innerException)
        {

        }

		public CXECustomerException(string alloyErrorCode)
			: base(ProductCode, AlloyCode, alloyErrorCode, string.Empty, null)
		{

		}

        public static readonly string REGISTRATION_FAILED_DUPLICATE_ID = "1000";
        public static readonly string REGISTRATION_FAILED = "1001";
        public static readonly string CUSTOMER_NOT_FOUND = "1002";
        public static readonly string CUSTOMER_UPDATE_FAILED = "1003";
        public static readonly string CUSTOMER_VALIDATESTATUS_FAILED = "1004";
	}
}
