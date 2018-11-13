using System.Collections.Generic;
using NUnit.Framework;
using Spring.Testing.NUnit;
using System;
using MGI.Biz.Synovus.Impl;


namespace MGI.Biz.Synovus.Test
{
	[TestFixture]
	public class SynovusCustomerTest : AbstractTransactionalSpringContextTests
	{

		//public ICustomerRepository InterceptedClientCustomerService { private get; set; }

		private SynovusCustomer ClientCustomerService { get; set; }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Biz.Synovus.Test/MGI.Biz.Synovus.Test/Biz.Synovus.Test.Spring.xml" };}
		}
	
        /// <summary>
        /// Unit Test For FISConnectDb US1931
        /// </summary>
        [Test]
        public void TestfetchAllFromConnectsDB()
        {
            int agentID = 20;
            int processorID = 13;
            int providerId = 404;
            int channelPartnerId = 33;
            Guid locationRowGuid = new Guid("5CE4DBBA-1AC9-48C3-8E85-E93F7B12B906");
            string timezone = "Eastern Standard Time";
			MGI.Common.Util.MGIContext context = new MGI.Common.Util.MGIContext();
            context.AgentId = agentID;
			context.ProcessorId = processorID;
            context.ProviderId = providerId;
            context.ChannelPartnerId = channelPartnerId;
            context.LocationRowGuid = locationRowGuid;
            context.TimeZone = timezone;
            context.BankId = "300";
            Dictionary<string,object>customerLookUpCriteria = new Dictionary<string, object>();
            customerLookUpCriteria.Add("SSN", "111111111");
            customerLookUpCriteria.Add("PhoneNumber", "0706568112");
            customerLookUpCriteria.Add("ZipCode", "31909");
            List<MGI.Biz.Customer.Data.Customer> customers = ClientCustomerService.FetchAll(0,customerLookUpCriteria, context);
            Assert.IsNotNull(customers);
            foreach (var customer in customers)
            {
                Assert.AreEqual(customer.Profile.SSN, "111111111");
            }
            if (customers.Count > 0)
            {
                Console.WriteLine(" {0} Customers Found on call of FetchAll", customers.Count);
            }
            else
            {
                Console.WriteLine(" No Customer Found on call of FetchAll");
            }
        }
 
    }
}
