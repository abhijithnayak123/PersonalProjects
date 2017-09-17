using MGI.Common.DataAccess.Data;

namespace MGI.Core.Catalog.Data
{
    public class Product : NexxoModel
    {
        public virtual int CatalogIndex { get; set; }
        public virtual int ProcessorID { get; set; }
        public virtual short ChannelPartnerID { get; set; }
        public virtual string BillerName { get; set; }
        public virtual string ProductKey { get; set; }
        public virtual int ProductType { get; set; }
        public virtual System.Nullable<decimal> Fee { get; set; }
        public virtual System.Nullable<decimal> MinimumLoad { get; set; }
        public virtual System.Nullable<decimal> MaximumLoad { get; set; }
        public virtual string LogoURL { get; set; }
        public virtual string BillerNotes { get; set; }
        public virtual string BillerDisclosure { get; set; }
        public virtual System.Nullable<System.Guid> LocationRegionId { get; set; }
        public virtual System.Nullable<bool> ExtraDataRequired { get; set; }
        public virtual string DataPrompt { get; set; } // has to discuss
        public virtual System.Nullable<bool> IsActive { get; set; }
    }
}
