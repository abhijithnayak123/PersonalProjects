using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data.Transactions
{
	public abstract class Transaction : NexxoModel
	{
		public abstract int Type { get; }

		public virtual long CXEId { get; set; }
		public virtual long CXNId { get; set; }
		public virtual int CXEState { get; set; }
		public virtual int CXNState { get; set; }
		public virtual decimal Amount { get; set; }
		public virtual decimal Fee { get; set; }
		public virtual string Description { get; set; }
		public virtual CustomerSession CustomerSession { get; set; }
		public virtual Account Account { get; set; }
        public virtual string ConfirmationNumber { get; set; }
		public virtual long AddOnCustomerId { get; set; }

		public Transaction() { }

		public Transaction( decimal Amount, decimal Fee, string Description,
			long CXEId, int CXEState, long CXNId, int CXNState, 
			Account Account, CustomerSession CustomerSession )
		{
			this.Amount = Amount;
			this.Fee = Fee;
			this.Description = Description;
			this.CXEId = CXEId;
			this.CXEState = CXEState;
			this.CXNId = CXNId;
			this.CXNState = CXNState;
			this.Id = CXEId;
			this.Account = Account;
			this.CustomerSession = CustomerSession;
		}

        //TODO: Yashasvi - Added a constructor not to impact existing code. Need to remove once BP and Xfr are up and running

        public Transaction(decimal Amount, decimal Fee, string Description,
            long CXEId, int CXEState, long CXNId, int CXNState,
            Account Account, CustomerSession CustomerSession, string ConfirmationNumber)
        {
            this.Amount = Amount;
            this.Fee = Fee;
            this.Description = Description;
            this.CXEId = CXEId;
            this.CXEState = CXEState;
            this.CXNId = CXNId;
            this.CXNState = CXNState;
            this.Id = CXEId;
            this.Account = Account;
            this.CustomerSession = CustomerSession;
            this.ConfirmationNumber = ConfirmationNumber;
        }
	}
}
