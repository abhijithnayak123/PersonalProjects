using System;

using MGI.Core.Partner.Data;
using System.Collections.Generic;

namespace MGI.Core.Partner.Contract
{
	public interface IAgentSessionService
	{
		/// <summary>
		/// Create an agent session
		/// </summary>
		/// <param name="Agent"></param>
		/// <param name="terminal"></param>
		/// <returns>AgentSession</returns>
		AgentSession Create(UserDetails Agent, Terminal terminal, MGI.Common.Util.MGIContext context);

		bool Update(AgentSession agentSession);

		/// <summary>
		/// Lookup agent session
		/// </summary>
		/// <param name="id">AgentSessionId</param>
		/// <returns></returns>
		AgentSession Lookup( long id );

	}
}
