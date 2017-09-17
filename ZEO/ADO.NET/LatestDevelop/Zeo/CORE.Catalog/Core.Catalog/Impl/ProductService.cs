using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Core.Catalog.Contract;
using MGI.Core.Catalog.Data;

namespace MGI.Core.Catalog.Impl
{
	public class ProductService : IProductService
	{
		private const int MAX_COUNT = 6;

		public IRepository<PartnerCatalog> PartnerCatalogRepository { private get; set; }
		
		public IRepository<MasterCatalog> MasterCatalogRepository { private get; set; }
		
		public MasterCatalog Get(long productID)
		{
			return MasterCatalogRepository.FindBy(x => x.Id == productID);
		}
		
		public MasterCatalog Get(long channelPartnerID, string productNameOrCode)
		{
			MasterCatalog productDetails = MasterCatalogRepository.FindBy(x => x.BillerName == productNameOrCode && x.ChannelPartnerId == channelPartnerID);

			if (productDetails == null)
				productDetails = MasterCatalogRepository.FindBy(x => x.BillerCode == productNameOrCode && x.ChannelPartnerId == channelPartnerID);

			return productDetails;
		}
		
		public MasterCatalog Get(string productCode, long channelPartnerID)
		{
			return MasterCatalogRepository.FindBy(x => x.BillerCode == productCode && x.ChannelPartnerId == channelPartnerID);
		}
		
		public MasterCatalog Get(string productName)
		{
			return MasterCatalogRepository.FindBy(x => x.BillerName == productName);
		}
		
		public List<PartnerCatalog> GetProducts(string term, int channelPartnerID, int providerId)
		{
			// List<string> productNames = new List<string>();
			//Removed channelPartnerID filter as per the Discusion(Quick fix for Carver)
			//Removed Provider id filter and re-introduced channelPartnerID filter
			var products = PartnerCatalogRepository.FilterBy(x => x.ChannelPartnerId == channelPartnerID
				&& (x.BillerName.StartsWith(term) || x.BillerCode.StartsWith(term) || x.Keywords.Contains(term)));

			if (products != null)
			{
				return products.ToList();
				//productNames = products.Select(x => x.BillerName).Take(MAX_COUNT).ToList();
			}
			return null;
		}
		
		public List<MasterCatalog> GetAll(int channelPartnerID, int providerId)
		{
			return MasterCatalogRepository.FilterBy(x => x.ChannelPartnerId == channelPartnerID && x.ProviderId == providerId).ToList();
		}
		
		public List<MasterCatalog> GetProductsByIDs(long[] productIDs)
		{
			return MasterCatalogRepository.FilterBy(x => productIDs.Contains(x.Id)).ToList();
		}
	}
}
