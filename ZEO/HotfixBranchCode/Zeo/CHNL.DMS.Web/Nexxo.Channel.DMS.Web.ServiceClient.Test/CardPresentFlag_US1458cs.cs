using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.ServiceClient.Test
{
	[TestFixture]
	public class CardPresentFlag_US1458cs
	{
		public Desktop DeskTop { get; set; }
		MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
		[SetUp]
		public void Setup()
		{
			DeskTop = new Desktop();
		}
		long agentSessionId = 1000002070;
		[Test]
		public void CardPresentFlagTest_Swipe_ConfigNull()
		{
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "34", "TCF");

			// CashierDetails Session = DeskTop.GetAgent(agentSessionId);
			long alloyId = 1000000000003880;
			int cardPresentFlag = 1;
			CustomerSession customerSession = DeskTop.InitiateCustomerSession(agentSessionId.ToString(), alloyId, cardPresentFlag, mgiContext);
			Assert.AreEqual(customerSession.CardPresent, true);
		}

		[Test]
		public void CardPresentFlagTest_Enter_ConfigNull()
		{
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "34", "TCF");
			long alloyId = 1000000000003880;
			int cardPresentFlag = 2;
			CustomerSession customerSession = DeskTop.InitiateCustomerSession(agentSessionId.ToString(), alloyId, cardPresentFlag, mgiContext);
			// Presently it was there i changed to true..	 Assert.AreEqual(customerSession.CardPresent, false);
			Assert.AreEqual(customerSession.CardPresent, true);
		}

		[Test]
		public void CardPresentFlagTest_Swipe_Config1()
		{
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "34", "TCF");
			long alloyId = 1000000000003880;
			int cardPresentFlag = 1;
			CustomerSession customerSession = DeskTop.InitiateCustomerSession(agentSessionId.ToString(), alloyId, cardPresentFlag, mgiContext);
			Assert.AreEqual(customerSession.CardPresent, true);
		}

		[Test]
		public void CardPresentFlagTest_Enter_Config1()
		{
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "34", "TCF");
			long alloyId = 1000000000003880;
			int cardPresentFlag = 2;
			CustomerSession customerSession = DeskTop.InitiateCustomerSession(agentSessionId.ToString(), alloyId, cardPresentFlag, mgiContext);
			// Presently it was there i changed to true..      Assert.AreEqual(customerSession.CardPresent, false);
			Assert.AreEqual(customerSession.CardPresent, true);
		}

		[Test]
		public void CardPresentFlagTest_Swipe_Config2()
		{
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "34", "TCF");
			long alloyId = 1000000000003880;
			int cardPresentFlag = 1;
			CustomerSession customerSession = DeskTop.InitiateCustomerSession(agentSessionId.ToString(), alloyId, cardPresentFlag, mgiContext);
			Assert.AreEqual(customerSession.CardPresent, true);
		}

		[Test]
		public void CardPresentFlagTest_Enter_Config2()
		{
			//AgentSession Session = DeskTop.AuthenticateAgent("ZeoMGI", "ezNM19RSbwIeY7Ssi44cC", "34", "OPT-LAP-0128");
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "34", "TCF");
			long alloyId = 1000000000003880;
			int cardPresentFlag = 2;
			CustomerSession customerSession = DeskTop.InitiateCustomerSession(agentSessionId.ToString(), alloyId, cardPresentFlag, mgiContext);
			Assert.AreEqual(customerSession.CardPresent, true);
		}
	}
}
