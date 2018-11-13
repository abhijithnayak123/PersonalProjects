using NUnit.Framework;
using MGI.Common.Util;
using MGI.Unit.Test;
using MGI.Biz.Partner.Contract;
using System.Collections.Generic;

namespace MGI.Biz.Customer.Test
{
    [TestFixture]
    public class ManageLocationsImplTest : BaseClass_Fixture
    {
		public IManageLocations BIZPartnerLocationService { get; set; }
        
		[Test]
		public void Can_Get_Location_By_Name()
		{
			long agentSessionId = 1000000001;
			string locationName = "TCF";
			MGIContext mgiContext = new MGIContext() { };

			MGI.Biz.Partner.Data.Location location = BIZPartnerLocationService.GetByName(agentSessionId, locationName, mgiContext);

			Assert.IsNotNull(location);
		}

		[Test]
		public void Can_Get_All_Location()
		{
			List<MGI.Biz.Partner.Data.Location> locations = BIZPartnerLocationService.GetAll();

			Assert.AreNotEqual(locations.Count, 0);
		}
		
		[Test]
		public void Can_Create_Location()
		{
			long agentSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { };
			MGI.Biz.Partner.Data.Location location = new Partner.Data.Location();

			long locationId = BIZPartnerLocationService.Create(agentSessionId, location, mgiContext);

			Assert.AreNotEqual(locationId, 0);
		}
		
		[Test]
		public void Can_Update_Location()
		{
			long agentSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { };
			MGI.Biz.Partner.Data.Location location = new Partner.Data.Location() { Id = 1000000001 };

			bool status = BIZPartnerLocationService.Update(agentSessionId, location, mgiContext);

			Assert.True(status);
		}

		[Test]
		public void Can_Lookup_Location()
		{
			long locationId = 1000000001;
			MGIContext mgiContext = new MGIContext() { };

			MGI.Biz.Partner.Data.Location location = BIZPartnerLocationService.Lookup(locationId, mgiContext);

			Assert.IsNotNull(location);
		}
		
		[Test]
		public void Can_Get_All_Location_BY_ChannelPartner()
		{
			long agentSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34 };

			List<MGI.Biz.Partner.Data.Location> locations = BIZPartnerLocationService.GetAll(agentSessionId, mgiContext);

			Assert.AreNotEqual(locations.Count, 0);
		}
     }
}
