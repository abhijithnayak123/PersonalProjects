using System;

using MGI.Common.Sys;

namespace MGI.Core.Partner.Contract
{
	public class PartnerCustomerException : NexxoException
	{
		const int CUSTOMER_EXCEPTION_MAJOR_CODE = 1001;

		public PartnerCustomerException( int MinorCode, string Message )
			: this( MinorCode, Message, null )
		{
		}

		public PartnerCustomerException( int MinorCode )
			: this( MinorCode, string.Empty )
		{
		}

		public PartnerCustomerException( int MinorCode, Exception innerException )
			: this( MinorCode, string.Empty, innerException )
		{
		}

		public PartnerCustomerException( int MinorCode, string Message, Exception innerException )
			: base( CUSTOMER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException )
		{
		}

		static public int ID_GENERATION_FAILED = 3000;
		static public int PROSPECT_SAVE_FAILED = 3001;
		static public int PROSPECT_NOT_FOUND = 3002;
		static public int REGISTRATION_FAILED_DUPLICATE_ID = 3003;
		static public int REGISTRATION_FAILED_DATABASE = 3004;
		static public int CUSTOMER_NOT_FOUND = 3005;
		static public int CUSTOMER_UPDATE_FAILED = 3006;
        static public int CUSTOMER_CREATE_FAILED = 3007;
        static public int CUSTOMER_SESSION_NOT_FOUND = 3008;
        static public int CUSTOMER_SESSION_CREATE_FAILED = 3009;
        static public int CUSTOMER_SESSION_UPDATE_FAILED = 3010;
        static public int CUSTOMER_SESSION_SAVE_FAILED = 3011;
        static public int CUSTOMER_SESSION_END_SESSION_FAILED = 3012;
        static public int CUSTOMER_CHANNELPARTNER_NOT_FOUND = 3013;
        static public int CUSTOMER_IDENTITY_RECORD_FAILED = 3014;
		static public int CUSTOMER_MULTIPLE_ACCOUNT_FOUND = 3015;

	}
}
