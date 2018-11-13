using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class ChannelPartnerFeeTypeMapping : NexxoModel
	{
		public ChannelPartnerFeeTypeMapping()
		{
			VisaFeeTypes = new VisaFeeTypes();			
		}
		public virtual VisaFeeTypes VisaFeeTypes { get; set; }
		public virtual VisaFee VisaFee { get; set; }
		public virtual long ChannelPartnerID { get; set; }
	}
}
