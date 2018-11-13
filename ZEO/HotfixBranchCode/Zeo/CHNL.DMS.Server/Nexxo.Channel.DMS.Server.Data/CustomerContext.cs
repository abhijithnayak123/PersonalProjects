using System;

namespace MGI.Channel.DMS.Server.Data
{
	public class CustomerContext : SessionContext
	{
		private SessionContext context;
		private long alloyId;

		public CustomerContext(long alloyId, SessionContext session)
		{
			this.alloyId = alloyId;
			context = session;
		}

		public long AlloyID { get { return alloyId; } }
		public new int AgentId { get { return context.AgentId; } }
		public new string AgentName { get { return context.AgentName; } }
		public new int LocationAgentId { get { return context.LocationAgentId; } }
		public new Guid LocationId { get { return context.LocationId; } }
		public new int ChannelPartnerId { get { return context.ChannelPartnerId; } }
		public new string AppName { get { return context.AppName; } }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "AlloyID = ", AlloyID.ToString().Substring(0, 6) + "XXXXXX" + AlloyID.ToString().Substring(AlloyID.ToString().Length - 4, 4), "\r\n");
			str = string.Concat(str, "AgentId = ", AgentId, "\r\n");
			str = string.Concat(str, "AgentName = ", AgentName, "\r\n");
			str = string.Concat(str, "LocationAgentId = ", LocationAgentId, "\r\n");
			str = string.Concat(str, "LocationId = ", LocationId, "\r\n");
			str = string.Concat(str, "ChannelPartnerId = ", ChannelPartnerId, "\r\n");
			str = string.Concat(str, "AppName = ", AppName, "\r\n");
			return str;
		}
	}
}
