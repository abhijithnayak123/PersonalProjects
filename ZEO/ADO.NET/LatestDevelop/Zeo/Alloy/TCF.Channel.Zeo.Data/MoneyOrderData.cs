using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    public class MoneyOrderData
    {
        public decimal Amount { get; set; }
        public string PromotionCode { get; set; }
		public bool IsSystemApplied { get; set; }
    }
}
