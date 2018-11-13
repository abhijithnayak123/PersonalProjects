using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.ServiceClient.Test
{
    [TestFixture]
    public class ReceiverTest
    {
        public Desktop DeskTop { get; set; }
		MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
        //private ShoppingCart cart = null;

        [SetUp]
        public void Setup()
        {
            DeskTop = new Desktop();
        }

        [Test]
        public void RetrieveReceiverTest()
        {
			long customerSessionId = 1000004249;
			long ReceiverId = 1000000000; // "34ce7b6b-f0a9-441b-9fd4-a61ff209c15c";
            Receiver receiver = DeskTop.GetReceiverDetails(customerSessionId, ReceiverId, mgiContext);
            Assert.True(receiver != null);
        }

        [Test]
        public void RetreiveReceiverforEditTest()
        {
			long customerSessionId = 1000004249;
			long ReceiverId = 1000000000;
          //  string ReceiverId = "34ce7b6b-f0a9-441b-9fd4-a61ff209c15c";
			Receiver receiver = DeskTop.GetReceiverDetailsForEdit(customerSessionId, ReceiverId, mgiContext);
            Assert.True(string.IsNullOrWhiteSpace(receiver.FirstName) == false);
            //Assert.True(string.IsNullOrWhiteSpace(receiver.City) == false);
        }

        [Test]
        public void RetrieveCitiesTest()
        {
           // string state = "TAMPS";
            List<System.Web.Mvc.SelectListItem> cities = DeskTop.GetCities();
            Assert.True(cities.Count >= 1);
        }
    }
}