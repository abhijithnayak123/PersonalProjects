using System;
using System.Collections.Generic;
using MGI.Core.Catalog.Data;

namespace MGI.Core.Catalog.Contract
{
	public interface IProductService
	{

		/// <summary>
		/// This method is to get the biller details by biller Id.
		/// </summary>
		/// <param name="productID">This is unique identifier for biller</param>
		/// <returns>biller details</returns>
		MasterCatalog Get(long productID);

		/// <summary>
		/// This method is to get the biller details by biller name.
		/// </summary>
		/// <param name="productName">This is Biller name</param>
		/// <returns>biller details</returns>
		MasterCatalog Get(string productName);

		/// <summary>
		/// This method is to get the biller details by channel partner Id and either biller name or biller code.
		/// </summary>
		/// <param name="channelPartnerID">This is channel partner id</param>
		/// <param name="productNameOrCode">This is either biller name or biller code</param>
		/// <returns>biller details</returns>
		MasterCatalog Get(long channelPartnerID, string productName);

		/// <summary>
		/// This method is to get the biller details by biller code and channel partner Id.
		/// </summary>
		/// <param name="productCode">This is biller code</param>
		/// <param name="channelPartnerID">This is channel partner id</param>
		/// <returns>biller details</returns>
		MasterCatalog Get(string productCode, long channelPartnerID);

		/// <summary>
		/// This method is to get the collection of billers by provider Id and channel partner Id and 
		/// the 'term' parameter value contains in biller name.
		/// </summary>
		/// <param name="term">This is biller name</param>
		/// <param name="channelPartnerID">This is channel partner id in billers</param>
		/// <param name="providerId">This is provider id for billers</param>
		/// <returns>Collection of biller details</returns>
		List<PartnerCatalog> GetProducts(string term, int channelPartnerID, int providerId);

		/// <summary>
		/// This method is to get the collection of products details by channel partner Id and provider Id.
		/// </summary>
		/// <param name="channelPartnerID">This is channel partner id in billers</param>
		/// <param name="providerId">This is provider id for billers</param>
		/// <returns>Collection of biller details</returns>
		List<MasterCatalog> GetAll(int channelPartnerID, int providerId);

		/// <summary>
		/// This method is to get the collection of billers by multiple biller ids.
		/// </summary>
		/// <param name="productIDs">This is collection of unique identifier for billers</param>
		/// <returns>Collection of biller details</returns>
		List<MasterCatalog> GetProductsByIDs(long[] productIDs);
	}
}
