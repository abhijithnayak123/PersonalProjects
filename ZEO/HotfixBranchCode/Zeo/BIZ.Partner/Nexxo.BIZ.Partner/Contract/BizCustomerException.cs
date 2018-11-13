using System;

using MGI.Common.Sys;

namespace MGI.Biz.Partner.Contract
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

		static public int ACTIVATION_FAILED = 4001;
		static public int ACTIVATION_FAILED_PROSPECT_INCOMPLETE = 4002;
		static public int OFAC_WARNING = 4003;
		static public int INVALID_PIN = 4004;
		static public int INVALID_CUSTOMER_DATA = 4005;
		static public int INVALID_CUSTOMER_DATA_NAME = 4006; // deprecated
		static public int INVALID_CUSTOMER_DATA_ADDRESS1 = 4007;
		static public int INVALID_CUSTOMER_DATA_PHONE1 = 4008;
		static public int INVALID_CUSTOMER_DATA_EMAIL = 4009;
		static public int INVALID_CUSTOMER_DATA_DOB = 4010;
		static public int INVALID_CUSTOMER_DATA_ID_TYPE_NOT_FOUND = 4011;
		static public int INVALID_CUSTOMER_DATA_CHECK_PROFILE = 4012;
		static public int INVALID_CUSTOMER_DATA_SSN = 4013;
		static public int LEAD_NOT_SAVED = 4014;
		static public int CONTACTS_NOT_SAVED = 4015;
		static public int ID_NOT_SAVED = 4016;
		static public int CHECK_PROFILE_NOT_SAVED = 4017;
		static public int LEAD_NOT_FOUND = 4018;
		static public int INVALID_CUSTOMER_DATA_REFERRAL_CODE = 4019;
		static public int INVALID_CUSTOMER_DATA_FNAME = 4020;
		static public int INVALID_CUSTOMER_DATA_MNAME = 4021;
		static public int INVALID_CUSTOMER_DATA_LNAME = 4022;
		static public int INVALID_CUSTOMER_DATA_LNAME2 = 4023;
		static public int INVALID_CUSTOMER_DATA_MOMANAME = 4024;
		static public int INVALID_CUSTOMER_DATA_ADDRESS2 = 4025;
		static public int INVALID_CUSTOMER_DATA_CITY = 4026;
		static public int INVALID_CUSTOMER_DATA_POSTAL_CODE = 4027;
		static public int INVALID_CUSTOMER_DATA_PHONE2 = 4028;
		static public int INVALID_CUSTOMER_DATA_INVALID_ID = 4029;

		static public int INVALID_CUSTOMER_DATA_OCCUPATION = 4030;
		static public int INVALID_CUSTOMER_DATA_EMPLOYER_NAME = 4031;
		static public int INVALID_CUSTOMER_DATA_EMPLOYER_PHONE = 4032;

		static public int INVALID_CUSTOMER_DATA_CARDNUMBER = 4033;
		static public int CARDNUMBER_NOT_UNIQUE = 4034;

		static public int INVALID_CUSTOMER_SEARCH_NO_CRITERIA_PROVIDED = 4035;

		static public int CUSTOMER_SESSION_ID_NOT_FOUND = 4036;

		static public int CUSTOMER_PROFILE_NOT_FOUND = 4037;

		static public int ERROR_RETREIVING_TRANSACTION_HISTORY = 4038;

		static public int INVALID_CUSTOMER_ID = 4039;
		static public int INVALID_SESSION_ID = 4040;

		static public int INVALID_CUSTOMER_SEARCH_NOT_ENOUGH_CRITERIA_PROVIDED = 4041;
		static public int INVALID_CUSTOMER_DATA_DUPLICATE_PHONE_PIN_COMBINATION = 4042;
	}
}
