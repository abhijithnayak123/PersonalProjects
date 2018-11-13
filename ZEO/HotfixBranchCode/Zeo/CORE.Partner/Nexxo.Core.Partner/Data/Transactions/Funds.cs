namespace MGI.Core.Partner.Data.Transactions
{
	public class Funds : ExtendedFeeTransaction
	{
		public override int Type { get { return (int)TransactionType.Funds; } }

        public virtual int FundType { get; set; }
		public virtual long AddOnCustomerId { get; set; }

		public Funds() : base() { }

		public Funds( decimal Amount, TransactionFee Fee, string Description,
			long CXEId, int CXEState, long CXNId, int CXNState,
			Account Account, CustomerSession CustomerSession, string ConfirmationNumber, int FundTransactionType )
			: base( Amount, Fee, Description, CXEId, CXEState, CXNId, CXNState, Account, CustomerSession, ConfirmationNumber ) {

                this.FundType = FundTransactionType;
        }
	}
}
