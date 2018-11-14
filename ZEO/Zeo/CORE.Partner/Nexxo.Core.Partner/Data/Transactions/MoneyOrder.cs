namespace MGI.Core.Partner.Data.Transactions
{
	public class MoneyOrder : ExtendedFeeTransaction
	{
		public override int Type { get { return (int)TransactionType.MoneyOrder; } }

		public virtual string CheckNumber { get; set; }

		public virtual string AccountNumber { get; set; }

		public virtual string RoutingNumber { get; set; }

		public MoneyOrder() : base() { }

		public MoneyOrder( decimal Amount, TransactionFee Fee, string Description,
			long CXEId, int CXEState, long CXNId, int CXNState,
            Account Account, CustomerSession CustomerSession)
			: base(Amount, Fee, Description, CXEId, CXEState, CXNId, CXNState, Account, CustomerSession)
		{
		}
	}
}
