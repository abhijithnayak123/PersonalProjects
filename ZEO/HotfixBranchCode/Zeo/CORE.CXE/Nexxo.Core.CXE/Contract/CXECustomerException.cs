using System;

using MGI.Common.Sys;

namespace MGI.Core.CXE.Contract
{
	public class CXECustomerException : NexxoException
	{
		const int CUSTOMER_EXCEPTION_MAJOR_CODE = 1001;

		public CXECustomerException( int MinorCode, string Message )
			: this( MinorCode, Message, null )
		{
		}

		public CXECustomerException( int MinorCode )
			: this( MinorCode, string.Empty )
		{
		}

		public CXECustomerException( int MinorCode, Exception innerException )
			: this( MinorCode, string.Empty, innerException )
		{
		}

		public CXECustomerException( int MinorCode, string Message, Exception innerException )
			: base( CUSTOMER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException )
		{
		}

		static public int ID_GENERATION_FAILED = 1000;
		static public int PROSPECT_SAVE_FAILED = 1001;
		static public int PROSPECT_NOT_FOUND = 1002;
		static public int REGISTRATION_FAILED_DUPLICATE_ID = 1003;
		static public int REGISTRATION_FAILED_DATABASE = 1004;
		static public int CUSTOMER_NOT_FOUND = 1005;
		static public int CUSTOMER_UPDATE_FAILED = 1006;
        static public int CUSTOMER_VALIDATESTATUS_FAILED = 1007;
	}
}
