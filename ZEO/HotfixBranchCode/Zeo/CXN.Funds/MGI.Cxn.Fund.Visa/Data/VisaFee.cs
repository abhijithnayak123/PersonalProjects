using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class VisaFee : NexxoModel
	{
		public VisaFee()
		{
			ChannelPartnerFeeTypeMapping = new ChannelPartnerFeeTypeMapping();			
		}
		public virtual double Fee { get; set; }
		public virtual int FeeCode { get; set; }
		public virtual string StockId { get; set; }
		public virtual ChannelPartnerFeeTypeMapping ChannelPartnerFeeTypeMapping { get; set; }
	}
}
