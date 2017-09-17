using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Biz.MoneyOrderEngine.Data
{
	public class BizMoneyOrderEngineException : AlloyException
	{
		public static string MOProductCode = "1006";
		public static string MOProviderCode = ((int)ProviderId.Alloy).ToString();

		public BizMoneyOrderEngineException(string alloyErrorCode, Exception innerException)
			: base(MOProductCode, MOProviderCode, alloyErrorCode, innerException)
		{
		}
		public BizMoneyOrderEngineException(string alloyErrorCode)
			: base(MOProductCode, MOProviderCode, alloyErrorCode, null)
		{
		}

		public static readonly string CHECKPRINT_TEMPLATE_NOT_FOUND = "6002";
		public static readonly string MONEYORDER_COMMIT_ALREADY_EXIST = "6003";
		public static readonly string RESUBMIT_MONEYORDER_FAILED = "6004";
		public static string GET_FEE_FAILED = "6005";
		public static string MONEYOREDER_ADD_FAILED = "6006";
		public static string UPDATE_MONEYORDER_FAILED = "6007";
		public static string COMMIT_MONEYORDER_FAILED = "6008";
		public static string GET_MONEYORDER_FAILED = "6009";
		public static string MONEYORDER_CHECK_PRINT_FAILED = "6010";
		public static string MONEYORDER_DIAGONOSTIC_FAILED = "6011";
		public static string MONEYORDER_EXCEPTION = "6012";

	}
}