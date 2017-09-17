using MGI.Alloy.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MGI.Alloy.Common.Util.Helper;

namespace MGI.Alloy.Core.Data
{
    public class ChannelPartnerExceptions : AlloyException
    {
        public static string ProductCode = "1000";
        public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

        public ChannelPartnerExceptions(string alloyErrorCode, Exception innerException)
            : base(ProductCode, ProviderCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string CHANNEL_PARTNER_GET_FAILED = "3100";
        public static readonly string CHANNEL_PARTNER_LOCATIONS_GET_FAILED = "3101";
        public static readonly string CHANNEL_PARTNER_GROUP_NOT_FOUND = "3102";
        public static readonly string CHANNEL_PARTNER_GROUP_CREATE_FAILED = "3103";
        public static readonly string CHANNEL_PARTNER_GROUP_UPDATE_FAILED = "3104";
        public static readonly string CHANNEL_PARTNER_GROUP_GET_FAILED = "3105";
        public static readonly string CHANNEL_PARTNER_CERTIFICATE_INFO_GET_FAILED = "3106";
        public static readonly string TIPS_AND_OFFERS_GET_FAILED = "3107";

    }
}
