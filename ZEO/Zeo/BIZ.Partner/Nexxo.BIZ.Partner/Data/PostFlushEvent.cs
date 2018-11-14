using MGI.Biz.Events.Contract;
using MGI.Common.Util;
using System.Collections.Generic;
using CXNCustomerTransactionDetails = MGI.Cxn.Partner.TCF.Data.CustomerTransactionDetails;


namespace MGI.Biz.Partner.Data
{
	public class PostFlushEvent : NexxoBizEvent
	{
		private static readonly string NAME = "PostFlush-ShoppingCart";

		public PostFlushEvent() : base(NAME) { }

		public CXNCustomerTransactionDetails CustomerTransactionDetails { get; set; }

		public MGIContext mgiContext;
	}
}
