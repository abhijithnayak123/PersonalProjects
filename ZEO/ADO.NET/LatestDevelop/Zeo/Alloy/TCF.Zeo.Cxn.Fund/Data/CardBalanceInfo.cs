using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class CardBalanceInfo
    {
        public double AccountBalance { get; set; }
        public decimal Balance { get; set; }
        public string CardStatus { get; set; }
        public string NewCardNumber { get; set; }
        public bool IsFraud { get; set; }
        public DateTime? ClosureDate { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
    }
}
