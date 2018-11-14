using System.Collections.Generic;
using NUnit.Framework;
using MGI.Biz.Synovus.Impl;
using MGI.Common.Util;
using MGI.Unit.Test;
using Moq;
using System;

namespace MGI.Biz.Synovus.Test
{
    [TestFixture]
    class SynovusCustomerTest : BaseClass_Fixture
    {
		public SynovusCustomer InterceptedClientCustomerService { get; set; }

		[Test]
		public void Can_Fetch_All_Customer()
		{
			long agentSessionId = 1000000000;
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string,object>();
			MGIContext mgiContext = new MGIContext() { };

			List<Customer.Data.Customer> customers = InterceptedClientCustomerService.FetchAll(agentSessionId, customerLookUpCriteria, mgiContext);

			Assert.AreNotEqual(customers.Count, 0);
		}

		[Test]
		public void Can_Validate_Customer_Status()
		{
			long agentSessionId = 1000000000;
			long cxnId = 10000000000000;
			MGIContext mgiContext = new MGIContext() { };

			InterceptedClientCustomerService.ValidateCustomerStatus(agentSessionId, cxnId, mgiContext);

			CxnClientCustomerService.Verify(moq => moq.ValidateCustomerStatus(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Client_Profile_Status()
		{
			long agentSessionId = 1000000000;
			long cxnId = 10000000000000;
			MGIContext mgiContext = new MGIContext() { };

			ProfileStatus status = InterceptedClientCustomerService.GetClientProfileStatus(agentSessionId, cxnId, mgiContext);

			Assert.AreEqual(status, ProfileStatus.Active);
		}

		[Test]
		public void Can_Validate_Customer_Required_Fields()
		{
			bool status = InterceptedClientCustomerService.ValidateCustomerRequiredFields(long.MaxValue, new Customer.Data.Customer(), new MGIContext());

			Assert.IsFalse(status);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void Can_Fetch_All_Customer_Negative()
		{
			long agentSessionId = 1000000000;
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();
			customerLookUpCriteria.Add("Exception", "Exception");
			MGIContext mgiContext = new MGIContext() { };

			List<Customer.Data.Customer> customers = InterceptedClientCustomerService.FetchAll(agentSessionId, customerLookUpCriteria, mgiContext);
		}

		[Test]
		public void Can_Fetch_All_Customer_None_FIS()
		{
			long agentSessionId = 1000000000;
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();
			customerLookUpCriteria.Add("BrandNewCustomer", "BrandNewCustomer");
			MGIContext mgiContext = new MGIContext() { };

			List<Customer.Data.Customer> customers = InterceptedClientCustomerService.FetchAll(agentSessionId, customerLookUpCriteria, mgiContext);

			Assert.AreEqual(customers.Count, 0);
		}

		[Test]
		public void Can_Fetch_All_Customer_None_FIS_Alloy()
		{
			long agentSessionId = 1000000000;
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();
			customerLookUpCriteria.Add("NoneFISCustomer", "NoneFISCustomer");
			MGIContext mgiContext = new MGIContext() { };

			List<Customer.Data.Customer> customers = InterceptedClientCustomerService.FetchAll(agentSessionId, customerLookUpCriteria, mgiContext);

			Assert.AreEqual(customers.Count, 1);
		}
    }
}
