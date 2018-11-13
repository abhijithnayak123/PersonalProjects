using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;
using MGI.Common.Util;
using CoreData = MGI.Core.Partner.Data;
using CoreContract = MGI.Core.Partner.Contract;
using BizData = MGI.Biz.Partner.Data;
using BizContract = MGI.Biz.Partner.Contract;
using AutoMapper;
using MGI.TimeStamp;


namespace MGI.Biz.Partner.Impl
{
    public class AgentServiceImpl : IAgentService
    {
        private const int MAX_LOGIN_FAILURES = 6;
        private const int PASSWORD_EXPIRATION_DAYS = 30;

        public CoreContract.IManageUsers ManageUserService { private get; set; }
        public CoreContract.IAgentSessionService AgentSessionService { private get; set; }
        public CoreContract.IManageLocations CorePartnerLocationService { private get; set; }
        public CoreContract.ITerminalService PartnerTerminalService { private get; set; }
        public CoreContract.IChannelPartnerService ChannelPartnerService { private get; set; }

        public AgentServiceImpl()
        {
            Mapper.CreateMap<CoreData.AgentSession, Session>();
            Mapper.CreateMap<Session, CoreData.AgentSession>();
            Mapper.CreateMap<CoreData.Terminal, Terminal>();
            Mapper.CreateMap<Terminal, CoreData.Terminal>();
			Mapper.CreateMap<CoreData.ChannelPartner, ChannelPartner>();
			Mapper.CreateMap<CoreData.Location, Location>();
        }

        /// <summary>
        /// Alternate authentication mechanism for SSO logins
        /// </summary>
        /// <param name="ssoAgent"></param>
        /// <param name="channelPartner"></param>
        /// <param name="terminalName"></param>
        /// <returns></returns>
		public BizData.Agent AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, MGI.Common.Util.MGIContext mgiContext)
        {
            // first check to see if the agent already exists
			int agentId;
            long channelPartnerId = long.Parse(channelPartner);
            try
            {
                CoreData.UserDetails userDetails = ManageUserService.GetUser(ssoAgent.UserName, channelPartnerId);
                userDetails.FirstName = ssoAgent.FirstName;
                userDetails.LastName = ssoAgent.LastName;
				userDetails.FullName = string.IsNullOrWhiteSpace(ssoAgent.FullName) ? string.Format("{0} {1}", ssoAgent.FirstName, ssoAgent.LastName) : ssoAgent.FullName;
                userDetails.UserRoleId = ssoAgent.Role.Id;
                userDetails.ClientAgentIdentifier = ssoAgent.ClientAgentIdentifier;				
				agentId = (int)userDetails.Id;
                userDetails.LocationId = GetLocationId(terminalName, channelPartnerId, mgiContext);
                ManageUserService.UpdateUser(userDetails);
            }
            catch (CoreContract.ChannelPartnerException cpex)
            {
                if (cpex.MinorCode != CoreContract.ChannelPartnerException.USER_NOT_FOUND)
                    throw cpex;
                // agent not found, so add
                agentId = ManageUserService.AddUser(
                    new CoreData.UserDetails
                    {
                        Rowguid = Guid.NewGuid(),
                        ChannelPartnerId = channelPartnerId,
                        UserName = ssoAgent.UserName,
                        FirstName = ssoAgent.FirstName,
                        LastName = ssoAgent.LastName,
						FullName = string.IsNullOrWhiteSpace(ssoAgent.FullName) ? string.Format("{0} {1}", ssoAgent.FirstName, ssoAgent.LastName) : ssoAgent.FullName,
                        IsEnabled = true,
                        UserRoleId = ssoAgent.Role.Id,
                        UserStatusId = 1,
                        LocationId = GetLocationId(terminalName, channelPartnerId, mgiContext),
                        ClientAgentIdentifier = ssoAgent.ClientAgentIdentifier
                    }
                );
            }
			return GetAgentDetails(agentId, mgiContext);
		}

        private long GetLocationId(string terminalName, long channelPartnerId, MGIContext mgiContext)
        {
            CoreData.Terminal Terminal = getTerminal(terminalName, channelPartnerId, mgiContext);
            long locationId = long.MinValue;
            if (Terminal != null && Terminal.Location != null)
            {
                locationId = Terminal.Location.Id;
            }
            return locationId;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="agentId"></param>
		/// <returns></returns>
		public BizData.Agent GetAgentDetails(int agentId, MGI.Common.Util.MGIContext context)
		{
			Data.Agent agent = new Data.Agent
			{
				Id = 0,
				Name = string.Empty,
				AuthStatus = AuthenticationStatus.Failed,
				PasswdStatus = new PasswordStatus(),
				UserName = string.Empty
			};
			CoreData.UserDetails agentProfile = ManageUserService.GetUser(agentId);
			agent.Id = agentId;
			agent.Name = agentProfile.FullName;
			agent.UserName = agentProfile.UserName;
			agent.AuthStatus = AuthenticationStatus.Authenticated;
			return agent;
		}

        /// <summary>
        /// Creates the agent(user) session. Associates the terminal with the session.
        /// Also creates the terminal object if not present already
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="terminalName"></param>
        /// <param name="ipAddress"></param>
        /// <returns>MGI.Biz.Partner.Session</returns>
		public BizData.Session CreateSession(string agentId, string terminalName, string channelPartner, MGI.Common.Util.MGIContext mgiContext)
        {
            CoreData.AgentSession agentSession = null;
            CoreData.Terminal terminal = getTerminal(terminalName, int.Parse(channelPartner), mgiContext);
            CoreData.UserDetails agent = ManageUserService.GetUser(Convert.ToInt32(agentId));		
            try
            {
                agentSession = AgentSessionService.Create(agent, terminal, mgiContext);
            }
            catch (Exception ex)
            {
                throw new BizAgentException(BizAgentException.AGENTSESSION_NOT_CREATED, ex);
            }

            BizData.Session bizSession = Mapper.Map<BizData.Session>(agentSession);
            return bizSession;
        }

		public bool UpdateSession(long agentSessionId, Terminal terminal, MGI.Common.Util.MGIContext context)
        {
            try
            {
                CoreData.AgentSession coreSession = AgentSessionService.Lookup(agentSessionId);
                coreSession.Terminal = Mapper.Map<CoreData.Terminal>(terminal);
                return AgentSessionService.Update(coreSession);
            }
            catch (Exception ex)
            {
                throw new BizAgentException(BizAgentException.AGENTSESSION_NOT_UPDATED, ex);
            }
        }

        /// <summary>
        /// Looks up Session Details
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <returns></returns>
		public BizData.Session GetSession(long agentSessionId, MGI.Common.Util.MGIContext context)
        {
            CoreData.AgentSession agentSession = null;
            try
            {
                agentSession = AgentSessionService.Lookup(agentSessionId);
            }
            catch (Exception ex)
            {
                throw new BizAgentException(BizAgentException.AGENTSESSION_NOT_FOUND, ex);
            }

            return Mapper.Map<BizData.Session>(agentSession);
        }

        private CoreData.Terminal getTerminal(string terminalName, long channelPartnerId, MGIContext mgiContext)
        {
            if (string.IsNullOrWhiteSpace(terminalName))
                return null;

            CoreData.ChannelPartner channelPartner = ChannelPartnerService.ChannelPartnerConfig(channelPartnerId);
			return PartnerTerminalService.Lookup(terminalName, channelPartner, mgiContext);
        }
        
    }
}
