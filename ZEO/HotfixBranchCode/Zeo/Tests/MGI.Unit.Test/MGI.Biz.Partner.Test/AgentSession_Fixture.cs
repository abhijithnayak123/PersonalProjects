using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using NUnit.Framework;
using Moq;
using PTNRData = MGI.Core.Partner.Data;


namespace MGI.Unit.Test.MGI.Biz.Partner.Test
{
	[TestFixture]
	public class AgentSession_Fixture : BaseClass_Fixture
	{
		public IAgentService BIZPartnerAgentService { get; set; }

		[Test]
		public void Can_AuthenticateSSO()
		{
			AgentSSO ssoAgent = new AgentSSO() { ClientAgentIdentifier = "", FirstName = "Nitish", LastName = "Biradar", Role = new UserRole() { Id = 1, role = "SystemAdmin" }, UserName = "Biradar" };
			string channelPartner = "36";
			string terminalName = "TCF";
			MGIContext mgiContext = new MGIContext() { };

			Agent agent = BIZPartnerAgentService.AuthenticateSSO(ssoAgent, channelPartner, terminalName, mgiContext);

			Assert.IsNotNull(agent);
			ManageUserService.Verify(moq => moq.AddUser(It.IsAny<PTNRData.UserDetails>()), Times.AtLeastOnce());
			//ManageUserService.Verify(moq => moq.AddUser(It.IsAny<PTNRData.UserDetails>()), Times.Exactly(1));
		}

		[Test]
		public void Can_AuthenticateSSOWithExisting()
		{
			AgentSSO ssoAgent = new AgentSSO() { ClientAgentIdentifier = "", FirstName = "Nitish", LastName = "Biradar", Role = new UserRole() { Id = 1, role = "SystemAdmin" } };
			string channelPartner = "34";
			string terminalName = "TCF";
			MGIContext mgiContext = new MGIContext() { };

			Agent agent = BIZPartnerAgentService.AuthenticateSSO(ssoAgent, channelPartner, terminalName, mgiContext);

			Assert.IsNotNull(agent);
			ManageUserService.Verify(moq => moq.UpdateUser(It.IsAny<PTNRData.UserDetails>()), Times.AtLeastOnce());
			//ManageUserService.Verify(moq => moq.UpdateUser(It.IsAny<PTNRData.UserDetails>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Agent_Details()
		{
			int agentId = 500001;
			MGIContext mgiContext = new MGIContext() { };

			Agent agent = BIZPartnerAgentService.GetAgentDetails(agentId, mgiContext);

			Assert.IsNotNull(agent);
		}

		[Test]
		public void Can_Create_Agent_Session()
		{
			string agentId = "500001";
			string channelPartner = "34";
			string terminalName = "TCF";
			MGIContext mgiContext = new MGIContext() { };

			Session session = BIZPartnerAgentService.CreateSession(agentId, terminalName, channelPartner, mgiContext);

			Assert.IsNotNull(session);
			AgentSessionService.Verify(moq => moq.Create(It.IsAny<PTNRData.UserDetails>(), It.IsAny<PTNRData.Terminal>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Update_Agent_Session()
		{
			long agentSessionId = 1000000000;
			Terminal terminal = new Terminal() { };
			MGIContext mgiContext = new MGIContext() { };

			bool status = BIZPartnerAgentService.UpdateSession(agentSessionId, terminal, mgiContext);

			Assert.True(status);
			AgentSessionService.Verify(moq => moq.Update(It.IsAny<PTNRData.AgentSession>()), Times.Exactly(1));
		}

		[Test]
		[ExpectedException(typeof(BizAgentException))]
		public void Can_Update_Agent_Session_Exception()
		{
			long agentSessionId = 10000099;
			Terminal terminal = new Terminal() { };
			MGIContext mgiContext = new MGIContext() { };

			BIZPartnerAgentService.UpdateSession(agentSessionId, terminal, mgiContext);
		}

		[Test]
		public void Can_Get_Agent_Session()
		{
			long agentSessionId = 1000000000;
			MGIContext mgiContext = new MGIContext() { };

			Session session = BIZPartnerAgentService.GetSession(agentSessionId, mgiContext);

			Assert.IsNotNull(session);
			AgentSessionService.Verify(moq => moq.Lookup(It.IsAny<long>()), Times.AtLeastOnce());
		}
	}
}
