using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Catalog.Data ;
using MGI.Core.Catalog.Contract;
using MGI.Core.Catalog.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

//using NHibernate;
//using NHibernate.Context;

//using Spring.Context;
//using Spring.Context.Support;

using NUnit.Framework;
using Spring.Testing.NUnit;


namespace MGI.Core.Catalog.Test
{
    [TestFixture]
    public class ProductServiceTest : AbstractTransactionalSpringContextTests
    {
         public IProductService ProductService { get; set; }      

        [Test]
        public void Can_GetProduct_Test()
        {
            long productID = 100025330;

            MasterCatalog product = ProductService.Get(productID);           

            Assert.IsTrue(product.Id == productID);
        }

        [Test]
        public void Cannot_GetProduct_Test()
        {
            long productID = 1111; // Give Not Existing ID

            MasterCatalog product = ProductService.Get(productID);

            Assert.IsTrue(product == null);
        }

        [Test]
        public void Can_GetAllProduct_Test()
        {
            int ChannelPartnerID = 34;
			int providerId = 401;

            IList<MasterCatalog> products = ProductService.GetAll(ChannelPartnerID, providerId);

            Assert.IsTrue(products.Count > 0);
        }

        [Test]
        public void Cannot_GetAllProduct_Test()
        {
            int ChannelPartnerID = 1;
			int providerId = 10;

            IList<MasterCatalog> products = ProductService.GetAll(ChannelPartnerID, providerId);

            Assert.IsTrue(products.Count == 0);
        }


		//[Test]
		////the Scenario is not used anywhere else
		//public void Can_GetPresentment_Test()
		//{
		//	long productID = 100029389;

		//	Presentment presentment = ProductService.GetPresentment(productID);

		//	Assert.IsTrue(presentment.ProductID == productID);
		//}

        //[Test]
        //public void Cannot_GetPresentment_Test()
        //{
        //    long productID = 112312312;

        //    Presentment presentment = new Presentment();

        //    try
        //    {
        //        presentment = ProductService.GetPresentment(productID);
        //    }
        //    catch 
        //    {
        //        Assert.IsTrue(presentment.ProductID == 0);
        //    }
           
        //}


        [Test]
        public void Can_GetProductNames_Test()
        {
			string term = "REGIONAL ACCEPTANCE";

            List<string> productNames = new List<string>();

            MasterCatalog test = ProductService.Get(term);
            Assert.IsTrue(test.BillerName  == term);
          //  Assert.IsTrue(productNames.Count>0);
        }

        [Test]
        public void Cannot_GetProductNames_Test()
        {
			string term = "ert";

            //List<string> productName = new List<string>();
			MasterCatalog productNames = ProductService.Get(term);

            //productNames = ProductService.Get(term);

           // Assert.IsTrue(productNames.Count == 0);
        }

        [Test]
        public void Can_GetProductListByIds_Test()
        {          
            //List<Product> productList;

            long[] productIDs = new long[] { 100025329, 100025330, 100025331, 100025332 };

       List<MasterCatalog>  productList = ProductService.GetProductsByIDs(productIDs);

            Assert.IsTrue(productList.Count > 0);
        }

        [Test]
        public void Cannot_GetProductListByIds_Test()
        {
           // List<Product> productList;

            long[] productIDs = new long[] { 2000000001, 2000000002, 2000000003, 2000000004 };

			List<MasterCatalog> productList = ProductService.GetProductsByIDs(productIDs);

            Assert.IsTrue(productList.Count == 0);
        }

        protected override string[] ConfigLocations
        {
            //get { return new string[] { "assembly://MGI.Biz.MoneyTransfer.Impl/MGI.Biz.MoneyTransfer.Impl/MoneyTransfer.Biz.xml" }; }
            get { return new string[] { "assembly://MGI.Core.Catalog.Test/MGI.Core.Catalog.Test/Core.Catalog.Test.xml" }; }
        }

    }
}
