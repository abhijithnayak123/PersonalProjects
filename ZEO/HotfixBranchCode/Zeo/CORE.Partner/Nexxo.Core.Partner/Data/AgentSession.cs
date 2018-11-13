using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
	public class AgentSession : NexxoModel
	{
		public virtual string AgentId { get; set; }
		public virtual Terminal Terminal { get; set; }
		public virtual UserDetails Agent { get; set; }
		public virtual System.Nullable<System.DateTime> BusinessDate { get; set; }
        public virtual string ClientAgentIdentifier { get; set; }
	}
}
