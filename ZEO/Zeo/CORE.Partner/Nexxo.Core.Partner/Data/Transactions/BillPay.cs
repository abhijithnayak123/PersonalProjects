namespace MGI.Core.Partner.Data.Transactions
{
	public class BillPay : Transaction
	{
		public override int Type { get { return (int)TransactionType.BillPay;	} }

		public virtual long ProductId { get; set; }
		public virtual string AccountNumber { get; set; }

		public BillPay() { }

		public BillPay( decimal Amount, decimal Fee, string Description,
			long CXEId, int CXEState, long CXNId, int CXNState, 
			Account Account, CustomerSession CustomerSession )
			: base( Amount, Fee, Description, CXEId, CXEState, CXNId, CXNState, Account, CustomerSession ) { }
	}
}
