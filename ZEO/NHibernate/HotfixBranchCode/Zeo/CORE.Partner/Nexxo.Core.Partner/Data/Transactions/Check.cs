namespace MGI.Core.Partner.Data.Transactions
{
	public class Check : ExtendedFeeTransaction
	{
		public override int Type { get { return (int)TransactionType.Check; } }

		public Check() : base() { }

		public Check( decimal Amount, TransactionFee Fee, string Description,
			long CXEId, int CXEState, long CXNId, int CXNState,
			Account Account, CustomerSession CustomerSession )
			: base( Amount, Fee, Description, CXEId, CXEState, CXNId, CXNState, Account, CustomerSession ) { }
	}
}
