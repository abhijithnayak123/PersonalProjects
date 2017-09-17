using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    public class CardBalanceInfo
    {
        public decimal Balance { get; set; }
        public int CardStatus { get; set; }
        public DateTime? ClosureDate { get; set; }
        public bool IsFraud { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
    }
}
