﻿using MGI.Channel.DMS.Server.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
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

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "TransactionHistoryTransactionType")]
		public string TransactionStatus { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "TransactionHistoryDateRange")]
		public string DateRange { get; set; }

		public IEnumerable<SelectListItem> TransactionStatuss { get; set; }
		public IEnumerable<SelectListItem> DateRanges { get; set; }

		public bool IsAccountClosed { get; set; }

		public bool DisableMaintenance { get; set; }

		public int DaysSinceClosed { get; set; }

		public decimal CardBalance { get; set; }

		public CardStatus CardStatus { get; set; }
	}
}

