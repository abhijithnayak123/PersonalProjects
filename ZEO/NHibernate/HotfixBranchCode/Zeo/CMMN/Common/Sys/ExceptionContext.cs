namespace MGI.Common.Sys
{
	public class NexxoExceptionContext
	{
		public string AgentSessionId;
		public string CustomerSessionId;

		public NexxoExceptionContext( string agentSessionId, string customerSessionId )
		{
			this.AgentSessionId = agentSessionId;
			this.CustomerSessionId = customerSessionId;
		}
	}
}
