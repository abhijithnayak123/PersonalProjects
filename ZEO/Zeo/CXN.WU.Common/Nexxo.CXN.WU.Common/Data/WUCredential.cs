using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.WU.Common.Data
{

	public class WUCredential : NexxoModel
	{
		public virtual string WUServiceUrl { get; set; }
		public virtual string WUClientCertificateSubjectName { get; set; }
		public virtual string AccountIdentifier { get; set; }
		public virtual string CounterId { get; set; }
		public virtual string ChannelName { get; set; }
		public virtual string ChannelVersion { get; set; }
		public virtual long ChannelPartnerId { get; set; }
	}
}