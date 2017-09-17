using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class ShoppingCartAdo
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long AlloyID { get; set; }
        [DataMember]
        public long CustomerSessionId { get; set; }
        [DataMember]
        public string AgentSessionId { get; set; }
        [DataMember]
        public List<Check> Checks { get; set; }
        [DataMember]
        public List<Cash> Cash { get; set; }
        [DataMember]
        public decimal CheckTotal { get; set; }
        [DataMember]
        public decimal CashTotal { get; set; }
        [DataMember]
        public bool IsReferral { get; set; }
    }
}
