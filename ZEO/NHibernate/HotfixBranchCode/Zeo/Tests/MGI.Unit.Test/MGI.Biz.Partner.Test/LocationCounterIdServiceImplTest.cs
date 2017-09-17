using NUnit.Framework;
using MGI.Common.Util;
using MGI.Biz.Partner.Contract;
using MGI.Unit.Test;
using MGI.Biz.Partner.Data;
using System.Collections.Generic;

namespace MGI.Biz.Customer.Test
{
    [TestFixture]
    public class LocationCounterIdServiceImplTest : BaseClass_Fixture
    {
		public ILocationCounterIdService BIZPartnerLocationCounterIdService { get; set; }

		[Test]
		public void Can_Update_Counter_Id()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { };

			bool status = BIZPartnerLocationCounterIdService.UpdateCounterId(customerSessionId, mgiContext);

			Assert.True(status);
		}

		[Test]
		public void Can_Update_Counter_Id_With_CustomerSessionCounter()
		{
			long customerSessionId = 1000000002;
			MGIContext mgiContext = new MGIContext() { };

			bool status = BIZPartnerLocationCounterIdService.UpdateCounterId(customerSessionId, mgiContext);

			Assert.That(status, Is.True);
		}

		[Test]
		[ExpectedException(typeof(BizPartnerException))]
		public void Can_Update_Counter_Id_With_Exception()
		{
			long customerSessionId = 1000000009;
			MGIContext mgiContext = new MGIContext() { };

			BIZPartnerLocationCounterIdService.UpdateCounterId(customerSessionId, mgiContext);
		}
    }

	[TestFixture]
	public class ManageLocationProcessorCredentials : BaseClass_Fixture
	{
		public ILocationProcessorCredentials BIZPartnerLocationProcessorDetails { get; set; }

		[Test]
		public void Can_Get_All_Location_Credential()
		{
			long agentSessionId = 1000000000;
			long locationId = 1;
			MGIContext mgiContext = new MGIContext() { };

			IList<ProcessorCredentials> processorCredentials = BIZPartnerLocationProcessorDetails.Get(agentSessionId, locationId, mgiContext);

			Assert.AreNotEqual(processorCredentials.Count, 0);
		}

		[Test]
		public void Can_Save_Location_Credential()
		{
			long agentSessionId = 1000000000;
			long locationId = 1;
			MGIContext mgiContext = new MGIContext() { };
			ProcessorCredentials processorCredential = new ProcessorCredentials() { };

			bool status = BIZPartnerLocationProcessorDetails.Save(agentSessionId, locationId, processorCredential, mgiContext);

			Assert.True(status);
		}
	}
}
