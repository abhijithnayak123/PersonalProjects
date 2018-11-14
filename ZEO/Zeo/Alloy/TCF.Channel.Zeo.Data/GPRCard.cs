using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class GPRCard
    {

        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public decimal ActivationFee { get; set; }
        [DataMember]
        public decimal InitialLoadAmount { get; set; }
        [DataMember]
        public decimal LoadAmount { get; set; }
        [DataMember]
        public decimal LoadFee { get; set; }
        [DataMember]
        public decimal WithdrawAmount { get; set; }
        [DataMember]
        public decimal WithdrawFee { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public string ItemType { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public decimal BaseFee { get; set; }
        [DataMember]
        public decimal NetFee { get; set; }
        [DataMember]
        public decimal DiscountApplied { get; set; }
        [DataMember]
        public string DiscountName { get; set; }
        [DataMember]
        public string AddOnCustomerName { get; set; }
    }
}
