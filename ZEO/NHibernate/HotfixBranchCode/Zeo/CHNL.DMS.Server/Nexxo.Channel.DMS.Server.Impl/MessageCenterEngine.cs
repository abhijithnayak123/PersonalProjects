using System;
using AutoMapper;
using Spring.Transaction.Interceptor;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;

using BizPartnerData = MGI.Biz.Partner.Data;

using BizMessageCenterService = MGI.Biz.Partner.Contract.IMessageCenterService;
using System.Collections.Generic;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IMessageCenterService
	{
		public BizMessageCenterService MessageCenterService { get; set; }


		internal static void MessageCenterConverter()
		{
			Mapper.CreateMap<BizPartnerData.AgentMessage, Server.Data.AgentMessage>();
		}
		

		[Transaction()]
		public List<AgentMessage> GetAgentMessages(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<AgentMessage> agentmsg = new List<AgentMessage>();

			var messages = MessageCenterService.GetMessagesByAgentID(agentSessionId, context);

			return Mapper.Map<List<Server.Data.AgentMessage>>(messages);
		}

		[Transaction()]
        public AgentMessage GetMessageDetails(long agentSessionId, long transactionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			var message = MessageCenterService.GetMessageDetails(agentSessionId, transactionId, context);
			return Mapper.Map<Server.Data.AgentMessage>(message);
		}
	}
}