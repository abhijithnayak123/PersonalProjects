using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface ILocationProcessorCredentialsService
	{

		/// <summary>
		/// This method is to get the collection of location processor credentials by location id
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="locationId">This is location id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Collection of location processor credentials</returns>
		[OperationContract]
		IList<ProcessorCredential> GetLocationProcessorCredentials(long agentSessionId, long locationId, MGIContext mgiContext);

		/// <summary>
		/// This method is to save the location processor credential
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier of agent session</param>
		/// <param name="locationId">This is location id</param>
		/// <param name="processorCredentials">This is processor credential details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Saved status of processor credential</returns>
		[OperationContract(Name = "SaveProcessorCredentials")]
		bool Save(long agentSessionId, long locationId, ProcessorCredential processorCredentials, MGIContext mgiContext);
	}
}
