using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Integration.Test.Data;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Integration.Test
{
	[TestFixture]
	public partial class AlloyIntegrationTestFixture
	{
		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void CustomerRegister(string channelPartnerName)
		{
			var registerCustomer = RegisterCustomer(channelPartnerName);
			Assert.That(registerCustomer, Is.Not.Null);
		}

		private CustomerSearchResult RegisterCustomer(string channelPartnerName)
		{
			Desktop client = new Desktop();
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			Prospect prospect = IntegrationTestData.GetCustomerProspect(channelPartner);
			Channel.DMS.Server.Data.MGIContext context = new Channel.DMS.Server.Data.MGIContext();
			if (channelPartner.Id == 34)
			{
				Dictionary<string, object> ssoAttributes = IntegrationTestData.GetSSOAttributes();
				context.Context = new Dictionary<string, object>();
				context.Context.Add("SSOAttributes", ssoAttributes);
			}
			string pan = client.GeneratePAN(agentSession.SessionId, prospect, context);
			client.SaveCustomerProfile(agentSession.SessionId, Convert.ToInt64(pan), prospect, context);
			prospect.CustomerScreen = CustomerScreen.Identification;
			client.SaveCustomerProfile(agentSession.SessionId, Convert.ToInt64(pan), prospect, context);
			prospect.CustomerScreen = CustomerScreen.Employment;
			client.SaveCustomerProfile(agentSession.SessionId, Convert.ToInt64(pan), prospect, context);
			client.NexxoActivate(agentSession.SessionId, Convert.ToInt64(pan), context); CustomerSearchCriteria criteria = new CustomerSearchCriteria();
			client.ClientActivate(agentSession.SessionId, Convert.ToInt64(pan), context);
			criteria.LastName = channelPartnerName;
			criteria.DateOfBirth = new DateTime(1950, 10, 10);
			CustomerSearchResult[] searchResult = client.SearchCustomers(agentSession.SessionId, criteria, context);
			var customer = searchResult.Where(c => c.ProfileStatus == "Active").FirstOrDefault();
			return customer;
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void CustomerModify(string channelPartnerName)
		{
			var modifyCustomer = ModifyCustomer(channelPartnerName);
			Assert.AreEqual("2952 NORTH AVENUE", modifyCustomer.Address.ToUpper());
	}

		private CustomerSearchResult ModifyCustomer(string channelPartnerName)
		{
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);
			CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria() { LastName = channelPartnerName, DateOfBirth = new DateTime(1950, 10, 10) };
			CustomerSearchResult[] customers = client.SearchCustomers(agentSession.SessionId, searchCriteria, MgiContext);

			if (customers.Length != 0)
			{
				AlloyId = long.Parse(customers[0].AlloyID);
			}

			Prospect prospect = client.GetCustomerProfile(agentSession.SessionId, AlloyId, context);
			prospect.Address1 = "2952 North Avenue";
			client.SaveCustomerProfile(agentSession.SessionId, AlloyId, prospect, context, true);
            customers = client.SearchCustomers(agentSession.SessionId, searchCriteria, MgiContext);
			var customer = customers.Where(c => c.ProfileStatus == "Active").FirstOrDefault();
 			return customer;
		}

	}
}
