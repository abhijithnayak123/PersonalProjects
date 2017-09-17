using NUnit.Framework;
using Spring.Testing.NUnit;
using System.Collections.Generic;

using MGI.Biz.Catalog.Data;
using MGI.Biz.Catalog.Contract;

using Spring.Data.Core;
using Spring.Context;
using Spring.Context.Support;

namespace MGI.Biz.Catalog.Test
{
	[TestFixture]
	class Catalog_Fixture : AbstractTransactionalSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Biz.Catalog.Test/MGI.Biz.Catalog.Test/MGI.Biz.Catalog.Test.Spring.xml" }; }

		}

		public IProductCatalog ProductCatalog { get; set; }

		[Test]
		public void Can_GetProduct_By_Id()
		{
			long productId = 100000000;

			Product product = ProductCatalog.Get(productId);

			Assert.That(product, Is.Not.Null);			
		}

		[Test]
		public void Can_GetProduct_By_ChannelPartnerId_And_ProductNameOrCode()
		{
			long channelPartnerId = 1;
			string billerNameOrCode = "03CASHCALL";

			Product product = ProductCatalog.Get(channelPartnerId, billerNameOrCode);

			Assert.That(product, Is.Not.Null);
			Assert.That(product.BillerName, Is.EqualTo("03CASHCALL"));

		}

		[Test]
		public void Can_GetProducts_By_SearchTerm_And_ChannelPartnerId()
		{
			int channelPartnerId = 1;
			string searchTerm = "CITY";

			List<MGI.Core.Catalog.Data.PartnerCatalog> products = ProductCatalog.GetProducts(searchTerm, channelPartnerId);

			Assert.That(products, Is.Not.Null);
			Assert.That(products.Count, Is.GreaterThan(1));

		}

		[Test]
		public void Can_GetProducts_By_ChannelPartnerId_And_ProviderId()
		{
			int providerId = 405;
			int channelPartnerId = 1;

			List<Product> products = ProductCatalog.GetAll(channelPartnerId, providerId);

			Assert.That(products, Is.Not.Null);
			Assert.That(products.Count, Is.GreaterThan(1));

		}

		
	}
}
