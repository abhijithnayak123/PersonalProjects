using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class CheckCashingProgress : CheckDetails
    {
        public string CheckSubmitted = "false";
        public decimal NetAmount { get; set; }
        public string CheckType { get; set; }
        public string CheckDate { get; set; }
        public string CheckEstablishmentFee { get; set; }
        public long FeeAdjustmentId { get; set; }
        public decimal BaseFee { get; set; }
        public string DiscountDescription { get; set; }
        public decimal AdditionalFee { get; set; }
    }
}


