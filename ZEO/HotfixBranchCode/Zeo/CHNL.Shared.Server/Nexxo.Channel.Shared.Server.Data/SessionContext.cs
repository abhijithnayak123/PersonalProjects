using System;

namespace MGI.Channel.Shared.Server.Data
{
	public class SessionContext
	{
		public int AgentId { get; set; }
		public Guid AgentSessionId { get; set; }
		public string AgentName { get; set; }
		public int LocationAgentId { get; set; }
		public Guid LocationId { get; set; }
        public string TimezoneId { get; set; }
		public int ChannelPartnerId { get; set; }
		public string AppName { get; set; }
		public string CustomerSessionId { get; set; }
		public DateTime DTKiosk { get; set; }
		public int SelectedLanguage { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "AgentId = ", AgentId, "\r\n");
			str = string.Concat(str, "AgentName = ", AgentName, "\r\n");
			str = string.Concat(str, "LocationAgentId = ", LocationAgentId, "\r\n");
            str = string.Concat(str, "TimezoneId = ", TimezoneId, "\r\n");
			str = string.Concat(str, "LocationId = ", LocationId, "\r\n");
			str = string.Concat(str, "ChannelPartnerId = ", ChannelPartnerId, "\r\n");
			str = string.Concat(str, "AppName = ", AppName, "\r\n");
			str = string.Concat(str, "CustomerSessionId = ", CustomerSessionId, "\r\n");
			str = string.Concat(str, "DTKiosk = ", DTKiosk, "\r\n");
			str = string.Concat(str, "SelectedLanguage = ", SelectedLanguage, "\r\n");
			str = string.Concat(str, "AgentSessionId = ", AgentSessionId, "\r\n");
			return str;
		}
	}
}
