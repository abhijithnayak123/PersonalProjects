using TCF.Zeo.Biz.Impl;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizAgent = TCF.Zeo.Biz.Contract;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IAgentService
    {
        #region Dependancies
        public BizAgent.IAgentService agentService;
        #endregion

        /// <summary>
        ///  this method will authenticate the sso agent
        /// </summary>
        /// <param name="ssoAgent"> ssoAgent Details</param>
        /// <param name="channelPartner">channel partner Id</param>
        /// <param name="terminalName"> terminal name</param>
        /// <param name="context">Alloy Context</param>
        /// <returns> this will return the agent session as a response</returns>
        public Response AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            agentService = new AgentServiceImpl();
            Agent agent = agentService.AuthenticateSSO(ssoAgent, channelPartner, terminalName, commonContext);
            response.Result = GetAgentSession(agent, channelPartner, terminalName, context);

            return response;
        }

        /// <summary>
        /// this method will get the agent session
        /// </summary>
        /// <param name="agent"> Agent Details</param>
        /// <param name="channelPartner">channel partner Id</param>
        /// <param name="terminalName">terminal name</param>
        /// <param name="context">Alloy Context</param>
        /// <returns>this will return the agent session</returns>
        private AgentSession GetAgentSession(Agent agent, string channelPartner, string terminalName, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            agentService = new AgentServiceImpl();
            AgentSession agentSession = null;
            if (agent.AuthStatus == Helper.AuthenticationStatus.Authenticated)
            {
                agentSession = agentService.CreateSession(agent, channelPartner, terminalName, commonContext);
            }
            return agentSession;
        }

        /// <summary>
		/// this method will get the agent roleId
		/// </summary>
		/// <param name="AgentId"></param>
		/// <returns></returns>
        public Response GetAgentRoleId(long agentId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            agentService = new AgentServiceImpl();
            response.Result = agentService.GetAgentRoleId(agentId, commonContext);

            return response;
        }

        /// <summary>
        /// this method will get the agent Details by passing sessionId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public Response GetAgentDetails(long agentSessionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            agentService = new AgentServiceImpl();
            if (agentSessionId > 0)
            {
                response.Result = agentService.GetAgentDetails(agentSessionId, commonContext);
            }

            return response;
        }

        public Response GetAgents(long locationId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            agentService = new AgentServiceImpl();
            response.Result = agentService.GetAgents(locationId, commonContext);

            return response;
        }
    }
}
