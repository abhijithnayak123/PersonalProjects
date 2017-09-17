using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class LocationCounterId : NexxoModel
	{
		public virtual Location Location { get; set; }

		public virtual Guid LocationId { get; set; }

		public virtual int ProviderId { get; set; }

		public virtual string CounterId { get; set; }

		public virtual bool IsAvailable { get; set; }
	}
}
