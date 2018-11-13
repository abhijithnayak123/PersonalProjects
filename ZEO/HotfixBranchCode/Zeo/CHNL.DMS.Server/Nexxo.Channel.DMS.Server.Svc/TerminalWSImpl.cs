using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : ITerminalService
	{
		public Terminal LookupTerminal(long Id, MGIContext mgiContext)
		{
			return DesktopEngine.LookupTerminal(Id, mgiContext);
		}

        public bool CreateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			return DesktopEngine.CreateTerminal(agentSessionId, terminal, mgiContext);
		}

        public bool UpdateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			return DesktopEngine.UpdateTerminal(agentSessionId, terminal, mgiContext);
		}

        public Terminal LookupTerminal(long agentSessionId, string terminalName, int channelPartnerId, MGIContext mgiContext)
		{
			return DesktopEngine.LookupTerminal(agentSessionId, terminalName, channelPartnerId, mgiContext);
		}
	}
}