using System;

using MGI.Common.Sys;

namespace MGI.Biz.Customer.Contract
{
	public class BizCustomerException : NexxoException
	{
		const int CUSTOMER_EXCEPTION_MAJOR_CODE = 1001;

		public BizCustomerException( int MinorCode, string Message )
			: this( MinorCode, Message, null )
		{
		}

		public BizCustomerException( int MinorCode )
			: this( MinorCode, string.Empty )
		{
		}

		public BizCustomerException( int MinorCode, Exception innerException )
			: this( MinorCode, string.Empty, innerException )
		{
		}
		
		public BizCustomerException( int MinorCode, string Message, Exception innerException )
			: base( CUSTOMER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException )
		{
		}

		static public int ACTIVATION_FAILED = 6000;
		static public int ACTIVATION_FAILED_PROSPECT_INCOMPLETE = 6001;
		static public int OFAC_WARNING = 6002;
		static public int INVALID_PIN = 6003;
		static public int INVALID_CUSTOMER_DATA = 6004;
		static public int INVALID_CUSTOMER_DATA_NAME = 6005; // deprecated
		static public int INVALID_CUSTOMER_DATA_ADDRESS1 = 6006;
		static public int INVALID_CUSTOMER_DATA_PHONE1 = 6007;
		static public int INVALID_CUSTOMER_DATA_EMAIL = 6008;
		static public int INVALID_CUSTOMER_DATA_DOB = 6009;
		static public int INVALID_CUSTOMER_DATA_ID_TYPE_NOT_FOUND = 6010;
		static public int INVALID_CUSTOMER_DATA_CHECK_PROFILE = 6011;
		static public int INVALID_CUSTOMER_DATA_SSN = 6012;
		static public int LEAD_NOT_SAVED = 6013;
		static public int CONTACTS_NOT_SAVED = 6014;
		static public int ID_NOT_SAVED = 6015;
		static public int CHECK_PROFILE_NOT_SAVED = 6016;
		static public int LEAD_NOT_FOUND = 6017;
		static public int INVALID_CUSTOMER_DATA_REFERRAL_CODE = 6018;
		static public int INVALID_CUSTOMER_DATA_FNAME = 6019;
		static public int INVALID_CUSTOMER_DATA_MNAME = 6020;
		static public int INVALID_CUSTOMER_DATA_LNAME = 6021;
		static public int INVALID_CUSTOMER_DATA_LNAME2 = 6022;
		static public int INVALID_CUSTOMER_DATA_MOMANAME = 6023;
		static public int INVALID_CUSTOMER_DATA_ADDRESS2 = 6024;
		static public int INVALID_CUSTOMER_DATA_CITY = 6025;
		static public int INVALID_CUSTOMER_DATA_POSTAL_CODE = 6026;
		static public int INVALID_CUSTOMER_DATA_PHONE2 = 6027;
		static public int INVALID_CUSTOMER_DATA_INVALID_ID = 6028;
		static public int INVALID_CUSTOMER_DATA_OCCUPATION = 6029;
		static public int INVALID_CUSTOMER_DATA_EMPLOYER_NAME = 6030;
		static public int INVALID_CUSTOMER_DATA_EMPLOYER_PHONE = 6031;
		static public int INVALID_CUSTOMER_DATA_CARDNUMBER = 6032;
		static public int CARDNUMBER_NOT_UNIQUE = 6033;
		static public int INVALID_CUSTOMER_SEARCH_NO_CRITERIA_PROVIDED = 6034;
		static public int CUSTOMER_SESSION_ID_NOT_FOUND = 6035;
		static public int CUSTOMER_PROFILE_NOT_FOUND = 6036;
		static public int ERROR_RETREIVING_TRANSACTION_HISTORY = 6037;
		static public int INVALID_CUSTOMER_ID = 6038;
		static public int INVALID_SESSION_ID = 6039;
		static public int INVALID_CUSTOMER_SEARCH_NOT_ENOUGH_CRITERIA_PROVIDED = 6040;
        static public int LOCATION_NOT_SET = 6041;
        static public int PROCESSOR_NOT_SET = 6042;
        static public int ANONYMOUS_CUSTOMER_NOT_EXISTS = 6043;
		static public int INVALID_REFERALCODE = 6044;
    }
}
