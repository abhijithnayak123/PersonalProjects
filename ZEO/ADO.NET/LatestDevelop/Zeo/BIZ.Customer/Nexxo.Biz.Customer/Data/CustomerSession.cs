using System;

namespace MGI.Biz.Customer.Data
{
	public class CustomerSession
	{
		public string CustomerSessionId;
		public Customer Customer;
		public bool CardPresent { get; set; }
        public string TimezoneID { get; set; }
	}
}
