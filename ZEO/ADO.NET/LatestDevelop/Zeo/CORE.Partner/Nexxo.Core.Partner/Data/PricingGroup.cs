using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class PricingGroup : NexxoModel
	{
		public virtual string Name { get; set; }

		public virtual ChannelPartnerPricing ChannelPartnerPricing { get; set; }

		public virtual IList<ChannelPartnerPricing> ChannelPartnerPricings { get; set; }
		
		public virtual IList<Pricing> Pricings { get; set; }
	}
}
