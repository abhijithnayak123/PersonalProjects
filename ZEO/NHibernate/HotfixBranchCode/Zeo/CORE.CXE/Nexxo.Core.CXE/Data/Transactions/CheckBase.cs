using System;

namespace MGI.Core.CXE.Data.Transactions
{
	public abstract class CheckBase : NexxoTransactionModel
	{
		public virtual Nullable<DateTime> IssueDate { get; set; }
		public virtual string MICR { get; set; }
		public virtual int CheckType { get; set; }

		public virtual CheckImages Images { get; set; }
	}
}
