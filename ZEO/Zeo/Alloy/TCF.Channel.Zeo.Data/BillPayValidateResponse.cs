using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class BillPayValidateResponse
    {
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public string BillerName { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public string ConfirmationNumber { get; set; }
        [DataMember]
        public string SenderFirstName { get; set; }
        [DataMember]
        public string SenderLastname { get; set; }
        [DataMember]
        public string SenderAddressLine1 { get; set; }
        [DataMember]
        public string SenderAddressLine2 { get; set; }
        [DataMember]
        public string SenderCity { get; set; }
        [DataMember]
        public string SenderState { get; set; }
        [DataMember]
        public string SenderPostalCode { get; set; }
        [DataMember]
        public string SenderEmail { get; set; }
        [DataMember]
        public string SenderContactPhone { get; set; }
        [DataMember]
        public string SenderDateOfBirth { get; set; }
        [DataMember]
        public string SenderWUGoldcardNumber { set; get; }
        [DataMember]
        public decimal UnDiscountedFee { set; get; }
        [DataMember]
        public decimal DiscountedFee { set; get; }
    }
}
