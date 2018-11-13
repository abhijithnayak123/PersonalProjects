using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class ChannelPartnerShippingTypeMapping
	{
		public virtual System.Guid ChannelPartnerShippingTypePK { get; set; }
		public virtual CardShippingTypes ShippingTypes { get; set; }
		public virtual long ChannelPartnerId { get; set; }
	}
}
