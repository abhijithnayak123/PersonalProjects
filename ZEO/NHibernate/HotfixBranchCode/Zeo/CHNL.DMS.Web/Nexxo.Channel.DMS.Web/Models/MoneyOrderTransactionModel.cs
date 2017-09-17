using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class MoneyOrderTransactionModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderAmount")]
        public decimal Amount { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderFee")]
        public decimal Fee { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderCheckNumber")]
        public string CheckNumber { get; set; }

        public string TransactionId { get; set; }

		public string TransactionType { get; set; }

		public string TransactionStatus { get; set; }
    }
}