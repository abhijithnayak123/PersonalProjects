using MGI.Common.DataAccess.Data;
using MGI.Core.Partner.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class Pricing : NexxoModel
	{
		public virtual int CompareType { get; set; }

		public virtual decimal MinimumAmount { get; set; }

		public virtual decimal MaximumAmount { get; set; }

		public virtual decimal MinimumFee { get; set; }

		public virtual decimal Value { get; set; }

		public virtual bool IsPercentage { get; set; }

		public virtual PricingGroup PricingGroup { get; set; }
	}
}
