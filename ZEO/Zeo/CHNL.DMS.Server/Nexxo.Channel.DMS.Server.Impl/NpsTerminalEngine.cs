using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSContract = MGI.Channel.DMS.Server.Contract;
using DMSData = MGI.Channel.DMS.Server.Data;
using BizPartnerData = MGI.Biz.Partner.Data;
using BizPartnerContract = MGI.Biz.Partner.Contract;
using AutoMapper;
using Spring.Transaction.Interceptor;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : DMSContract.INpsTerminalService
	{
		public BizPartnerContract.IManageNpsTerminal BIZPartnerNpsTerminalService { private get; set; }

		internal static void NpsTerminalConveter()
		{
			Mapper.CreateMap<BizPartnerData.NpsTerminal, DMSData.NpsTerminal>();
			Mapper.CreateMap<DMSData.NpsTerminal, BizPartnerData.NpsTerminal>();
			Mapper.CreateMap<DMSData.ChannelPartner, BizPartnerData.ChannelPartner>();
			Mapper.CreateMap<BizPartnerData.ChannelPartner, DMSData.ChannelPartner>();
		}

		[Transaction()]
		public Response CreateNpsTerminal(long agentSessionId, DMSData.NpsTerminal npsTerminal, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.NpsTerminal terminal = Mapper.Map<BizPartnerData.NpsTerminal>(npsTerminal);
			Response response = new Response();
			
			response.Result = BIZPartnerNpsTerminalService.Create(agentSessionId, terminal, context);
			return response;
		}

		[Transaction()]
		public Response UpdateNpsTerminal(long agentSessionId, DMSData.NpsTerminal npsTerminal, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.NpsTerminal terminal = Mapper.Map<BizPartnerData.NpsTerminal>(npsTerminal);
			Response response = new Response();
			
			response.Result = BIZPartnerNpsTerminalService.Update(agentSessionId, terminal, context);
			return response;
		}

		[Transaction()]
		public Response LookupNpsTerminal(long agentSessionId, long Id, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			BizPartnerData.NpsTerminal terminal = BIZPartnerNpsTerminalService.Lookup(Convert.ToString(agentSessionId), Id, context);
			
			response.Result = Mapper.Map<DMSData.NpsTerminal>(terminal);
			return response;
		}

		[Transaction()]
		public Response LookupNpsTerminal(long agentSessionId, string ipAddress, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			BizPartnerData.NpsTerminal terminal = BIZPartnerNpsTerminalService.Lookup(agentSessionId, ipAddress, context);
			
			response.Result = Mapper.Map<DMSData.NpsTerminal>(terminal);
			return response;
		}

		[Transaction()]
		public Response LookupNpsTerminal(long agentSessionId, string name, ChannelPartner channelPartner, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.ChannelPartner bizChannelPartner = Mapper.Map<BizPartnerData.ChannelPartner>(channelPartner);
			BizPartnerData.NpsTerminal terminal = BIZPartnerNpsTerminalService.Lookup(agentSessionId, name, bizChannelPartner, context);
			Response response = new Response();
			
			response.Result = Mapper.Map<DMSData.NpsTerminal>(terminal);
			return response;
		}

		[Transaction()]
		public Response LookupNpsTerminalByLocationID(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			List<BizPartnerData.NpsTerminal> terminals = BIZPartnerNpsTerminalService.GetByLocationID(agentSessionId, locationId, context);
			
			response.Result = Mapper.Map<List<DMSData.NpsTerminal>>(terminals);
			return response;
		}

	}
}
