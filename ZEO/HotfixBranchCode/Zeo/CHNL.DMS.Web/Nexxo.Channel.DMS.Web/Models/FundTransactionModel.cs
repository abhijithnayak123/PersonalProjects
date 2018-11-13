using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
	public class FundTransactionModel : BaseModel
	{
		public string Title { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidCardNumberPopup")]
		public string CardNumber { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "Fees")]
		public decimal Fee { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidNewCardBalance")]
		public decimal NewCardBalance { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ConfirmationNumber")]
		public string ConfirmationNumber { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidLoadAmount")]
		public decimal LoadAmount { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidWithdrawAmount")]
		public decimal WithdrawAmount { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidActivationFee")]
		public decimal ActivationFee { get; set; }

		public int FundType { get; set; }
		
		public string ReceiptData { get; set; }

        public string TransactionId { get; set; }

		public string TransactionType { get; set; }

		public string TransactionStatus { get; set; }
	}
}