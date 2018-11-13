using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
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

		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void CreateTerminalIT(string channelPartnerName)
		{
			bool IsSuccess = CreateTerminal(channelPartnerName);
			Assert.That(IsSuccess, Is.True);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void GetTerminalIT(string channelPartnerName)
		{
			Terminal terminal = LookupTerminal(channelPartnerName);
			Assert.That(terminal, Is.Not.Null);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
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
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);
			var locations = client.GetAllLocationNames().FindAll(loc => loc.LocationName == "IT_"+channelPartner.Name);
			Terminal terminal = new Terminal()
			{
				Name = System.Net.Dns.GetHostName(),
				ChannelPartner = channelPartner,
				Location = locations.First(),
			};
			var isSuccess = client.CreateTerminal(Convert.ToInt64(agentSession.SessionId), terminal, context);
			return isSuccess;
		}

		private Terminal LookupTerminal(string channelPartnerName)
		{
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);
			string terminalName = System.Net.Dns.GetHostName();
			Terminal terminal = client.LookupTerminal(Convert.ToInt64(agentSession.SessionId), terminalName, Convert.ToInt16(channelPartner.Id), context);
			return terminal;
		}

		private Terminal UpdateTerminal(string channelPartnerName, Terminal newTerminal)
		{
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);
			Terminal oldTerminal = LookupTerminal(channelPartnerName);
			oldTerminal.IpAddress = newTerminal.IpAddress;
			client.UpdateTerminal(Convert.ToInt64(agentSession.SessionId), oldTerminal, context);
			oldTerminal = LookupTerminal(channelPartnerName);
			return oldTerminal;
		}

	}
}
