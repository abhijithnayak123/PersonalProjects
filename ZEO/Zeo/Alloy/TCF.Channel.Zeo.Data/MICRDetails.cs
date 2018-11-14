using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class MICRDetails
    {
        [DataMember]
        public long RoutingNumber { get; set; }

        [DataMember]
        public long AccountNumber { get; set; }

        [DataMember]
        public long CheckNumber { get; set; }
    }
}
