using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IMessageCenterService
	{
		public List<AgentMessage> GetAgentMessages(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetAgentMessages(agentSessionId, mgiContext);
		}

        public AgentMessage GetMessageDetails(long agentSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetMessageDetails(agentSessionId, transactionId, mgiContext);
		}
	}
}

