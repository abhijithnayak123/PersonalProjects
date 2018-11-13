using System;

namespace MGI.Biz.Partner.Data
{
	public class NpsTerminal
	{
		public long Id { get; set; }
		public Guid rowguid { get; set; }

		public string Name { get; set; }
		public string Status { get; set; }
		public string Description { get; set; }
		public string IpAddress { get; set; }
		public string Port { get; set; }
		public string PeripheralServiceUrl { get; set; }
		public Location Location { get; set; }
        //public DateTime DTCreate { get; set; }
        //public DateTime? DTLastMod { get; set; }
		public ChannelPartner ChannelPartner { get; set; }
		public long ChannelPartnerId { get; set; }
	}
}
