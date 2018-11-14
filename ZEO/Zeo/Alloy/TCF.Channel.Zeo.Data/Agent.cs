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
    public class Agent
    {
        [DataMember]
        public  long AgentID { get; set; }
        [DataMember]
        public  string Name { get; set; }
        [DataMember]
        public  Helper.AuthenticationStatus AuthStatus { get; set; }
        [DataMember]
        public  string UserName { get; set; }
        [DataMember]
        public  string ClientAgentIdentifier { get; set; }
        [DataMember]
        public  int UserRoleId { get; set; }
        [DataMember]
        public string AgentFirstName { get; set; }
        [DataMember]
        public string AgentLastName { get; set; }
        [DataMember]
        public string AgentFullName { get; set; }
        [DataMember]
        public long LocationId { get; set; }
    }
}
