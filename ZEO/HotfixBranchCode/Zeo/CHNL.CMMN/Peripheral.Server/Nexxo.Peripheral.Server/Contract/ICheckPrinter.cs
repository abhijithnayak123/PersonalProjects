using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using MGI.Peripheral.Server.Data;

namespace MGI.Peripheral.Server.Contract
{
    [ServiceContract]
    public interface ICheckPrinter
    {
        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/PrintCheckStream?printparams={printparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        PrintResponse PrintCheckStream(String printparams);

        //[OperationContract]
        //[FaultContract(typeof(FaultInfo))]
        //[WebInvoke(Method = "GET", UriTemplate = "/PrinterDiagnostics", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        //DiagnosticsResponse PrinterDiagnostics();

        //[OperationContract]
        //[FaultContract(typeof(FaultInfo))]
        //[WebInvoke(Method = "GET", UriTemplate = "/PrintStream?printparams={printparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        //PrintResponse PrintStream(String printparams);
    }
}
