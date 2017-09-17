using System;

using MGI.Common.Sys;
using MGI.Common.Util;


namespace MGI.Biz.Partner.Data
{
	public class BizAgentException : AlloyException
	{
		public static string ProductCode = "1000";
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

		public BizAgentException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}		

		public static readonly string AGENTSESSION_CREATE_FAILED = "4000";
		public static readonly string AGENTSESSION_GET_FAILED = "4001";
		public static readonly string AGENTSESSION_UPDATE_FAILED = "4002";
		public static readonly string USER_GET_FAILED = "4003";	
	}
}
