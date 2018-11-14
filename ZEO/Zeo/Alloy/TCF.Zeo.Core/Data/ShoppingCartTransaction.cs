using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data
{
    public class ShoppingCartTransaction
    {
        public long TransactionId { set; get; }
        public Helper.Product ProductId { set; get; }
        public Helper.TransactionStates State { set; get; }
        public int TransactionType { set; get; }
        public int? TransactionSubType { set; get; }
        public decimal Amount { set; get; }
        public string CheckNumber { get; set; }
        public ProviderId ProviderId { get; set; }

    }
}
