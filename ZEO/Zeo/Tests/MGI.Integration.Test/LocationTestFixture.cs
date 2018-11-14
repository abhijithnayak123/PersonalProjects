using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MGI.Integration.Test.Data;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

namespace MGI.Integration.Test
{
    [TestFixture]
    public partial class AlloyIntegrationTestFixture
    {
        #region Location service  methods


        [TestCase("TCF")]
        public void CreateLocationsIT(string channelPartnerName)
        {
            var IsSuccess = CreateLocation(channelPartnerName);
            Assert.That(IsSuccess, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void UpdateLocationsIT(string channelPartnerName)
        {
            Location newLocation = new Location()
            {
                Address1 = "Silk Board",
                Address2 = "Chennai",
            };
            Location oldLocation = EditLocation(channelPartnerName, newLocation);
            Assert.AreEqual(newLocation.Address1, oldLocation.Address1);
            Assert.AreEqual(newLocation.Address2, oldLocation.Address2);
        }

        [TestCase("TCF")]
        public void GetLocationsByChannelPartnerIT(string channelPartnerName)
        {
            var locations = GetLocationsbyChannelpartner(channelPartnerName);
            Assert.That(locations, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void GetLocationByIdIT(string channelPartnerName)
        {
            var locations = GetLocation(channelPartnerName);
            Assert.That(locations, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void ValidateLocationIT(string channelPartnerName)
        {
            bool validate = validateLocation(channelPartnerName);
            Assert.AreNotEqual(true, validate);
        }

        [TestCase("TCF")]
        public void GetLocationProcessorCredentialsIT(string channelPartnerName)
        {
            var locationProcessorCredentials = GetLocationProcessorCredentials(channelPartnerName);
            Assert.That(locationProcessorCredentials, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void CreateCustomerSessionCounterIdIT(string channelPartnerName)
        {
            string customerSessionCounterId = CreateCustomerSessionCounterId(channelPartnerName);
            Assert.That(customerSessionCounterId, Is.Not.Null);
        }
        [TestCase("TCF")]
        public void GetLocationCounterIDIT(string channelPartnerName)
        {
            string locationCounterId = GetLocationCounterID(channelPartnerName);
            Assert.That(locationCounterId, Is.Not.Null);
        }

        #endregion


        #region Location Related Private  methods
        private long CreateLocation(string channelPartnerName)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            Location location = IntegrationTestData.LocationData(channelPartner);
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            Response response = client.CreateLocation(location, zeoContext);
            long IsSuccess = Convert.ToInt64(response.Result);
            response = client.GetLocationById(IsSuccess, zeoContext);
            List<Location> locations = response.Result as List<Location>;
            Location locationDetail = locations.FirstOrDefault();
            LocationProcessorCredentials proccessor = IntegrationTestData.LocationProcessorData(channelPartnerName);
            proccessor.locationId = locationDetail.LocationID;
            response = client.GetProvidersbyChannelPartnerName(channelPartnerName, zeoContext);
            List<ChannelPartnerProductProvider> productProvider = response.Result as List<ChannelPartnerProductProvider>;
            foreach (var provider in productProvider)
            {
                if (provider.ProcessorName == "INGO" || provider.ProcessorName == "WesternUnion")
                {
                    proccessor.ProviderId = provider.ProcessorId;
                    client.SaveLocationProcessorCredential(proccessor, zeoContext);
                }
            }
            return IsSuccess;
        }

        private Location EditLocation(string channelPartnerName, Location newLocation)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            List<Location> locations = GetLocationsbyChannelpartner(channelPartnerName);
            string locationId = locations.Where(x => x.LocationName == "IT_TCF").FirstOrDefault().LocationIdentifier;
            Response response = client.GetLocationById(Convert.ToInt64(locationId), zeoContext);
            Location oldLocation = (response.Result as List<Location>).FirstOrDefault();
            oldLocation.Address1 = newLocation.Address1;
            oldLocation.Address2 = newLocation.Address2;
            oldLocation.ChannelPartnerId = Convert.ToInt32(channelPartner.Id);
            response = client.UpdateLocation(oldLocation, zeoContext);
            bool IsSuccess = Convert.ToBoolean(response.Result);
            response = client.GetLocationById(Convert.ToInt64(locationId), zeoContext);
            oldLocation = (response.Result as List<Location>).FirstOrDefault();
            return oldLocation;

        }

        private Location GetLocation(string channelPartnerName)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            List<Location> locations = GetLocationsbyChannelpartner(channelPartnerName);
            string locationId = locations.Where(x => x.LocationName == "IT_TCF").FirstOrDefault().LocationIdentifier;
            Response response = client.GetLocationById(Convert.ToInt64(locationId), zeoContext);
            Location oldLocation = (response.Result as List<Location>).FirstOrDefault();
            return oldLocation;

        }
        private List<Location> GetLocationsbyChannelpartner(string channelPartnerName)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            Response response = client.GetLocationsByChannelPartnerId(channelPartner.Id, zeoContext);
            List<Location> locations = response.Result as List<Location>;
            return locations;
        }
        private bool validateLocation(string channelPartnerName)
        {
            bool Exception = false;
            try
            {
                ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
                Location location = IntegrationTestData.LocationData(channelPartner);
                location.ChannelPartnerId = Convert.ToInt32(channelPartner.Id);
                Response response = client.ValidateLocation(location, zeoContext);
            }
            catch (Exception ex)
            {
                Exception = true;
            }
            return Exception;
        }
        private List<LocationProcessorCredentials> GetLocationProcessorCredentials(string channelPartnerName)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            List<Location> locations = GetLocationsbyChannelpartner(channelPartnerName);
            string locationId = locations.Where(x => x.LocationName == "IT_TCF").FirstOrDefault().LocationIdentifier;
            Response response = client.GetLocationProcessorCredentials(Convert.ToInt64(locationId), zeoContext);
            List<LocationProcessorCredentials> locationProcessorCredentials = response.Result as List<LocationProcessorCredentials>;
            return locationProcessorCredentials;
        }

        private string CreateCustomerSessionCounterId(string channelPartnerName)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            List<Location> locations = GetLocationsbyChannelpartner(channelPartnerName);
            string locationId = locations.Where(x => x.LocationName == "IT_TCF").FirstOrDefault().LocationIdentifier;
            CustomerProfile customerCreate = RegisterCustomer(channelPartnerName, zeoContext);
            CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
            zeoContext.CustomerSessionId = customerSession.CustomerSessionId;
            Response response = client.CreateCustomerSessionCounterId(301, Convert.ToInt64(locationId), zeoContext);
            string counterId = response.Result.ToString();
            return counterId;
        }

        private string GetLocationCounterID(string channelPartnerName)
        {
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            List<Location> locations = GetLocationsbyChannelpartner(channelPartnerName);
            string locationId = locations.Where(x => x.LocationName == "IT_TCF").FirstOrDefault().LocationIdentifier;
            Response response = client.GetLocationCounterID(Convert.ToInt64(locationId), 301, zeoContext);
            string counerId = response.Result.ToString();
            return counerId;
        }
        #endregion
    }
}
