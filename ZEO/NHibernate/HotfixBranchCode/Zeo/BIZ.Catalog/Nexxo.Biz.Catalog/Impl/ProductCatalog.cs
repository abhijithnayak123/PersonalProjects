using System;
using System.Collections.Generic;
using AutoMapper;
using MGI.Biz.Catalog.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.Catalog.Contract;
using MGI.Core.Catalog.Data;

namespace MGI.Biz.Catalog.Impl
{
    public class ProductCatalog : IProductCatalog
    {
        public IProductService ProductService { private get; set; }

        public ProductCatalog()
        {
            Mapper.CreateMap<MGI.Core.Catalog.Data.MasterCatalog, Data.Product>()
                .ForMember(x => x.ProductId, o => o.MapFrom(src => src.Id));

            Mapper.CreateMap<MGI.Core.Catalog.Data.Presentment, Data.Presentment>();
        }

        public Data.Product Get(long productID)
        {
            return Mapper.Map<Data.Product>(ProductService.Get(productID));
        }

        public Data.Product Get(long channelPartnerID, string productNameOrCode)
        {
            return Mapper.Map<Data.Product>(ProductService.Get(channelPartnerID, productNameOrCode));
        }

        public List<PartnerCatalog> GetProducts(string searchTerm, int channelPartnerID)
        {
            return ProductService.GetProducts(searchTerm, channelPartnerID, 0);
        }

        public List<Data.Product> GetAll(int channelPartnerID, int providerId)
        {
            List<Data.Product> productDTOList = new List<Data.Product>();

			List<MasterCatalog> productList = ProductService.GetAll(channelPartnerID, providerId);

            productDTOList = Mapper.Map<List<Data.Product>>(productList);
            return productDTOList;
        }
    }
}
