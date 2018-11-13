using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
	public class RefundRequest
	{
		
		public long TransactionId { get; set; }
		
		public string ConfirmationNumber { get; set; }

		public string ReasonCode { get; set; }

		public string ReasonDesc { get; set; }
		
		public string Comments { get; set; }
		
		public string RefundStatus { get; set; }

		public long CancelTransactionId { get; set; }

		public long RefundTransactionId { get; set; }

        //Only for Biz Layer - to Write in CXE and PTNR
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        //public string TransactionSubType { get; set; }
        public long OriginalTransactionId { get; set; }

        public string FeeRefund { get; set; }
	}
}
