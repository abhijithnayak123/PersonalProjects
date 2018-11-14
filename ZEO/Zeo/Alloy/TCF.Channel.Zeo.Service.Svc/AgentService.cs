using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;


namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService: IAgentService
    {

        #region IBillPayService methods
        public Response AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName,ZeoContext context)
        {
            return serviceEngine.AuthenticateSSO(ssoAgent, channelPartner, terminalName,context);
        }

        public Response GetAgentDetails(long agentSessionId,ZeoContext context)
        {
            return serviceEngine.GetAgentDetails(agentSessionId, context);
        }
        public Response GetAgentRoleId(long agentId, ZeoContext context)
        {
            return serviceEngine.GetAgentRoleId(agentId, context);
        }

        public Response GetAgents(long locationId, ZeoContext context)
        {
            return serviceEngine.GetAgents(locationId, context);
        }
        #endregion
    }
}