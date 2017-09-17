using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
    public class CashTransaction
    {
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
    }
}
