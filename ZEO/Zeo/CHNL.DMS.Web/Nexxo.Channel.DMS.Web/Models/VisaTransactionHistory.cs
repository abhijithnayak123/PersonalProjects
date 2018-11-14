using TCF.Channel.Zeo.Web.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
	public class VisaTransactionHistory : BaseModel
	{
		public VisaTransactionHistory()
		{
			DateRanges = new List<SelectListItem>()
			{
				new SelectListItem() { Text="30 Days", Value = "30",Selected = true},
				new SelectListItem() { Text="60 Days", Value = "60"},
				new SelectListItem() { Text="90 Days", Value = "90"}
			};

			TransactionStatuss = new List<SelectListItem>()
			{
				new SelectListItem() { Text="Denied", Value = "Denied"},
				new SelectListItem() { Text="Pending", Value = "Pending"},
				new SelectListItem() { Text="Posted", Value = "Posted", Selected = true}
			};
		}

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TransactionHistoryTransactionType")]
		public string TransactionStatus { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TransactionHistoryDateRange")]
		public string DateRange { get; set; }

		public IEnumerable<SelectListItem> TransactionStatuss { get; set; }
		public IEnumerable<SelectListItem> DateRanges { get; set; }

		public bool IsAccountClosed { get; set; }

		public bool DisableMaintenance { get; set; }

		public int DaysSinceClosed { get; set; }

		public decimal CardBalance { get; set; }

		public CardStatus CardStatus { get; set; }

        public bool IsCardLoadEnabled { get; set; }
    }
}

