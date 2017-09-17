using System;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Biz.Customer.Data
{
    public class BizCustomerException : AlloyException
    {
        public static string ProductCode = "1001";
        public const string AlloyCode = "100";

        public BizCustomerException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

		public BizCustomerException(string alloyErrorCode)
			: base(ProductCode, AlloyCode, alloyErrorCode, null)
		{
		}

        public static readonly string INVALID_CUSTOMER_DATA_ID_TYPE_NOT_FOUND = "6000";
        public static readonly string INVALID_CUSTOMER_SEARCH_NO_CRITERIA_PROVIDED = "6001";
        public static readonly string INVALID_SESSION_ID = "6002";
        public static readonly string INVALID_CUSTOMER_SEARCH_NOT_ENOUGH_CRITERIA_PROVIDED = "6003";
        public static readonly string LOCATION_NOT_SET = "6004";
        public static readonly string INVALID_REFERALCODE = "6005";
        public static readonly string CUSTOMER_SEARCH_FAILED = "6006";
        public static readonly string CUSTOMER_REGISTRATION_FAILED = "6007";
        public static readonly string CUSTOMER_INITIATION_FAILED = "6008";
        public static readonly string PROFILE_STATUS_FETCH_FAILED = "6009";
        public static readonly string CUSTOMER_FETCH_FAILED = "6010";
        public static readonly string CUSTOMER_SAVE_FAILED = "6011";
        public static readonly string CUSTOMER_VALIDATION_FAILED = "6012";
        public static readonly string CUSTOMER_UPDATE_FAILED = "6013";
        public static readonly string CUSTOMER_SYNC_IN_FAILED = "6014";
        public static readonly string SSN_VALIDATION_FAILED = "6015";
        public static readonly string CUSTOMER_EXCEPTION = "6016";
    }
}
