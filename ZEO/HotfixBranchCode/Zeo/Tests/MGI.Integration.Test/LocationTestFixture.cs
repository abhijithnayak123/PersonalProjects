using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MGI.Integration.Test.Data;

namespace MGI.Integration.Test
{
	[TestFixture]
	public partial class AlloyIntegrationTestFixture
	{

		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void CreateLocationsIT(string channelPartnerName)
		{
			var IsSuccess = CreateLocation(channelPartnerName);
			Assert.That(IsSuccess, Is.Not.Null);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void GetLocationsIT(string channelPartnerName)
		{
			var locations = GetLocations(channelPartnerName);
			Assert.That(locations, Is.Not.Null);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void EditLocationsIT(string channelPartnerName)
		{
			MGI.Channel.DMS.Server.Data.Location newLocation = new MGI.Channel.DMS.Server.Data.Location()
			{
				Address1 = "Silk Board",
				Address2 = "Chennai",
			};
			MGI.Channel.DMS.Server.Data.Location oldLocation = EditLocation(channelPartnerName, newLocation);
			Assert.AreEqual(newLocation.Address1, oldLocation.Address1);
			Assert.AreEqual(newLocation.Address2, oldLocation.Address2);
		}
		private List<SelectListItem> GetLocations(string channelPartnerName)
		{			
			List<SelectListItem> locations = new List<SelectListItem>();
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			var availableLocations = client.GetAllLocationNames().FindAll(loc => loc.ChannelPartnerId == channelPartner.Id);
			foreach (var location in availableLocations)
			{
				locations.Add(new SelectListItem() { Text = location.LocationName, Value = location.Id.ToString() });
			}
			return locations;
		}

		private long CreateLocation(string channelPartnerName)
		{
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			MGI.Channel.DMS.Server.Data.Location location = IntegrationTestData.LocationData(channelPartner);			
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);		
		    long IsSuccess = client.SaveLocation(Convert.ToInt64(agentSession.SessionId), location, context);
			location = client.GetLocationDetailsForEdit(Convert.ToInt64(agentSession.SessionId), location.LocationName, context);
			ProcessorCredential proccessor = IntegrationTestData.LocationProcessorData(channelPartnerName);
			foreach(var provider in channelPartner.Providers)
			{
				if(provider.ProcessorName == "INGO" || provider.ProcessorName == "WesternUnion" )
				{
					proccessor.ProviderId = provider.ProcessorId;
					client.SaveLocationProcessorCredentials(Convert.ToInt64(agentSession.SessionId), location.Id, proccessor, context);
				}
			}
			return IsSuccess;
		}

		private MGI.Channel.DMS.Server.Data.Location EditLocation(string channelPartnerName, MGI.Channel.DMS.Server.Data.Location newLocation)
		{
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);
			string locationName = "IT_" + channelPartnerName;
			MGI.Channel.DMS.Server.Data.Location oldLocation = client.GetLocationDetailsForEdit(Convert.ToInt64(agentSession.SessionId), locationName, context);
			oldLocation.Address1 = newLocation.Address1;
			oldLocation.Address2 = newLocation.Address2;
			bool IsSuccess = client.UpdateLocation(Convert.ToInt64(agentSession.SessionId), oldLocation, context);
			oldLocation = client.GetLocationDetailsForEdit(agentSession.SessionId, oldLocation.Id, context);
			return oldLocation;

		}	

	}
}
