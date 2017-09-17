using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using MGI.Biz.MoneyTransfer.Contract;
using NHibernate;
using NHibernate.Context;
using Spring.Testing.NUnit;
using MGI.Cxn.MoneyTransfer.Contract;

namespace MGI.Biz.MoneyTransfer.Test
{
    [TestFixture]
    public class MoneyTransferSetupEngineTest : AbstractTransactionalSpringContextTests
    {
        //private ISession _sessionCXN;
        
        public IMoneyTransferSetupEngine MoneyTransferSetup { get; set; }
		Dictionary<string, object> context = new Dictionary<string, object>();
		//[SetUp]
		//public void Setup()
		//{
		//	IApplicationContext ctx = ContextRegistry.GetContext();
		//	MoneyTransferSetup = (IMoneyTransferSetup)ctx.GetObject("MoneyTransferSetup");
		//	var _sessionCXN = (ISessionFactory)ctx.GetObject("SessionFactoryCXN");

		//}
        [Test]
        public void Test_DasEnquiry()
        {
            Dictionary<string, object> context = new Dictionary<string, object>();
            context.Add("ChannelPartnerId", 33);
            context.Add("TimeZone", "Central Mountain Time");
            context.Add("ProviderID", 301);
            context.Add("ChannelPartnerName", "Synovus");
            long CustomerSessionID = 1000007819;
            long PartnerTrnxID = 1000001301;

         //   MoneyTransferSetup.GetReasonsByType(CustomerSessionID,"REFUND",context);


        }

        [Test]
        public void GetCountryTest()
        {
			//List<MasterData> GetCountries(Dictionary<string, object> context);
            var country = MoneyTransferSetup.GetCountries(context);
            Assert.IsTrue(country.Count > 0);

        }

        [Test]
        public void ShouldGetStateTest()
        {

            var state = MoneyTransferSetup.GetStates("US",context);
            Assert.IsTrue(state.Count > 0);

        }

        [Test]
        public void ShouldNotGetStateTest()
        {
            //using (ISession session = _sessionCXN.OpenSession())
            //{
            //CallSessionContext.Bind(session);
            var state = MoneyTransferSetup.GetStates("U",context);
            Assert.IsTrue(state.Count == 0);
                
            //}
           
            
        }

        [Test]
        public void ShouldGetCityTest()
        {
            //using (ISession session = _sessionCXN.OpenSession())
            //{
            //CallSessionContext.Bind(session);
            var city = MoneyTransferSetup.GetCities("KA",context);
            Assert.IsTrue(city.Count > 0);
            //}
        }

        [Test]
        public void ShouldNotGetcityTest()
        {
            //using (ISession session = _sessionCXN.OpenSession())
            //{
            //CallSessionContext.Bind(session);
            var city = MoneyTransferSetup.GetCities("INN",context);
            Assert.AreEqual(0, city.Count);
            //}
        }

        ////[Test]
        ////public void DeliveryMethodsTest()
        ////{
        ////    //using (ISession session = _sessionCXN.OpenSession())
        ////    //{
        ////    //CallSessionContext.Bind(session);
        ////    var deliveryMethods = MoneyTransferSetup.GetDeliveryMethods();
        ////    Assert.IsTrue(deliveryMethods.Count > 0);
        ////    //}
        ////}

        ////[Test]
        ////public void DeliveryOptionTest()
        ////{
        ////    //using (ISession session = _sessionCXN.OpenSession())
        ////    //{
        ////    //CallSessionContext.Bind(session);
        ////    var deliveryOptions = MoneyTransferSetup.GetDeliveryOptions();
        ////    Assert.IsTrue(deliveryOptions.Count > 0);
        ////    //}
        ////}

        ////[Test]
        ////public void PaymentMethodTest()
        ////{
        ////    //using (ISession session = _sessionCXN.OpenSession())
        ////    //{
        ////    //CallSessionContext.Bind(session);
        ////    var paymentMethods = MoneyTransferSetup.GetPaymentMethods();
        ////    Assert.IsTrue(paymentMethods.Count > 0);
        ////    //}
        ////}

        ////[Test]
        ////public void PickupMethodTest()
        ////{
        ////    //using (ISession session = _sessionCXN.OpenSession())
        ////    //{
        ////    //CallSessionContext.Bind(session);
        ////    var pickupMethods = MoneyTransferSetup.GetPickupMethods();
        ////    Assert.IsTrue(pickupMethods.Count > 0);
        ////    //}
        ////}

        [Test]
        public void AmountTypesTest()
        {
            var amountTypes = MoneyTransferSetup.GetAmountTypes(context);
            Assert.IsTrue(amountTypes.Count > 0);
        }

        [Test]
        public void StatusesTest()
        {
            var statuses = MoneyTransferSetup.GetStatuses(context);
            Assert.IsTrue(statuses.Count > 0);
        }

        [Test]
        public void RelationshipsTest()
        {
            var relationships = MoneyTransferSetup.GetRelationships(context);
            Assert.IsTrue(relationships.Count > 0);
        }

        [Test]
        public void PickupOptionsTest()
        {
            var pickupoptions = MoneyTransferSetup.GetRelationships(context);
            Assert.IsTrue(pickupoptions.Count > 0);
        }

        [Test]
        public void GetCurrencyCodeTest()
        {
            var CurrencyCode = MoneyTransferSetup.GetCurrencyCode("US",context);
            Assert.IsNotNullOrEmpty(CurrencyCode);
        }

        
        protected override string[] ConfigLocations
        {
            //get { return new string[] { "assembly://MGI.Biz.MoneyTransfer.Impl/MGI.Biz.MoneyTransfer.Impl/MoneyTransfer.Biz.xml" }; }
            get { return new string[] { "assembly://MGI.Biz.MoneyTransfer.Test/MGI.Biz.MoneyTransfer.Test/Biz.MoneyTransfer.Test.xml" }; }
        }
    }
}
