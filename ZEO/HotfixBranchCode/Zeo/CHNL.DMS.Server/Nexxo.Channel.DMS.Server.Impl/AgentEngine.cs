using System;
using System.Linq;
using AutoMapper;
using Spring.Transaction.Interceptor;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;

using BizPartnerData = MGI.Biz.Partner.Data;

using BizAgentService = MGI.Biz.Partner.Contract.IAgentService;
using AuthStatus = MGI.Biz.Partner.Data.AuthenticationStatus;
using System.Collections.Generic;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IAgentService
	{
		public BizAgentService AgentService { get; set; }


		[Transaction()]
		public AgentSession AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.Agent agent = AgentService.AuthenticateSSO(Mapper.Map<BizPartnerData.AgentSSO>(ssoAgent), channelPartner, terminalName, context);
			return getAgentSession(agent, channelPartner, terminalName, mgiContext);
		}

		[Transaction()]
		private SessionContext GetSessionContext(string sessionId)
		{
			MGI.Common.Util.MGIContext mgiContext = new MGI.Common.Util.MGIContext();
			BizPartnerData.Session session = AgentService.GetSession(long.Parse(sessionId), mgiContext);
			BizPartnerData.UserDetails profile = UserService.GetUser(long.Parse(sessionId), session.AgentId, mgiContext);

			return new SessionContext
			{
				AgentId = session.AgentId,
				AgentName = profile.FullName,
				LocationId = Guid.Parse(session.Terminal.Location.RowGuid.ToString()),
                TimezoneId = session.Terminal.Location.TimezoneID,
				ChannelPartnerId = profile.ChannelPartnerId,
				AppName = "DMS-Server",
				AgentSessionId = session.rowguid
			};
		}

		private CashierDetails GetAgentDetails(string sessionId, MGI.Common.Util.MGIContext mgiContext)
		{
			CashierDetails cashierDetails = new CashierDetails();
			if (!string.IsNullOrWhiteSpace(sessionId))
			{
				BizPartnerData.Session session = AgentService.GetSession(long.Parse(sessionId), mgiContext);
				if (session != null)
				{
					BizPartnerData.UserDetails profile = UserService.GetUser(long.Parse(sessionId), session.AgentId, mgiContext);
					cashierDetails.AgentId = session.AgentId;
					cashierDetails.AgentFirstName = profile.FirstName;
					cashierDetails.AgentLastName = profile.LastName;
				} 
			}
			return cashierDetails;
		}
		internal static void AgentConverter()
		{
			Mapper.CreateMap<BizPartnerData.Location, Location>();
			Mapper.CreateMap<BizPartnerData.Terminal, Terminal>();
			Mapper.CreateMap<AgentSSO, BizPartnerData.AgentSSO>();
			Mapper.CreateMap<UserRole, BizPartnerData.UserRole>();
		}

		[Transaction()]
		public bool UpdateSession(long agentSessionId, Terminal terminal, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.Terminal bizTerminal = Mapper.Map<BizPartnerData.Terminal>(terminal);
            return AgentService.UpdateSession(agentSessionId, bizTerminal, context);
		}

		private AgentSession getAgentSession(BizPartnerData.Agent agent, string channelPartner, string terminalName, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			string agentSessionId = "0";
			Terminal agentTerminal = null;
			if (agent.AuthStatus == AuthStatus.Authenticated)
			{
				MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
				BizPartnerData.Session agentSession = AgentService.CreateSession(agent.Id.ToString(), terminalName, channelPartner, context);
				agentSessionId = agentSession.Id.ToString();
				agentTerminal = Mapper.Map<Terminal>(agentSession.Terminal);
			}

			return new AgentSession
			{
				SessionId = agentSessionId,
				Agent = new Data.Agent
				{
					Id = agent.Id,
					Name = agent.Name,
					AuthenticationStatus = (int)agent.AuthStatus,
					PasswordExpirationDate = agent.PasswdStatus.ExpirationDate,
					PasswordChangeRequired = agent.PasswdStatus.ChangeRequired,
					UserName = agent.UserName
				},
				Terminal = agentTerminal
			};
		}
	}
}