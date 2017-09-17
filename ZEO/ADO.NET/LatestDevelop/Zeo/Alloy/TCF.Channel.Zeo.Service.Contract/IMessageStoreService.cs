using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IMessageStoreService
    {
        /// <summary>
        /// To Retrive error messages for web layer.
        /// </summary>
        /// <param name="messageKey">Error Code</param>
        /// <param name="context">Common Paramater</param>
        /// <returns>Response object</returns>
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetMessage(string messageKey, ZeoContext context);

        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response LookUp(MessageStoreSearch search, ZeoContext context);
    }
}
