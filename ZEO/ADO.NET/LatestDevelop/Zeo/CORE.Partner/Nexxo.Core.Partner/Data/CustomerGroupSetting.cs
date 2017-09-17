using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class CustomerGroupSetting
	{
		public virtual Guid rowguid { get; set; }
		public virtual Customer customer { get; set; }
		public virtual ChannelPartnerGroup channelPartnerGroup { get; set; }
		public virtual int channelPartnerGroupId { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual DateTime? DTServerLastModified { get; set; }

		public CustomerGroupSetting() { }

		public CustomerGroupSetting(ChannelPartnerGroup g)
		{
			DTServerCreate = DateTime.Now;
			channelPartnerGroup = g;
            channelPartnerGroupId = g.Id;
		}

		public virtual DateTime DTTerminalCreate { get; set; }
		public virtual DateTime? DTTerminalLastModified { get; set; }
	}
}
