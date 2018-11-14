using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MGI.Integration.Test
{
    [TestFixture]
    public partial class AlloyIntegrationTestFixture
    {
        [TestCase("TCF")]
        public void CreateTerminalIT(string channelPartnerName)
        {
            bool IsSuccess = CreateTerminal(channelPartnerName);
            Assert.That(IsSuccess, Is.True);
        }


        [TestCase("TCF")]
        public void GetTerminalIT(string channelPartnerName)
        {
            Terminal terminal = LookupTerminal(channelPartnerName);
            Assert.That(terminal, Is.Not.Null);
        }


        [TestCase("TCF")]
        public void UpdateTerminalIT(string channelPartnerName)
        {
            Terminal newTerminal = new Terminal()
            {
                IpAddress = "10.111.109.12"
            };
            Terminal oldTerminal = UpdateTerminal(channelPartnerName, newTerminal);
            Assert.AreEqual(newTerminal.IpAddress, oldTerminal.IpAddress);
        }

        private string GetTerminalName()
        {
            string terminalName = System.Net.Dns.GetHostName();
            return terminalName;
        }


        private bool CreateTerminal(string channelPartnerName)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);

            Response Contextresponse = client.GetZeoContextForAgent(Convert.ToUInt32(agentSession.SessionId), zeoContext);
            ZeoContext context = Contextresponse.Result as ZeoContext;
            NpsTerminal nps = new NpsTerminal()
            {
                Name = System.Net.Dns.GetHostName(),
                Status = "Available",
                PeripheralServiceUrl = "https://nps.nexxofinancial.com:18732/Peripheral/",
                LocationId = Convert.ToUInt32(context.LocationId),
                ChannelPartnerId = context.ChannelPartnerId,
            };
            Response npsResponse = client.CreateNpsTerminal(nps, context);

            Response terminalresponse = client.GetNpsTerminalByName(nps.Name, context.ChannelPartnerId, context);
            NpsTerminal CreatenpsTerminal = terminalresponse.Result as NpsTerminal;

            Response response = client.GetLocationsByChannelPartnerId(context.ChannelPartnerId, context);
            List<Location> locations = response.Result as List<Location>;
            Location location = locations.FirstOrDefault();
            Terminal terminal = new Terminal()
            {

                Name = System.Net.Dns.GetHostName(),
                ChannelPartnerId = channelPartner.Id,
                LocationId = Convert.ToInt32(location.LocationIdentifier),
                PeripheralServerId = CreatenpsTerminal.NpsTerminalId.ToString(),
            };

            response = client.CreateTerminal(terminal, context);
            var isSuccess = Convert.ToBoolean(response.Result);
            return isSuccess;
        }

        private Terminal LookupTerminal(string channelPartnerName)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            string terminalName = System.Net.Dns.GetHostName();
            Response response = client.GetTerminalByName(terminalName, Convert.ToInt16(channelPartner.Id), zeoContext);
            Terminal terminal = response.Result as Terminal;
            return terminal;
        }

        private Terminal UpdateTerminal(string channelPartnerName, Terminal newTerminal)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            Terminal oldTerminal = LookupTerminal(channelPartnerName);
            oldTerminal.IpAddress = newTerminal.IpAddress;
            client.UpdateTerminal(oldTerminal, zeoContext);
            oldTerminal = LookupTerminal(channelPartnerName);
            return oldTerminal;
        }

    }
}
