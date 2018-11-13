using MGI.Biz.Events.Contract;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Customer.Data
{
	public class CustomerSyncInEvent : NexxoBizEvent
	{
		private static readonly string NAME = "Customer-SyncIn";

		public CustomerSyncInEvent() : base(NAME) { }
		public MGIContext mgiContext { get; set; }
		public long cxnCustomerId { get; set; }
	}
}
