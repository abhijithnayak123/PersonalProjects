using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class FundReceiptData : BaseReceiptData
    {
        public decimal DiscountApplied { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal BaseFee { get; set; }
        public string ConfirmationNo { get; set; }
        public int FundType { get; set; }
        public decimal PreviousCardBalance { get; set; }
        public string CardNumber { get; set; }
        public string DiscountName { get; set; }
        public string CompanionName { get; set; }
        public long TransactionId { get; set; }
    }
}
