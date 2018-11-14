using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    public class Transaction
    {
        public long Id { get; set; }

        public int Type { get; set; }

        public int Behaviour { get; set; }

        public decimal Amount { get; set; }

        public decimal Fee { get; set; }

        public string Description { get; set; }

        public int State { set; get; }

        public long TransactionId { set; get; }
    }
}
