using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Contract
{
	[ServiceContract]
	public interface ILocationProcessorCredentialsService
	{
        [OperationContract]
        Response SaveLocationProcessorCredential(LocationProcessorCredentials processorCredentials, ZeoContext context);

        /// <summary>
        /// This method is to get the collection of location processor credentials by location id
        /// </summary>
        /// <param name="locationId">This is location id</param>
        /// <returns>Collection of location processor credentials</returns>
        [OperationContract]
		Response GetLocationProcessorCredentials(long locationId,ZeoContext context);
    }
}
