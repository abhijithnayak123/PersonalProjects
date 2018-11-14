using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Cxn.BillPay.Data
{
    public class BillPayment
    {
        public decimal PaymentAmount { get; set; }
        public decimal Fee { get; set; }
        public string BillerName { get; set; }
        public string AccountNumber { get; set; }
        public string CouponCode { get; set; }
        public string CityCode { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
    }
}
