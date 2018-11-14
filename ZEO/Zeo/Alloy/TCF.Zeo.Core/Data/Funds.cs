using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class Funds : Transaction
    {
        public long CustomerSessionId { set; get; }
        public long ProviderAccountId { set; get; }
        public int ProviderId { set; get; }
        public Helper.FundType FundsType { get; set; }
        public decimal BaseFee { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal AdditionalFee { get; set; }
        public string DiscountName { get; set; }
        public bool IsSystemApplied { get; set; }
        public long AddOnCustomerId { get; set; }
        public string PromoCode { get; set; }
        public string CardNumber { set; get; }
        public string AddOnCustomerName { set; get; }
    }
}
