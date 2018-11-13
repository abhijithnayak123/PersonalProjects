using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.BillPay.Data
{
	public class Customer
	{
		public string AccountNumber { get; set; }

		public long AlloyID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string City { get; set; }

		public string PhoneNumber { get; set; }

		public string State { get; set; }

		public string Zip { get; set; }

		public DateTime? DateOfBirth { get; set; }
	}
}
