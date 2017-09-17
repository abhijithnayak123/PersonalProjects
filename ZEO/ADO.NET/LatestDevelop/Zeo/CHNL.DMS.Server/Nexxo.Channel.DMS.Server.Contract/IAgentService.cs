using System;
using System.ServiceModel;

using MGI.Channel.DMS.Server.Data; // Nexxo Data references.
using MGI.Common.Sys;
using System.Collections.Generic;

namespace MGI.Channel.DMS.Server.Contract
{
	/// <summary>
	/// Agent service to perform agent related ops.
	/// </summary>
	[ServiceContract]
	public interface IAgentService
	{

		/// <summary>
		/// This method is used to get the authentication repository in SSO login
		/// </summary>
		/// <param name="ssoAgent">This field is used to get SSO Agent details</param>
		/// <param name="channelPartner">This is channelpartner id</param>
		/// <param name="terminalName">This is terminal name</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Agent Session Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, MGI.Channel.DMS.Server.Data.MGIContext context);

		/// <summary>
		/// This method is used to Update the Agent session 
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for agentSessionID to get the agentsession table</param>
		/// <param name="terminal">This is terminal name</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Updated agent session</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response UpdateSession(long agentSessionId, Terminal terminal, MGI.Channel.DMS.Server.Data.MGIContext context);
	}
}

