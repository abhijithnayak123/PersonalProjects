using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.BillPay.Contract
{
	public class BillPayException : NexxoException
	{
		const int MAJOR_CODE = 1004;

		public BillPayException(int minorCode, string message, Exception innerException)
			: base(MAJOR_CODE, minorCode, message, innerException)
		{
		}

		public BillPayException(int minorCode, Exception innerException)
			: base(MAJOR_CODE, minorCode, innerException)
		{
		}

		public BillPayException(int minorCode, string message)
			: base(MAJOR_CODE, minorCode, message)
		{
		}

		public static int CREDENTIALS_NOT_FOUND             = 2400;
		public static int BILLPAY_VALIDATE_FAILED           = 2401;
		public static int BILLPAY_COMMIT_FAILED             = 2402;
		public static int LOCATION_RETRIEVAL_FAILED         = 2403;
		public static int DELIVERY_METHODS_RETRIEVAL_FAILED = 2404;
		public static int BILLER_MESSAGE_RETRIEVAL_FAILED   = 2405;
		public static int BILLER_FIELDS_RETRIEVAL_FAILED    = 2406;
        public static int ACCOUNT_CREATE_FAILED             = 2409;
        public static int ACCOUNT_NOT_FOUND                 = 2410;        
        public static int PROVIDER_IMPORT_FAILED            = 2414;
        public static int PROVIDER_ERROR                    = 2415;
		public static int COUNTERID_NOT_FOUND				= 2416;
		//		 Begin AL-471 Changes
		//       User Story Number: AL-471 | Web |   Developed by: Sunil Shetty     Date: 25.06.2015
		//       Purpose: This method takes only ssn exception message. We have found with ssn we have only 3 exception and below are the one
		public static int INVALID_SOCIAL_SECURITY_NUMBER	= 6008;
		public static int REQUIRES_SSN_ITIN					= 7490;
		public static int MISSING_SSN_ITIN					= 5050;
		//		END AL-471 changes
        public static int TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION = 0425; //AL-2967

		public static int DO_TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION = 0415;//AL-3175

	}
}
