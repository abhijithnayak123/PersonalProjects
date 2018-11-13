using System;

namespace MGI.Core.Partner.Data
{
	public class ProspectEmploymentDetails : BaseEmploymentDetails
	{
		public virtual Prospect Prospect { get; set; }
		public virtual Guid Id { get; set; }
	}
}
