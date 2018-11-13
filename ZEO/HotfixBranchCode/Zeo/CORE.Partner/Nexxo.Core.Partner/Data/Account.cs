using System;
using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
	public class Account : NexxoModel
	{
		public Account() { }

		public Account( int ProviderId, long CXEId, long CXNId )
		{
			this.Id = CXEId;
			this.CXEId = CXEId;
			this.CXNId = CXNId;
			this.ProviderId = ProviderId;
			this.DTServerCreate = DateTime.Now;
		}

		public virtual Customer Customer { get; set; }
		public virtual long CXEId { get; set; }
		public virtual long CXNId { get; set; }
		public virtual int ProviderId { get; set; }
	}
}
