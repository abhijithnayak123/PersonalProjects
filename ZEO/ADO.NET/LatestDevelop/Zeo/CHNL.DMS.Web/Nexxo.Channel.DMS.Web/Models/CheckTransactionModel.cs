using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace TCF.Channel.Zeo.Web.Models
{
	public class CheckTransactionModel : BaseModel
	{

		public string Id { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckNo")]
		public string CheckNumber { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckAmt")]
		public decimal Amount { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckType")]
		public string Type { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckFees")]
		public decimal Fee { get; set; }

		public string ImageFront { get; set; }

		public string ImageBack { get; set; }

		[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckConfirmationNo")]
		public string ConfirmationNumber { get; set; }

		public string ReceiptData { get; set; }

        public string TransactionId { get; set; }

        public string TransactionType { get; set; }

		public string TransactionStatus { get; set; }
	}
}