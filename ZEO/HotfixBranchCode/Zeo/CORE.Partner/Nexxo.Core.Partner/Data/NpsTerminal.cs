using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
	public class NpsTerminal : NexxoModel
	{
		public virtual string Name { get; set; }
		public virtual string Status { get; set; }
		public virtual string Description { get; set; }
		public virtual string IpAddress { get; set; }
		public virtual string Port { get; set; }
		public virtual string PeripheralServiceUrl { get; set; }
		public virtual Location Location { get; set; }
        public virtual ChannelPartner ChannelPartner { get; set; }
	}
}
