using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
	public class RefundResponse
	{
		public long CancelTransactionId { get; set; }
		public long ModifyTransactionId { get; set; }
	}
}
