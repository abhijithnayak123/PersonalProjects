using AutoMapper;
using TCF.Zeo.Biz.Contract;
using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;
using CoreData = TCF.Zeo.Core;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Biz.Common.Data.Exceptions;

namespace TCF.Zeo.Biz.Impl
{
    public class AgentServiceImpl : IAgentService
    {
        CoreData.Contract.IAgentService AgentService { get; set; }
        IMapper mapper;

        public AgentServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoreData.Data.UserDetails, Agent>().ReverseMap();
                cfg.CreateMap<CoreData.Data.AgentSession, AgentSession>().ReverseMap();
            });

            mapper = config.CreateMapper();
        }
        /// <summary>
        /// Authenticating the Agent
        /// </summary>
        /// <param name="ssoAgent">passing Agent Details</param>
        /// <param name="channelPartner">Channel partner id</param>
        /// <param name="terminalName">Terminal Name</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Agent AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, commonData.ZeoContext context)
        {
            CoreData.Data.UserDetails userDetails = new CoreData.Data.UserDetails();
            int channelPartnerId = int.Parse(channelPartner);
            try
            {
                using (AgentService = new CoreData.Impl.ZeoCoreImpl())
                {
                    userDetails = AgentService.AuthenticateSSOAgent(ssoAgent.UserName, ssoAgent.FirstName, ssoAgent.LastName, ssoAgent.FullName, ssoAgent.RoleId, ssoAgent.ClientAgentIdentifier, terminalName, channelPartnerId, context);
                }

                var agent = mapper.Map<Agent>(userDetails);
                agent.AuthStatus = Helper.AuthenticationStatus.Authenticated;
                agent.Name = userDetails.FullName;
                agent.UserName = userDetails.UserName;
                agent.ClientAgentIdentifier = ssoAgent.ClientAgentIdentifier;
                return agent;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new AgentException(AgentException.AUTHENTICATE_SSO_FAILED, ex);
            }

        }

        /// <summary>
        /// This method will Create the agent session.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="channelPartner"></param>
        /// <param name="terminalName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public AgentSession CreateSession(Agent agent, string channelPartner, string terminalName, commonData.ZeoContext context)
        {
            int channelPartnerId = int.Parse(channelPartner);
            DateTime dateTime;
            string businessDate = null;
            try
            {
                AgentSession agentSession = new AgentSession();
               
                if (context.SSOAttributes != null && context.SSOAttributes.Any())
                {
                    businessDate = Helper.GetDictionaryValueIfExists(context.SSOAttributes, "BusinessDate");
                    if (!(!string.IsNullOrEmpty(businessDate) && DateTime.TryParse(businessDate, out dateTime)))
                    {
                        businessDate = string.Empty;
                    }
                }

                using (AgentService = new CoreData.Impl.ZeoCoreImpl())
                {
                    agentSession = mapper.Map<AgentSession>(AgentService.CreateSession(agent.AgentID, agent.ClientAgentIdentifier, channelPartnerId, terminalName, businessDate, context));
                }

                if (agentSession != null)
                {
                    agentSession.SessionId = Convert.ToString(agentSession.SessionId);
                    agentSession.AgentID = agent.AgentID;
                    agentSession.Name = agent.Name;
                    agentSession.UserName = agent.UserName;
                    agentSession.AuthenticationStatus = (int)agent.AuthStatus;
                    agentSession.LocationName = agentSession.LocationName;
                }
                return agentSession;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new AgentException(AgentException.AGENTSESSION_CREATE_FAILED, ex);
            }

        }

        /// <summary>
        /// This method is fetch the Agent RoleId by agentId
        /// </summary>
        /// <param name="agentId">This is the unique identifier for user</param>
        /// <param name="context"></param>
        /// <returns>Agent Details</returns>
        public int GetAgentRoleId(long agentId, commonData.ZeoContext context)
        {
            int roleId = 0;

            try
            {
                using (AgentService = new CoreData.Impl.ZeoCoreImpl())
                {
                    roleId = AgentService.GetAgentRoleId(agentId, context);
                }

                return roleId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new AgentException(AgentException.AGENT_GET_FAILED, ex);
            }
        }

        /// <summary>
        ///  this method will get the agent Details by passing sessionId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Agent GetAgentDetails(long sessionId, commonData.ZeoContext context)
        {
            Agent agent = new Agent();
            CoreData.Data.UserDetails agentDetail = new CoreData.Data.UserDetails();
            try
            {
                using (AgentService = new CoreData.Impl.ZeoCoreImpl())
                {
                    agentDetail = AgentService.GetAgentDetails(sessionId, context);
                }

                if (agentDetail != null)
                {
                    agent.AgentID = agentDetail.AgentID;
                    agent.AgentFirstName = agentDetail.FirstName;
                    agent.AgentLastName = agentDetail.LastName;
                    agent.UserRoleId = agentDetail.UserRoleId;
                    agent.LocationId = agentDetail.LocationId;
                    agent.AgentFullName = agentDetail.FullName;
                    agent.AuthStatus = (Helper.AuthenticationStatus)agentDetail.AuthStatus;
                }
                return agent;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new AgentException(AgentException.AGENTSESSION_GET_FAILED, ex);
            }
        }

        /// <summary>
        /// this method will get the agents by location Id
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<Agent> GetAgents(long locationId, commonData.ZeoContext context)
        {
            List<Agent> agents = new List<Agent>();
            try
            {
                using (AgentService = new CoreData.Impl.ZeoCoreImpl())
                {
                    AgentService.GetAgents(locationId, context).ForEach(x => agents.Add(new Agent()
                    {
                        AgentFirstName = x.FirstName,
                        AgentFullName = x.FullName,
                        AgentID = x.AgentID,
                        AgentLastName = x.LastName,
                        LocationId = x.LocationId,
                        UserRoleId = x.UserRoleId,
                        UserName = x.UserName,
                        AuthStatus = (Helper.AuthenticationStatus)x.AuthStatus
                    }));
                }

                return agents;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new AgentException(AgentException.AGENT_GET_FAILED, ex);
            }
        }
    }
}
