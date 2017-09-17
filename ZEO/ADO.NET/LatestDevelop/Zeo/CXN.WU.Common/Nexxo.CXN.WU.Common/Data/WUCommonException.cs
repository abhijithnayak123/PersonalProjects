using System;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Cxn.WU.Common.Data
{
	public class WUCommonException : AlloyException
    {
		static string AlloyCode = ((int)ProviderId.Alloy).ToString();

		public WUCommonException(string wuCommonProductCode, string alloyErrorCode, Exception innerException)
			: base(wuCommonProductCode, AlloyCode, alloyErrorCode, innerException)
		{
		}

		public static readonly string WESTERNUNION_CREDENTIALS_NOTFOUND									= "2079";
		public static readonly string INVALID_ACCOUNTIDENTIFIER_COUNTERID								= "2001";
		public static readonly string CERIFICATE_NOTFOUND												= "2002";
		public static readonly string BANNERMESSAGE_GET_FAILED											= "2005";
		public static readonly string PAST_BILLERS_GET_FAILED											= "2006";
		public static readonly string WUCARD_LOOKUP_FAILED												= "2053";
		public static readonly string WUCARD_ENROLLMENT_FAILED											= "2052";
	}
}
