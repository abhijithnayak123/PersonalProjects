using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    public class  ProcessCheckTransactionDetailsViewModel: TransactionDetailsViewModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ChkNumberProcessCheck")]
        public string CheckNumber { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckType")]
        public string CheckType { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckAmt")]
        public decimal Amount { get; set; }
    }
}