using CommonData = TCF.Zeo.Common.Data;
using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IMessageCenterService
    {
        /// <summary>
		/// This method is to get the collection of agent messages
		/// </summary>		
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Collection of agent messages</returns>
        ///  
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetAgentMessages(ZeoContext context);

        /// <summary>
        /// This method is to get the agent message by transaction id
        /// </summary>       
        /// <param name="transactionId">This is transaction id</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Agent message</returns>
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetMessageDetails(long transactionId, ZeoContext context);


    }
}
