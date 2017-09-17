using TCF.Zeo.Common.Data;
using System;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Cxn.Check.Data.Exceptions
{
    public class CheckException : ZeoException
	{
		public static string CheckProcessingProductCode = ((int)Helper.ProductCode.CheckProcessing).ToString();
		public static string AlloyCode = ((int)Helper.ProviderId.Alloy).ToString();

		public CheckException(string errorCode)
			: base(CheckProcessingProductCode, AlloyCode, errorCode, null)
		{
		}

		public CheckException(string errorCode, Exception innerException)
			: base(CheckProcessingProductCode, AlloyCode, errorCode, innerException)
		{
		}

        
		public static string CHECK_CREDENTIALS_NOT_FOUND = "2003";
		public static string CHECK_LOGIN_FAILED = "2004";
		public static string CHECK_COMMIT_FAILED = "2006";
		public static string CHECK_SUBMIT_FAILED = "2007";
		public static string CHECK_GET_TRANSACTION_FAILED = "2008";
		public static string CHECK_GET_STATUS_FAILED = "2009";
		public static string CHECK_CANCEL_TRANSACTION_FAILED = "2010";
		public static string CHECK_GET_CHECK_PROCESSOR_INFO_FAILED = "2011";
		public static string CHECK_UPDATE_TRANSACTION_FAILED = "2012";
		public static string CHECK_PENDING_CHECK_FAILED = "2013";
		public static string CHECK_REGISTER_FAILED = "2014";
		public static string CHECK_GET_ACCOUNT_FAILED = "2015";
		public static string CHECK_SETUP_PARTNER_FAILED = "2016";
		public static string CHECK_GET_SESSION_FAILED = "2017";
		public static string CHECK_GET_WAIT_TIME_FAILED = "2018";
		public static string CHECK_CLOSE_TRANSACTION_FAILED = "2019";
		public static string CHECK_TRANSACTION_CREATE_FAILED = "2020";
		public static string GET_CHECK_MICR_DETAILS_FAILED = "2021";
		public static string CHECK_EMPLOYEE_LOGIN_ERROR = "2022";
		public static string CHECK_ACCOUNT_UPDATE_FAILED = "2023";

	}
}
