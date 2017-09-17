using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    public class ParkedTransaction
    {
        public long TransactionId { get; set; }
        public long CustomerSessionId { get; set; }
        public string TransactionType { get; set; }
    }
}
