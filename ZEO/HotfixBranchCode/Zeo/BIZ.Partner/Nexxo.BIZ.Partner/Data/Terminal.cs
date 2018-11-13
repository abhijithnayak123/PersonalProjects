using System;

namespace MGI.Biz.Partner.Data
{
	public class Terminal
	{
        public long Id { get; set; }
        public Guid rowguid { get; set; }

		public string Name { get; set; }
		public string IpAddress { get; set; }
		public string MacAddress { get; set; }
		public Location Location { get; set; }
		public NpsTerminal PeripheralServer { get; set; }
		public ChannelPartner ChannelPartner { get; set; }
        //public DateTime DTCreate { get; set; }
        //public DateTime? DTLastMod { get; set; }
    }
}
