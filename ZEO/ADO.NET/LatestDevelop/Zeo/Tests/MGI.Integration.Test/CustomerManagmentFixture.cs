using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
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

            Response customerResponse = Client.GetCustomerProfile(AgentSession.SessionId, AlloyId, MgiContext);
            if (VerifyException(customerResponse)) throw new AlloyWebException(customerResponse.Error.Details); 
            Prospect customer = (Prospect)customerResponse.Result;

			customer.Occupation = "Student";

			Response saveResponse = Client.SaveCustomerProfile(AgentSession.SessionId, AlloyId, customer, MgiContext, true);
            if (VerifyException(saveResponse)) throw new AlloyWebException(saveResponse.Error.Details); 

			Response profileResponse = Client.GetCustomerProfile(AgentSession.SessionId, AlloyId, MgiContext);
            if (VerifyException(profileResponse)) throw new AlloyWebException(profileResponse.Error.Details); 
            customer = (Prospect)profileResponse.Result;

			Assert.AreEqual("STUDENT", customer.Occupation.ToUpper());
		}

		

		#region Private Methods
		private void RegisterCustomer(string channelPartnerId, string terminalName, string channelPartnerName)
		{
			ChannelPartnerId = channelPartnerId;
			TerminalName = terminalName;
			ChannelPartnerName = channelPartnerName;

			AgentSession = GetAgentSession();

			Prospect customer = GetCustomerProspect(AgentSession.SessionId, "Janson", new DateTime(1996, 04, 08));

			Response panResponse = Client.GeneratePAN(AgentSession.SessionId, customer, MgiContext);
            if (VerifyException(panResponse)) throw new AlloyWebException(panResponse.Error.Details); 
            AlloyId = Convert.ToInt64(panResponse.Result);

            Response customerResponse = Client.SaveCustomerProfile(AgentSession.SessionId, AlloyId, customer, MgiContext, false);
            if (VerifyException(customerResponse)) throw new AlloyWebException(customerResponse.Error.Details); 

			MgiContext.EditMode = false;

            Response activateResponse = Client.NexxoActivate(AgentSession.SessionId, AlloyId, MgiContext);
            if (VerifyException(activateResponse)) throw new AlloyWebException(activateResponse.Error.Details);

            Response clientResponse = Client.ClientActivate(AgentSession.SessionId, AlloyId, MgiContext);
            if (VerifyException(clientResponse)) throw new AlloyWebException(clientResponse.Error.Details); 
		}
		#endregion
	}
}
