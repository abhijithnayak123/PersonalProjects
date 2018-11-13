using System;
namespace MGI.Biz.Catalog.Data
{
    public class Product
    {
        public long ProductId { get; set; }
		public long ProviderCatalogId { get; set; }
        public string BillerCode { get; set; }
        public short ProductVariant { get; set; }
		public string BillerName { get; set; }
		public int ChannelPartnerId { get; set; }
		public int ProviderId { get; set; }
		public System.Nullable<int> ProductType { get; set; }
		public string LogoURL { get; set; }
		public bool IsActive { get; set; }
        public string ProviderName { get; set; }
    }
}
