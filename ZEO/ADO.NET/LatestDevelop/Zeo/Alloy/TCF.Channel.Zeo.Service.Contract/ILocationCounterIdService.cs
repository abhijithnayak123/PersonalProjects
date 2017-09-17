using System.ServiceModel;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Contract
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
		Response UpdateCounterId(ZeoContext context);

        [OperationContract]
        Response  GetLocationCounterID(long locationId,int providerId,ZeoContext context);

        [OperationContract]
        Response  CreateCustomerSessionCounterId(long productProviderId, long locationId, ZeoContext context);


    }
}
