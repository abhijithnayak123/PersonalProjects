using MGI.Common.Util;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.MoneyTransfer.Data;
using NUnit.Framework;
using Spring.Testing.NUnit;
using System.Collections.Generic;

namespace MGI.Cxn.MoneyTransfer.MG.Test
{
	[TestFixture]
	public class MoneyGramSetup_Fixture : AbstractTransactionalSpringContextTests
	{
		public MGIContext MgiContext { get; set; }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.MoneyTransfer.MG.Test/MGI.Cxn.MoneyTransfer.MG.Test/MGI.Cxn.MoneyTransfer.MG.Test.Spring.xml" }; }
		}

		public IMoneyTransfer CXNMoneyTransfer { get; set; }

		[Test]
		public void Can_GetCountries()
		{
			List<MasterData> moneyGramCountries = CXNMoneyTransfer.GetCountries();

			Assert.That(moneyGramCountries, Is.Not.Null);
			Assert.That(moneyGramCountries, Is.Not.Empty);
		}

		[Test]
		public void Can_GetStates()
		{
			string countryCode = "USA";
			List<MasterData> moneyGramStates = CXNMoneyTransfer.GetStates(countryCode);

			Assert.That(moneyGramStates, Is.Not.Null);
			Assert.That(moneyGramStates, Is.Not.Empty);
		}

		[Test]
		public void Can_GetDeliveryMethods()
		{
			DeliveryServiceRequest request = new DeliveryServiceRequest();

			List<DeliveryService> deliveryMethods = CXNMoneyTransfer.GetDeliveryServices(request, MgiContext);

			Assert.That(deliveryMethods, Is.Not.Null);
			Assert.That(deliveryMethods, Is.Not.Empty);
		}

		[Test]
		public void Can_GetCurrencyCodeList()
		{
			string countryCode = "USA";
			List<MasterData> currencyCodes = CXNMoneyTransfer.GetCurrencyCodeList(countryCode);

			Assert.That(currencyCodes, Is.Not.Null);
			Assert.That(currencyCodes, Is.Not.Empty);
		}

		[Test]
		public void Can_GetCurrencyCode()
		{
			string countryCode = "USA";
			string currencyCode = CXNMoneyTransfer.GetCurrencyCode(countryCode);

			Assert.That(currencyCode, Is.Not.Null);
			Assert.That(currencyCode, Is.EqualTo("USD"));
		}


        [Test]
        public void CanGetStatus()
        {
            SearchRequest searchRequest = new SearchRequest()
                                {
                                    ConfirmationNumber = "92859134"
                                };

			SearchResponse searchResponse = CXNMoneyTransfer.Search(searchRequest, MgiContext);
            Assert.That(searchResponse.TransactionStatus, Is.Not.Null);

        }
	}
}
