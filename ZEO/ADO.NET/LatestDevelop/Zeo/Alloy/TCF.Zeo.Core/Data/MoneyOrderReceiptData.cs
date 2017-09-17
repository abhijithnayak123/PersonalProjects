using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class MoneyOrderReceiptData : BaseReceiptData
    {
        public string MONumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string DiscountName { get; set; }
        public decimal Fee { get; set; }
        public decimal NetAmount { get; set; }
        public long TransactionId { get; set; }
    }
}
