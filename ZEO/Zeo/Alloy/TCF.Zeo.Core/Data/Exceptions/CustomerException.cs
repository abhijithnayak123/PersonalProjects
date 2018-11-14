using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data.Exceptions
{
    class CustomerException : ZeoException
    {
        static string ProductCode = ((int)Helper.ProductCode.Customer).ToString();
        static string ProviderCode = ((int)Helper.ProviderId.Alloy).ToString();

        public CustomerException(string errorCode)
            : base(ProductCode, ProviderCode, errorCode, null)
        {
        }

        public CustomerException(string errorCode, Exception innerException)
            : base(ProductCode, ProviderCode, errorCode, innerException)
        {
        }

        public static readonly string CUSTOMER_CREATE_FAILED = "3000";
        public static readonly string CUSTOMER_NOT_FOUND = "3002";
        public static readonly string CUSTOMER_UPDATE_FAILED = "3003";
        public static readonly string CUSTOMER_SESSION_CREATE_FAILED = "3005";

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
        public static readonly string SSN_VALIDATE_FAILED = "3029";
        public static readonly string SEARCH_CUSTOMERS_FAILED = "3030";
        public static readonly string GET_STATENAME_FAILED = "3031";
        public static readonly string UPDATE_ERRORREASON_FAILED = "3032";

    }
}
