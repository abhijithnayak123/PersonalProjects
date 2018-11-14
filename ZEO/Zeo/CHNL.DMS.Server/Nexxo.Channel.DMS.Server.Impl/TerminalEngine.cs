using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;
using BizPartnerData = MGI.Biz.Partner.Data;
using BizPartnerContract = MGI.Biz.Partner.Contract;
using AutoMapper;
using Spring.Transaction.Interceptor;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : ITerminalService
	{
		public BizPartnerContract.IManageTerminal BIZPartnerTerminalService { private get; set; }

		public static void TerminalEngineConverter()
		{
			Mapper.CreateMap<BizPartnerData.Terminal, Terminal>();
			Mapper.CreateMap<Terminal, BizPartnerData.Terminal>();
			Mapper.CreateMap<BizPartnerData.ChannelPartner, ChannelPartner>();
			Mapper.CreateMap<ChannelPartner, BizPartnerData.ChannelPartner>();
			Mapper.CreateMap<ChannelPartnerProductProvider, BizPartnerData.ChannelPartnerProductProvider>();
		}

		[Transaction()]
		public Response LookupTerminal(long Id, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			BizPartnerData.Terminal terminal = BIZPartnerTerminalService.Lookup(Id, context);

			response.Result = Mapper.Map<Terminal>(terminal);
			return response;
		}

		[Transaction()]
		public Response CreateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.Terminal bizTerminal = Mapper.Map<BizPartnerData.Terminal>(terminal);
			Response response = new Response();

			response.Result = BIZPartnerTerminalService.Create(agentSessionId, bizTerminal, context);
			return response;
		}

		[Transaction()]
		public Response UpdateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.Terminal bizTerminal = Mapper.Map<BizPartnerData.Terminal>(terminal);
			Response response = new Response();

			response.Result = BIZPartnerTerminalService.Update(agentSessionId, bizTerminal, context);
			return response;
		}

		[Transaction()]
		public Response LookupTerminal(long agentSessionId, string terminalName, int channelPartnerId, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();
			BizPartnerData.Terminal terminal = BIZPartnerTerminalService.Lookup(agentSessionId, terminalName, channelPartnerId, context);

			response.Result = Mapper.Map<Terminal>(terminal);
			return response;
		}
	}
}
