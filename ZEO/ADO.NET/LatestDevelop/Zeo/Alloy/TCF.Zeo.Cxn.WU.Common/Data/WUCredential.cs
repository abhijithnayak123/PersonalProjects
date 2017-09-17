

using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Cxn.WU.Common.Data
{

    public class WUCredential : ZeoModel
    {
        public virtual string WUServiceUrl { get; set; }
        public virtual string WUClientCertificateSubjectName { get; set; }
        public virtual string AccountIdentifier { get; set; }
        public virtual string CounterId { get; set; }
        public virtual string ChannelName { get; set; }
        public virtual string ChannelVersion { get; set; }
        public virtual long ChannelPartnerId { get; set; }
    }
}