using System;

namespace MGI.Core.Partner.Data
{
	public class ProspectGovernmentId : BaseGovernmentId
	{
		public virtual Prospect Prospect { get; set; }
		public virtual Guid Id { get; set; }
	}
}
