using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using MGI.Unit.Test;
using NUnit.Framework;
using System.Collections.Generic;

namespace MGI.Biz.Partner.Test
{
	[TestFixture]
	public class MessageCenterService_Fixture : BaseClass_Fixture
	{
		public IMessageCenterService BIZMessageCenterService { get; set; }

		[Test]
		public void Can_Update_Agent_Message()
		{
			long customerSessionId = 1000000000; 
			AgentMessage agentMessage = new AgentMessage(){};
			MGIContext mgiContext = new MGIContext() { };

			bool status = BIZMessageCenterService.Update(customerSessionId, agentMessage, mgiContext);

			Assert.True(status);
		}

		[Test]
		public void Can_Get_Message_By_Agent_Id()
		{
			long agentSessionId = 1000000000;
			MGIContext mgiContext = new MGIContext() { };

			List<AgentMessage> agentMessages = BIZMessageCenterService.GetMessagesByAgentID(agentSessionId, mgiContext);

			Assert.AreNotEqual(agentMessages.Count, 0);
		}

		[Test]
		public void Can_Get_Message_Details()
		{
			long agentSessionId = 1000000000;
			long transactionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { };

			AgentMessage agentMessage = BIZMessageCenterService.GetMessageDetails(agentSessionId, transactionId, mgiContext);

			Assert.IsNotNull(agentMessage);
		}

	}
}
