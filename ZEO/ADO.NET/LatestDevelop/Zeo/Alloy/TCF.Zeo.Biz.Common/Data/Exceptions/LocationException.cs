using System;
using TCF.Zeo.Common.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
	public class LocationException : ZeoException
    {
		public static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

		public LocationException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}

        public static readonly string LOCATION_CREATE_FAILED = "4200";
        public static readonly string LOCATION_ALREADY_EXISTS = "4201";
        public static readonly string LOCATION_UPDATE_FAILED = "4202";
        public static readonly string LOCATION_GET_FAILED = "4203";
        public static readonly string LOCATION_COUNTERID_STATUS_UPDATE_FAILED = "4204";
        public static readonly string LOCATION_PROCESSOR_CREDENTIALS_GET_FAILED = "4205";
        public static readonly string LOCATION_PROCESSOR_CREDENTIALS_CREATE_FAILED = "4206";
        public static readonly string LOCATION_COUNTERID_CUSTOMER_SESSION_FAILED = "4207";
        public static readonly string LOCATION_NAME_ALREADY_EXIST = "4208";
        public static readonly string LOCATION_BANK_OR_BRANCH_ID_ALREADY_EXIST = "4209";
        public static readonly string LOCATION_IDENTIFIER_ALREADY_EXIST = "4210";

    }
}
