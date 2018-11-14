using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Cxn.Customer.Data.Exceptions
{
    public class CustomerException : ZeoException
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

        public static string SEARCH_CUSTOMER_FAILED = "2000";
        public static string CREATE_CUSTOMER_FAILED = "2001";
        public static string ADD_ACCOUNT_FAILED = "2005";
        public static string FIND_ACCOUNT_FAILED = "2009";
        public static string SYNC_IN_FAILED = "2013";
        public static string SSO_ATTRIBUTES_EMPTY = "2014";
        public static string CERIFICATE_NOTFOUND = "2015";
        public static string CUSTOMER_REGISTRATION_EWS_SCANNING_IS_FAILED = "2016";
    }
}
