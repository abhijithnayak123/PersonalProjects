using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data.Exceptions
{
	public class TerminalException : ZeoException
	{
		public static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();


		public TerminalException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}


		public static readonly string TERMINAL_CREATE_FAILED = "3300";
		public static readonly string TERMINAL_UPDATE_FAILED = "3301";
		public static readonly string TERMINAL_GET_FAILED = "3302";
		public static readonly string NPSTERMINAL_CREATE_FAILED = "3303";
		public static readonly string NPSTERMINAL_UPDATE_FAILED = "3304";
		public static readonly string NPSTERMINAL_GET_FAILED = "3305";
	}
}
