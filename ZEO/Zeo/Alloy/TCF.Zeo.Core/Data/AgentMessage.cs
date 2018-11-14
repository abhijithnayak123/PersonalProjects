using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class AgentMessage
    {
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string Amount { get; set; }
        public string TransactionState { get; set; }
        public string TicketNumber { get; set; }
        public virtual bool IsParked { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual long AgentId { get; set; }
        public virtual long TransactionId { get; set; }
        public string TimeZone { get; set; }
        public string DeclineMessage { get; set; }
    }
}
