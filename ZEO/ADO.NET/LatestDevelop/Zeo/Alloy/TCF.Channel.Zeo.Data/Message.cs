using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string MessageKey { get; set; }

        [DataMember]
        public Helper.Language Language { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string AddlDetails { get; set; }

        [DataMember]
        public string Processor { get; set; }

        [DataMember]
        public long Partner { get; set; }

        [DataMember]
        public int ErrorType { get; set; }
    }
}
