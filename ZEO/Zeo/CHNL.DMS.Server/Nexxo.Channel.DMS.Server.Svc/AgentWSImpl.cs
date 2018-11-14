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
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IAgentService
	{
		public Response AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.AuthenticateSSO(ssoAgent, channelPartner, terminalName, context);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response UpdateSession(long agentSessionId, Terminal terminal, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.UpdateSession(agentSessionId, terminal, context);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}

