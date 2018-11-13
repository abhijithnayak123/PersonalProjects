using System;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Check.Certegy.Data
{
	public class Credential : NexxoModel
	{
		public virtual string ServiceUrl { get; set; }
		public virtual string CertificateName { get; set; }
		public virtual string Version { get; set; }
		public virtual long ChannelPartnerId { get; set; }
		public virtual string DeviceType { get; set; }		
		public virtual string DeviceIP { get; set; }
	}
}
