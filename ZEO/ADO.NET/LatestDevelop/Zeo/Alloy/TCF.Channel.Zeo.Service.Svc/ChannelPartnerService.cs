using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IChannelPartnerService
    {
        public Response GetPartnerGroups(long channelPartnerId,ZeoContext context)
        {
            return serviceEngine.GetPartnerGroups(channelPartnerId, context);
        }
        public Response ChannelPartnerConfig(ZeoContext context)
        {
            return serviceEngine.ChannelPartnerConfig(context);
        }
        public Response ChannelPartnerConfigByName(string channelPartnerName, ZeoContext context)
        {
            return serviceEngine.ChannelPartnerConfigByName(channelPartnerName, context);
        }
       
        public Response GetTipsAndOffers(long channelPartnerId, string language, string viewName,string optionalFilter, ZeoContext context)
        {
            return serviceEngine.GetTipsAndOffers(channelPartnerId, language, viewName, optionalFilter, context);
        }
        public Response GetChannelPartnerCertificateInfo(long channelpartnerId, string issuer, ZeoContext context)
        {
            return serviceEngine.GetChannelPartnerCertificateInfo(channelpartnerId, issuer, context);
        }

        public Response GetProvidersbyChannelPartnerName(string channelPartnerName, ZeoContext context)
        {
            return serviceEngine.GetProvidersbyChannelPartnerName(channelPartnerName, context);
        }
    }
}