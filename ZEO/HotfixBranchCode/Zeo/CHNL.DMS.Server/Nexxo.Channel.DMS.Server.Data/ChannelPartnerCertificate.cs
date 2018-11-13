using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class ChannelPartnerCertificate
	{
		[DataMember]
		public Guid ChannelPartnerCertificatePK { get; set; }
		[DataMember]
		public long ChannelPartnerCertificateId { get; set; }

		[DataMember]
		public ChannelPartner ChannelPartner { get; set; }
		[DataMember]
		public string Issuer { get; set; }
		[DataMember]
		public string ThumbPrint { get; set; }

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("ChannelPartnerCertificatePK = {0}", ChannelPartnerCertificatePK);
			builder.AppendFormat("ChannelPartnerCertificateId = {0}", ChannelPartnerCertificateId);
			builder.AppendFormat("ChannelPartner = {0}", ChannelPartner.Name);
			builder.AppendFormat("Issuer = {0}", Issuer);
			builder.AppendFormat("ThumbPrint = {0}", ThumbPrint);

			return builder.ToString();
		}
	}
}
