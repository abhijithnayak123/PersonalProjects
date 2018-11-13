using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class TransactionHistoryPopup:BaseModel
    {
        public string FundPaymentId { get; set; }
        public string ReceiptData { get; set; }
    }
}