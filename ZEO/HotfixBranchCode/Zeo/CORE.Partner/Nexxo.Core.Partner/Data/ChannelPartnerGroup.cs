using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class ChannelPartnerGroup
	{
		public virtual Guid ChannelPartnerGroupPK { get; set; }
		public virtual string Name { get; set; }
		public virtual ChannelPartner channelPartner { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual DateTime? DTServerLastModified { get; set; }
		public virtual int Id { get; set; }
	}
}
