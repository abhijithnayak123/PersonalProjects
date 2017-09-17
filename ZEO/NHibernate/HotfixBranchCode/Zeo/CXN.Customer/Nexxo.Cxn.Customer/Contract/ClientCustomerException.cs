using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using MGI.Common.Sys;

namespace MGI.Cxn.Customer.Contract
{
    public class ClientCustomerException : NexxoException
	{
		const int CUSTOMER_EXCEPTION_MAJOR_CODE = 1011;

		public ClientCustomerException(int MinorCode, string Message, Exception innerException)
            : base(CUSTOMER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{

		}

		public ClientCustomerException(int MinorCode, Exception innerException)
            : base(CUSTOMER_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
		{

		}

        public ClientCustomerException(int MinorCode, string Message)
            : base(CUSTOMER_EXCEPTION_MAJOR_CODE, MinorCode, Message)
        {

        }

        public static int CREATE_ACCOUNT_FAILED = 2000; 
        public static int CONTEXT_NOT_FOUND = 2001;
        public static int GET_LOCATIONINFO_ERROR = 2002;
        public static int FIS_CREDENTIALS_NOT_FOUND = 2003;
        public static int PROVIDER_ERROR = 2004;
        public static int MULTIPLE_ACCOUNT_FOUND = 2005;
        public static int CCIS_LOOKUP_ERROR = 2006;
		public static int TCIS_REGISTRATION_SOFTSTOP = 2007;
		public static int TCIS_REGISTRATION_HARDSTOP = 2008;
		public static int TCIS_CUSTOMERDATA_NOTFOUND = 2009;
		public static int TCIS_FIND_CUSTOMER_FAILED = 2010;
        public static int TCIS_FIND_CUSTOMER_HARDSTOP = 2011;//AL-4348
    }
}
