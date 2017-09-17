using System;

namespace MGI.Biz.FundsEngine.Data
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
