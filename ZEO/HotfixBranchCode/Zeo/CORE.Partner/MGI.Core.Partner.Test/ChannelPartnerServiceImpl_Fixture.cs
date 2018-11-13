using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using NUnit.Framework;
using Spring.Testing.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class ChannelPartnerServiceImpl_Fixture : AbstractTransactionalDbProviderSpringContextTests
	{
		private IChannelPartnerService _channelPartnerSvc;
		public IChannelPartnerService ChannelPartnerSvc { set { _channelPartnerSvc = value; } }

		protected override string[] ConfigLocations
		{
			get
			{
				return new string[] { "assembly://MGI.Core.Partner.Test/MGI.Core.Partner.Test/MGI.Core.Partner.Test.Spring.xml" };
			}
		}

		[Test]
		public void CanChannelPartnerConfigTest()
		{
			ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig("TCF");
			Assert.AreEqual(2, channelPartner.CardPresenceVerificationConfig);
		}


		[Test]
		public void LocationsTest()
		{
			List<string> channelPartner = _channelPartnerSvc.Locations("TCF");
			Assert.IsNotNull(channelPartner);
		}

		[Test]
		public void CanGetTipsAndOffersTest()
		{
			var tipsAndOffers = _channelPartnerSvc.GetTipsAndOffers("Centris", "en-us", "PrepaidCard");
			Assert.IsNotNull(tipsAndOffers);
		}

		[Test]
		public void CheckTypesTest()
		{
			var checkTypes = _channelPartnerSvc.GetCheckTypes();

			Console.WriteLine("Check Types: " + checkTypes.Count);

			Assert.IsTrue(checkTypes.Count > 0);
			Assert.AreEqual(checkTypes[0], "Government");
		}

		[Test]
		public void CheckTypeIdTest()
		{
			CheckType checkType = _channelPartnerSvc.GetCheckType("Ins/Attorney/Cashiers");
			Assert.AreEqual(checkType.Id, 1);
		}

		[Test]
		public void CheckTypeNameTest()
		{
			CheckType checkType = _channelPartnerSvc.GetCheckType(3);
			Assert.AreEqual(checkType.Name, "Government");

		}

		[Test]
		[ExpectedException(typeof(ChannelPartnerException))]
		public void BadCheckType()
		{
			CheckType checkType = _channelPartnerSvc.GetCheckType("nonexistent check type");
			Assert.AreNotEqual(checkType.Name, true);
		}

		[Test]
		public void Can_GetChannelPartnerCertificateInfo()
		{
			long channelPartnerId = 34;
			string issuer = "TCF-Okta";
			ChannelPartnerCertificate certificateInfo = _channelPartnerSvc.GetChannelPartnerCertificateInfo(channelPartnerId, issuer);

			Assert.That(certificateInfo, Is.Not.Null);
			Assert.That(certificateInfo.ThumbPrint, Is.EqualTo("8BE85F904A293BD4463BB98F18BDA5E33B6A9883"));
			Assert.That(certificateInfo.ChannelPartner.Name, Is.EqualTo("TCF"));
		}

		[Test]
		public void ChannelPartnerMinimunTransactionAgeTest()
		{
			ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig("TCF");
			Assert.AreEqual(18, channelPartner.Providers.FirstOrDefault().MinimumTransactAge);
		}

		[Test]
		public void ChannelPartnerGPRMinimunTransactionAgeTest()
		{
			ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig("TCF");
			int age = channelPartner.Providers.Where(x => x.ProductProcessor.Product.Name == "ProductCredential")
									.FirstOrDefault().MinimumTransactAge;
			Assert.AreEqual(18, age);
		}
	}
}
