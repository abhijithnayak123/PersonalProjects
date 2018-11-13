using System;

namespace MGI.Core.Partner.Data
{
	public abstract class BaseGovernmentId
	{
		//public virtual int IdentificationTypeId { get; set; }
		public virtual string Identification { get; set; }
        public virtual Nullable<DateTime> IssueDate { get; set; }
        public virtual Nullable<DateTime> ExpirationDate { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual Nullable<DateTime> DTServerLastModified { get; set; }
		public virtual NexxoIdType IdType { get; set; }
		public virtual DateTime DTTerminalCreate { get; set; }
		public virtual DateTime? DTTerminalLastModified { get; set; }
	}
}
