using MGI.Common.DataAccess.Data;
using MGI.Core.Partner.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class ChannelPartnerPricing : NexxoModel
	{
		public virtual int ProductType { get; set; }

		public virtual PricingGroup PricingGroup { get; set; }

		public virtual ChannelPartner ChannelPartner { get; set; }

		public virtual Location Location { get; set; }

		public virtual Product Product { get; set; }
	}
}
