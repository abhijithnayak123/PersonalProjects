using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public abstract class ExtendedFeeTransaction : Transaction
    {

        public decimal BaseFee { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal AdditionalFee { get; set; }
        public bool IsSystemApplied { get; set; }
        public string DiscountName { get; set; }
        public string DiscountDescription { set; get; }
    }
        
}
