using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
using Spring.Testing.NUnit;
using Spring.Data.Core;
using Spring.Context;
using Spring.Context.Support;

using MGI.Biz.Customer.Contract;
using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Impl;
using MGI.Common.Util;

namespace MGI.Biz.Customer.Test
{
    [TestFixture]
    public class CustomerInitiateSessionTest : AbstractTransactionalSpringContextTests
    {
        private MGI.Biz.Customer.Impl.CustomerServiceCoreWrapper _bizCustEng;
		public MGIContext MgiContext { get; set; }
        protected override string[] ConfigLocations
        {
			get { return new string[] { "assembly://MGI.Biz.Customer.Test/MGI.Biz.Customer.Test/BizSpring.xml" }; }
            //get { return new string[] { "assembly://MGI.Biz.Customer.Test/MGI.Biz.Customer.Test/BizSpring.xml" }; }
        }

        [SetUp]
        public void setup()
        {
            _bizCustEng = (MGI.Biz.Customer.Impl.CustomerServiceCoreWrapper)applicationContext.GetObject("CustomerServiceCoreWrapper");
			MgiContext = new MGIContext() {
				TimeZone = "Central Standard Time",
				ChannelPartnerId = 34,
				ChannelPartnerName = "TCF"
			};
        }

        [Test]
        public void Can_TestInitiateCustomerSession_OnlySwipe_channelPartnerConfigEntery1()
        {
			long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 1;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);
            Assert.AreEqual(customersession.CardPresent, true);
        }

		[Test]
		public void Cannot_TestInitiateCustomerSession_Enter_chPConfigentry1()
		{
            long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 2;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);
			Assert.AreNotEqual(customersession.CardPresent, false);
		}

		[Test]
		public void Cannot_TestInitiateCustomerSession_Other_chPConfigentry1()
		{
            long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 3;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);
			Assert.AreEqual(customersession.CardPresent, false);
		}

		[Test]
		public void Can_TestInitiateCustomerSession_swipe_chPConfigentry2()
		{
            long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 1;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);
			Assert.AreEqual(customersession.CardPresent, true);
		}

		[Test]
		public void Can_TestInitiateCustomerSession_Enter_chPConfigentry2()
		{
            long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 2;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);
			Assert.AreEqual(customersession.CardPresent, true);
		}

		[Test]
		public void Cannot_TestInitiateCustomerSession_Other_chPConfigentry2()
		{
            long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 3;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);
			Assert.AreEqual(customersession.CardPresent, false);
		}

		[Test]
		public void Cannot_TestInitiateCustomerSession_Swipe_chPConfigentrynull()
		{
            long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 1;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);
			Assert.AreNotEqual(customersession.CardPresent, false);
		}

		[Test]
		public void Cannot_TestInitiateCustomerSession_Enter_chPConfigentrynull()
		{
            long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 2;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);
			Assert.AreNotEqual(customersession.CardPresent, false);
		}

		[Test]
		public void Cannot_TestInitiateCustomerSession_Other_chPConfigentrynull()
		{
            long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 3;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);
			Assert.AreEqual(customersession.CardPresent, false);
		}

        [Test]
        public void can_SearchCriteriaResultFound()
        {
            CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria()
            {
                LastName = "patel",
				SSN = "192837645"

            };

            long agentSessionId = 1000000066;
			Assert.IsFalse(_bizCustEng.Search(agentSessionId, searchCriteria, MgiContext).Count() > 0);
        }
		//[Test]
		//public void TestCustomerLookUp()
		//{
		//   Dictionary<string, object>customerLookUpCriteria = new Dictionary<string, object>();
		//   customerLookUpCriteria.Add("SSN", "192837645");
		//	customerLookUpCriteria.Add("PhoneNumber", "0706568112");
		//	customerLookUpCriteria.Add("ZipCode", "31909");
		//	Dictionary<string, object> dic = new Dictionary<string, object>();
		//	dic["ChannelPartnerName"] = "TCF";
		//	dic["ChannelPartnerId"] = 34;
		//	Assert.True(_bizCustEng.CustomerLookUp(customerLookUpCriteria, dic).Count()>0);

		//}

        [Test]
        public void can_SearchCriteriaResultNotFound()
        {
            CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria()
            {
                LastName = "sdfasf",
                SSN = "324324"

            };

            long agentSessionId = 1000000066;

			Assert.IsTrue(_bizCustEng.Search(agentSessionId, searchCriteria, MgiContext).Count() == 0);
        }

		[Test]
        public void can_GetCustomerProfile()
        {
            long agentSessionId = 1000000066;
			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 3;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);

			Data.Customer customer = _bizCustEng.GetCustomer(long.Parse(customersession.CustomerSessionId), alloyId, MgiContext);

			//  Assert.IsNotNullOrEmpty(customer.Profile.ReceiptLanguage);
            Assert.IsNotNull(customer.Profile.ProfileStatus);
        }

        [Test]
        public void can_SaveCustomerProfile()
        {
            long agentSessionId = 1000000066;

			long alloyId = 1000000000000050;
			MgiContext.CardPresentedType = 3;
			CustomerSession customersession = _bizCustEng.InitiateCustomerSession(agentSessionId, alloyId, MgiContext);

			Data.Customer customer = _bizCustEng.GetCustomer(long.Parse(customersession.CustomerSessionId), alloyId, MgiContext);
            customer.Profile.ReceiptLanguage = "English";
			customer.Profile.ProfileStatus = ProfileStatus.Active;




			_bizCustEng.SaveCustomer(agentSessionId, alloyId, customer, MgiContext);

			customer = _bizCustEng.GetCustomer(long.Parse(customersession.CustomerSessionId), alloyId, MgiContext);

            Assert.IsTrue(customer.Profile.ReceiptLanguage == "English");

        }

		[Test]
		public void Can_Change_ProfileStatus()
		{
			long agentId = 100000;//Admin
			string profileStatus = "Active";
			bool hasPermission = _bizCustEng.CanChangeProfileStatus(0, agentId, profileStatus, MgiContext);
			Assert.IsTrue(hasPermission);
		}

		[Test]
		public void Cannot_Change_ProfileStatus()
		{
			long agentId = 500017;//Teller
			string profileStatus = "Close";
			bool hasPermission = _bizCustEng.CanChangeProfileStatus(0, agentId, profileStatus, MgiContext);
			Assert.IsFalse(hasPermission);
		}
    }
}
