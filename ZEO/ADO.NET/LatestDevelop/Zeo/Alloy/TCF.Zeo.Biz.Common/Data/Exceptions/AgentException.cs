using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
	public class AgentException : ZeoException
    {
		public static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

		public AgentException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}		

		public static readonly string AGENTSESSION_CREATE_FAILED = "4000";
		public static readonly string AGENTSESSION_GET_FAILED = "4001";
		public static readonly string AGENT_GET_FAILED = "4002";
        public static readonly string AUTHENTICATE_SSO_FAILED = "4003";
    }
}
