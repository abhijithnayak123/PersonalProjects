using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Core.Partner.Data
{
	public class PartnerAgentException : AlloyException
	{
		public static string ProductCode = "1000";
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

		public PartnerAgentException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}

		public static readonly string AGENTSESSION_CREATE_FAILED = "3000";
		public static readonly string AGENTSESSION_GET_FAILED = "3001";
		public static readonly string AGENTSESSION_UPDATE_FAILED = "3002";
		public static readonly string USER_CREATE_FAILED = "3003";
		public static readonly string USER_UPDATE_FAILED = "3004";
		public static readonly string USER_NOT_FOUND = "3005";
		public static readonly string USER_GET_FAILED = "3006";
		public static readonly string USER_AUTHENTICATION_FAILED = "3007";

	}
}
