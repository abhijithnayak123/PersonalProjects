
namespace TCF.Zeo.Core.Data
{
	public class NpsTerminal : Common.Data.ZeoModel
    {
        public virtual long NpsTerminalId { get; set; }
        public virtual string Name { get; set; }
		public virtual string Status { get; set; }
		public virtual string Description { get; set; }
		public virtual string IpAddress { get; set; }
		public virtual string Port { get; set; }
		public virtual string PeripheralServiceUrl { get; set; }
		public virtual long LocationId { get; set; }
        public virtual long ChannelPartnerId { get; set; }
	}
}
