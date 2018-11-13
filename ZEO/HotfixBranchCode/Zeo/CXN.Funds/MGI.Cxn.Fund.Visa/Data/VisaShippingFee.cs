using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class VisaShippingFee : NexxoModel
	{
		public VisaShippingFee()
		{
			ChannelPartnerShippingTypeMapping = new ChannelPartnerShippingTypeMapping();

		}
		public virtual double Fee { get; set; }
		public virtual int FeeCode { get; set; }
		public virtual ChannelPartnerShippingTypeMapping ChannelPartnerShippingTypeMapping { get; set; }
	}
}
