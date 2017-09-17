using System;
using MGI.Common.Util;
using MGI.Common.Sys;

namespace MGI.Biz.Partner.Data
{
	public class BizTerminalException : AlloyException
	{
		public static string ProductCode = "1000";
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();


		public BizTerminalException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}


		public static readonly string TERMINAL_CREATE_FAILED = "4300";
		public static readonly string TERMINAL_UPDATE_FAILED = "4301";
		public static readonly string TERMINAL_GET_FAILED = "4302";

		public static readonly string NPSTERMINAL_CREATE_FAILED = "4303";
		public static readonly string NPSTERMINAL_UPDATE_FAILED = "4304";
		public static readonly string NPSTERMINAL_GET_FAILED = "4305";
	}
}