using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	public class CashierDetails
	{
		public int AgentId { get; set; }
		public string AgentFirstName { get; set; }
		public string AgentLastName { get; set; }
		public bool IsAgentLocationState { get; set; }
		public string AgentLocationState { get; set; }

		public override string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "AgentId = ", AgentId, "\r\n");
			str = string.Concat(str, "AgentFirstName=", AgentFirstName, "\r\n");
			str = string.Concat(str, "AgentLastName=", AgentLastName, "\r\n");
			str = string.Concat(str, "AgentLastName=", IsAgentLocationState, "\r\n");
			str = string.Concat(str, "AgentLocationState=", AgentLocationState, "\r\n");
			return str;
		}		
	}
}
