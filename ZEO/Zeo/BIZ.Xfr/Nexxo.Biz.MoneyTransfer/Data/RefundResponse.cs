using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class RefundResponse
	{
		public long ModifyTransactionId { get; set; }
		public long CancelTransactionId { get; set; }
	}
}
