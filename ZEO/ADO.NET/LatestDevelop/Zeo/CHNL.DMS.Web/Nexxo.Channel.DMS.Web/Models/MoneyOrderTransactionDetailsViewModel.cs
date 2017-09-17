using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
    public class MoneyOrderTransactionDetailsViewModel : TransactionDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the Amount
        /// </summary>
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderAmount")]
        public decimal Amount { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderTrxCheckNumber")]
        public string CheckNumber { get; set; }

    }
}