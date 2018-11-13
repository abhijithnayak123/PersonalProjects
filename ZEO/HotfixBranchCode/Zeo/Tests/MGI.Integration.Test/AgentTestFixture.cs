using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Integration.Test.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test
{
	[TestFixture]
	public partial class AlloyIntegrationTestFixture
	{
		#region Members
		Desktop client = new Desktop();
		Channel.DMS.Server.Data.MGIContext context = new Channel.DMS.Server.Data.MGIContext();
		# endregion

		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void CreateAgentAndAgentSessionIT(string channelPartnerName)
		{
			var agentSession = CreateAgentAndAgentSession(channelPartnerName);
			Assert.That(agentSession, Is.Not.Null);
		}

		private AgentSession CreateAgentAndAgentSession(string channelPartnerName)
		{
			AgentSSO agent = GetAgentDetails(channelPartnerName);
			string terminalName = GetTerminalName();
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			Channel.DMS.Server.Data.MGIContext context = new Channel.DMS.Server.Data.MGIContext();
			AgentSession agentSession = client.AuthenticateSSO(agent, channelPartner.Id.ToString(), terminalName, context);
			return agentSession;
		}

		private AgentSSO GetAgentDetails(string channelPartnerName)
		{
			AgentSSO ssoDetails = IntegrationTestData.GetSSOAgentData(channelPartnerName);
			return ssoDetails;
		}

		private ChannelPartner GetChannelPartner(string channelPartnerName)
		{
			ChannelPartner channelPartner = client.GetChannelPartner(channelPartnerName, null);
			return channelPartner;
		}

	}
}
