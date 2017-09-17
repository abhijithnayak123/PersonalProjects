using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using MGI.Integration.Test.Data;
using NUnit.Framework;

namespace MGI.Integration.Test
{
    [TestFixture]
	public partial class AlloyIntegrationTestFixture
	{
        #region Members
        ZeoServiceClient client = new ZeoServiceClient();
		ZeoContext zeoContext = new ZeoContext();
        #endregion

        [TestCase("TCF")]
        public void CreateAgentAndAgentSessionIT(string channelPartnerName)
		{
			var agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
			Assert.That(agentSession, Is.Not.Null);
		}

		private AgentSession CreateAgentAndAgentSession(string channelPartnerName, ZeoContext zeoContext)
		{
			AgentSSO agent = GetAgentDetails(channelPartnerName, zeoContext);
			string terminalName = GetTerminalName();
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
			Response response = client.AuthenticateSSO(agent, channelPartner.Id.ToString(), terminalName, zeoContext);
			AgentSession agentSession = response.Result as AgentSession;
			return agentSession;
		}

		private AgentSSO GetAgentDetails(string channelPartnerName, ZeoContext zeoContext)
		{
			AgentSSO ssoDetails = IntegrationTestData.GetSSOAgentData(channelPartnerName, zeoContext);
			return ssoDetails;
		}

		private ChannelPartner GetChannelPartner(string channelPartnerName, ZeoContext zeoContext)
		{
            //alloyContext.ProviderId = 401;
            Response response = client.ChannelPartnerConfigByName(channelPartnerName, zeoContext);
			ChannelPartner channelPartner = response.Result as ChannelPartner;
			return channelPartner;
		}

	}
}
