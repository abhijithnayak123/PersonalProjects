using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class TransactionRequest
    {
		public long PTNRTransactionId { get; set; }
		public long CXNTransactionId { get; set; }
		public string ConfirmationId { get; set; }

    }
}
