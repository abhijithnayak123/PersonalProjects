using System.ServiceModel;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface ILocationService
    {

        /// <summary>
        /// This method is to get the location details location id
        /// </summary>
        /// <param name="locationId">This is location id</param>
        /// <returns>Location details</returns>
        [OperationContract]
        [FaultContract(typeof(TCF.Zeo.Common.Data.ZeoSoapFault))]
        Response GetLocationById(long locationId, ZeoContext context);

        /// <summary>
        /// This method is to create a new location
        /// </summary>
        /// <param name="manageLocation">This is location details</param>
        /// <returns>Unique identifier of location</returns>
        [OperationContract(Name = "CreateLocation")]
        [FaultContract(typeof(TCF.Zeo.Common.Data.ZeoSoapFault))]
        Response CreateLocation(Location location, ZeoContext context);

        /// <summary>
        /// This method is to update the location details
        /// </summary>
        /// <param name="manageLocation">This is location details to be updated</param>
        /// <returns>Updated status of location</returns>
        [OperationContract(Name = "UpdateLocation")]
        [FaultContract(typeof(TCF.Zeo.Common.Data.ZeoSoapFault))]
        Response UpdateLocation(Location location, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(TCF.Zeo.Common.Data.ZeoSoapFault))]
        Response ValidateLocation(Location location, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(TCF.Zeo.Common.Data.ZeoSoapFault))]
        Response GetLocationsByChannelPartnerId(long channelPartnerId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(TCF.Zeo.Common.Data.ZeoSoapFault))]
        Response GetStateNamesAndIdByChannelPartnerId(ZeoContext context);

    }
}
