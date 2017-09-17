using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class ChannelPartnerCertificate
    {
        public Guid ChannelPartnerCertificatePK { get; set; }
        public long ChannelPartnerCertificateId { get; set; }
        public ChannelPartner ChannelPartner { get; set; }
        public string Issuer { get; set; }
        public string ThumbPrint { get; set; }
        public DateTime DTServerCreate { get; set; }
        public Nullable<System.DateTime> DTServerLastModified { get; set; }
    }
}
