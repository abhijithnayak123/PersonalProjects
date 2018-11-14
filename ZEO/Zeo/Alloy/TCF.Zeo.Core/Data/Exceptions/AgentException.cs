using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class AgentException : ZeoException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string ProviderCode = ((int)Helper.ProviderId.Alloy).ToString();

        public AgentException(string alloyErrorCode, Exception innerException)
           : base(ProductCode, ProviderCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string AGENTSESSION_CREATE_FAILED = "3000";
        public static readonly string AGENTSESSION_GET_FAILED = "3001";

    }
}
