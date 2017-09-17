using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
	public class CustomerSessionCounter 
	{
		public virtual CustomerSession CustomerSession { get; set; }
		public virtual Guid CustomerSessionId { get; set; }
		public virtual string CounterId { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual Nullable<DateTime> DTServerLastModified { get; set; }
		public virtual DateTime DTTerminalCreate { get; set; }
		public virtual DateTime? DTTerminalLastModified { get; set; }
	}
}
