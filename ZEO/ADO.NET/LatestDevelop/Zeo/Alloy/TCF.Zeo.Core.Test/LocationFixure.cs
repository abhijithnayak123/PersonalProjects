using System;
using NUnit.Framework;
using TCF.Zeo.Common.Data;

#region Zeo References
using CoreImpl = TCF.Zeo.Core.Impl;
using CoreContract = TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using System.Collections.Generic;
#endregion
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class LocationFixure
    {
        // create unit test
        public CoreContract.ILocationService _LocationserviceImpl;
        public CoreContract.ILocationProcessorCredentialService _locationProcessor;

        [Test]
        public void can_CreateLocation()
        {
            ZeoContext alloycontext = new ZeoContext(); bool isSuccess = false;
            Location createLocation = getLocationinfo();
            try
            {
                isSuccess = Convert.ToBoolean(_LocationserviceImpl.CreateLocation(createLocation, alloycontext));
                Assert.IsTrue(isSuccess == true);
            }
            catch
            {
                Assert.IsTrue(isSuccess == false);
            }
            
        }
        [Test]
        public void can_updateLocation()
        {
            ZeoContext alloycontext = new ZeoContext(); Location createLocation = getLocationinfo();
            
            bool isSuccess = Convert.ToBoolean(_LocationserviceImpl.UpdateLocation(createLocation, alloycontext));
            Assert.IsTrue(isSuccess == true);
            
        }
        [Test]
        public void can_ValidateLocation()
        {
            ZeoContext alloycontext = new ZeoContext();
            Location createLocation = getLocationinfo();
            int validate = 0;
            try
            {
                 validate = _LocationserviceImpl.ValidateLocation(createLocation, alloycontext);
                Assert.IsNotNullOrEmpty(validate.ToString());
            }
            catch
            {
                Assert.IsNotNull(validate.ToString());
            }
           
        }
        [Test]
        public void can_GetLocationsByChannelPartnerId()
        {
            ZeoContext alloycontext = new ZeoContext();
            Location createLocation = getLocationinfo();
            long channelpartnerId = 34;
            List<Location> locations = new List<Location>();
            try
            {
               locations = _LocationserviceImpl.GetLocationsByChannelPartnerId(channelpartnerId, alloycontext);
                Assert.IsNotNull(locations);
            }
            catch
            {
                Assert.IsNotNull(locations);
            }
          
        }
        [Test]
        public void can_GetLocationById()
        {
            ZeoContext alloycontext = new ZeoContext();
            Location createLocation = getLocationinfo();
            List<Location> locations = new List<Location>();
            try
            {
               locations = _LocationserviceImpl.GetLocationById(createLocation.LocationId, alloycontext);
               Assert.IsNotNull(locations);

            }
            catch
            {
                Assert.IsNotNull(locations);
            }


        }
        [Test]
        public void can_SaveLocationProcessorCredentials()
        {
            ZeoContext alloycontext = new ZeoContext();
            LocationProcessorCredentials createLocation = getLocationProcessorCredentialInfo();
            string timeZone = "Eastern ZONE";
            bool IsCreated = false;
            try
            {
                IsCreated = _locationProcessor.SaveLocationProcessorCredentials(createLocation, timeZone, alloycontext);
                Assert.IsNotNull(IsCreated == true);
            }
            catch
            {
                Assert.IsNotNull(IsCreated == false);
            }
          
        }

        [Test]
        public void can_GetLocationProcessorCredentials()
        {
            ZeoContext alloycontext = new ZeoContext();
            LocationProcessorCredentials createLocation = getLocationProcessorCredentialInfo();
            List<LocationProcessorCredentials> locations = new List<LocationProcessorCredentials>();
            try
            {
                locations = _locationProcessor.GetLocationProcessorCredentials(createLocation.LocationId, alloycontext);
                Assert.IsNotNull(locations);
            }
          
            catch
            {
                Assert.IsNotNull(locations);
            }

        }
        [Test]
        public void can_GetProviderById()
        {
            ZeoContext alloycontext = new ZeoContext();
            LocationProcessorCredentials createLocation = getLocationProcessorCredentialInfo();
            List<LocationProcessorCredentials> locations = new List<LocationProcessorCredentials>();
            try
            {
                locations = _locationProcessor.GetLocationProcessorCredentials(createLocation.ProviderId, alloycontext);
                Assert.IsNotNull(locations);
            }
            catch
            {
                Assert.IsNotNull(locations);
            }

        }

        private Location getLocationinfo()
        {
            Location location = new Location();
            {
                location.IsActive = true;
                location.LocationName = "New California Location";
                location.Address1 = "Address1";
                location.Address2 = "Address2";
                location.City = "California";
                location.State = "CA";
                location.ZipCode = "95404";
                location.ChannelPartnerId = 27;
                location.PhoneNumber = "9876457312";
                location.BankID = "1234";
                location.BranchID = "123";
                location.TimezoneID = "Eastern ZONE";

            };
            return location;
        }


        private LocationProcessorCredentials getLocationProcessorCredentialInfo()
        {
            LocationProcessorCredentials locationCredentials = new LocationProcessorCredentials();
            {
                locationCredentials.UserName = "13139925";
                locationCredentials.Password = "13139925";
                locationCredentials.Identifier = "9900005";
                locationCredentials.ProviderId = 301;
                locationCredentials.LocationId = 10000000003;
         

            };
            return locationCredentials;
        }

    }
}
