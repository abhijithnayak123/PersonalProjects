using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class MasterCountry : BaseRequest
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Abbr2 { get; set; }

        [DataMember]
        public string Abbr3 { get; set; }
    }
}
