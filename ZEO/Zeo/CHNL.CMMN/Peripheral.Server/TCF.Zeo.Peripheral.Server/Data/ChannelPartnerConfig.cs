using System;
using System.Runtime.Serialization;

namespace TCF.Zeo.Peripheral.Server.Data
{
    [DataContract]
    public class ChannelPartnerConfig
    {
        [DataMember]
        public int ChannelPartnerId { get; set; }

        [DataMember]
        public string ServiceURL { get; set; }
    }
}
