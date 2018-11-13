using System;
using System.Collections.Generic;

namespace MGI.Biz.BillPay.Data
{
	public class BillPayTransaction
	{
		public long Id { get; set; }
        public string BillerName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public BillPayAccount Account { get; set; }
        //public DateTime DTCreate { get; set; }
        //Provider specific data
        public Dictionary<string, object> MetaData { get; set; }
	}
}
