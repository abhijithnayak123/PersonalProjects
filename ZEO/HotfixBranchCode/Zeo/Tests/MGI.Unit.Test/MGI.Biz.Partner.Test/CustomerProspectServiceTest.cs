using NUnit.Framework;
using MGI.Common.Util;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Unit.Test;
using Moq;
using PTNRData = MGI.Core.Partner.Data;
using System;

namespace MGI.Biz.Customer.Test
{
    [TestFixture]
    public class CustomerProspectServiceTest : BaseClass_Fixture
    {
		public ICustomerProspectService BIZCustomerProspectService { get; set; }
        
		[Test]
		public void Can_Save_Prospect()
		{
			long agentSessionId = 1000000000;
			SessionContext sessionContext = new SessionContext() { ChannelPartnerId = 34 };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			Prospect prospect = new Prospect() { Groups = new System.Collections.Generic.List<string>() { } };

			string alloyId = BIZCustomerProspectService.SaveProspect(agentSessionId, sessionContext, prospect, mgiContext);

			Assert.AreNotEqual(alloyId, "");
			PTNRCustomerService.Verify(moq => moq.SaveProspect(It.IsAny<PTNRData.Prospect>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Save_Prospect_Incremental_Save()
		{
			long alloyId = 1000000000000000;
			long agentSessionId = 1000000000;
			SessionContext sessionContext = new SessionContext() { ChannelPartnerId = 34 };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			Prospect prospect = new Prospect() { Groups = new System.Collections.Generic.List<string>() { } };

			BIZCustomerProspectService.SaveProspect(agentSessionId, sessionContext, alloyId, prospect, mgiContext);

			PTNRCustomerService.Verify(moq => moq.SaveProspect(It.IsAny<PTNRData.Prospect>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Save_Prospect_Identification_Save()
		{
			long alloyId = 1000000000000000;
			long agentSessionId = 1000000000;
			SessionContext sessionContext = new SessionContext() { ChannelPartnerId = 33 };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "Synovus" };
			Prospect prospect = new Prospect() { DateOfBirth = new System.DateTime(1990,10,10), Groups = new System.Collections.Generic.List<string>() { }, ID = new Identification() { Country = "US", IDType = "DL"},
					CustomerScreen = CustomerScreen.Identification
			 };

			BIZCustomerProspectService.SaveProspect(agentSessionId, sessionContext, alloyId, prospect, mgiContext);

			PTNRCustomerService.Verify(moq => moq.SaveProspect(It.IsAny<PTNRData.Prospect>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Save_Prospect_Identification_Save_BillPay()
		{
			long alloyId = 1000000000000000;
			long agentSessionId = 1000000000;
			SessionContext sessionContext = new SessionContext() { ChannelPartnerId = 34 };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			Prospect prospect = new Prospect()
			{
				DateOfBirth = new System.DateTime(1990, 10, 10),
				Groups = new System.Collections.Generic.List<string>() { },
				ID = new Identification() { Country = "US", IDType = "DL" },
				CustomerScreen = CustomerScreen.Identification
			};

			BIZCustomerProspectService.SaveProspect(agentSessionId, sessionContext, alloyId, prospect, mgiContext);

			PTNRCustomerService.Verify(moq => moq.SaveProspect(It.IsAny<PTNRData.Prospect>()), Times.AtLeastOnce());
		}
		
		[Test]
		public void Can_Get_Prospect()
		{
			long alloyId = 1000000000000000;
			long agentSessionId = 1000000000;
			SessionContext sessionContext = new SessionContext() { ChannelPartnerId = 34 };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			Prospect prospect = BIZCustomerProspectService.GetProspect(agentSessionId, sessionContext, alloyId, mgiContext);

			Assert.IsNotNull(prospect);
			PTNRCustomerService.Verify(moq => moq.LookupProspect(It.IsAny<long>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Conform_Customer_Identity()
		{
			long agentSessionId = 1000000000;
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			bool status = true;

			string id = BIZCustomerProspectService.ConfirmIdentity(agentSessionId, customerSessionId, status, mgiContext);

			Assert.AreNotEqual(id, "");
			PTNRCustomerService.Verify(moq => moq.ConfirmIdentity(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<bool>()), Times.Exactly(1));
		}

		/// <summary>
		/// User Story - AL-1626
		/// Scenario - Negative
		/// </summary>
		[Test]
		public void Can_Save_Prospect_MinimumAge_Error()
		{
			long alloyId = 1000000000000000;
			long agentSessionId = 1000000000;
			SessionContext sessionContext = new SessionContext() { ChannelPartnerId = 34 };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			Prospect prospect = new Prospect() { Groups = new System.Collections.Generic.List<string>() { }, DateOfBirth = DateTime.Now };

			BIZCustomerProspectService.SaveProspect(agentSessionId, sessionContext, alloyId, prospect, mgiContext);

			ProspectFieldValidator.Verify(moq => moq.ValidateDOB(It.IsAny<Prospect>(), 0), "false");
		}

		/// <summary>
		/// User Story - AL-1626
		/// Scenario - Positive
		/// </summary>
		[Test]
		public void Can_Save_Prospect_MinimumAge_Valid()
		{
			long alloyId = 1000000000000000;
			long agentSessionId = 1000000000;
			SessionContext sessionContext = new SessionContext() { ChannelPartnerId = 28 };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "Carver" };
			Prospect prospect = new Prospect() { Groups = new System.Collections.Generic.List<string>() { }, DateOfBirth = new DateTime(1994, 2, 2) };

			BIZCustomerProspectService.SaveProspect(agentSessionId, sessionContext, alloyId, prospect, mgiContext);

			ProspectFieldValidator.Verify(moq => moq.ValidateDOB(It.IsAny<Prospect>(), It.IsAny<int>()), Times.AtLeastOnce());
		}
    }
}
