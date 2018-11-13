using System;

namespace MGI.Core.CXE.Data.Transactions
{
	public abstract class FundsBase : NexxoTransactionModel
	{
		public virtual int Type { get; set; }
	}
}
