using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
    public class  ProcessCheckTransactionDetailsViewModel: TransactionDetailsViewModel
    {
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ChkNumberProcessCheck")]
        public string CheckNumber { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckType")]
        public string CheckType { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckAmt")]
        public decimal Amount { get; set; }
    }
}