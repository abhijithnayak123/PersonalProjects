using System;
using MGI.Biz.Partner.Data;
using System.Collections.Generic;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
	public interface IManageTerminal
	{
        /// <summary>
        /// This method is to fetch the terminal details by id.
        /// </summary>
        /// <param name="Id">This is unique identifier Terminal</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Terminal Details.</returns>
        Terminal Lookup(long Id, MGIContext mgiContext);

        /// <summary>
        /// This method is to get the terminal details by terminal name and channel partner id.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="terminalName">The terminal name</param>
        /// <param name="channelPartner">This is the unique identifier for channel partner</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Terminal Details.</returns>
		Terminal Lookup(long agentSessionId, string terminalName, int channelPartner, MGIContext mgiContext);

        /// <summary>
        /// This method is to create the terminal.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="terminal">A transient instance of Terminal[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Terminal Create status</returns>
        bool Create(long agentSessionId, Terminal terminal, MGIContext mgiContext);

        /// <summary>
        /// This method is to update the terminal.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="terminal">A transient instance of Terminal[Class] containing the updated state</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Terminal Update Status</returns>
        bool Update(long agentSessionId, Terminal terminal, MGIContext mgiContext);
	}
}
