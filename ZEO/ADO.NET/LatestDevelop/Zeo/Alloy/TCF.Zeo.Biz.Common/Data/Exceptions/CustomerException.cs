using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class CustomerException : ZeoException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Customer).ToString();
        public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

        public CustomerException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, AlloyCode, alloyErrorCode, innerException)
        {
        }

        public CustomerException(string alloyErrorCode)
            : base(ProductCode, AlloyCode, alloyErrorCode, null)
        {
        }
        public static readonly string GET_COUNTRIES_FAILED = "4021";
        public static readonly string GET_USSTATES_FAILED = "4022";
        public static readonly string GET_STATES_FAILED = "4023";
        public static readonly string GET_IDCOUNTRIES_FAILED = "4024";
        public static readonly string GET_IDSTATES_FAILED = "4025";
        public static readonly string GET_IDTYPES_FAILED = "4026";
        public static readonly string GET_LEGALCODES_FAILED = "4027";
        public static readonly string GET_OCCUPATIONS_FAILED = "4028";
        public static readonly string GET_PHONETYPES_FAILED = "4029";
        public static readonly string GET_MOBILEPROVIDERS_FAILED = "4030";
        public static readonly string GET_MASTERCOUNTRIES_FAILED = "4031";
        public static readonly string GET_MASTER_COUNTRY_BY_CODE_FAILED = "4032";
        public static readonly string ERROR_IN_FIND_IDTYPE = "4033";
        public static readonly string GET_STATENAME_FAILED = "4034";

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
        public static readonly string UPDATE_ERRORREASON_FAILED = "6017";
        public static readonly string NO_CUSTOMERS_FOUND_IN_ZEO = "6018";
    }
}
