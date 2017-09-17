using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.Data
{
	public class TransactionHistory
	{

		public DateTime PostedDateTime { get; set; }

		public DateTime TransactionDateTime { get; set; }

		public string MerchantName { get; set; }

		public string Location { get; set; }

		public double TransactionAmount { get; set; }

		public string TransactionDescription { get; set; }

		public string DeclineReason { get; set; }

		public double AvailableBalance { get; set; }

		public double ActualBalance { get; set; }
	}
}
