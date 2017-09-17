using System;
using System.ServiceModel;
using TCF.Zeo.Common.Util;
using System.Collections.Generic;
using TCF.Channel.Zeo.Data;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Contract
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
		[OperationContract(Name = "GetTerminalById")]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetTerminalById(long terminalId, ZeoContext context);

        /// <summary>
        /// This method to search terminal based on the terminal name.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="terminalName">This parameter shows the terminal name.</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel Partner</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Terminal Details</returns>
		[OperationContract(Name = "GetTerminalByName")]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetTerminalByName(string terminalName, ZeoContext context);

        /// <summary>
        /// This method to add the terminal details.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="terminal">This parameter contains the all terminal details</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>The terminal create status</returns>
		[OperationContract(Name = "CreateTerminal")]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response CreateTerminal(Terminal terminal, ZeoContext context);

        /// <summary>
        /// This method to Update the terminal details
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="terminal">This parameter contains the all terminal details</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>The terminal update status</returns>
		[OperationContract(Name = "UpdateTerminal")]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response UpdateTerminal(Terminal terminal, ZeoContext context);

        [OperationContract(Name = "GetNpsdiagnosticInfo")]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetNpsdiagnosticInfo(long terminalId, ZeoContext context);
    }
}
