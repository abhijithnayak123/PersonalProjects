using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class Error
    {
        [DataMember]
        public string MajorCode { get; set; }
        [DataMember]
        public string MinorCode { get; set; }
        [DataMember]
        public string Processor { get; set; }
        [DataMember]
        public string Details { get; set; }
    }
}
