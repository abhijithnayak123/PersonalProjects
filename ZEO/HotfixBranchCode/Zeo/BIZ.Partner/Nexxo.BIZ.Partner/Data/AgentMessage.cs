using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Biz.Partner.Data.Transactions;

namespace MGI.Biz.Partner.Data
{
	public class AgentMessage
	{
		public string CustomerFirstName { get; set; }
		public string CustomerLastName { get; set; }
		public string Amount { get; set; }
		public string TransactionState { get; set; }
		public string TicketNumber { get; set; }
		public long	TransactionId { get; set; }
	
		public  bool IsParked { get; set; }
		public  bool IsActive { get; set; }
		public  int AgentId { get; set; }
		public  Check Transaction { get; set; }
		public string TimeZone { get; set; }
        public string DeclineMessage { get; set; }
	}
}
