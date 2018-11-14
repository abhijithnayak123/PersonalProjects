using System;

namespace MGI.Core.Partner.Data
{
	public abstract class BaseEmploymentDetails
	{
		public virtual string Occupation { get; set; }
		public virtual string OccupationDescription { get; set; }
		public virtual string Employer { get; set; }
		public virtual string EmployerPhone { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual Nullable<DateTime> DTServerLastModified { get; set; }
		public virtual DateTime DTTerminalCreate { get; set; }
		public virtual DateTime? DTTerminalLastModified { get; set; }
	}
}
