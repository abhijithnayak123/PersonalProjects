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
    public class SendMoneySearchRequest
    {
        [DataMember]
        public string ConfirmationNumber { get; set; }
        [DataMember]
        public Helper.SearchRequestType SearchRequestType { get; set; }
    }
}
