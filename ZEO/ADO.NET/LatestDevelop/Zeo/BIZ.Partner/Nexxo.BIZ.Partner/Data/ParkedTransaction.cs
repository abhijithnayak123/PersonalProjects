using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
    public class ParkedTransaction
    {
        public long TransactionId { get; set; }
        public string TransactionType { get; set; }
        public long CustomerSessionId { get; set; }
    }
}
