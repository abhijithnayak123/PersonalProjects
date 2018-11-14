using System;

namespace MGI.Biz.BillPay.Data
{
    public class Product
    {
        public virtual long Id { get; set; }
        public virtual Guid rowguid { get; set; }
        public virtual int CatalogIndex { get; set; }
        public virtual int ProcessorID { get; set; }
        public virtual short ChannelPartnerID { get; set; }
        public virtual string BillerName { get; set; }
        public virtual string ProductKey { get; set; }
        public virtual int ProductType { get; set; }
        public virtual decimal? Fee { get; set; }
        public virtual decimal? MinimumLoad { get; set; }
        public virtual decimal? MaximumLoad { get; set; }
        public virtual string LogoURL { get; set; }
        public virtual string BillerNotes { get; set; }
        public virtual string BillerDisclosure { get; set; }
        public virtual Guid? LocationRegionId { get; set; }
        public virtual bool? ExtraDataRequired { get; set; }
        public virtual string DataPrompt { get; set; }
        public virtual bool? IsActive { get; set; }
        public string BillerCode { get; set; }
    }
}
