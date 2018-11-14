using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class FeatureException : ZeoException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

        public FeatureException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, ProviderCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string GET_FEATRUES_FAILED = "4501";
        public static readonly string FEATURE_UPDATE_FAILED = "4502";
    }
}
