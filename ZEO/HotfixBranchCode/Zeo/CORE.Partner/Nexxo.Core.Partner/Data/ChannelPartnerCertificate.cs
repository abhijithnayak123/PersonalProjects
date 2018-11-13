using System;

namespace MGI.Core.Partner.Data
{
	public class ChannelPartnerCertificate
	{
		public virtual Guid ChannelPartnerCertificatePK { get; set; }
		public virtual long ChannelPartnerCertificateId { get; set; }
		public virtual ChannelPartner ChannelPartner { get; set; }
		public virtual string Issuer { get; set; }
		public virtual string ThumbPrint { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual Nullable<System.DateTime> DTServerLastModified { get; set; }
	}
}
