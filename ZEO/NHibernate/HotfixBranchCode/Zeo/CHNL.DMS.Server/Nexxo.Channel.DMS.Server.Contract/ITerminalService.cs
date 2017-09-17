using System;
using MGI.Channel.DMS.Server.Data;
using System.ServiceModel;
using MGI.Common.Sys;
using System.Collections.Generic;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface ITerminalService
	{
        /// <summary>
        /// This method to search terminal based on the unique id identifier.
        /// </summary>
        /// <param name="Id">This is unique identifier for Terminal</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Terminal details</returns>
		[OperationContract(Name = "LookupTerminalByGuid")]
		[FaultContract(typeof(NexxoSOAPFault))]
        Terminal LookupTerminal(long Id, MGIContext mgiContext);

        /// <summary>
        /// This method to search terminal based on the terminal name.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="terminalName">This parameter shows the terminal name.</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel Partner</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Terminal Details</returns>
		[OperationContract(Name = "LookupTerminalByChannelPartner")]
		[FaultContract(typeof(NexxoSOAPFault))]
        Terminal LookupTerminal(long agentSessionId, string terminalName, int channelPartnerId, MGIContext mgiContext);

        /// <summary>
        /// This method to add the terminal details.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="terminal">This parameter contains the all terminal details</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>The terminal create status</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        bool CreateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext);

        /// <summary>
        /// This method to Update the terminal details
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="terminal">This parameter contains the all terminal details</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>The terminal update status</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        bool UpdateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext);
	}
}
