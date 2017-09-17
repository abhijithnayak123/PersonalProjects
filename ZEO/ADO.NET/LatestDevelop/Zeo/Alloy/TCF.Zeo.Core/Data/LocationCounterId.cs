using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Data
{
	public class LocationCounterId : ZeoModel
	{
		public virtual long LocationId { get; set; }

		public virtual int ProviderId { get; set; }

		public virtual string CounterId { get; set; }

		public virtual bool IsAvailable { get; set; }
	}
}
