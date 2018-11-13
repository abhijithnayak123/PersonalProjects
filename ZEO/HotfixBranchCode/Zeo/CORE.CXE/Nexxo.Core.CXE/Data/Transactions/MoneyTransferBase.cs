using System;

namespace MGI.Core.CXE.Data.Transactions
{
	public abstract class MoneyTransferBase : NexxoTransactionModel
	{
		public virtual string ReceiverName { get; set; }
		public virtual string Destination { get; set; }
		public virtual decimal DestinationAmount { get; set; }
		public virtual string ConfirmationNumber { get; set; }
	}
}
