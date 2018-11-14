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
		public Response Locations(long agentSessionId, string channelPartner, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			response.Result = ChannelPartnerService.Locations(agentSessionId, channelPartner, context);
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response ChannelPartnerConfig(string channelPartner, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			ChannelPartner cp = ChannelPartnerService.ChannelPartnerConfig(channelPartner, context);
			Response response = new Response();
			response.Result = Mapper.Map<ChannelPartner, ChannelPartnerDTO>(cp);
			return response;
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
		public Response GetTipsAndOffers(long agentSessionId, string channelPartner, string language, string viewName, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<TipsAndOffers> bizTipsAndOffers = ChannelPartnerService.GetTipsAndOffers(agentSessionId, channelPartner, language, viewName, context);
			Response response = new Response();
			response.Result = Mapper.Map<List<TipsAndOffersDTO>>(bizTipsAndOffers);
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response GetPartnerGroups(string channelPartner, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			response.Result = ChannelPartnerService.GetGroups(channelPartner, context);
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			MGI.Biz.Partner.Data.ChannelPartnerCertificate certificateInfo = ChannelPartnerService.GetChannelPartnerCertificateInfo(channelPartnerId, issuer, context);
			Response response = new Response();
			response.Result = Mapper.Map<ChannelPartnerCertificate>(certificateInfo);
			return response;
		}
	}
}
