using MGI.Common.Sys;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Customer.Data
{
    public class CustomerException : AlloyException
    {
        static string ProductCode = "1001";
        static string ProviderCode = ((int)ProviderId.Alloy).ToString();

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
        public static string UPDATE_CUSTOMER_FAILED = "2002";
        public static string CREATE_MISC_CUSTOMER_FAILED = "2003";
        public static string UPDATE_ACCOUNT_FAILED = "2004";
        public static string ADD_ACCOUNT_FAILED = "2005";
        public static string VALIDATE_CUSTOMER_STATUS_FAILED = "2006";
        public static string FIND_CLIENT_PROFILESTATUS_FAILED = "2007";
        public static string FIND_CLIENT_CUSTIND_FAILED = "2008";
        public static string FIND_ACCOUNT_FAILED = "2009";
        public static string CREDENTIALS_NOT_FOUND = "2010";
        public static string MULTIPLE_ACCOUNT_FOUND = "2011";
        public static string APPLICATION_INFO_ERROR = "2012";
        
    }
}
