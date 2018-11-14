using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public abstract class BaseRequest
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long AgentSessionId { get; set; }
        [DataMember]
        public long CustomerSessionId { get; set; }
        [DataMember]
        public string ChannelPartnerName { get; set; }
        [DataMember]
        public long ChannelPartnerId { get; set; }
        [DataMember]
        public string AgentBankId { get; set; }
        [DataMember]
        public string AgentBranchId { get; set; }
        [DataMember]
        public string CustomerBankId { get; set; }
        [DataMember]
        public string CustomerBranchId { get; set; }
        [DataMember]
        public Dictionary<string, object> SSOAttributes { get; set; }

    }
}
