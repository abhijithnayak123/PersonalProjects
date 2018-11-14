using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using NHibernate;
using NHibernate.Context;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MGI.Core.Partner.Test
{

	[TestFixture]
	public class AgentSessionServiceImpl_Fixture : AbstractPartnerTest
	{
		public IAgentSessionService AgentSessionService { private get; set; }
		public IManageUsers ManageUserService { private get; set; }
		public ITerminalService PartnerTerminalService { private get; set; }
		public IChannelPartnerService ChannelPartnerService { private get; set; }

		[Test]
		public void CreateAgentSession()
		{
			MGI.Common.Util.MGIContext mgiContext = new MGI.Common.Util.MGIContext();
			Dictionary<string, object> ssoAttributes = new Dictionary<string, object>();
			ssoAttributes.Add("BusinessDate", "2015-06-20");
			mgiContext.Context = ssoAttributes;
			string terminalName = "CE82EDE1-4E96-41CB-8C7F-EB69BE85E919";
			string channelPartner = "34";
			int agentId = 500001;
			UserDetails Agent = ManageUserService.GetUser(Convert.ToInt32(agentId));
			Terminal terminal = getTerminal(terminalName, int.Parse(channelPartner));
			AgentSession agentSession = AgentSessionService.Create(Agent, terminal, mgiContext);
			Assert.That(agentSession, Is.Not.Null);
		}

		[Test]
		public void Invalid_Business_Date_Test()
		{
			MGI.Common.Util.MGIContext mgiContext = new MGI.Common.Util.MGIContext();
			Dictionary<string, object> ssoAttributes = new Dictionary<string, object>();
			ssoAttributes.Add("BusinessDate", "2015-15-10");
			mgiContext.Context = ssoAttributes;
			string terminalName = "CE82EDE1-4E96-41CB-8C7F-EB69BE85E919";
			string channelPartner = "34";
			int agentId = 500001;
			UserDetails Agent = ManageUserService.GetUser(Convert.ToInt32(agentId));
			Terminal terminal = getTerminal(terminalName, int.Parse(channelPartner));
			AgentSession agentSession = AgentSessionService.Create(Agent, terminal, mgiContext);
			Assert.That(agentSession, Is.Not.Null);
			Assert.That(agentSession.BusinessDate, Is.Null);
		}

		private Terminal getTerminal(string terminalName, long channelPartnerId)
		{
			if (string.IsNullOrWhiteSpace(terminalName))
				return null;

			MGIContext context = new MGIContext();
			ChannelPartner channelPartner = ChannelPartnerService.ChannelPartnerConfig(channelPartnerId);
			return PartnerTerminalService.Lookup(terminalName, channelPartner, context);
		}
	}
}
