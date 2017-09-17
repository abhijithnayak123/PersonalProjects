using TCF.Zeo.Cxn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class Transaction : BaseRequest
    {
        public long AccountId { get; set; }
        public Helper.FundType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal? Fee { get; set; }
        public string Description { get; set; }
        public Helper.TransactionStates Status { get; set; }
        public string ConfirmationId { get; set; }
        public decimal Balance { get; set; }
        public string PromoCode { get; set; }
        public long LocationNodeId { get; set; }
        public DateTime? DTTransmission { get; set; }
        public long TransactionId { get; set; }
    }
}
