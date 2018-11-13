using System;

namespace MGI.Cxn.BillPay.WU.Data
{
	public class ImportedBiller
	{
		public virtual Guid rowguid { get; set; }
		public virtual long Id { get; set; }
		public virtual string BillerName { get; set; }
		public virtual string AccountNumber { get; set; }
		public virtual string CardNumber { get; set; }
		public virtual string WUIndex { get; set; }
		public virtual WesternUnionAccount WUAccount { get; set; }
		public virtual long AgentSessionId { get; set; }
		public virtual long CustomerSessionId { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual DateTime? DTServerLastModified { get; set; }
	}
}
