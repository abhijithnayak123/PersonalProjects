using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IChannelPartnerService
	{
		public List<string> Locations(long agentSessionId, string channelPartner, MGIContext mgiContext)
		{
			return DesktopEngine.Locations(agentSessionId, channelPartner, mgiContext);
		}

        public ChannelPartner ChannelPartnerConfig(string channelPartner, MGIContext mgiContext)
		{
			return DesktopEngine.ChannelPartnerConfig(channelPartner, mgiContext);
		}

        public List<TipsAndOffers> GetTipsAndOffers(long agentSessionId, string channelPartner, string language, string viewName, MGIContext mgiContext)
        {
            return DesktopEngine.GetTipsAndOffers(agentSessionId, channelPartner, language, viewName, mgiContext);
        }

        public List<string> GetPartnerGroups(string channelPartner, MGIContext mgiContext)
		{
			return DesktopEngine.GetPartnerGroups(channelPartner, mgiContext);
		}

        public ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, MGIContext mgiContext)
		{
			return DesktopEngine.GetChannelPartnerCertificateInfo(channelPartnerId, issuer, mgiContext);
		}
	}
}