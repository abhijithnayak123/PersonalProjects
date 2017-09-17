using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TCF.Channel.Zeo.Web.Models
{
    public class AgentTransactionHistory : BaseModel
    {
        public AgentTransactionHistory()
        {
            TransactionTypes = new List<SelectListItem>()
			{
				new SelectListItem() { Text="All", Value = string.Empty},
				new SelectListItem() { Text="Check Processing", Value = "Check Processing"},
				new SelectListItem() { Text="Cash Out", Value = "Cash Out"},
				new SelectListItem() { Text="Cash In", Value = "Cash In"},
				new SelectListItem() { Text="Prepaid-Activate", Value = "Prepaid-Activate"},
                new SelectListItem() { Text="Prepaid-Load", Value = "Prepaid-Load"},
                new SelectListItem() { Text="Prepaid-Withdraw", Value = "Prepaid-Withdraw"},
                new SelectListItem() { Text="Money Order", Value = "MoneyOrder"},
                new SelectListItem() { Text="Bill Pay", Value = "BillPay"},
                new SelectListItem() { Text="Send Money",Value = "SendMoney"},
                new SelectListItem() { Text="Send Money Refund", Value = "SendMoneyRefund"},
                new SelectListItem() { Text="Receive Money", Value = "ReceiveMoney"},
			};
        }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TransactionType")]
        public string TransactionType { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Teller")]
        public string Agent { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IsTransactionStatusSelected")]
        public bool IsTransactionStatusSelected { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TransactionHistoryTransactionID")]
        public long? TransactionID { get; set; }

        public IEnumerable<SelectListItem> TransactionTypes { get; set; }
        public IEnumerable<SelectListItem> Agents { get; set; }
    }
}
