using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using TCF.Zeo.Peripheral.Server.Data;

namespace TCF.Zeo.Peripheral.Server.Contract
{
	[ServiceContract]
	public interface IPrinter
	{
		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/PrintCashCheckReceipt?printparams={printparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		PrintResponse PrintCashCheckReceipt(String printparams);

		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/PrintCashDrawerReport?printparams={printparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		PrintResponse PrintCashDrawerReport(String printparams);

		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/PrinterDiagnostics?printparams={printparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		DiagnosticsResponse PrinterDiagnostics(String printparams);

        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/PrintStream?printparams={printparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PrintResponse PrintStream(String printparams);

        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/PrintDocStream?printparams={printparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PrintResponse PrintDocStream(String printparams);
    }
}
