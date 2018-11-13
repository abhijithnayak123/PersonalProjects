using System;

using MGI.Core.CXE.Data.Transactions;

namespace MGI.Core.CXE.Data
{
	public class CheckImages
	{
		public virtual Guid id { get; set; }
		public virtual Transactions.Stage.Check Check { get; set; }
		public virtual byte[] Front { get; set; }
		public virtual byte[] Back { get; set; }
		public virtual string Format { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
	}
}
