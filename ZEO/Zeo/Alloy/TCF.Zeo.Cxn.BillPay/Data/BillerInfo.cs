using System.Collections.Generic;

namespace TCF.Zeo.Cxn.BillPay.Data
{
    public class BillerInfo
    {
        public string Message { get; set; }

        public List<decimal> Denominations { get; set; }

        public string DeliveryOption { get; set; }

        public string BillerState { get; set; }

        public string BillerCity { get; set; }
    }
}
