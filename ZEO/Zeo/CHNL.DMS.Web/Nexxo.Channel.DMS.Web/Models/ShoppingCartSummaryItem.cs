using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class ShoppingCartSummaryItem 
    {
        public string Product { get; set; }
        public string Status { get; set; }
        public int TxnCount { get; set; }
        public string  TxnType { get; set; }
        public decimal Amount { get; set; }

        public decimal Fee { get; set; }
        public decimal Total { get; set; }
    }
}