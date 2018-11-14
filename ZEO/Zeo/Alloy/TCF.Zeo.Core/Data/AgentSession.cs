using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace TCF.Zeo.Core.Data
{
    public  class AgentSession
    {
        public  string SessionId { get; set; }
        public  string UserName { get; set; }
        public  long AgentID { get; set; }
        public  string LocationName { get; set; }
        public  int AuthenticationStatus { get; set; }
        public  System.Nullable<System.DateTime> BusinessDate { get; set; }
        public  string Name { get; set; }
        public  string ClientAgentIdentifier { get; set; }
        public  string BankID { get; set; }
        public  string BranchID { get; set; }
        public  string ChannelPartnerName { get; set; }
        public long TerminalId { get; set; }
        public string TerminalName { get; set; }
        public string PeripheralServiceUrl { get; set; }

    }
}
