using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class MoneyOrder
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public DateTime PurchaseDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal BaseFee { get; set; }
        [DataMember]
        public decimal DiscountApplied { get; set; }
        [DataMember]
        public string DiscountName { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string RoutingNumber { get; set; }
        [DataMember]
        public string MICR { get; set; }
        [DataMember]
        public byte[] FrontImage { get; set; }
        [DataMember]
        public byte[] BackImage { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        [DataMember]
        public string DiscountDescription { set; get; }
        [DataMember]
        public int ProviderId { set; get; }


    }
}
