using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
   public interface IAgentService
    {
        /// <summary>
        ///  this method will authenticate the sso agent
        /// </summary>
        /// <param name="ssoAgent"> ssoAgent Details</param>
        /// <param name="channelPartner">channel partner Id</param>
        /// <param name="terminalName"> terminal name</param>
        /// <returns> this will return the agent Details</returns>
        Agent AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, commonData.ZeoContext context);

        /// <summary>
        ///  this method will Create the agent session
        /// </summary>
        /// <param name="Agent"> Agent Details</param>
        /// <param name="channelPartner">channel partner Id</param>
        /// <param name="terminalName"> terminal name</param>
        /// <returns> this will return the agent Session</returns>
        AgentSession CreateSession(Agent agent, string channelPartner, string terminalName, commonData.ZeoContext context);

        /// <summary>
        /// This method is fetch the Agent RoleId by agentId
        /// </summary>
        /// <param name="agentId">This is the unique identifier for user</param>
        /// <returns>Agent Details</returns>
        int GetAgentRoleId(long agentId, commonData.ZeoContext context);

        /// <summary>
        ///  this method will get the agent Details by passing sessionId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Agent GetAgentDetails(long sessionId, commonData.ZeoContext context);
        /// <summary>
        /// To get the all the agents for a particular locationId
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        List<Agent> GetAgents(long locationId, commonData. ZeoContext context);
    }
}
