using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public class FundsFee : FeeModel
	{
		public virtual int FundsType { get; set; }
		public virtual decimal Fee { get; set; }
	}
}
