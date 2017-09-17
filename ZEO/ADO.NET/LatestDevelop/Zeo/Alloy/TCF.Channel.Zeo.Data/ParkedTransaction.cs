using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class ParkedTransaction
    {
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public string ProductId { get; set; }
        [DataMember]
        public long CustomerSessionId { get; set; }
        [DataMember]
        public long CustomerId { get; set; }
        [DataMember]
        public string CheckPassword { get; set; }
        [DataMember]
        public string CheckUserName { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public long LocationID { get; set; }
        [DataMember]
        public string WUCounterId { get; set; }
        [DataMember]
        public long ChannelPartnerId { get; set; }
        [DataMember]
        public string ChannelPartnerName { get; set; }
        [DataMember]
        public string BankId { get; set; }
        [DataMember]
        public string BranchId { get; set; }
        [DataMember]
        public string TimeZone { get; set; }
        [DataMember]
        public string AgentFirstName { get; set; }
        [DataMember]
        public string AgentLastName { get; set; }
        [DataMember]
        public string AgentName { get; set; }
        [DataMember]
        public int AgentId { get; set; }

    }
}
