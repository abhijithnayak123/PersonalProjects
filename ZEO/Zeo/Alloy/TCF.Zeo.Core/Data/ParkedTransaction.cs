using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class ParkedTransaction
    {
        public long TransactionId { get; set; }
        public Helper.Product ProductId { get; set; }
        public long CustomerSessionId { get; set; }
        public long CustomerId { get; set; }
        public string CheckPassword { get; set; }
        public string CheckUserName { get; set; }
        public string LocationName { get; set; }
        public long LocationID { get; set; }
        public string WUCounterId { get; set; }
        public int ChannelPartnerId { get; set; }
        public string ChannelPartnerName { get; set; }
        public string BankId { get; set; }
        public string BranchId { get; set; }
        public string TimeZone { get; set; }
        public string AgentFirstName { get; set; }
        public string AgentLastName { get; set; }
        public string AgentName { get; set; }
        public long AgentId { get; set; }
        public int ProviderId { get; set; }
    }
}
