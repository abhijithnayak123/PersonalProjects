using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class ChannelPartnerException : ZeoException
    {
        public static string ProductCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

        public ChannelPartnerException(string alloyErrorCode, Exception innerException)
            : base(ProductCode, ProviderCode, alloyErrorCode, innerException)
        {
        }

        public static readonly string CHANNEL_PARTNER_GET_FAILED = "4100";
        public static readonly string CHANNEL_PARTNER_GROUP_GET_FAILED = "4101";
        public static readonly string TIPS_AND_OFFERS_GET_FAILED = "4102";
        public static readonly string CHANNEL_PARTNER_CERTIFICATE_INFO_GET_FAILED = "4103";
        public static readonly string CHANNEL_PARTNER_GET_PROVIDERS_FAILED = "4104";

    }
}
