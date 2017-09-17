using TCF.Channel.Zeo.Data;
using System.ServiceModel;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IZeoContext
    {
        /// <summary>
        /// This method is used to return the AlloyContext when the agent session is initiated.
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetZeoContextForAgent(long agentSessionId,ZeoContext context);

        /// <summary>
        /// This method is used to return the AlloyContext when the customer session is initiated.
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetZeoContextForCustomer(long customerSessionId,ZeoContext context);
    }
}
