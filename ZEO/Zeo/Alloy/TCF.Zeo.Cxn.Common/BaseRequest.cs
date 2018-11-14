using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Common
{
    public abstract class BaseRequest
    {
        public long Id { get; set; }
        public long AgentSessionId { get; set; }
        public long CustomerSessionId { get; set; }
        public long ChannelPartnerId { get; set; }
        public string AgentBankId { get; set; }
        public string AgentBranchId { get; set; }
        public long CustomerID { get; set; }

        public string CustomerBankId { get; set; }
        public string CustomerBranchId { get; set; }
        public DateTime DTTerminalCreate { get; set; }
        public DateTime DTTerminalLastModified { get; set; }
        public DateTime DTServerCreate { get; set; }
        public DateTime DTServerLastModified { get; set; }
        public Dictionary<string, object> SSOAttributes { get; set; }
    }
}
