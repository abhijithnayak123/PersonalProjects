using System;
using System.Collections.Generic;
using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface INpsTerminalService
	{
		/// <summary>
		/// This method is to create the peripheral service terminal
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session </param>
		/// <param name="psTerminal">This is peripheral service terminal details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Created status of peripheral service terminal</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response CreateNpsTerminal(long agentSessionId, NpsTerminal psTerminal, MGIContext mgiContext);

		/// <summary>
		/// This method is to update the peripheral service terminal
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="psTerminal">This is peripheral service terminal details to be updated</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Updated status of peripheral service terminal</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response UpdateNpsTerminal(long agentSessionId, NpsTerminal psTerminal, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the peripheral service terminal details by id
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session </param>
		/// <param name="Id">This is unique identifier of peripheral service terminal</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Peripheral service terminal details</returns>
		[OperationContract(Name = "LookupNpsTerminalById")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response LookupNpsTerminal(long agentSessionId, long Id, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the peripheral service terminal details by system IP address
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="ipAddress">This is system IP address</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Peripheral service terminal details</returns>
		[OperationContract(Name = "LookupNpsTerminalByIpAddress")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response LookupNpsTerminal(long agentSessionId, string ipAddress, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the peripheral service terminal details by terminal name and channel partner
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier agent session id</param>
		/// <param name="name">This is terminal name</param>
		/// <param name="channelPartner">This is channel partner details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Peripheral service terminal details</returns>
		[OperationContract(Name = "LookupNpsTerminalByName")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response LookupNpsTerminal(long agentSessionId, string name, ChannelPartner channelPartner, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the collection of peripheral service terminal details by location id
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session </param>
		/// <param name="locationId">This is location id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Collection of peripheral service terminal details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response LookupNpsTerminalByLocationID(long agentSessionId, long locationId, MGIContext mgiContext);

	}
}
