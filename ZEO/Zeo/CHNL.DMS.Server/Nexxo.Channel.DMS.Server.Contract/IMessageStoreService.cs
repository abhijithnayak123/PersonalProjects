using MGI.Channel.DMS.Server.Data;
using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface IMessageStoreService
	{
		/// <summary>
		/// To retrieve error message from Database
		/// </summary>
		/// <param name="messageKey">Error Code</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Error Message</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		Response GetMessage(long agentSessionId, string messageKey, MGIContext mgiContext);
	}
}
