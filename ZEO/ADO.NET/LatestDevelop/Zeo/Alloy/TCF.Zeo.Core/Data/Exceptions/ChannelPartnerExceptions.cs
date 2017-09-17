using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class ChannelPartnerExceptions : ZeoException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string ProviderCode = ((int)Helper.ProviderId.Alloy).ToString();

        public ChannelPartnerExceptions(string alloyErrorCode, Exception innerException)
            : base(ProductCode, ProviderCode, alloyErrorCode, innerException)
        {
        }
        
        public static readonly string CHANNEL_PARTNER_GROUP_GET_FAILED = "3105";
        public static readonly string CHANNEL_PARTNER_CERTIFICATE_INFO_GET_FAILED = "3106";
        public static readonly string TIPS_AND_OFFERS_GET_FAILED = "3107";
        public static readonly string CHANNEL_PARTNER_CONFIG_GET_FAILED = "3108";
        public static readonly string CHANNEL_PARTNER_PROVIDERS_GET_FAILED = "3109";
    }
}
