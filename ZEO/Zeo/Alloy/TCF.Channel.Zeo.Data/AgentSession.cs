using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public  class AgentSession
    {
        [DataMember]
        public string SessionId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public long AgentID { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public int AuthenticationStatus { get; set; }

        [DataMember]
        public string  Name { get; set; }

        [DataMember]
        public string BankID { get; set; }
        [DataMember]
        public  string BranchID { get; set; }

        [DataMember]
        public string ChannelPartnerName { get; set; }

        [DataMember]
        public  long TerminalId { get; set; }

        [DataMember]
        public string TerminalName { get; set; }

        [DataMember]
        public string PeripheralServiceUrl { get; set; }

    }
}
