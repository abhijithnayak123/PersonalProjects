using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public abstract class ServiceBase
    {
        [DataMember]
        public long? Id { get; set; }
        [DataMember]
        public Header Header { get; set; }
    }

    [DataContract]
    public class Header
    {
        [DataMember]
        public long? AgentSessionId { get; set; }
        [DataMember]
        public long? CustomerSessionId { get; set; }
        [DataMember]
        public string ChannelPartnerName { get; set; }
        public int ChannelPartnerId { get; set; }
        [DataMember]
        public string AgentBankId { get; set; }
        [DataMember]
        public string AgentBranchId { get; set; }
        [DataMember]
        public Dictionary<string, object> SSOAttributes { get; set; }
    }
}
