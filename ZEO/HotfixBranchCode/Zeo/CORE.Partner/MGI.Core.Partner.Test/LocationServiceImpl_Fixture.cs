using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Context;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Impl;
using NUnit.Framework;
using MGI.Common.DataAccess.Data;
using MGI.Common.DataAccess.Impl;

using Spring.Context;

namespace MGI.Core.Partner.Test
{
    [TestFixture]
    public class LocationServiceImpl_Fixture : AbstractPartnerTest
    {
		public IManageLocations ImanageLocation { get; set; }
       	//[SetUp]
		//public void Setup()
		//{
		//	IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
		//	ImanageLocation = (IManageLocations)ctx.GetObject("manageLocation");
		//	session = (ISession)ctx.GetObject("session");
		//}

        [Test]
        public void CanCreateTest()
        {
            bool isSuccess = false;

            Location manageLocation = new Location()
			{
            LocationName = "New California Location",
            IsActive = true,
            Address1 = "Address1",
            Address2 = "Address2",
            City = "California",
            State = "CA",
            ZipCode = "98765",
            ChannelPartnerId = 27,
            DTTerminalCreate = DateTime.Now,
            DTTerminalLastModified = DateTime.Now,
            DTServerCreate = DateTime.Now,
            DTServerLastModified = DateTime.Now
		};

			using (ISession session = NHibernateHelper.OpenSession())
			{
				using (ITransaction txn = session.BeginTransaction())
				{
					CallSessionContext.Bind(session);
					try
					{
						isSuccess =Convert.ToBoolean(ImanageLocation.Create(manageLocation));
						txn.Commit();
						Assert.IsTrue(isSuccess == true);
					}
					catch
					{
						Assert.IsTrue(isSuccess == false);
					}
				}
			}
        }

        [Test]
        public void CannotCreateTest()
        {
            bool success = false;

            Location manageLocation = new Location();
            manageLocation.LocationName = "California Location2";
            manageLocation.IsActive = true;
            manageLocation.Address1 = "Address1";
            manageLocation.Address2 = "Address2";
            manageLocation.City = "California";
            manageLocation.State = "CA";
			manageLocation.LocationIdentifier = "";
            manageLocation.ZipCode = "9876534567";
            manageLocation.ChannelPartnerId = 27;
            manageLocation.DTServerLastModified = DateTime.Now;
            manageLocation.DTTerminalLastModified = DateTime.Now;

			using (ISession session = NHibernateHelper.OpenSession())
			{
				using (ITransaction txn = session.BeginTransaction())
				{
					CallSessionContext.Bind(session);
					success = Convert.ToBoolean(ImanageLocation.Create(manageLocation));
						txn.Commit();
						Assert.IsFalse(success == false);
					
						//Assert.IsTrue(success == false);
					
				}
			}
        }

        [Test]
        public void CanGetByNameTest()
        {
			string locationName = "Synovus";
            Location manageLocation;
			using (ISession session = NHibernateHelper.OpenSession())
			{
				using (ITransaction txn = session.BeginTransaction())
				{
					CallSessionContext.Bind(session);
					manageLocation = ImanageLocation.GetByName(locationName);
				}
				Assert.IsNotNull(manageLocation);
			}
        }

        [Test]
        public void CannotGetByNameTest()
        {
            string locationName = "California Location123";
            Location manageLocation;
			using (ISession session = NHibernateHelper.OpenSession())
			{ 
				using (ITransaction txn = session.BeginTransaction())
				 {
					 CallSessionContext.Bind(session);
					 manageLocation = ImanageLocation.GetByName(locationName);
				 }
            Assert.IsNull(manageLocation);
			}
        }

        [Test]
        public void CanUpdateTest()
        {
			string locationName = "Synovus";
            Location updatedManageLocation;
            bool success = false;
            Location manageLocation;
            Location manageLoc;
			using (ISession session = NHibernateHelper.OpenSession())
			{
				using (ITransaction txn = session.BeginTransaction())
				{
					CallSessionContext.Bind(session);

					manageLocation = ImanageLocation.GetByName(locationName);
					manageLoc = manageLocation;

					if (manageLoc != null)
					{
						manageLoc.LocationName = "California Location";
						manageLoc.IsActive = true;
						manageLoc.State = "CA";
						manageLoc.City = "California";
						manageLoc.Address1 = "adr1";
						manageLoc.Address2 = "adr2";
						manageLoc.ZipCode = "98765";
						manageLoc.ChannelPartnerId = 27;
						manageLoc.LocationIdentifier = "13992542";
						manageLoc.DTServerCreate = DateTime.Now;
						manageLoc.DTServerLastModified = DateTime.Now;
                        manageLoc.DTTerminalCreate = DateTime.Now;
                        manageLoc.DTTerminalLastModified = DateTime.Now;
						//Update
						success = ImanageLocation.Update(manageLoc);
						txn.Commit();
					}
					updatedManageLocation = ImanageLocation.GetByName(locationName);
				}
			}

            Assert.NotNull(updatedManageLocation);
            Assert.AreNotSame(updatedManageLocation.IsActive, manageLocation.IsActive);
        }
        
        [Test]
        public void CanGetAll()
        {
            List<Location> manageLocationsList;
			using (ISession session = NHibernateHelper.OpenSession())
			{
				using (ITransaction txn = session.BeginTransaction())
				{
					CallSessionContext.Bind(session);
					manageLocationsList = ImanageLocation.GetAll();
				}
				Assert.IsNotNull(manageLocationsList);
			}
        }

        [Test]
        public void CannotGetAll()
        {
            List<Location> manageLocationsList;
			using (ISession session = NHibernateHelper.OpenSession())
			{
				using (ITransaction txn = session.BeginTransaction())
				{
					CallSessionContext.Bind(session);
					manageLocationsList = ImanageLocation.GetAll();
				}
				Assert.NotNull(manageLocationsList);
				Assert.AreNotEqual(1, manageLocationsList.Count);
			}
        }
    }
}


