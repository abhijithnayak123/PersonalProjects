using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
    public class RefundRequest
    {       
        public string ReasonDesc { get; set; }
        public string ReasonCode { get; set; }
        public string Comments { get; set; }
		public string RefundStatus { get; set; }
		public long TransactionId { get; set; }
        public string ReferenceNumber { get; set; }
        public string FeeRefund { get; set; }
    }
}
