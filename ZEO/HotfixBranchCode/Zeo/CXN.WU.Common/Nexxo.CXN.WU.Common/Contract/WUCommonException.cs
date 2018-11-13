using System;

using MGI.Common.Sys;

namespace MGI.Cxn.WU.Common.Contract
{
	public class WUCommonException : NexxoException
	{
		const int COMMON_EXCEPTION_MAJOR_CODE = 1005;

		public WUCommonException(int MinorCode, string Message): this(MinorCode, Message, null)
		{
		}

		public WUCommonException(int MinorCode)
			: this(MinorCode, string.Empty)
		{
		}

		public WUCommonException(int MinorCode, Exception innerException)
			: this(MinorCode, string.Empty, innerException)
		{
		}

		public WUCommonException(int MinorCode, string Message, Exception innerException)
			: base(COMMON_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

		public static int WESTERNUNION_CREDENTIALS_NOTFOUND = 2000;
		public static int INVALID_ACCOUNTIDENTIFIER_COUNTERID = 2001;
		public static int CERIFICATE_NOTFOUND = 2002;
		public static int PROVIDER_ERROR = 2003;
		public static int INVALID_COUNTERID = 2004;
		//		 Begin AL-471 Changes
		//       User Story Number: AL-471 | Web |   Developed by: Sunil Shetty     Date: 25.06.2015
		//       Purpose: This method takes only ssn exception message. We have found with ssn we have only 3 exception and below are the one
		public static int INVALID_SOCIAL_SECURITY_NUMBER = 6008;
		public static int REQUIRES_SSN_ITIN = 7490;
		public static int MISSING_SSN_ITIN = 5050;
        
		//		END AL-471 changes
        	public static int TRANSACTION_CANNOT_BE_ADJUSTED = 3343; //AL-2547
		public static int TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION = 0425; //AL-2967
		public static int DO_TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION = 0415;//AL-3175
		
	}
}
