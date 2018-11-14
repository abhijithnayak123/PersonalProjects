using System;
using MGI.Common.DataAccess.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using System.Collections.Generic;
using MGI.Common.Util;
using MGI.TimeStamp;

namespace MGI.Core.Partner.Impl
{
	public class AgentSessionServiceImpl : IAgentSessionService
	{
		private IRepository<AgentSession> _agtSessionRepo;
		public IRepository<AgentSession> AgentSessionRepo { set { _agtSessionRepo = value; } }

		public AgentSession Create(UserDetails Agent, Terminal terminal, MGI.Common.Util.MGIContext mgiContext)
		{
			try
			{
				Dictionary<string, object> ssoAttributes = new Dictionary<string, object>();

				if (mgiContext.Context.ContainsKey("SSOAttributes") && mgiContext.Context["SSOAttributes"] != null)
				{
					ssoAttributes = (Dictionary<string, object>)mgiContext.Context["SSOAttributes"];
				}
				string businessDate = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BusinessDate");
				DateTime dateTime;
				if (!(!string.IsNullOrEmpty(businessDate) && DateTime.TryParse(businessDate, out dateTime)))
				{
					businessDate = string.Empty;
				}

				AgentSession agentSession = new AgentSession
				{
					rowguid = Guid.NewGuid(),
					DTServerCreate = DateTime.Now,
					Agent = Agent,
					AgentId = Agent.Id.ToString(),
					Terminal = terminal,
					BusinessDate = (!string.IsNullOrEmpty(businessDate)) ? Convert.ToDateTime(businessDate) : (DateTime?)null,
					ClientAgentIdentifier = Agent.ClientAgentIdentifier
				};
				_agtSessionRepo.AddWithFlush(agentSession);
				return agentSession;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new PartnerAgentException(PartnerAgentException.AGENTSESSION_CREATE_FAILED, ex);
			}
		}

		public bool Update(AgentSession agentSession)
		{
			string timezone = agentSession.Terminal.Location.TimezoneID;
			try
			{
				agentSession.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);//Changes for timestamp
				agentSession.DTServerLastModified = DateTime.Now;
				return _agtSessionRepo.Merge(agentSession);
			}
			catch (Exception ex)
			{
				throw new PartnerAgentException(PartnerAgentException.AGENTSESSION_UPDATE_FAILED, ex);
			}
		}

		public AgentSession Lookup(long id)
		{
			try
			{
				return _agtSessionRepo.FindBy(x => x.Id == id);
			}
			catch (Exception ex)
			{
				throw new PartnerAgentException(PartnerAgentException.AGENTSESSION_GET_FAILED, ex);
			}
		}

		public void End(AgentSession agentSession)
		{
			throw new NotImplementedException();
		}
	}
}
