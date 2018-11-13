using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public class MoneyOrderTransaction
	{
		public decimal Amount { get; set; }
		public decimal Fee { get; set; }
		public string CheckNumber { get; set; }
		public int ProviderId { get; set; }
		public decimal DiscountApplied { get; set; }
		public string DiscountName { get; set; }
		public string DiscountDescription { get; set; }
		public decimal BaseFee { get; set; }
	}
}
