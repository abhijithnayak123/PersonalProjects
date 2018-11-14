using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

#region Alloy Reference
using CommonData = TCF.Zeo.Common.Data;
#endregion

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public  interface IAgentService
    {
        /// <summary>
        ///  this method will authenticate the sso agent
        /// </summary>
        /// <param name="ssoAgent"> ssoAgent Details</param>
        /// <param name="channelPartner">channel partner Id</param>
        /// <param name="terminalName"> terminal name</param>
        /// <param name="context">Alloy Context</param>
        /// <returns> this will return the agent session as a response</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, ZeoContext context);

        /// <summary>
		/// This method is to get the agent RoleId based on agent Id.
		/// </summary>
		/// <param name="AgentId">This is unique identifier for Agent ID for agent details </param>
		/// <returns>Agent information details</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetAgentRoleId(long agentId,ZeoContext context);

        /// <summary>
        /// To get the Agent details using the AgentSessionId
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetAgentDetails(long agentSessionId,ZeoContext context);

        /// <summary>
        /// To get all the agents from the locationId
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetAgents(long locationId, ZeoContext context);
    }
}
