using System;
using System.Text;
using System.Collections.Generic;
using MGI.Common.DataAccess.Data;


namespace MGI.Core.Partner.Data
{

	public class ChannelPartnerMasterCountryMapping : NexxoModel
	{
		public virtual System.Guid Rowguid { get; set; }
		public virtual ChannelPartner ChannelPartner { get; set; }
		public virtual MasterCountry MasterCountry { get; set; }
		public virtual System.Nullable<bool> IsActive { get; set; }
	}
}
