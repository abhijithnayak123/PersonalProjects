using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class MoneyOrderTransactionModel
    {
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderAmount")]
        public decimal Amount { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderFee")]
        public decimal Fee { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderCheckNumber")]
        public string CheckNumber { get; set; }

        public string TransactionId { get; set; }

		public string TransactionType { get; set; }

		public string TransactionStatus { get; set; }
    }
}