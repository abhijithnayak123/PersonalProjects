using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
   
    public class TransactionHistoryModel : BaseModel
    {

		public TransactionHistoryModel()
		{
			DateRanges = new List<SelectListItem>()
			{
				new SelectListItem() { Text="30 Days", Value = "30"},
				new SelectListItem() { Text="60 Days", Value = "60", Selected = true },
				new SelectListItem() { Text="90 Days", Value = "90"}
			};

			TransactionTypes = new List<SelectListItem>()
			{
				new SelectListItem() { Text="All", Value = "0"},
				new SelectListItem() { Text="Check Processing", Value = "Check Processing"},
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

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "TransactionHistoryTransactionType")]
		public string TransactionType { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "TransactionHistoryDateRange")]
		public string DateRange { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "TransactionHistoryLocation")]
		public string Location { get; set; }

		public IEnumerable<SelectListItem> TransactionTypes { get; set; }
		public IEnumerable<SelectListItem> DateRanges { get; set; }
		public IEnumerable<SelectListItem> Locations { get; set; }
    }


}
