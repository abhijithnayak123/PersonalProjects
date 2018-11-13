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
		public Terminal LookupTerminal(long Id, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.Terminal terminal = BIZPartnerTerminalService.Lookup(Id, context);
			return Mapper.Map<Terminal>(terminal);
		}

		[Transaction()]
        public bool CreateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.Terminal bizTerminal = Mapper.Map<BizPartnerData.Terminal>(terminal);
			return BIZPartnerTerminalService.Create(agentSessionId, bizTerminal, context);
		}

		[Transaction()]
        public bool UpdateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.Terminal bizTerminal = Mapper.Map<BizPartnerData.Terminal>(terminal);
			return BIZPartnerTerminalService.Update(agentSessionId, bizTerminal, context);
		}

		[Transaction()]
        public Terminal LookupTerminal(long agentSessionId, string terminalName, int channelPartnerId, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.Terminal terminal = BIZPartnerTerminalService.Lookup(agentSessionId, terminalName, channelPartnerId, context);
			return Mapper.Map<Terminal>(terminal);
		}
	}
}
