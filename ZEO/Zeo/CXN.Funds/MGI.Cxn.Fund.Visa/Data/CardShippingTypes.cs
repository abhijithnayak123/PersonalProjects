using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class CardShippingTypes : NexxoModel
	{
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
		public virtual bool Active { get; set; }
		//public virtual IList<ChannelPartnerShippingTypeMapping> ChannelPartnerShippingTypeMapping { get; set; }
	}
}
