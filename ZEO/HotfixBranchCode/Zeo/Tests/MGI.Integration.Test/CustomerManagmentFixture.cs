using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test
{
	[TestFixture]
	class CustomerManagmentFixture : BaseFixture
	{
		[SetUp]
		public void Setup()
		{
			Client = new Desktop();
		}

		[Test]
		public void Can_Register_CustomerMGI()
		{
			GetChannelPartnerDataMGI();

			RegisterCustomer(ChannelPartnerId, TerminalName, ChannelPartnerName);

			Assert.AreNotEqual(AlloyId, 0);
		}

		[Test]
		public void Can_Register_Customer()
		{
			GetChannelPartnerDataCarver();

			RegisterCustomer(ChannelPartnerId, TerminalName, ChannelPartnerName);

			Assert.AreNotEqual(AlloyId, 0);
		}

		[Test]
		public void Can_Update_Customer()
		{
			GetChannelPartnerDataCarver();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "McDonald", new DateTime(1996, 04, 08));

			Prospect customer = Client.GetCustomerProfile(AgentSession.SessionId, AlloyId, MgiContext);

			customer.Occupation = "Student";

			Client.SaveCustomerProfile(AgentSession.SessionId, AlloyId, customer, MgiContext, true);

			customer = Client.GetCustomerProfile(AgentSession.SessionId, AlloyId, MgiContext);

			Assert.AreEqual("STUDENT", customer.Occupation.ToUpper());
		}

		[Test]
		public void Can_Update_CustomerMGI()
		{
			GetChannelPartnerDataMGI();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "Mack", new DateTime(1996, 04, 10));

			Prospect customer = Client.GetCustomerProfile(AgentSession.SessionId, AlloyId, MgiContext);

			customer.Occupation = "Student";

			Client.SaveCustomerProfile(AgentSession.SessionId, AlloyId, customer, MgiContext, true);

			customer = Client.GetCustomerProfile(AgentSession.SessionId, AlloyId, MgiContext);

			Assert.AreEqual("STUDENT", customer.Occupation.ToUpper());
		}

		[Test]
		public void Can_Search_CustomerMGI()
		{
			GetChannelPartnerDataMGI();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "McDonald", new DateTime(1996, 04, 08));

			Assert.AreNotEqual(AlloyId, 0);
		}

		#region Private Methods
		private void RegisterCustomer(string channelPartnerId, string terminalName, string channelPartnerName)
		{
			ChannelPartnerId = channelPartnerId;
			TerminalName = terminalName;
			ChannelPartnerName = channelPartnerName;

			AgentSession = GetAgentSession();

			Prospect customer = GetCustomerProspect(AgentSession.SessionId, "Janson", new DateTime(1996, 04, 08));

			AlloyId = long.Parse(Client.GeneratePAN(AgentSession.SessionId, customer, MgiContext));

			Client.SaveCustomerProfile(AgentSession.SessionId, AlloyId, customer, MgiContext, false);

			MgiContext.EditMode = false;

			Client.NexxoActivate(AgentSession.SessionId, AlloyId, MgiContext);

			Client.ClientActivate(AgentSession.SessionId, AlloyId, MgiContext);
		}
		#endregion
	}
}
