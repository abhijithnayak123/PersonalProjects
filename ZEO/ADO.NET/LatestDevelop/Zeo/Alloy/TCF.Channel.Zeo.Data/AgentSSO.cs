using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
   public class AgentSSO
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string role { get; set; }
        [DataMember]
        public string ClientAgentIdentifier { get; set; }
        [DataMember]
        public System.Nullable<DateTime> BusinessDate { get; set; }

    }
}
