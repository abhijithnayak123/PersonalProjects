using System.Collections.Generic;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Channel.DMS.Server.Data;
using System.Net.Mail;
using System;

namespace MGI.Channel.DMS.Server.Contract
{
    [ServiceContract]
    public interface IUserService
    {
		/// <summary>
		/// This method is used to added the "Agent" details in PTNR database
		/// </summary>
		/// <param name="userInfo">Agent information details</param>
		/// <param name="mode">Mode Operation example Add or Edit</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Agent ID</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		int SaveUser(UserDetails userInfo, SaveMode mode, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the agent details based on agent Id.
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="UserId">This is unique identifier for Agent ID for agent details </param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Agent information details</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		UserDetails GetUser(long agentSessionId, int UserId, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the collection of agent details based on location id
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for agentSessionID to get the agentsession table</param>
		/// <param name="locationId">This is unique identifier for locationID based on channelpartner</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Collection of users</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<UserDetails> GetUsers(long agentSessionId, long locationId, MGIContext mgiContext);
    }
}
