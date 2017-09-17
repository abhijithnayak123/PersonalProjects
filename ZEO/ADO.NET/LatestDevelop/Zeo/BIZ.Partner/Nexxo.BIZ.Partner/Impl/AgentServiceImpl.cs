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
				userDetails.UserRoleId = ssoAgent.Role.Id;
				userDetails.ClientAgentIdentifier = ssoAgent.ClientAgentIdentifier;
				agentId = (int)userDetails.Id;
				userDetails.LocationId = GetLocationId(terminalName, channelPartnerId, mgiContext);
				ManageUserService.UpdateUser(userDetails);
			}
			catch (CoreData.PartnerAgentException cpex)
			{
				if (cpex.AlloyErrorCode != CoreData.PartnerAgentException.USER_NOT_FOUND)
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
			try
			{
				Data.Agent agent = new Data.Agent
				{
					Id = 0,
					Name = string.Empty,
					AuthStatus = AuthenticationStatus.Failed,
					UserName = string.Empty
				};
				CoreData.UserDetails agentProfile = ManageUserService.GetUser(agentId);
				agent.Id = agentId;
				agent.Name = agentProfile.FullName;
				agent.UserName = agentProfile.UserName;
				agent.AuthStatus = AuthenticationStatus.Authenticated;
				return agent;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizAgentException(BizAgentException.USER_GET_FAILED, ex);
			}
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
				BizData.Session bizSession = Mapper.Map<BizData.Session>(agentSession);
				return bizSession;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizAgentException(BizAgentException.AGENTSESSION_CREATE_FAILED, ex);
			}


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
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizAgentException(BizAgentException.AGENTSESSION_UPDATE_FAILED, ex);
			}
		}

		/// <summary>
		/// Looks up Session Details
		/// </summary>
		/// <param name="agentSessionId"></param>
		/// <returns></returns>
		public BizData.Session GetSession(long agentSessionId, MGI.Common.Util.MGIContext context)
		{

			try
			{
				CoreData.AgentSession agentSession = null;
				agentSession = AgentSessionService.Lookup(agentSessionId);
				return Mapper.Map<BizData.Session>(agentSession);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizAgentException(BizAgentException.AGENTSESSION_GET_FAILED, ex);
			}


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
