using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class CheckCashingProgress : CheckDetails
    {
        public string CheckSubmitted = "false";
        public decimal NetAmount { get; set; }
        public string CheckType { get; set; }
        public string CheckDate { get; set; }
        public string CheckEstablishmentFee { get; set; }
    }
}


