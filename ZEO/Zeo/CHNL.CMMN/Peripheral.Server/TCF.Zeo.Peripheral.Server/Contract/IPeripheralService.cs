using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using TCF.Zeo.Peripheral.Server.Data;


namespace TCF.Zeo.Peripheral.Server.Contract
{
    [ServiceContract]
    public interface IPeripheralService : ICheckScanner, IPrinter, ICheckPrinter, ICheckFranking, ICheckMessenger, IEpsonDiagnostics
    {
        // some other methods for generic consumption e.g. startup, shutdown device etc. 
        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/GetHostName?localNps={localNps}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        HostNameResponse GetHostName(bool localNps);

        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/SetRedirectHost?redirectHost={redirectHost}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        RedirectResponse SetRedirectHost(string redirectHost);

        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/GetRedirectHost", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        RedirectResponse GetRedirectHost();
    }
}
