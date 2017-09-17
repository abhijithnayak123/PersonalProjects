using System;

namespace MGI.Core.Partner.Data
{
	public class TransactionHistory
	{
		public virtual Guid rowguid { get; set; }
		public virtual long CustomerId { get; set; }
		public virtual string CustomerName { get; set; }
		public virtual DateTime TransactionDate { get; set; }
		public virtual string Teller { get; set; }
		public virtual long TellerId { get; set; }
		public virtual long SessionId { get; set; }
		public virtual long TransactionId { get; set; }
		public virtual string Location { get; set; }
		public virtual string TransactionType { get; set; }
		public virtual string TransactionDetail { get; set; }
		public virtual string TransactionStatus { get; set; }
		public virtual decimal TotalAmount { get; set; }

		//public virtual decimal Amount { get; set; }
		//public virtual decimal Fee { get; set; }
		//public virtual string Status { get; set; }
		//public virtual string ConfirmationNumber { get; set; }
		//public virtual string TxnType { get; set; }
	}
}
