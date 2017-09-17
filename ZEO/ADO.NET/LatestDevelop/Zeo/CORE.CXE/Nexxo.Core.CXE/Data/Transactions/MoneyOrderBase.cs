using System;

namespace MGI.Core.CXE.Data.Transactions
{
	public abstract class MoneyOrderBase : NexxoTransactionModel
	{
		public virtual Nullable<DateTime> PurchaseDate { get; set; }
		public virtual string MICR { get; set; }
        public virtual string MoneyOrderCheckNumber { get; set; }
		public virtual string AccountNumber { get; set; }
		public virtual string RoutingNumber { get; set; }
	}
}
