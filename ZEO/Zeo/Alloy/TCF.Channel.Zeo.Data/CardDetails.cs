using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CardDetails
    {
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string ForiegnSystemId { get; set; }
        [DataMember]
        public string ForiegnRefNum { get; set; }
        [DataMember]
        public string CounterId { get; set; }
    }
}
