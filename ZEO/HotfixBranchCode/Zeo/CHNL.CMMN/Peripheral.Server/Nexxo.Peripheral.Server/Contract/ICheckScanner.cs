using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using MGI.Peripheral.Server.Data;

namespace MGI.Peripheral.Server.Contract
{
    [ServiceContract]
    public interface ICheckScanner
    {
        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/ScanCheck?scanparams={scanparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        ScanCheckResponse ScanCheck(String scanparams);
        
        [OperationContract]
        [WebGet( UriTemplate = "/GetImage?id={imageId}", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetImage(String imageId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/ConvertStream?streamparams={streamparams}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        String ConvertStream(String streamparams);
    }
}
