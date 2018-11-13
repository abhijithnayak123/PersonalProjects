using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using MGI.Biz.Partner.Impl;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;

using NHibernate;
using NHibernate.Context;
using Spring.Context;
using Spring.Testing.NUnit;
using MGI.Common.Util;



namespace MGI.Biz.Partner.Test
{
	[TestFixture]
	public class LocationTest : AbstractPartnerTest
	{



		//private MGI.Biz.Partner.Contract.IManageLocations _locationService;
		//public MGI.Biz.Partner.Contract.IManageLocations LocationService
		//{
		//	get { return _locationService; }
		//	set { _locationService = value; }
		//}

		//public IManageLocations LocationService { get; set; }

		//public IManageLocations ImanageLocation { get; set; }
		ISession session { get; set; }
		IManageLocations LocationService { get; set; }
		private static string IManage_Locations = "BIZPartnerLocationService";
        public MGIContext mgiContext { get; set; }
        long agentSessionId = 0;

		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			LocationService = (IManageLocations)ctx.GetObject(IManage_Locations);
			//	session = (ISession)ctx.GetObject("session");
		}


		[Test]
		public void CanCreateTest()
		{
			bool success = false;
			Location newManageLocation = new Location();
			newManageLocation.LocationName = "New California";
			newManageLocation.IsActive = true;
			newManageLocation.Address1 = "Address1";
			newManageLocation.Address2 = "Address2";
			newManageLocation.City = "California";
			newManageLocation.State = "CA";
			newManageLocation.ZipCode = "98765";
			newManageLocation.LocationIdentifier = "13993652";
			newManageLocation.ChannelPartnerId = 34;

			try
			{
                success = Convert.ToBoolean(LocationService.Create(agentSessionId, newManageLocation, mgiContext));

				Assert.IsTrue(success == true);
			}
			catch (Exception)
			{
				Assert.IsFalse(success == false);
			}

		}



		[Test]
		public void CannotCreateTest()
		{
			bool success = false;
			Location newManageLocation = new Location();
			newManageLocation.LocationName = "California Composite";
			newManageLocation.IsActive = true;
			newManageLocation.Address1 = "Address1";
			newManageLocation.Address2 = "Address2";
			newManageLocation.City = "California";
			newManageLocation.State = "CA";
			newManageLocation.ZipCode = "98765";
			newManageLocation.ChannelPartnerId = 27;
			try
			{
                success = Convert.ToBoolean(LocationService.Create(agentSessionId, newManageLocation, mgiContext));

				Assert.IsFalse(success == true);
			}
			catch (Exception)
			{
				Assert.IsTrue(success == false);
			}

		}

		[Test]
		public void CannotEmptyObjectCreateTest()
		{
			bool success = false;
			Location newManageLocation = new Location();

			try
			{
                success = Convert.ToBoolean(LocationService.Create(agentSessionId, newManageLocation, mgiContext));

				Assert.IsTrue(success == true);
			}
			catch 
			{
				Assert.IsTrue(success == false);
			}

		}

		[Test]
		public void CanGetByName()
		{
			string locationName = "Synovus";
			Location manageLocation;

            manageLocation = LocationService.GetByName(agentSessionId, locationName, mgiContext);


			Assert.IsNotNull(manageLocation);
		}

		[Test]
		public void CannotGetByName()
		{
			string locationName = "New California";
			Location manageLocation;


            manageLocation = LocationService.GetByName(agentSessionId, locationName, mgiContext);


			Assert.IsNull(manageLocation);
		}

		[Test]
		public void CanGetAll()
		{
			List<Location> manageLocationsList;


			manageLocationsList = LocationService.GetAll();

			Assert.IsNotNull(manageLocationsList);
		}

		[Test]
		public void CannotGetAll()
		{
			List<Location> manageLocationsList;

			manageLocationsList = LocationService.GetAll();

			Assert.NotNull(manageLocationsList);
			Assert.AreNotEqual(1, manageLocationsList.Count);
		}

		[Test]
		public void CanUpdateTest()
		
		{
			string locationName = "Nexxo";
			Location updatedManageLocation;
			bool success = false;
			Location manageLocation;
			Location manageLoc;

            manageLocation = LocationService.GetByName(agentSessionId, locationName, mgiContext);
			manageLoc = manageLocation;

			if (manageLoc != null)
			{
				manageLoc.LocationName = "California Composite";
				manageLoc.IsActive = true;
				manageLoc.State = "CA";
				manageLoc.City = "California";
				manageLoc.Address1 = "adr1";
				manageLoc.Address2 = "adr2";
				manageLoc.ZipCode = "98765";
				manageLoc.ChannelPartnerId = 27;
				manageLoc.LocationIdentifier = "1399625";
				//Update
                success = LocationService.Update(agentSessionId, manageLoc, mgiContext);

			}
            updatedManageLocation = LocationService.GetByName(agentSessionId, locationName, mgiContext);


			Assert.NotNull(updatedManageLocation);
			Assert.AreNotSame(updatedManageLocation.IsActive, manageLocation.IsActive);
		}
	}
}
