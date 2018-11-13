using System;

namespace MGI.Core.CXE.Data.Transactions
{
	public abstract class BillPayBase : NexxoTransactionModel
	{
		public virtual int ProductId { get; set; }
		public virtual string ProductName { get; set; }
		public virtual string AccountNumber { get; set; }
		public virtual string ConfirmationNumber { get; set; }
	}
}
