using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class FeeResponse
    {
        [DataMember]
        public List<FeeInformation> FeeInformations { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public Dictionary<string, object> MetaData { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "FeeInformations = ", FeeInformations, "\r\n");
            str = string.Concat(str, "TransactionId = ", TransactionId, "\r\n");
            str = string.Concat(str, "MetaData = ", MetaData, "\r\n");
            return str;
        }

    }
}
