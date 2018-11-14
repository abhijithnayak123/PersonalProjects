using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface IAgentService : IDisposable
    {
        /// <summary>
        /// Creating/updating the Agent Details.
        /// </summary>
        /// <param name="UserName"> Agent User name</param>
        /// <param name="FirstName">Agent First Name</param>
        /// <param name="LastName">Agent Last Name</param>
        /// <param name="RoleId">Agent Role</param>
        /// <param name="ClientAgentIdentifier">Agent ClientAgentIdentifier</param>
        /// <param name="terminalName">Terminal Name</param>
        /// <param name="channelPartnerId">Agent Chanel partner id</param>
        /// <returns>Agent Details after Create/Update</returns>
        UserDetails AuthenticateSSOAgent(string userName, string firstName, string lastName, string fullName, int roleId, string clientAgentIdentifier, string terminalName, int channelPartnerId, ZeoContext context);

        /// <summary>
        /// Creating the Agent Session
        /// </summary>
        /// <param name="AgentID">agent id which is unique for agent</param>
        /// <param name="ClientAgentIdentifier">Agent ClientAgentIdentifier</param>
        /// <param name="channelPartnerId">Agent Chanel partner id</param>
        /// <param name="terminalName">Terminal Name</param>
        /// <param name="businessDate">Business Date</param>
        /// <returns>returns the agent session</returns>
        AgentSession CreateSession( long agentID,string clientAgentIdentifier, int channelPartnerId, string terminalName, string businessDate, ZeoContext context);

        /// <summary>
		/// This method is get the agent RoleId by agentId
		/// </summary>
		/// <param name="agentId">This is unique identifier of user details</param>
		/// <returns>This is user details</returns>
		int GetAgentRoleId(long agentId, ZeoContext context);

        /// <summary>
        ///  this method will get the agent Details by passing sessionId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        UserDetails GetAgentDetails(long sessionId, ZeoContext context);
        /// <summary>
        /// To get the Agents using the location Id
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        List<UserDetails> GetAgents(long locationId, ZeoContext context);
    }
}
