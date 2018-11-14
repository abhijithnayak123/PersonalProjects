using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class CardMaintenanceInfo
    {
        public string CardStatus { get; set; }

        public string ShippingType { get; set; }

        public double ShippingFee { get; set; }

        public double ShippingFeeCode { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public int CardClass { get; set; }

        public string CardNumber { get; set; }

        public double ReplacementFee { get; set; }

        public int ReplacementFeeCode { get; set; }

        public string SelectedCardStatus { get; set; }

        public string StockId { get; set; }
    }
}
