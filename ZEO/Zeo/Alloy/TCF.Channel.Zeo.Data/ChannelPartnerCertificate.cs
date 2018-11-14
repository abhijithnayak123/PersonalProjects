using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class ChannelPartnerCertificate
    {
        [DataMember]
        public Guid ChannelPartnerCertificatePK { get; set; }
        [DataMember]
        public long ChannelPartnerCertificateId { get; set; }
        [DataMember]
        public ChannelPartner ChannelPartner { get; set; }
        [DataMember]
        public string Issuer { get; set; }
        [DataMember]
        public string ThumbPrint { get; set; }
        [DataMember]
        public DateTime DTServerCreate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DTServerLastModified { get; set; }
    }
}
