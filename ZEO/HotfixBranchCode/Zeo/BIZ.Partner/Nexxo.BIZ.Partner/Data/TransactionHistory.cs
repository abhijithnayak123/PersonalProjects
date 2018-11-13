using System;

namespace MGI.Biz.Partner.Data
{
	public class TransactionHistory
	{
		public long CustomerId { get; set; }
		public string CustomerName { get; set; }
		public DateTime TransactionDate { get; set; }
		public string Teller { get; set; }
		public long TellerId { get; set; }
		public long SessionId { get; set; }
		public long TransactionId { get; set; }
		public string Location { get; set; }
		public string TransactionType { get; set; }
		public string TransactionDetail { get; set; }
		public string TransactionStatus { get; set; }
		public decimal TotalAmount { get; set; }
	}
}
