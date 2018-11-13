using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using MGI.Cxn.WU.Common.Impl;
using NHibernate;
using NHibernate.Context;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using Spring.Testing.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Util;

namespace MGI.Cxn.WU.Common.Test
{
    [TestFixture]
    public class WUMoneyTransferCmnSetupTest : AbstractTransactionalSpringContextTests
    {
        
        public IO WesternUnionIO { get; set; }
		public MGIContext MgiContext { get; set; }

		[TestFixtureSetUp]
		public void SetUpAttribute()
		{
			MgiContext = new MGIContext()
			{
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				ChannelPartnerId = 34,
				ProviderId = 302,
				WUCounterId = "990000402"
			};
		}

        protected override string[] ConfigLocations
        {
			get { return new string[] { "assembly://MGI.CXN.WU.Common.Test/MGI.CXN.WU.Common.Test/CXNTestSpring.xml" }; }
        }
		#region Commented Code
		//[Test]
        //public void feeInquiry()
        //{
			
        //    FeeRequest feereq = new FeeRequest();
        //    Financials financials = new Financials(); 
 
        //    financials.originators_principal_amount =  42556; 
        //    financials.originators_principal_amountSpecified = true;
        //    feereq.financials = financials;

        //    PaymentDetails paymentDetails = new PaymentDetails();
        //    paymentDetails.recording_country_currency = new CountryCurrencyInfo() { country_code = "US", currency_code = "USD" };
        //    paymentDetails.destination_country_currency = new CountryCurrencyInfo() { country_code = "IN", currency_code = "INR" };
        //    paymentDetails.originating_country_currency = new CountryCurrencyInfo() { country_code = "US", currency_code = "USD" };
							 
        //    paymentDetails.transaction_type =  WUEnums.Transaction_type.WMN;
        //    paymentDetails.Transaction_TypeSpecified = true; 
        //    paymentDetails.Payment_type =  WUEnums.Payment_type.Cash;
        //    paymentDetails.Payment_typeSpecified = true; 

        //    feereq.paymentDetails = paymentDetails; 

        //    Promotions promotions = new Promotions();
        //    promotions.coupons_promotions = "A027";

        //    feereq.promotions = promotions; 

        //    Dictionary<string, object> context = new Dictionary<string, object>();
        //    context.Add("ChannelPartnerId", 33);
        //    FeeReponse response = CXNMoneyTransferCmnSetup.FeeInquiry(feereq, context);
        //}

		//[Test]
		//public void WUcardenrollment()
		//{
		//    Sender sender = new Sender();
		//    sender.NameType = "D"; 
		//    sender.FirstName = "Alfons"; 
		//    sender.MiddleName ="Derks";
		//    sender.LastName = "LastName"; 
		//    sender.AddressAddrLine1 ="AddrLine1";
		//    sender.AddressCity = "City";
		//    sender.AddressState ="State";
		//    sender.AddressPostalCode = "53224"; 
		//    sender.ContactPhone ="9740065378";
		//    sender.CountryCode = "US";
		//    sender.CurrencyCode = "USD";
		//    sender.CountryName = "US"; 

		//    PaymentDetails payment = new PaymentDetails();
		//    payment.destinationcountrycurrency_CountryCode = "US";
		//    payment.destinationcountrycurrency_CurrencyCode = "USD";
		//    payment.originatingcountrycurrency_CountryCode = "US";
		//    payment.originatingcountrycurrency_CurrencyCode = "USD";
		//    payment.recordingcountrycurrency_CountryCode = "US";
		//    payment.recordingcountrycurrency_CurrencyCode = "USD";            
             
		//    Dictionary<string,object> context = new Dictionary<string,object>(); 
		//    context.Add("ChannelPartnerId",33);
		//    CardDetails response = CXNMoneyTransferCmnSetup.WUCardEnrollment(sender, payment, context);


		//    //CardDetails response = CXNMoneyTransferCmnSetup.WUCardEnrollment();

		//    //var country = CXNMoneyTransferCmnSetup.WUCardEnrollment();
		//    //Assert.IsTrue(country.Count > 0);
		//}


		//[Test]
		//public void ShouldGetStateTest()
		//{
		//    var state = CXNMoneyTransferSetup.GetStates("US");
		//    Assert.True(state.Count > 0);
		//}

		//[Test]
		//public void ShouldNotGetStateTest()
		//{
		//    var state = CXNMoneyTransferSetup.GetStates("AU");
		//    Assert.True(state.Count == 0);
		//}

		//[Test]
		//public void ShouldGetCityTest()
		//{
		//    var city = CXNMoneyTransferSetup.GetCities("KA");
		//    Assert.IsTrue(city.Count > 0);
		//}

		//[Test]
		//public void ShouldNotGetcityTest()
		//{
		//    var city = CXNMoneyTransferSetup.GetCities("INM");
		//    Assert.IsTrue(city.Count == 0);
		//}

		//[Test]
		//public void DeliveryMethodsTest()
		//{
		//    var deliveryMethods = CXNMoneyTransferSetup.GetDeliveryMethods();
		//    Assert.IsTrue(deliveryMethods.Count > 0);
		//}

		//[Test]
		//public void DeliveryOptionTest()
		//{
		//    var deliveryOptions = CXNMoneyTransferSetup.GetDeliveryOptions();
		//    Assert.IsTrue(deliveryOptions.Count > 0);
		//}

		//[Test]
		//public void PaymentMethodTest()
		//{
		//    var paymentMethods = CXNMoneyTransferSetup.GetPaymentMethods();
		//    Assert.True(paymentMethods.Count > 0);
		//}

		//[Test]
		//public void PickupMethodTest()
		//{
		//    var pickupMethods = CXNMoneyTransferSetup.GetPickupMethods();
		//    Assert.IsTrue(pickupMethods.Count > 0);
		//}

		//[Test]
		//public void AmountTypesMethodTest()
		//{
		//    var amounttypes = CXNMoneyTransferSetup.GetAmountTypes();
		//    Assert.IsTrue(amounttypes.Count > 0);
		//}

		//[Test]
		//public void StatusTest()
		//{
		//    var statuses = CXNMoneyTransferSetup.GetStatuses();
		//    Assert.IsTrue(statuses.Count > 0);
		//}

		//[Test]
		//public void RelationShipTest()
		//{
		//    var relationships = CXNMoneyTransferSetup.GetRelationships();
		//    Assert.IsTrue(relationships.Count > 0);
		//}


		//[Test]
		//public void PickupOptionsTest()
		//{
		//    var pickupOptions = CXNMoneyTransferSetup.GetPickupOptions();
		//    Assert.IsTrue(pickupOptions.Count > 0);
		//}

        //[Test]
        //public void DeliveryMethods()
        //{
        //    Dictionary<string, object> context = new Dictionary<string, object>();
        //    context.Add("ChannelPartnerId", 33);
        //    DeliveryServicesRequest request = new DeliveryServicesRequest();
        //    CountryCurrencyInfo ocountry = new CountryCurrencyInfo();
        //    ocountry.country_code = "US";
        //    ocountry.currency_code = "USD";
        //    request.Originating_Country_Currency = ocountry;  
        //    //request.Originating_Country_Currency.currency_code ="USD"; 
        //    CountryCurrencyInfo odes = new CountryCurrencyInfo();
        //    //request.Destination_Country_Currency.country_code ="US"; 
        //    //request.Destination_Country_Currency.currency_code ="USD";
        //    odes.country_code = "US";
        //    odes.currency_code = "USD";
        //    request.Destination_Country_Currency = odes; 
        //    List<DeliveryServicesResponse> DM = CXNMoneyTransferCmnSetup.GetDeliveryMethods(context, request); 
		//}
		#endregion
		[Test]
        public void Can_CreateRequest()
        {
            long channelPartnerId = 33;

			WUBaseRequestResponse response = WesternUnionIO.CreateRequest(channelPartnerId, MgiContext);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public void Can_Get_LookupCardDetails()
        {

            MGI.Cxn.WU.Common.Data.CardLookupDetails cardLkpDetails = null;
            MGI.Cxn.WU.Common.Data.CardLookUpRequest cardLkpRequest = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
            {               
                card_lookup_search_type = "M",
                sender = new MGI.Cxn.WU.Common.Data.Sender()
                   {
                        PreferredCustomerAccountNumber =  "500525629"
                   }
            };

			cardLkpDetails = WesternUnionIO.WUCardLookupForCardNumber(cardLkpRequest, MgiContext);

            Assert.IsTrue(cardLkpDetails.Sender != null);
            Assert.IsTrue(cardLkpDetails.Sender.First().FirstName != null);
            Assert.IsTrue(cardLkpDetails.Sender.First().LastName != null);
        }

        [Test]
        public void Can_Get_LookupCardDetailsWithDiffSearchType()
        {

            MGI.Cxn.WU.Common.Data.CardLookupDetails cardLkpDetails = null;
            MGI.Cxn.WU.Common.Data.CardLookUpRequest cardLkpRequest = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
            {
                card_lookup_search_type = "M",
                sender = new MGI.Cxn.WU.Common.Data.Sender()
                {
                    PreferredCustomerAccountNumber = "500525629"
                }
            };

			cardLkpDetails = WesternUnionIO.WUCardLookupForCardNumber(cardLkpRequest, MgiContext);

            Assert.IsTrue(cardLkpDetails.Sender != null);
            Assert.IsTrue(cardLkpDetails.Sender.First().FirstName != null);
            Assert.IsTrue(cardLkpDetails.Sender.First().LastName != null);
        }

        [Test]
        [ExpectedException]
        public void Cannot_Get_LookupCardDetails()
        {
            MGI.Cxn.WU.Common.Data.CardLookupDetails cardLkpDetails = null;
            MGI.Cxn.WU.Common.Data.CardLookUpRequest cardLkpRequest = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
            {
                card_lookup_search_type = "M",
                sender = new MGI.Cxn.WU.Common.Data.Sender()
                {
                    PreferredCustomerAccountNumber = "11111"
                }
            };

			cardLkpDetails = WesternUnionIO.WUCardLookupForCardNumber(cardLkpRequest, MgiContext);

            Assert.IsTrue(cardLkpDetails.Sender == null);
        }

        [Test]
        [ExpectedException]
        public void Cannot_Get_LookupCardDetailsWithoutSearchType()
        {
            MGI.Cxn.WU.Common.Data.CardLookupDetails cardLkpDetails = null;
            MGI.Cxn.WU.Common.Data.CardLookUpRequest cardLkpRequest = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
            {
                card_lookup_search_type = "",
                sender = new MGI.Cxn.WU.Common.Data.Sender()
                {
                    PreferredCustomerAccountNumber = "11111"
                }
            };

			cardLkpDetails = WesternUnionIO.WUCardLookupForCardNumber(cardLkpRequest, MgiContext);

            Assert.IsTrue(cardLkpDetails.Sender == null);
        }

    }
}
