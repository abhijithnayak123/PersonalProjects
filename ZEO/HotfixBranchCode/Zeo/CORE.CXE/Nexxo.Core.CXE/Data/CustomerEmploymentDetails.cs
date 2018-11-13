using System;

namespace MGI.Core.CXE.Data
{
    public class CustomerEmploymentDetails : BaseEmploymentDetails
	{
		public virtual Customer Customer { get; set; }
		public virtual Guid Id { get; set; }
	}
}
