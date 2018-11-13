using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Contract;
using Spring.Context;
using MGI.Channel.DMS.Server.Data;
using Spring.Testing.NUnit;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Test
{
	[TestFixture]
	public class WUReceiverMasterDataTest : AbstractTransactionalSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }

		}
		IDesktopService DeskTopTest { get; set; }
		private static string DESKTOP_ENGINE = "DesktopEngine";
        public MGIContext mgiContext { get; set; }

		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);
            mgiContext = new MGIContext()
            {
                ChannelPartnerId = 34,
                ChannelPartnerName = "TCF"
            };
		}

		[Test]
		public void WUCountriesTest()
		{
			//List<WUReceiverMasterData> countries = new List<WUReceiverMasterData>();
			List<XferMasterData> countries = new List<XferMasterData>();
            countries = DeskTopTest.GetXfrCountries(0, mgiContext);
			Assert.AreEqual(251, countries.Count, "There is no any countries.");
		}

		[Test]
		public void WUStatesTest()
		{
			//List<WUReceiverMasterData> states = new List<WUReceiverMasterData>();
			List<XferMasterData> states = new List<XferMasterData>();
			string countryCode = "IN";
            states = DeskTopTest.GetXfrStates(0, countryCode, mgiContext);
			Assert.AreEqual(0, states.Count, "There is no any countries.");
		}

		[Test]
		public void WUCitiesTest()
		{
			//List<WUReceiverMasterData> cities = new List<WUReceiverMasterData>();
			List<XferMasterData> cities = new List<XferMasterData>();
			string stateCode = "KA";
            cities = DeskTopTest.GetXfrCities(0, stateCode, mgiContext);
			Assert.AreEqual(0, cities.Count, "There is no any countries.");
		}

		[Test]
		public void WUDeliveryMethodsTest()
		{
			List<DeliveryService> deliveryMethods = new List<DeliveryService>();
			string state = "California";
			string stateCode = "CA";
			string city = "California";
			string deliveryService = string.Empty;
			Dictionary<string, object> metaData = new Dictionary<string, object>();

			metaData.Add("State", state);
			metaData.Add("StateCode", stateCode);
			metaData.Add("City", city);
			metaData.Add("DeliveryService", "");
			DeliveryServiceRequest request = new DeliveryServiceRequest()
			{
				CountryCode = "US",
				CountryCurrency = "USD",
				Type = DeliveryServiceType.Option,
				MetaData = metaData
			};


			//long customerSessionId = 1000001838;
			//deliveryMethods = DeskTopTest.GetDeliveryServices(customerSessionId, request, context);// WUDeliveryMethods();
			//Assert.AreEqual(2, deliveryMethods.Count, "There is no any countries.");
		}

		//[Test]
		//public void WUDeliveryOptionsTest()
		//{
		//	List<WUReceiverMasterData> deliveryOptions = new List<WUReceiverMasterData>();
		//	deliveryOptions = DeskTopTest.WUDeliveryOptions();
		//	Assert.AreEqual(1, deliveryOptions.Count, "There is no any countries.");
		//}

		[Test]
		public void MobileProvidersTest()
		{
			List<string> mobileProviders = new List<string>();
            mobileProviders = DeskTopTest.MobileProviders(0, mgiContext);
			Assert.AreEqual(12, mobileProviders.Count);
		}

		[Test]
		public void PhoneTypesTest()
		{
			List<string> phoneTypes = new List<string>();
            phoneTypes = DeskTopTest.PhoneTypes(0, mgiContext);
			Assert.AreEqual(4, phoneTypes.Count);
		}
	}
}
