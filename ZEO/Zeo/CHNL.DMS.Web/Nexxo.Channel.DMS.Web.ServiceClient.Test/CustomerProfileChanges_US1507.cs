using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Web.ServiceClient.Test
{
	[TestFixture]
	public class CustomerProfileChanges_US1507
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
		public void Can_GetCustomerProfile_Test()
		{
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "27", "CCCCCCBLDFHF");
			long alloyId = 1000000000001120;

			Prospect prospect = DeskTop.GetCustomerProfile(agentSessionId.ToString(), alloyId, mgiContext);
			//ReceiptLangauage Column is Null from the [tProspects] table in PTNR Database in all rows...
			//Assert.IsNotNullOrEmpty(prospect.ReceiptLanguage);
			Assert.IsNotNull(prospect.ProfileStatus);
		}

		[Test]
		public void Cannot_GetCustomerProfile_Test()
		{
			Prospect prospect = new Prospect();
			try
			{
				long alloyId = 1000000001003370;
				 prospect = DeskTop.GetCustomerProfile("1000103890", alloyId, mgiContext);
				Assert.IsNull(prospect.ReceiptLanguage);
				
			}
			catch 
			{
				Assert.AreEqual(prospect.ProfileStatus,ProfileStatus.Inactive);
			}
		
		}

		[Test]
		public void Can_SaveCustomerProfile_Test()
		{
			long alloyId = 1000000000004840;
			Prospect prospect = DeskTop.GetCustomerProfile(agentSessionId.ToString(), alloyId, mgiContext);
			prospect.ProfileStatus = ProfileStatus.Active;
			prospect.ReceiptLanguage = "English";
			DeskTop.SaveCustomerProfile(agentSessionId.ToString(), alloyId, prospect, mgiContext, true);
			prospect = DeskTop.GetCustomerProfile(agentSessionId.ToString(), alloyId, mgiContext);
			Assert.IsTrue(prospect.ReceiptLanguage == "English");
			Assert.AreEqual(prospect.ProfileStatus,ProfileStatus.Active);
		}

		[Test]
		public void Can_AddCustomerProfile_Test()
		{
			//long agentSessionId = 1000001044;
			Guid channelpartnerid = Guid.Parse("6D7E785F-7BDD-42C8-BC49-44536A1885FC");
			long alloyId = 1000000000004840;
			Prospect prospect = new Prospect()
			{
				FName = "LOKESH",
				LName = "MN",
				Address1 = "421 BAY ST",
				//Address2 = "DSDFDSFSDFSDFSDFDSFSDFDSF",
				ChannelPartnerId = channelpartnerid,
				DateOfBirth = DateTime.Parse("1991-09-28 00:00:00.000"),
				Phone1 = "8867619249",
				PIN = ""
			};
		//	long alloyId = long.Parse(DeskTop.GeneratePAN("1000004249", prospect));

			DeskTop.SaveCustomerProfile(agentSessionId.ToString(), alloyId, prospect, mgiContext, false);
			Prospect prospectnew = DeskTop.GetCustomerProfile(agentSessionId.ToString(), alloyId, mgiContext);

			Assert.NotNull(prospectnew);

		}

		[Test]
		[ExpectedException]
		public void Cannot_SaveCustomerProfile_Test()
		{
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "27", "CCCCCCBLDFHF");
			long alloyId = 1000000000001061; // Not Exists 

			Prospect prospect = DeskTop.GetCustomerProfile(agentSessionId.ToString(), alloyId, mgiContext);
			prospect.ProfileStatus = ProfileStatus.Active;
			prospect.ReceiptLanguage = "English";

			DeskTop.SaveCustomerProfile(agentSessionId.ToString(), alloyId, prospect, mgiContext, true);

			prospect = DeskTop.GetCustomerProfile(agentSessionId.ToString(), alloyId, mgiContext);
			Assert.IsTrue(prospect.ReceiptLanguage == "English");
			Assert.AreEqual(prospect.ProfileStatus, ProfileStatus.Active);
		}



	}
}