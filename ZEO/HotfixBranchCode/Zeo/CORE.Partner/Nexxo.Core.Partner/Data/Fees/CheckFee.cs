using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public class CheckFee : FeeModel
	{
		public virtual int CheckType { get; set; }
		public virtual decimal FeeRate { get; set; }
		public virtual decimal FeeMinimum { get; set; }
	}
}
