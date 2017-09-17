using System;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Data
{
	public class BizLocationException : AlloyException
	{
		public static string ProductCode = "1000";
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

		public BizLocationException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}

		public static readonly string LOCATION_CREATE_FAILED = "4200";
		public static readonly string LOCATION_ALREADY_EXISTS = "4201";
		public static readonly string LOCATION_UPDATE_FAILED = "4202";
		public static readonly string LOCATION_GET_FAILED = "4203";
		public static readonly string LOCATION_COUNTERID_STATUS_UPDATE_FAILED = "4204";
		public static readonly string LOCATION_PROCESSOR_CREDENTIALS_GET_FAILED = "4205";
		public static readonly string LOCATION_PROCESSOR_CREDENTIALS_CREATE_FAILED = "4206";

	}
}
