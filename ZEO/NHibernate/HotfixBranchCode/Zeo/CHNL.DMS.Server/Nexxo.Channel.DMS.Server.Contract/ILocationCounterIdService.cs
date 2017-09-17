using System.Collections.Generic;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface ILocationCounterIdService
	{

		/// <summary>
		/// This method is to update the counter id
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier of customer session</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Updated status of counter id</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		bool UpdateCounterId(long customerSessionId, MGIContext mgiContext);

	}
}
