using System;
using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public int CatalogIndex { get; set; }
        [DataMember]
        public int ProcessorID { get; set; }
        [DataMember]
        public short ChannelPartnerID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ProductKey { get; set; }
        [DataMember]
        public int ProductType { get; set; }
        [DataMember]
        public decimal? Fee { get; set; }
        [DataMember]
        public decimal? MinimumLoad { get; set; }
        [DataMember]
        public decimal? MaximumLoad { get; set; }
        [DataMember]
        public string LogoURL { get; set; }
        [DataMember]
        public string BillerNotes { get; set; }
        [DataMember]
        public string BillerCode { get; set; }
        [DataMember]
        public string BillerDisclosure { get; set; }
        [DataMember]
        public Guid? LocationRegionId { get; set; }
        [DataMember]
        public bool? ExtraDataRequired { get; set; }
        [DataMember]
        public string DataPrompt { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public string ProviderName { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "Id = ", Id, "\r\n");
            str = string.Concat(str, "CatalogIndex = ", CatalogIndex, "\r\n");
            str = string.Concat(str, "ProcessorID = ", ProcessorID, "\r\n");
            str = string.Concat(str, "ChannelPartnerID = ", ChannelPartnerID, "\r\n");
            str = string.Concat(str, "ProductName = ", ProductName, "\r\n");
            str = string.Concat(str, "ProductKey = ", ProductKey, "\r\n");
            str = string.Concat(str, "ProductType = ", ProductType, "\r\n");
            str = string.Concat(str, "Fee = ", Fee, "\r\n");
            str = string.Concat(str, "MinimumLoad = ", MinimumLoad, "\r\n");
            str = string.Concat(str, "MaximumLoad = ", MaximumLoad, "\r\n");
            str = string.Concat(str, "LogoURL = ", LogoURL, "\r\n");
            str = string.Concat(str, "BillerNotes = ", BillerNotes, "\r\n");
            str = string.Concat(str, "BillerCode = ", BillerCode, "\r\n");
            str = string.Concat(str, "BillerDisclosure = ", BillerDisclosure, "\r\n");
            str = string.Concat(str, "LocationRegionId = ", LocationRegionId, "\r\n");
            str = string.Concat(str, "ExtraDataRequired = ", ExtraDataRequired, "\r\n");
            str = string.Concat(str, "DataPrompt = ", DataPrompt, "\r\n");
            str = string.Concat(str, "IsActive = ", IsActive, "\r\n");
            str = string.Concat(str, "ProviderName = ", ProviderName, "\r\n");
            return str;
        }
    }
}
