using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Spring.Testing.NUnit;
using Spring.Data.Core;
using Spring.Context;
using Spring.Context.Support;

using Moq;

using MGI.Biz.Customer.Contract;
using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Impl;
using MGI.Common.DataAccess.Contract;
using MGI.Core.Partner.Data;

namespace MGI.Biz.Customer.Test
{
    [TestFixture]
    public class FISCustomerRegistrationTest : AbstractTransactionalSpringContextTests
    {
        private MGI.Biz.Customer.Impl.CustomerServiceCoreWrapper _bizCustEng;
        private MGI.Biz.Partner.Impl.CustomerProspectService _prospectSvc;
        //private MGI.Core.Partner.Impl.NexxoDataStructuresServiceImpl _IdTypes;

        //private Mock<IRepository<Data.Customer>> mokCustomerRepo;

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Biz.Customer.Test/MGI.Biz.Customer.Test/BizSpring.xml" }; }
        }

        [SetUp]
        public void setup()
        {
            _bizCustEng = (MGI.Biz.Customer.Impl.CustomerServiceCoreWrapper)applicationContext.GetObject("CustomerServiceCoreWrapper");
            _prospectSvc = (MGI.Biz.Partner.Impl.CustomerProspectService)applicationContext.GetObject("InterceptedBIZCustomerProspect");
        }

		//[Test]
		//public void RegisterCustomerHasNoFISProfile()
		//{
		//	Data.Customer TestCustomer = new Data.Customer();

		//	MGI.Biz.Partner.Data.Prospect TestProspectProfile = new MGI.Biz.Partner.Data.Prospect();
		//	TestProspectProfile.FName = "Test FN";
		//	TestProspectProfile.LName = "TestLN";
		//	TestProspectProfile.Address1 = "Address 1";
		//	TestProspectProfile.ChannelPartnerId = Guid.Parse("E46A6297-77D1-4AC9-9548-ECD75DE3E66E");
		//	TestProspectProfile.City = "Bangalore";
		//	TestProspectProfile.DOB = Convert.ToDateTime("1985-07-07");
		//	TestProspectProfile.DoNotCall = true;
		//	TestProspectProfile.Gender = "M";
		//	TestProspectProfile.MailingAddressDifferent = false;
		//	TestProspectProfile.TextMsgOptIn = false;
		//	TestProspectProfile.MoMaName = "Maiden Name";
		//	TestProspectProfile.Phone1 = "8754587895";
		//	TestProspectProfile.Phone1Provider = "Alltell";
		//	TestProspectProfile.Phone1Type = "Home";
		//	TestProspectProfile.SSN = "568795214";
		//	TestProspectProfile.State = "KA";
		//	TestProspectProfile.PostalCode = "94000";
            
		//	//TestCustomer.Profile = TestCustomerProfile;

		//	MGI.Biz.Partner.Data.Identification Id = new MGI.Biz.Partner.Data.Identification();
		//	Id.Country = "UNITED STATES";
		//	Id.ExpirationDate = Convert.ToDateTime("07-07-2015");
		//	Id.ID = "d1234562";
		//	Id.IDType=  "DRIVER'S LICENSE";
		//	Id.IssueDate = Convert.ToDateTime("07-07-2012");
		//	Id.State = "CALIFORNIA";

		//	TestProspectProfile.ID = Id;

		//	TestProspectProfile.Employer = string.Empty;
		//	TestProspectProfile.EmployerPhone = string.Empty;
		//	TestProspectProfile.Occupation = string.Empty;

		//	MGI.Biz.Partner.Data.SessionContext context = new MGI.Biz.Partner.Data.SessionContext()
		//	{
		//		AgentId = 200000,
		//		AgentName = "SysAdmin",
		//		AppName = "DMS-Server",
		//		ChannelPartnerId = 33,
		//		LocationId = Guid.Parse("BC46F466-16D3-47B9-97CC-A9F95E2A2CCB"),
		//		LocationAgentId = 200000,
		//		DTKiosk = DateTime.Today,
		//		SelectedLanguage = 1,
		//		CustomerSessionId = "1000000006"
		//	};
		//	string alloyId = _prospectSvc.SaveProspect(context, TestProspectProfile);

		//	SessionContext custContext = new SessionContext()
		//	{
		//		AgentId = 200000,
		//		AgentName = "SysAdmin",
		//		AppName = "DMS-Server",
		//		ChannelPartnerId = 33,
		//		LocationId = Guid.Parse("BC46F466-16D3-47B9-97CC-A9F95E2A2CCB"),
		//		LocationAgentId = 200000
		//	};
		//	_bizCustEng.Register(custContext,long.Parse(alloyId));
		//}
    }
}
