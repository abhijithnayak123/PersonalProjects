using System.Collections.Generic;
using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface IMessageCenterService
	{

		/// <summary>
		/// This method is to get the collection of agent messages
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Collection of agent messages</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<AgentMessage> GetAgentMessages(long agentSessionId, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the agent message by transaction id
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="transactionId">This is transaction id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Agent message</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		AgentMessage GetMessageDetails(long agentSessionId, long transactionId, MGIContext mgiContext);

	}
}

