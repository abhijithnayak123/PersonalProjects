using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using TCF.Zeo.Peripheral.Server.Data;

namespace TCF.Zeo.Peripheral.Server.Contract
{
    [ServiceContract]
    public interface ICheckFranking
    {
        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/FrankCheck?frankparams={frankparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        PrintResponse FrankCheck(String frankparams);
    }
}
