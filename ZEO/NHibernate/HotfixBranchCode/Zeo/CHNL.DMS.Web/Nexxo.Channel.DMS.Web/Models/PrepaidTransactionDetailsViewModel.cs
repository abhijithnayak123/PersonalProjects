using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
	public class PrepaidTransactionDetailsViewModel : TransactionDetailsViewModel
	{
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidLoadAmount")]
		public decimal LoadAmount { get; set; }


		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidCardNumberPopup")]
		public string CardNumber { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidNewCardBalance")]
		public decimal NewCardBalance { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidWithdrawAmount")]
		public decimal WithdrawAmount { get; set; }
	}
}