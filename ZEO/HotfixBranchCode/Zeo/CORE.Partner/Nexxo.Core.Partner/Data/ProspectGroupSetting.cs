using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class ProspectGroupSetting
	{
		public virtual Guid rowguid { get; set; }
		public virtual Prospect prospect { get; set; }
		public virtual int channelPartnerGroupId { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual DateTime? DTServerLastModified { get; set; }
		public virtual ChannelPartnerGroup ChannelPartnerGroup { get; set; }
		public virtual DateTime DTTerminalCreate { get; set; }
		public virtual DateTime? DTTerminalLastModified { get; set; }

		public ProspectGroupSetting() { }

		public ProspectGroupSetting(ChannelPartnerGroup g)
		{
            ChannelPartnerGroup = g;
            channelPartnerGroupId = g.Id;
			DTServerCreate = DateTime.Now;			
		}
	}
}
