using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Core.Partner.Data
{
	public class PartnerLocationException : AlloyException
	{
		public static string ProductCode = "1000";
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

		public PartnerLocationException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}

		public static readonly string LOCATION_CREATE_FAILED = "3200";
		public static readonly string LOCATION_ALREADY_EXISTS = "3201";
		public static readonly string LOCATION_UPDATE_FAILED = "3202";
		public static readonly string LOCATION_GET_FAILED = "3203";
		public static readonly string LOCATION_BANKID_BRANCHID_ALREADY_EXISTS = "3204";
		public static readonly string LOCATION_COUNTERID_GET_FAILED = "3205";
		public static readonly string LOCATION_COUNTERID_STATUS_UPDATE_FAILED = "3206";
		public static readonly string DUPLICATE_LOCATIONID = "3207";
		public static readonly string LOCATION_PROCESSOR_CREDENTIALS_GET_FAILED = "3208";
		public static readonly string LOCATION_PROCESSOR_CREDENTIALS_CREATE_FAILED = "3209";
	}
}
