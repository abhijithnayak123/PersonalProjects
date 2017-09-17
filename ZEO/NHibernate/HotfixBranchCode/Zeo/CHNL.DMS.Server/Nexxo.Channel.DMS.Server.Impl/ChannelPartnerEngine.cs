using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AutoMapper;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;

using ChannelPartnerService = MGI.Biz.Partner.Contract.IChannelPartnerService;

using ChannelPartner = MGI.Biz.Partner.Data.ChannelPartner;
using ChannelPartnerDTO = MGI.Channel.DMS.Server.Data.ChannelPartner;

using TipsAndOffers = MGI.Biz.Partner.Data.TipsAndOffers;
using TipsAndOffersDTO = MGI.Channel.DMS.Server.Data.TipsAndOffers;
using Spring.Transaction.Interceptor;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IChannelPartnerService
	{
		public ChannelPartnerService ChannelPartnerService { get; set; }

		[Transaction(ReadOnly = true)]
		public List<string> Locations(long agentSessionId, string channelPartner, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            return ChannelPartnerService.Locations(agentSessionId, channelPartner, context);
		}

		[Transaction(ReadOnly = true)]
		public ChannelPartnerDTO ChannelPartnerConfig(string channelPartner, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			ChannelPartner cp = ChannelPartnerService.ChannelPartnerConfig(channelPartner,context);
			return Mapper.Map<ChannelPartner, ChannelPartnerDTO>(cp);
		}

		[Transaction()]
		internal static void ChannelPartnerConverter()
		{
			Mapper.CreateMap<MGI.Biz.Partner.Data.ChannelPartnerProductProvider, ChannelPartnerProductProvider>();
			Mapper.CreateMap<ChannelPartner, ChannelPartnerDTO>();
			Mapper.CreateMap<TipsAndOffers, TipsAndOffersDTO>();
			Mapper.CreateMap<MGI.Biz.Partner.Data.ChannelPartnerCertificate, ChannelPartnerCertificate>();
		}


		[Transaction()]
		public List<TipsAndOffersDTO> GetTipsAndOffers(long agentSessionId, string channelPartner, string language, string viewName, MGIContext mgiContext)
        {
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<TipsAndOffers> bizTipsAndOffers = ChannelPartnerService.GetTipsAndOffers(agentSessionId, channelPartner, language, viewName, context);
            List<TipsAndOffersDTO> tipsAndOffers = Mapper.Map<List<TipsAndOffersDTO>>(bizTipsAndOffers);
            return tipsAndOffers;
        }

		[Transaction(ReadOnly=true)]
		public List<string> GetPartnerGroups(string channelPartner, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return ChannelPartnerService.GetGroups(channelPartner, context);
		}

		[Transaction(ReadOnly = true)]
		public ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			MGI.Biz.Partner.Data.ChannelPartnerCertificate certificateInfo = ChannelPartnerService.GetChannelPartnerCertificateInfo(channelPartnerId, issuer, context);
			return Mapper.Map<ChannelPartnerCertificate>(certificateInfo);
		}
	} 
}
