using System;
using System.Text;
using System.Collections.Generic;


namespace MGI.Core.Partner.Data
{

	public class MasterCountry
	{
		public MasterCountry()
		{
			ChannelPartnerMasterCountryMappings = new List<ChannelPartnerMasterCountryMapping>();
		}
		public virtual System.Guid Rowguid { get; set; }
		public virtual long Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Abbr2 { get; set; }
		public virtual string Abbr3 { get; set; }
		public virtual System.DateTime DTServerCreate { get; set; }
		public virtual System.Nullable<System.DateTime> DTServerLastModified { get; set; }
		public virtual IList<ChannelPartnerMasterCountryMapping> ChannelPartnerMasterCountryMappings { get; set; }
	}
}
