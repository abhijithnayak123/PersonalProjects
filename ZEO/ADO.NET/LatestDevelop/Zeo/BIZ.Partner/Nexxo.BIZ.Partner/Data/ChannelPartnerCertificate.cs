using System;

namespace MGI.Biz.Partner.Data
{
	public class ChannelPartnerCertificate
	{
		public Guid ChannelPartnerCertificatePK { get; set; }
		public long ChannelPartnerCertificateId { get; set; }

		public ChannelPartner ChannelPartner { get; set; }
		public string Issuer { get; set; }
		public string ThumbPrint { get; set; }
	}
}
