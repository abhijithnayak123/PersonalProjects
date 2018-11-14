using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace ChexarIO
{
	public class CheckException : AlloyException
	{
		public static string CheckProcessingProductCode = ((int)MGI.Common.Util.ProductCode.CHECK_PROCESSING_PRODUCTCODE).ToString();
		public static string AlloyCode = ((int)ProviderId.Alloy).ToString();

		public CheckException(string errorCode)
			: base(CheckProcessingProductCode, AlloyCode, errorCode, null)
		{
		}

		public CheckException(string errorCode, Exception innerException)
			: base(CheckProcessingProductCode, AlloyCode, errorCode, innerException)
		{
		}


		public static string LOCATION_NOT_SET = "2000";
		public static string ACCOUNT_NOT_FOUND = "2001";
		public static string TRANSACTION_NOT_FOUND = "2002";
		public static string CHEXAR_CREDENTIALS_NOT_FOUND = "2003";
		public static string CHEXAR_LOGIN_FAILED = "2004";
		public static string CHEXAR_CHECK_TYPE_NOT_FOUND = "2005";
		public static string CHEXAR_COMMIT_FAILED = "2006";
		public static string CHEXAR_SUBMIT_FAILED = "2007";
		public static string CHEXAR_GET_TRANSACTION_FAILED = "2008";
		public static string CHEXAR_GET_STATUS_FAILED = "2009";
		public static string CHEXAR_CANCEL_TRANSACTION_FAILED = "2010";
		public static string CHEXAR_GET_CHECK_PROCESSOR_INFO_FAILED = "2011";
		public static string CHEXAR_UPDATE_TRANSACTION_FAILED = "2012";
		public static string CHEXAR_PENDING_CHECK_FAILED = "2013";
		public static string CHEXAR_REGISTER_FAILED = "2014";
		public static string CHEXAR_GET_ACCOUNT_FAILED = "2015";
		public static string CHEXAR_SETUP_PARTNER_FAILED = "2016";
		public static string CHEXAR_GET_SESSION_FAILED = "2017";
		public static string CHEXAR_GET_WAIT_TIME_FAILED = "2018";
		public static string CHEXAR_CLOSE_TRANSACTION_FAILED = "2019";
		public static string CHEXAR_TRANSACTION_CREATE_FAILED = "2020";
		public static string CHEXAR_GET_MICR_DETAILS_FAILED = "2021";
		public static string CHEXAR_EMPLOYEE_LOGIN_ERROR = "2022";
		public static string CHEXAR_ACCOUNT_UPDATE_FAILED = "2023";

	}
}
