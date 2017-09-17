using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Alloy.CXN.Common
{
    
    public abstract class ServiceBase
    {
        public long? Id { get; set; }
        public Header Header { get; set; }
    }


    public class Header
    {
        public long? AgentSessionId { get; set; }
        public long? CustomerSessionId { get; set; }
        public string ChannelPartnerName { get; set; }
        public int ChannelPartnerId { get; set; }
        public string AgentBankId { get; set; }
        public string AgentBranchId { get; set; }
        public string CustomerBankId { get; set; }
        public string CustomerBranchId { get; set; }
        public Dictionary<string, object> SSOAttributes { get; set; }
    }
}
