using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Cxn.Check.Contract
{
	public class CheckException : NexxoException
	{
		const int CHECK_EXCEPTION_MAJOR_CODE = 1002;

		public CheckException(int MinorCode, string Message, Exception innerException)
			: base(CHECK_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

		public CheckException(int MinorCode, Exception innerException)
			: base(CHECK_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
		{
		}

		public CheckException(int MinorCode, string Message)
			: base(CHECK_EXCEPTION_MAJOR_CODE, MinorCode, Message)
		{
		}

		public static int LOCATION_NOT_SET = 2000;
		public static int ACCOUNT_NOT_FOUND = 2001;
		public static int MISSING_IMAGE = 2002;
		public static int TRANSACTION_NOT_FOUND = 2003;
		public static int CHEXAR_CREDENTIALS_NOT_FOUND = 2004;
		public static int CHEXAR_LOGIN_FAILED = 2005;
		public static int CHEXAR_BADGE_NOT_CREATED = 2006;
		public static int CHEXAR_INVOICE_NOT_CREATED = 2007;
		public static int CHEXAR_INVOICE_NOT_FOUND = 2008;
		public static int CHEXAR_CHECK_TYPE_NOT_FOUND = 2009;
        public static int PROVIDER_ERROR = 2010;
        public static int PARTNER_NOT_SET = 2011;
		public static int INVALID_DICTIONARY_KEY = 2012;
        public static int CHECK_TOO_OLD_TOCASH = -2;
        public static int CHECK_POST_DATED = -3;
        public static int CUSTOMER_ON_HOLD =-1;

	}
}
