
using MGI.CXN.MG.Common.AgentConnectService;
using AgentConnect = MGI.CXN.MG.Common.AgentConnectService;
using MGI.CXN.MG.Common.Data;
using MGI.CXN.MG.Common.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TranslationsRequest = MGI.CXN.MG.Common.Data.TranslationsRequest;
using TranslationsResponse = MGI.CXN.MG.Common.Data.TranslationsResponse;
using MGI.Common.Util;
using MGI.CXN.MG.Common.Contract;

namespace MGI.CXN.MG.Common.Test
{
	[TestFixture]
	public class MGCommonIO_Fixture // : AbstractSpringContextTests
	{
		public MGIContext MgiContext { get; set; }
		//protected override string[] ConfigLocations
		//{
		//	get { return new string[] { "assembly://MGI.CXN.MG.Common.Test/MGI.CXN.MG.Common.Test/MGI.CXN.MG.Common.Test.Spring.xml" }; }
		//}

		//public IMGCommonIO MoneyGramCommonIO { private get; set; }

		[Test]
		public void Can_GetMetaData()
		{
			IO MoneyGramCommonIO = new IO();
			BaseRequest request = PopulateBaseRequest();
			CTResponse response = MoneyGramCommonIO.GetMetaData(request, MgiContext);

			Assert.That(response, Is.Not.Null);
			Assert.That(response.StateProvinces, Is.Not.Empty);
			Assert.That(response.Countries, Is.Not.Empty);
			Assert.That(response.CountryCurrencies, Is.Not.Empty);
			Assert.That(response.Currencies, Is.Not.Empty);
			Assert.That(response.DeliveryOptions, Is.Not.Empty);
		}

		[Test]
		public void Can_GetDoddFrankStateRegulatorInfo()
		{
			IMGCommonIO MoneyGramCommonIO = new IO();
			StateRegulatorRequest request = PopulateStateRegulatorRequest();

			StateRegulatorResponse response = MoneyGramCommonIO.GetDoddFrankStateRegulatorInfo(request, MgiContext);

			Assert.That(response, Is.Not.Null);
			Assert.That(response.Version, Is.Not.Null);
			Assert.That(response.StateRegulators, Is.Not.Empty);

		}

		[Test]
		public void Can_GetTranslations()
		{
			IO MoneyGramCommonIO = new IO();
			TranslationsRequest request = PopulateTranslationsRequest();

			TranslationsResponse response = MoneyGramCommonIO.GetTransalations(request, MgiContext);

			Assert.That(response, Is.Not.Null);
			Assert.That(response.Countries, Is.Not.Null);
			Assert.That(response.Currencies, Is.Not.Empty);
			Assert.That(response.DeliveryOptions, Is.Not.Empty);
			Assert.That(response.Industries, Is.Not.Empty);
			Assert.That(response.FQDOTexts, Is.Not.Empty);
		}

		[Test]
		public void Can_GetFee()
		{
			IO MoneyGramCommonIO = new IO();
			FeeLookupRequest request = PopulateFeeRequest();

			FeeLookupResponse response = MoneyGramCommonIO.GetFee(request, MgiContext);

			Assert.That(response, Is.Not.Null);
			Assert.That(response.feeInfo, Is.Not.Empty);
			Assert.That(response.feeInfo[0].receiveAmounts.receiveAmount, Is.GreaterThan(0));
			Assert.That(response.feeInfo[0].sendAmounts.totalSendFees, Is.GreaterThan(0));
		}

        [Test]
        public void Can_GetFieldsForProduct()
        {
			IO MoneyGramCommonIO = new IO();

            AgentConnect.GetFieldsForProductRequest request = PopulateGetFieldsForProductRequest();

			AgentConnect.GetFieldsForProductResponse response = MoneyGramCommonIO.GetFieldsForProduct(request, MgiContext);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.productFieldInfo.Length, Is.GreaterThan(0));
        }

		#region Private Methods

		private TranslationsRequest PopulateTranslationsRequest()
		{
			TranslationsRequest request = new TranslationsRequest()
			{
				AgentID = "43677869",
				AgentSequence = "1",
				Token = "TEST",
				Language = "en",
				TimeStamp = DateTime.Now,
				ApiVersion = "1305",
				ClientSoftwareVersion = "1",
				LanguageCode = "fr"
			};
			return request;
		}

		private FeeLookupRequest PopulateFeeRequest()
		{
            var request = new FeeLookupRequest()
			{
				agentID = "43685767",
				agentSequence = "1",
				token = "TEST",
				apiVersion = "1305",
				Item = 100,
				ItemElementName = ItemChoiceType.receiveAmount,
				clientSoftwareVersion = "10.2",
				timeStamp = Convert.ToDateTime("2014-07-04T00:32:42.247-05:00"),
                productType = productType.SEND,
				allOptions = false,
				receiveCountry = "USA"
			};

			return request;
		}

		private BaseRequest PopulateBaseRequest()
		{
			BaseRequest baseRequest = new BaseRequest()
			{
				AgentID = "43677869",
				AgentSequence = "1",
				Token = "TEST",
				Language = "en",
				TimeStamp = DateTime.Now,
				ApiVersion = "1305",
				ClientSoftwareVersion = "1"
			};
			return baseRequest;
		}

		private StateRegulatorRequest PopulateStateRegulatorRequest()
		{
			StateRegulatorRequest request = new StateRegulatorRequest()
			{
				AgentID = "43677869",
				AgentSequence = "1",
				Token = "TEST",
				Language = "en",
				TimeStamp = DateTime.Now,
				ApiVersion = "1305",
				ClientSoftwareVersion = "1",
				Languages = new string[] { "ENG" }
			};
			return request;
		}

        private GetFieldsForProductRequest PopulateGetFieldsForProductRequest()
        {
            GetFieldsForProductRequest request = new GetFieldsForProductRequest()
            {
                
                agentID = "43685767",
                agentSequence = "1",
                token = "TEST",
                apiVersion  = "1305",
                amount = 100,
                clientSoftwareVersion = "10.2",
                timeStamp = Convert.ToDateTime("2014-08-21T04:26:58.711-05:00"),

                productType = productType.BP,
                receiveCountry = "USA",
                thirdPartyType = thirdPartyType.NONE,
                receiveAgentID = "42023524",
                billerAccountNumber = "515425654789",
                receiveCurrency = "USD",
                productVariant = productVariant.EP,
                productVariantSpecified =true,
                consumerId = "0"
            };

            return request;
        }

		#endregion
	}
}
