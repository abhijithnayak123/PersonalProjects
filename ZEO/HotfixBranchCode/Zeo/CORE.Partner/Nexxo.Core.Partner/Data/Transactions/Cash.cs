namespace MGI.Core.Partner.Data.Transactions
{
	public class Cash : Transaction
	{
		public override int Type { get { return (int)TransactionType.Cash; } }
        public virtual int CashType { get; set; }
		public Cash() { }

		public Cash( decimal Amount, decimal Fee, string Description,
			long CXEId, int CXEState, long CXNId, int CXNState,
			Account Account, CustomerSession CustomerSession, string ConfirmationNumber, int CashType )
			: base( Amount, Fee, Description, CXEId, CXEState, CXNId, CXNState, Account, CustomerSession, ConfirmationNumber ) {

                this.CashType = CashType;
        }
	}
}
