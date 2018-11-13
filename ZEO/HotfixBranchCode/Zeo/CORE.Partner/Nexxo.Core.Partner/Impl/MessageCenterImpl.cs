using System;
using System.Linq;
using System.Collections.Generic;
using MGI.Common.DataAccess.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.TimeStamp;
using MGI.Common.Util;

namespace MGI.Core.Partner.Impl
{
	public class MessageCenterImpl : IMessageCenter
	{
		public IRepository<AgentMessage> MessageCenterRepository { private get; set; }
		public NLoggerCommon NLogger { get; set; }

		public bool Create(AgentMessage agentMessage, string timezone)
		{
			try
			{
				agentMessage.DTTerminalCreate = Clock.DateTimeWithTimeZone(timezone);
				agentMessage.DTServerCreate = DateTime.Now;
				agentMessage.rowguid = Guid.NewGuid();

				MessageCenterRepository.Add(agentMessage);

				return true;
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerException(ChannelPartnerException.AGENT_MESSAGE_CREATE_FAILED, ex);
			}
		}

		public bool Update(AgentMessage agentMessage)
		{
			try
			{
				return MessageCenterRepository.Update(agentMessage);
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerException(ChannelPartnerException.AGENT_MESSAGE_UPDATE_FAILED, ex);
			}
		}

		public bool UpdateStatus(AgentMessage agentMessage, string timezone)
		{
			try
			{
				agentMessage.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
				agentMessage.DTServerLastModified = DateTime.Now;
				return MessageCenterRepository.Update(agentMessage);
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerException(ChannelPartnerException.AGENT_MESSAGE_UPDATE_FAILED, ex);
			}
		}

		public bool Delete(Transaction transaction)
		{
			try
			{
				AgentMessage agentMessage = MessageCenterRepository.FindBy(x => x.Transaction == transaction);
				// can be null if check transaction was not in pending state, i.e. directly approved or declined
				if (agentMessage == null)
				{
					return true;
				}

				agentMessage.IsActive = false;

				return MessageCenterRepository.Update(agentMessage);
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerException(ChannelPartnerException.AGENT_MESSAGE_UPDATE_FAILED, ex);
			}
		}

		public AgentMessage Lookup(Transaction transaction)
		{
			try
			{
				AgentMessage agentMessage = MessageCenterRepository.FindBy(x => x.Transaction == transaction);

				return agentMessage;
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerException(ChannelPartnerException.AGENT_MESSAGE_UPDATE_FAILED, ex);
			}
		}

		public List<AgentMessage> GetByAgentID(long AgentId)
		{
			return MessageCenterRepository.All().Where(x => x.Agent.Id == AgentId && x.IsActive == true).ToList();
		}

		public bool DeleteAllMessages()
		{

			try
			{
				var messages = MessageCenterRepository.FilterBy(m => m.IsActive == true);

				NLogger.Info(string.Format("{0} active messages found", messages.Count()));

				foreach (var message in messages)
				{
					message.IsActive = false;

					message.DTTerminalLastModified = Clock.DateTimeWithTimeZone(message.Transaction.CustomerSession.AgentSession.Terminal.Location.TimezoneID);
					message.DTServerLastModified = DateTime.Now;
					MessageCenterRepository.UpdateWithFlush(message);
				}

				return true;
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerException(ChannelPartnerException.AGENT_MESSAGE_DELETE_FAILED, ex);
			}
		}
	}
}
