namespace MGI.Core.Partner.Data.Transactions
{
	public class MoneyTransfer : Transaction
	{
		public override int Type { get { return (int)TransactionType.MoneyTransfer; } }
        public virtual int TransferType { get; set; } 
		public virtual long? RecipientId { get; set; }
		public virtual decimal? ExchangeRate { get; set; }
        public virtual string TransactionSubType { get; set; }
        public virtual long OriginalTransactionId { get; set; }

		public MoneyTransfer() { }

		public MoneyTransfer( decimal Amount, decimal Fee, string Description,
			long CXEId, int CXEState, long CXNId, int CXNState,
			Account Account, CustomerSession CustomerSession )
			: base( Amount, Fee, Description, CXEId, CXEState, CXNId, CXNState, Account, CustomerSession ) { }
	}
}
