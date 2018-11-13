using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChexarIO.SimEntities
{
	public class ChexarSimInvoice
	{
		public virtual Guid rowguid { get; set; }
		public virtual int Id { get; set; }
		public virtual int TicketId { get; set; }
		public virtual decimal Amount { get; set; }
		public virtual decimal Fee { get; set; }
		public virtual int CheckType { get; set; }
		public virtual string Status { get; set; }
		public virtual string WaitTime { get; set; }
		public virtual int DeclineId { get; set; }
		public virtual string DeclineReason { get; set; }
		public virtual ChexarSimAccount Account { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual DateTime? DTServerLastModified { get; set; }
	}
}
