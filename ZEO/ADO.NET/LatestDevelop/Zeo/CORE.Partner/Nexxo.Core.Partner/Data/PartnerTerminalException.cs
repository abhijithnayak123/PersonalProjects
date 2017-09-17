using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Util;
using MGI.Common.Sys;

namespace MGI.Core.Partner.Data
{
	public class PartnerTerminalException : AlloyException
	{
		public static string ProductCode = "1000";
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();


		public PartnerTerminalException(string alloyErrorCode, Exception innerException)
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
