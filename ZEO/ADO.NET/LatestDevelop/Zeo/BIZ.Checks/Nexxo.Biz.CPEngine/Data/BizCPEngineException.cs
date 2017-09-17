using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Biz.CPEngine.Data
{
	public class BizCPEngineException : AlloyException
	{

		public static string ProductCode = "1002";
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

		public BizCPEngineException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}
		public BizCPEngineException(string alloyErrorCode)
			: base(ProductCode, ProviderCode, alloyErrorCode, null)
		{
		}


		public static readonly string CHECKFRANK_TEMPLATE_NOT_FOUND = "6002";
		public static readonly string CHECK_SUBMIT_FAILED = "6003";
		public static readonly string CHECK_CANCEL_FAILED = "6004";
		public static readonly string CHECK_UPDATE_FAILED = "6005";
		public static readonly string RESUBMIT_FAILED = "6006";
		public static readonly string COMMIT_FAILED = "6007";
		public static readonly string GET_CHECK_TYPE_FAILE = "6008";
		public static readonly string GET_FEE_FAILED = "6009";
		public static readonly string GET_CHECK_FRANK_FAILED = "6010";
		public static readonly string GET_TRANSACTION_FAILED = "6011";
		public static readonly string CHECK_GET_FAILED = "6012";
		public static readonly string GET_STATUS_FAILED = "6013";
		public static readonly string CPENGINE_EXCEPTION = "6014";


		// Used in  bizlayer CPEngineServiceImpl for logic to do look up form MessageStore
		public static readonly string UNHANDLED_DECLINE_CODE = "6015";

	}
}
