using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class CouponReceiptData : BaseReceiptData
    {
        public long CustomerId { get; set; }
        public string PromoName { get; set; }
        public string PromoDescription { get; set; }
    }
}
