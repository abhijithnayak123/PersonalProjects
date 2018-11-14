using System;

using MGI.Common.Sys;

namespace MGI.Core.CXE.Contract
{
	public class CXEFundException : AlloyException
	{
		const string ProductCode = "1003";
		const string AlloyCode = "100";

		public CXEFundException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, AlloyCode, alloyErrorCode, string.Empty, innerException)
		{
		}

		public static string FUND_GET_FAILED = "1000";
		public static string FUND_CREATE_FAILED = "1001";
		public static string FUND_UPDATE_FAILED = "1002";
		public static string FUND_COMMIT_FAILED = "1003";
	}
}
