using System;

namespace MGI.Common.DataAccess.Data
{
	public abstract class NexxoModel
	{
		public virtual Guid rowguid { get; set; }
		public virtual long Id { get; set; }
		public virtual DateTime DTTerminalCreate { get; set; }
		public virtual Nullable<DateTime> DTTerminalLastModified { get; set; }
        public virtual DateTime DTServerCreate { get; set; }
        public virtual Nullable<DateTime> DTServerLastModified { get; set; }
	}
}