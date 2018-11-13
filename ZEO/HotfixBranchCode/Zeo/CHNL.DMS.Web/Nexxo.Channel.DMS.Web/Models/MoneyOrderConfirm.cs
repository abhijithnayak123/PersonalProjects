using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
	public class MoneyOrderConfirm : MoneyOrderImage
    {
        public decimal Amount { get; set; }
        public string AmountInWord { get; set; }
        public string Signature { get; set; }
        public string PhoneNumber { get; set; }
        public string TransactionId { get; set; }
        public string PrintData { get; set; }

    }
}