using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;

namespace MGI.Biz.Partner.Contract
{
	public interface IAgentService
	{
        /// <summary>
        /// Alternate authentication mechanism for SSO logins.
        /// </summary>
        /// <param name="ssoAgent">A transient instance of AgentSSO[Class]</param>
        /// <param name="channelPartner">This is channelpartner id </param>
        /// <param name="terminalName">This is the terminal name</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Agent Details.</returns>
		Agent AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// Creates the agent(user) session. Associates the terminal with the session.
        /// Also creates the terminal object if not present already
        /// </summary>
        /// <param name="agentId">This is unique identifier for agent</param>
        /// <param name="terminalName">This is the terminal name</param>
        /// <param name="channelPartnerName">This channel partner Name.</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Session Details</returns>
		Session CreateSession(string agentId, string terminalName, string channelPartnerName, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// This method is used to Update the Agent session 
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSessionID to get the agentsession table</param>
        /// <param name="terminal">A transient instance of Terminal[Class]</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Updated Agent Session Status</returns>
		bool UpdateSession(long agentSessionId, Terminal terminal, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// Looks up Session Details.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Session Details.</returns>
		Session GetSession(long agentSessionId, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// To get agent details based on agent unique id.
        /// </summary>
        /// <param name="agentId">This is the unique identifier for Agent</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Agent Details</returns>
		Agent GetAgentDetails(int agentId, MGI.Common.Util.MGIContext mgiContext);
	}
}
