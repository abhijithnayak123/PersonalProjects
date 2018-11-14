using System;

using MGI.Common.Sys;

namespace MGI.Core.Partner.Data
{
	public class PartnerCustomerException : AlloyException
	{
        const string ProductCode = "1001";
        const string AlloyCode = "100";

        public PartnerCustomerException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, AlloyCode, alloyErrorCode, string.Empty, innerException)
        {

        }

		public PartnerCustomerException(string alloyErrorCode)
			: base(ProductCode, AlloyCode, alloyErrorCode, string.Empty, null)
		{

		}

        public static readonly string PROSPECT_SAVE_FAILED = "3000";
        public static readonly string PROSPECT_NOT_FOUND = "3001";
        public static readonly string CUSTOMER_NOT_FOUND = "3002";
        public static readonly string CUSTOMER_UPDATE_FAILED = "3003";
        public static readonly string CUSTOMER_SESSION_NOT_FOUND = "3004";
        public static readonly string CUSTOMER_SESSION_CREATE_FAILED = "3005";
        public static readonly string CUSTOMER_SESSION_UPDATE_FAILED = "3006";
        public static readonly string CUSTOMER_CHANNELPARTNER_NOT_FOUND = "3007";
        public static readonly string CUSTOMER_IDENTITY_RECORD_FAILED = "3008";
        public static readonly string CUSTOMER_MULTIPLE_ACCOUNT_FOUND = "3009";
        public static readonly string CUSTOMER_SAVE_GROUP_SETTINGS_FAILED = "3010";
        public static readonly string CUSTOMER_CREATE_FEEADJUSTMENT_FAILED = "3011";
        public static readonly string CUSTOMER_FETCH_FEEADJUSTMENT_FAILED = "3012";
        public static readonly string CUSTOMER_UPDATE_FEEADJUSTMENT_FAILED = "3013";

        public static readonly string GET_COUNTRIES_FAILED = "3014";
        public static readonly string GET_USSTATES_FAILED = "3015";
        public static readonly string GET_STATES_FAILED = "3016";
        public static readonly string GET_IDCOUNTRIES_FAILED = "3017";
        public static readonly string GET_IDSTATES_FAILED = "3018";
        public static readonly string GET_IDTYPES_FAILED = "3019";
        public static readonly string GET_LEGALCODES_FAILED = "3020";
        public static readonly string GET_OCCUPATIONS_FAILED = "3021";
        public static readonly string GET_PHONETYPES_FAILED = "3022";
        public static readonly string GET_MOBILEPROVIDERS_FAILED = "3023";
        public static readonly string GET_MASTERCOUNTRIES_FAILED = "3024";
        public static readonly string GET_MASTER_COUNTRY_BY_CODE_FAILED = "3025";
        public static readonly string GET_COUNTRY_BY_CODE_FAILED = "3026";
        public static readonly string GET_ID_STATE_FAILED = "3027";
        public static readonly string GET_NEXXOIDTYPE_FAILED = "3028";

	}
}
