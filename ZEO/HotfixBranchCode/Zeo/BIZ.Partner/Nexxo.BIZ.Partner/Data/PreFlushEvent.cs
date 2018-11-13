using MGI.Biz.Events.Contract;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CXNCustomerTransactionDetails = MGI.Cxn.Partner.TCF.Data.CustomerTransactionDetails;

namespace MGI.Biz.Partner.Data
{
	public class PreFlushEvent : NexxoBizEvent
	{
		private static readonly string NAME = "PreFlush-ShoppingCart";

		public PreFlushEvent() : base(NAME) { }

		public CXNCustomerTransactionDetails CustomerTransactionDetails { get; set; }

		public MGIContext mgiContext { get; set; }
	}
}
