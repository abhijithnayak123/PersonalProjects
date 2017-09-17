using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Common.Data
{
	public class TransactionFee
	{
		public decimal BaseFee { get; set; }
		public decimal DiscountApplied { get; set; }
		public decimal NetFee { get; set; }
		public string DiscountName { get; set; }
		public string DiscountDescription { get; set; }
		public string IsSystemApplied { get; set; }
	}
}
