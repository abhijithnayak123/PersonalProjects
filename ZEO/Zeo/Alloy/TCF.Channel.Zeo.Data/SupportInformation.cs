using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class SupportInformation
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string EmailId { get; set; }

        [DataMember]
        public string Phone1 { get; set; }

        [DataMember]
        public string Phone2 { get; set; }

        [DataMember]
        public string ContactType { get; set; }
    }
}
