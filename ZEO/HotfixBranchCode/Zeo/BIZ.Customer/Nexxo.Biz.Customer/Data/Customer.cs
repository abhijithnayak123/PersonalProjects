using System;
using System.Linq;
using System.Collections.Generic;

namespace MGI.Biz.Customer.Data
{
	public class Customer
	{
		public CustomerProfile Profile { get; set; }
		public Identification ID { get; set; }
		public EmploymentDetails Employment { get; set; }
		public List<string> Groups { get; set; }
	}
}

