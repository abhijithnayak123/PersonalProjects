using System.Collections.Generic;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface ILocationService
	{
		/// <summary>
		/// This method is to get the location details by agent session id, location name and context
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="locationName">This is location name</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Location details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Location GetByName(long agentSessionId, string locationName, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the collection of all locations
		/// </summary>
		/// <returns>Collection  of  locations</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<Location> GetAll();

		/// <summary>
		/// This method is to create a new location
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="manageLocation">This is location details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Unique identifier of location</returns>
		[OperationContract(Name = "CreateLocation")]
		[FaultContract(typeof(NexxoSOAPFault))]
		long Create(long agentSessionId, Location manageLocation, MGIContext mgiContext);

		/// <summary>
		/// This method is to update the location details
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="manageLocation">This is location details to be updated</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Updated status of location</returns>
		[OperationContract(Name = "UpdateLocation")]
		[FaultContract(typeof(NexxoSOAPFault))]
		bool Update(long agentSessionId, Location manageLocation, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the location by location id
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="locationId">This is location id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Location details</returns>
		[OperationContract(Name = "LookupLocationById")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Location Lookup(string agentSessionId, long locationId, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the collection of locations by channel partner id
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session </param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Collection of locations</returns>
		[OperationContract(Name = "GetAllLocationByChannelPartnerId")]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<Location> GetAll(long agentSessionId, MGIContext mgiContext);
	}
}
