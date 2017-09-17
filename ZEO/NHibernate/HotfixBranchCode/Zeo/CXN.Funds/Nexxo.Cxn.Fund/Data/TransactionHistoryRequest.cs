using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.Data
{
	public class TransactionHistoryRequest
	{
		public long AliasId { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
		public int DateRange { get; set; }
	}
}
