using System.Collections.Generic;
using MGI.Biz.Events.Contract;
using MGI.Common.Util;

namespace MGI.Biz.Customer.Data
{
	public class CustomerRegistrationEvent : NexxoBizEvent
	{
		private static readonly string NAME = "Customer-Registration";

		public CustomerRegistrationEvent() : base(NAME) { }
		public CustomerProfile profile { get; set; }
		public MGIContext mgiContext { get; set; }
	}
}
