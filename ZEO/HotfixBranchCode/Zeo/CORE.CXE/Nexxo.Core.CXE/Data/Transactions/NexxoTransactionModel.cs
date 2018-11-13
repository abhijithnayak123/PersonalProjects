using System;
using MGI.Common.DataAccess.Data;

namespace MGI.Core.CXE.Data.Transactions
{
	public abstract class NexxoTransactionModel : NexxoModel
	{
		public virtual decimal Amount { get; set; }
		public virtual decimal Fee { get; set; }

		public virtual Account Account { get; set; }

		public virtual int Status { get; set; }
	}
}
