using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
	public class Terminal : NexxoModel
	{
		public virtual string Name { get; set; }
		public virtual string IpAddress { get; set; }
		public virtual string MacAddress { get; set; }
		public virtual Location Location { get; set; }
		public virtual ChannelPartner ChannelPartner { get; set; }
		public virtual NpsTerminal PeripheralServer { get; set; }
	}
}
