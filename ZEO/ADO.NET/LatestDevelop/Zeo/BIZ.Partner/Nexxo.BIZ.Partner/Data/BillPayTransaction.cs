using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public class BillPayTransaction
	{
        public string BillerName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public int ProviderId { get; set; }
        public Dictionary<string, object> MetaData { get; set; } 
	}
}
