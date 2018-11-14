using System;
using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
	[DataContract]
	public class NpsTerminal
	{
		[DataMember]
		public virtual long NpsTerminalId { get; set; }
		[DataMember]
		public virtual string Name { get; set; }
		[DataMember]
		public virtual string Status { get; set; }
		[DataMember]
		public virtual string Description { get; set; }
		[DataMember]
		public virtual string IpAddress { get; set; }
		[DataMember]
		public string PeripheralServiceUrl { get; set; }
		[DataMember]
		public virtual string Port { get; set; }
		[DataMember]
		public virtual long LocationId { get; set; }
        [DataMember]
        public virtual long ChannelPartnerId { get; set; }
		
	}
}
