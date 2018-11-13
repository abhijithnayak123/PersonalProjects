using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.BillPay.Data
{
	public class BillPayLocation
	{
		public long TransactionId { get; set; }
		public List<Location> Location { set; get; }
	}
}
