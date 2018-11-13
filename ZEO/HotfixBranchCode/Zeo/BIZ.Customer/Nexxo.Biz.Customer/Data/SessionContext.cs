using System;

namespace MGI.Biz.Customer.Data
{
	public class SessionContext
	{
		public SessionContext() { }
		public Guid AgentSessionId { get; set; }
		public int AgentId { get; set; }
		public string AgentName { get; set; }
		public int LocationAgentId { get; set; }
		public Guid LocationId { get; set; }
        public string TimezoneId { get; set; }
		public int ChannelPartnerId { get; set; }
		public string AppName { get; set; }
	}
}
