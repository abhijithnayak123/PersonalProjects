using System;

using MGI.Common.Sys;

namespace MGI.Biz.Partner.Data
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

        public static readonly string PROSPECT_SAVE_FAILED = "4000";
        public static readonly string PROSPECT_FETCH_FAILED = "4001";
        public static readonly string CONFIRM_IDENTITY_FAILED = "4002";
        public static readonly string INVALID_CUSTOMER_DATA_FNAME = "4003";
        public static readonly string INVALID_CUSTOMER_DATA_MNAME = "4004";
        public static readonly string INVALID_CUSTOMER_DATA_LNAME = "4005";
        public static readonly string INVALID_CUSTOMER_DATA_LNAME2 = "4006";
        public static readonly string INVALID_CUSTOMER_DATA_MOMANAME = "4007";
        public static readonly string INVALID_CUSTOMER_DATA_CITY = "4008";
        public static readonly string INVALID_CUSTOMER_DATA_POSTAL_CODE = "4009";
        public static readonly string INVALID_CUSTOMER_DATA_PHONE1 = "4010";
        public static readonly string INVALID_CUSTOMER_DATA_PHONE2 = "4011";
        public static readonly string INVALID_CUSTOMER_DATA_EMAIL = "4012";
        public static readonly string INVALID_CUSTOMER_DATA_DOB = "4013";
        public static readonly string INVALID_CUSTOMER_DATA_SSN = "4014";
        public static readonly string INVALID_CUSTOMER_DATA_OCCUPATION = "4015";
        public static readonly string INVALID_CUSTOMER_DATA_EMPLOYER_NAME = "4016";
        public static readonly string INVALID_CUSTOMER_DATA_EMPLOYER_PHONE = "4017";
        public static readonly string INVALID_CUSTOMER_DATA_INVALID_ID = "4018";
        public static readonly string INVALID_CUSTOMER_DATA_ID_TYPE_NOT_FOUND = "4019";
        public static readonly string CUSTOMERSESSION_NOT_FOUND	= "4020";
		public static readonly string GET_COUNTRIES_FAILED = "4021";
		public static readonly string GET_USSTATES_FAILED = "4022";
		public static readonly string GET_STATES_FAILED	= "4023";
		public static readonly string GET_IDCOUNTRIES_FAILED = "4024";
		public static readonly string GET_IDSTATES_FAILED = "4025";
		public static readonly string GET_IDTYPES_FAILED = "4026";
		public static readonly string GET_LEGALCODES_FAILED	= "4027";
		public static readonly string GET_OCCUPATIONS_FAILED = "4028";
		public static readonly string GET_PHONETYPES_FAILED = "4029";
		public static readonly string GET_MOBILEPROVIDERS_FAILED = "4030";
		public static readonly string GET_MASTERCOUNTRIES_FAILED = "4031";
		public static readonly string GET_MASTER_COUNTRY_BY_CODE_FAILED	= "4032";
		public static readonly string ERROR_IN_FIND_IDTYPE = "4033";
	}
}
