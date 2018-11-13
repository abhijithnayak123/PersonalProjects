using System;

using MGI.Common.DataAccess.Data;
using MGI.Core.Partner.Data.Transactions;

namespace MGI.Core.Partner.Data
{
	public class AgentMessage : NexxoModel
	{		
		public virtual bool IsParked { get; set; }
		public virtual bool IsActive { get; set; }
		public virtual UserDetails Agent { get; set; }
		public virtual Check Transaction { get; set; }
	}
}
