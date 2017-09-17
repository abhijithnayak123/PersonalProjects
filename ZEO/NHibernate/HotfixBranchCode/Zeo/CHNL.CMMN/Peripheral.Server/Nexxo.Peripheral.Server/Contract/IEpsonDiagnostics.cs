using MGI.Peripheral.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MGI.Peripheral.Server.Contract
{
	[ServiceContract]
	public interface IEpsonDiagnostics
	{
		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/DeviceStatus", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		List<DeviceStatus> GetDeviceStatus();

		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/PDSDriverVerification", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		VerificationResponse PDSDriverVerification();

		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/SDKDriverVerification", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		VerificationResponse SDKDriverVerification();

		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/PSFileVerification", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		FileVerificationResponse PSFileVerification();

		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/ReceiptPrintTest", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		PrintResponse ReceiptPrintTest();

		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/CheckScanTest", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		ScanCheckResponse CheckScanTest();

		[OperationContract]
		[FaultContract(typeof(FaultInfo))]
		[WebInvoke(Method = "GET", UriTemplate = "/MoneyOrderPrintTest", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		PrintResponse MoneyOrderPrintTest();
	}
}
