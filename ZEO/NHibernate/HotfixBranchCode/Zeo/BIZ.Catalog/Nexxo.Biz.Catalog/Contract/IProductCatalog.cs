using System;
using System.Collections.Generic;
using MGI.Core.Catalog.Data;
using Presentment = MGI.Biz.Catalog.Data.Presentment;
using Product = MGI.Biz.Catalog.Data.Product;

namespace MGI.Biz.Catalog.Contract
{
	public interface IProductCatalog
	{
        /// <summary>
        /// To fetch product[BillPay] details by product id.
        /// </summary>
        /// <param name="productID">This is the unique identifier for product</param>
        /// <returns>Product Details</returns>
        Product Get(long productID);

        /// <summary>
        /// To fetch product[BillPay] details by product name.
        /// </summary>
        /// <param name="channelPartnerId">This is the unique identifier for channel partner</param>
        /// <param name="productName">the product name</param>
        /// <returns>Product Details</returns>
        Product Get(long channelPartnerId,string productName);

        /// <summary>
        /// To fetch product[BillPay] based on search term.
        /// </summary>
        /// <param name="searchTerm">the search term</param>
        /// <param name="channelPartnerId">This is the unique identifier for channel partner</param>
        /// <returns>List Of Product Catalog</returns>
        List<PartnerCatalog> GetProducts(string searchTerm, int channelPartnerId);

        /// <summary>
        /// To fetch the collection of product[BillPay] Details based on provider unique identifier.
        /// </summary>
        /// <param name="channelPartnerId">This is the unique identifier for channel partner</param>
        /// <param name="providerId">This is the unique identifier for provider</param>
        /// <returns>List Of Produc</returns>
        List<Product> GetAll(int channelPartnerId, int providerId);
	}
}
