using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TCF.Channel.Zeo.Web.Models
{
    public class CheckTransaction : CheckDetails
    {
        public decimal CheckCashingFee { get; set; }
        public decimal AmountCredited { get; set; }
        public Guid uniqueId { get; set; }
        public string Source { get; set; }
        public decimal NetAmount { get; set; }
        public string CheckStatus { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "FailureDescription")]
        public string StatusDescription { get; set; }
    }
}