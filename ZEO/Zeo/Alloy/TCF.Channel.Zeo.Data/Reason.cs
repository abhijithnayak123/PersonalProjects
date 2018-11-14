using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class Reason
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
