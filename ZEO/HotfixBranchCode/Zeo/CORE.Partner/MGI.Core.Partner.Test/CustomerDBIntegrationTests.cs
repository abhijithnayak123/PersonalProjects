using System;
using System.Collections.Generic;
using System.Data;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Common.Util;
using NUnit.Framework;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class CustomerDBIntegrationTests : AbstractPartnerTest
	{
		private ICustomerService _custSvc;
		public ICustomerService CustomerService { set { _custSvc = value; } }

		private IRepository<ChannelPartner> _channelPartnerRepo;
		public IRepository<ChannelPartner> ChannelPartnerRepo { set { _channelPartnerRepo = value; } }

		private IChannelPartnerGroupService _groupSvc;
		public IChannelPartnerGroupService GroupSvc { set { _groupSvc = value; } }

		private Customer CreateCustomer()
		{
			ChannelPartner cp = _channelPartnerRepo.FindBy(c => c.Name == "Synovus");
			Customer customer = _custSvc.Create(
			new MGI.Core.Partner.Data.Customer
				{
					Id = 101101,
					IsPartnerAccountHolder = false,
					ReferralCode = string.Empty,
					ChannelPartnerId = cp.rowguid,
					AgentSessionId =Guid.Empty,
					CustomerProfileStatus = ProfileStatus.Active,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone("Eastern Standard Time")
				});
			//Customer Create(long CXEId, bool IsPartnerAccountHolder, string ReferralCode, Guid ChannelPartnerId, Guid AgentGuid, bool CustProfileStatus);
			customer.AddAccount( 101, 1000027, 22 );

			return customer;
		}

		private ChannelPartnerGroup CreateChannelPartnerGroup(string groupName)
		{
			ChannelPartner cp = _channelPartnerRepo.FindBy(c => c.Name == "Synovus");
			ChannelPartnerGroup g = new ChannelPartnerGroup
			{
				channelPartner = cp,
				Name = groupName,
				DTServerCreate = DateTime.Now
			};

			_groupSvc.Create(g);

			return g;
		}

		private Guid SetupProspect(long alloyId, string fName, string LName, DateTime DOB, string phone, string occupation)
		{
			if (DOB == DateTime.MinValue)
				DOB = DateTime.Today.AddYears(-20);
			// setup customer to lookup
			Guid _prospectId = Guid.NewGuid();
			AdoTemplate.ExecuteNonQuery(CommandType.Text,
				string.Format("INSERT tProspects(Id,PAN,DTCreate,FirstName,LastName,DOB,Phone1) VALUES('{0}',{1},getdate(),'{2}','{3}','{4}','{5}')", _prospectId, alloyId, fName, LName, DOB, phone));
			AdoTemplate.ExecuteNonQuery(CommandType.Text,
				string.Format("INSERT tProspectEmploymentDetails(ProspectId,Occupation,DTCreate) VALUES('{0}','{1}',getdate())", _prospectId, occupation));
			return _prospectId;
		}

		[Test]
		public void CreateCustomerTest()
		{
			Customer customer = CreateCustomer();

			Assert.IsTrue( customer.Id == 101101 );
			Assert.IsTrue( customer.Accounts.Count == 1 );
		}

		[Test]
		public void CreateWithAdditionalInfo()
		{
			ChannelPartner cp = _channelPartnerRepo.FindBy(c => c.Name == "Synovus");
			Customer customer = _custSvc.Create(
			new MGI.Core.Partner.Data.Customer
				{
					Id = 112233,
					IsPartnerAccountHolder = true,
					ReferralCode = "08891234",
					ChannelPartnerId = cp.rowguid,
					AgentSessionId =Guid.Empty,
					CustomerProfileStatus = ProfileStatus.Active,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone("Eastern Standard Time")
				});
			bool holder = (bool)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select IsPartnerAccountHolder from tPartnerCustomers where CXEId={0}", 112233 ) );
			string refCode = AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select ReferralCode from tPartnerCustomers where CXEId={0}", 112233 ) ).ToString();

			Assert.IsTrue( holder );
			Assert.IsTrue( refCode == "08891234" );
		}

		[Test]
		[ExpectedException(typeof(PartnerCustomerException))]
		public void CreateCustomerException()
		{
			Customer customer = _custSvc.Create(
			new MGI.Core.Partner.Data.Customer
				{
					Id = 112233,
					IsPartnerAccountHolder = true,
					ReferralCode = "08891234",
					ChannelPartnerId = Guid.Empty,
					AgentSessionId =Guid.Empty,
					CustomerProfileStatus = ProfileStatus.Active,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone("Eastern Standard Time")
				});
			Assert.IsNull(customer.ChannelPartnerId);
		}

		[Test]
		public void LookupTest()
		{
			Customer createCust = CreateCustomer();

			Customer cust = _custSvc.LookupByCXNAccountId(1000000000, 302);

			Assert.IsTrue(cust.Id == 1000000000001240);

			cust.AddAccount(302, 33, 33);

			Assert.IsTrue(cust.Accounts.Count == 5);
			Assert.IsTrue(cust.Accounts[1].Id == 1000000371);
		}

		[Test]
		public void FindAccountTest()
		{
			Customer customer = CreateCustomer();
			customer.AddAccount( 302,33,33 );

			Account acct = customer.GetAccount( 302 );
			Assert.IsTrue( acct.Id == 33 );
		}

		[Test]
		public void AddToGroup()
		{
			Customer customer = CreateCustomer();
			ChannelPartnerGroup g = CreateChannelPartnerGroup("Group 1");

			customer.AddtoGroup(g);

			Assert.AreEqual(1, customer.Groups.Count);
			Assert.AreEqual(g.Name, customer.Groups[0].channelPartnerGroup.Name);
		}

		[Test]
		public void RemoveFromGroup()
		{
			Customer customer = CreateCustomer();
			ChannelPartnerGroup g = CreateChannelPartnerGroup("Group 1");

			CustomerGroupSetting cg = customer.AddtoGroup(g);

			Assert.AreEqual(1, customer.Groups.Count);
			Assert.AreEqual(g.Name, customer.Groups[0].channelPartnerGroup.Name);

			customer.Groups.Remove(cg);

			Assert.AreEqual(0, customer.Groups.Count);
		}

		[Test]
		public void ChangeGroup()
		{
			Customer customer = CreateCustomer();
			ChannelPartnerGroup g1 = CreateChannelPartnerGroup("Group 1");
			ChannelPartnerGroup g2 = CreateChannelPartnerGroup("Group 2");

			CustomerGroupSetting cg1 = customer.AddtoGroup(g1);

			Assert.AreEqual(1, customer.Groups.Count);
			Assert.AreEqual(g1.Name, customer.Groups[0].channelPartnerGroup.Name);

			CustomerGroupSetting cg2 = customer.AddtoGroup(g2);

			Assert.AreEqual(2, customer.Groups.Count);
			Assert.AreEqual(g2.Name, customer.Groups[1].channelPartnerGroup.Name);

			customer.Groups.Remove(cg1);

			Assert.AreEqual(1, customer.Groups.Count);
			Assert.AreEqual(g2.Name, customer.Groups[0].channelPartnerGroup.Name);
		}

		[Test]
		public void GetProspect()
		{
			long alloyId = 1234567890;
			Guid prospectId = SetupProspect(alloyId, "testFirst", "testLast", DateTime.MinValue, "4151234567", "tester");

			Prospect prospect = _custSvc.LookupProspect(alloyId);

			Assert.AreEqual("testFirst", prospect.FirstName);
		}

		[Test]
		public void AddProspectToGroup()
		{
			long alloyId = 1234567890;
			Guid prospectId = SetupProspect(alloyId, "testFirst", "testLast", DateTime.MinValue, "4151234567", "tester");

			Prospect prospect = _custSvc.LookupProspect(alloyId);

			ChannelPartnerGroup g = CreateChannelPartnerGroup("Group 1");

			ProspectGroupSetting pg = prospect.AddtoGroup(g);

			Assert.AreEqual(1, prospect.Groups.Count);
			Assert.AreEqual(g.Name, prospect.Groups[0].ChannelPartnerGroup.Name);
		}

		[Test]
		public void RemoveProspectFromGroup()
		{
			long alloyId = 1234567890;
			Guid prospectId = SetupProspect(alloyId, "testFirst", "testLast", DateTime.MinValue, "4151234567", "tester");

			Prospect prospect = _custSvc.LookupProspect(alloyId);

			ChannelPartnerGroup g = CreateChannelPartnerGroup("Group 1");

			ProspectGroupSetting cg = prospect.AddtoGroup(g);

			Assert.AreEqual(1, prospect.Groups.Count);
			Assert.AreEqual(g.Name, prospect.Groups[0].ChannelPartnerGroup.Name);

			prospect.Groups.Remove(cg);

			Assert.AreEqual(0, prospect.Groups.Count);
		}

		[Test]
		public void ChangeProspectGroup()
		{
			long alloyId = 1234567890;
			Guid prospectId = SetupProspect(alloyId, "testFirst", "testLast", DateTime.MinValue, "4151234567", "tester");

			Prospect prospect = _custSvc.LookupProspect(alloyId);

			ChannelPartnerGroup g1 = CreateChannelPartnerGroup("Group 1");
			ChannelPartnerGroup g2 = CreateChannelPartnerGroup("Group 2");

			ProspectGroupSetting cg1 = prospect.AddtoGroup(g1);

			Assert.AreEqual(1, prospect.Groups.Count);
			Assert.AreEqual(g1.Name, prospect.Groups[0].ChannelPartnerGroup.Name);

			ProspectGroupSetting cg2 = prospect.AddtoGroup(g2);

			Assert.AreEqual(2, prospect.Groups.Count);
			Assert.AreEqual(g2.Name, prospect.Groups[1].ChannelPartnerGroup.Name);

			prospect.Groups.Remove(cg1);

			Assert.AreEqual(1, prospect.Groups.Count);
			Assert.AreEqual(g2.Name, prospect.Groups[0].ChannelPartnerGroup.Name);
		}
	}
}
