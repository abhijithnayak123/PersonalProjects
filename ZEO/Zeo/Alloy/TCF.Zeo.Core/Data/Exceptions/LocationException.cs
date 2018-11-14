using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class LocationException : ZeoException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

        public LocationException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, ProviderCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string LOCATION_CREATE_FAILED = "3200";
        public static readonly string LOCATION_UPDATE_FAILED = "3202";
        public static readonly string LOCATION_GET_FAILED = "3203";
        public static readonly string LOCATION_COUNTERID_GET_FAILED = "3205";
        public static readonly string LOCATION_COUNTERID_STATUS_UPDATE_FAILED = "3206";
        public static readonly string DUPLICATE_LOCATIONID = "3207";
        public static readonly string LOCATION_PROCESSOR_CREDENTIALS_GET_FAILED = "3208";
        public static readonly string LOCATION_PROCESSOR_CREDENTIALS_CREATE_FAILED = "3209";
        public static readonly string LOCATION_COUNTERID_CUSTOMER_SESSION_FAILED = "3210";
        public static readonly string GET_LACATION_STATE_NAMES = "3211";
    }
}
