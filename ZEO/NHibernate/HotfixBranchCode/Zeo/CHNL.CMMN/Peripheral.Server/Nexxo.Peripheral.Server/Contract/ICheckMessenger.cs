using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using MGI.Peripheral.Server.Data;

namespace MGI.Peripheral.Server.Contract
{
    [ServiceContract]
    public interface ICheckMessenger
    {
        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/InitializeChannelPartner?channelpartnerid={channelPartnerId}&serviceurl={serviceUrl}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        void InitializeChannelPartner(long channelPartnerId, string serviceUrl);

        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/CheckMessageStatus?channelpartnerid={channelPartnerId}&tokenno={tokenNo}&employeeid={employeeId}&ticketnos={ticketNos}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        IList<CheckStatusResponse> CheckMessageStatus(long channelPartnerId, string tokenNo, int employeeId, string ticketNos);

        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/CheckForMessages?ticketno={ticketNo}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        IList<ChatMessage> CheckForMessages(int ticketNo);

        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/ComposeMessage?ticketno={ticketNo}&message={message}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        bool ComposeMessage(int ticketNo, string message);

        [OperationContract]
        [FaultContract(typeof(FaultInfo))]
        [WebInvoke(Method = "GET", UriTemplate = "/ConfirmAllMessages?ticketno={ticketNo}&lastmessageid={lastMessageId}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        HashSet<int> ConfirmAllMessages(int ticketNo, int lastMessageId);
    }
}
