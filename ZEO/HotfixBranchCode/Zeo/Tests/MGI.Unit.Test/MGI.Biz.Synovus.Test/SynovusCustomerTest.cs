using System.Collections.Generic;
using NUnit.Framework;
using MGI.Biz.Synovus.Impl;
using MGI.Common.Util;
using MGI.Unit.Test;
using Moq;

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
    }
}
