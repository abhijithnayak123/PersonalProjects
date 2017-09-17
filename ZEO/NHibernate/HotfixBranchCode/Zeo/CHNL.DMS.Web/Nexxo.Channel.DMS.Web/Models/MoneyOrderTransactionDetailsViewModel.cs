using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    public class MoneyOrderTransactionDetailsViewModel : TransactionDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the Amount
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderAmount")]
        public decimal Amount { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderTrxCheckNumber")]
        public string CheckNumber { get; set; }

    }
}