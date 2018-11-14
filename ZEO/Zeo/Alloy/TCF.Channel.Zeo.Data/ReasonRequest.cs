using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class ReasonRequest
    {
        [DataMember]
        public string TransactionType { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "TransactionType = ", TransactionType, "\r\n");
            return str;
        }
    }
}
