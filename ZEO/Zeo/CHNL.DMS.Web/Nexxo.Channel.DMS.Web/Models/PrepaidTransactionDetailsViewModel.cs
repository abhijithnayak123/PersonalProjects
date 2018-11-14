using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
	public class PrepaidTransactionDetailsViewModel : TransactionDetailsViewModel
	{
		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrepaidLoadAmount")]
		public decimal LoadAmount { get; set; }


		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrepaidCardNumberPopup")]
		public string CardNumber { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrepaidNewCardBalance")]
		public decimal NewCardBalance { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PrepaidWithdrawAmount")]
		public decimal WithdrawAmount { get; set; }
	}
}