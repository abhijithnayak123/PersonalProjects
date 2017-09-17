using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : INpsTerminalService
	{
        public bool CreateNpsTerminal(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext)
		{
			return DesktopEngine.CreateNpsTerminal(agentSessionId, npsTerminal, mgiContext);
		}

        public bool UpdateNpsTerminal(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext)
		{
			return DesktopEngine.UpdateNpsTerminal(agentSessionId, npsTerminal, mgiContext);
		}

        public NpsTerminal LookupNpsTerminal(long agentSessionId, long Id, MGIContext mgiContext)
		{
			return DesktopEngine.LookupNpsTerminal(agentSessionId, Id, mgiContext);
		}

        public NpsTerminal LookupNpsTerminal(long agentSessionId, string ipAddress, MGIContext mgiContext)
		{
			return DesktopEngine.LookupNpsTerminal(agentSessionId, ipAddress, mgiContext);
		}

        public List<NpsTerminal> LookupNpsTerminalByLocationID(long agentSessionId, long locationId,  MGIContext mgiContext)
		{
			return DesktopEngine.LookupNpsTerminalByLocationID(agentSessionId, locationId, mgiContext);
		}

        public NpsTerminal LookupNpsTerminal(long agentSessionId, string name, ChannelPartner channelPartner, MGIContext mgiContext)
        {
            return DesktopEngine.LookupNpsTerminal(agentSessionId, name, channelPartner, mgiContext);
        }
	}
}