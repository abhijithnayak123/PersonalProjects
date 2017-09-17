using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MGI.Channel.DMS.Web.Models
{
    public class SendMoneyTransactionModel : BaseModel
    {

        public string Id { get; set; }

        public string Title { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTranHistoryAmt")]
        public decimal Amount { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTranHistoryType")]
        public string TransactionType { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckFees")]
        public decimal Fee { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ChkTranHistoryCheckConfirmationNo")]
        public string ConfirmationNumber { get; set; }

        public string ReceiptData { get; set; }

        public string TransactionId { get; set; }

		public string TransactionStatus { get; set; }
    }
}