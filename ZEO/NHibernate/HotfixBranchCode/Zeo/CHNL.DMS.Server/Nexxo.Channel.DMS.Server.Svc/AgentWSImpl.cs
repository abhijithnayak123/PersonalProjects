using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IAgentService
	{
		public AgentSession AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, MGI.Channel.DMS.Server.Data.MGIContext context)
        {
            return DesktopEngine.AuthenticateSSO(ssoAgent, channelPartner, terminalName, context);
        }

		public bool UpdateSession(long agentSessionId, Terminal terminal, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
            return DesktopEngine.UpdateSession(agentSessionId, terminal, context);
		}
	}
}

