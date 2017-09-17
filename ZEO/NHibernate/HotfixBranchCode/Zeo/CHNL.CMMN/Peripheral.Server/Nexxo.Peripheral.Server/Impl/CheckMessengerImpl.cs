using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using MGI.Peripheral.Check.Messenger.Data;
using MGI.Peripheral.Server.Contract;
using MGI.Peripheral.Server.Data;
using System.Diagnostics;
using Spring.Context.Support;
using Spring.Context;
using ChatMessage = MGI.Peripheral.Server.Data.ChatMessage;
using IPeripheralMessenger = MGI.Peripheral.Check.Messenger.Contract;
using IPeripheralMessengerData = MGI.Peripheral.Check.Messenger.Data;

namespace MGI.Peripheral.Server.Impl
{
    public partial class PeripheralServiceImpl : ICheckMessenger
    {
        #region Public Methods

        public void InitializeChannelPartner(long channelPartnerId, string serviceUrl)
        {
            Trace.WriteLine("CheckMessengerImpl:InitializeChannelPartner() Invoked", DateTime.Now.ToString());

            Config.Update(channelPartnerId, serviceUrl);

            Trace.WriteLine("CheckMessengerImpl:InitializeChannelPartner() Completed", DateTime.Now.ToString());
        }

        public IList<CheckStatusResponse> CheckMessageStatus(long channelPartnerId, string tokenNo, int employeeId, string ticketNos)
        {
            Trace.WriteLine("CheckMessengerImpl:CheckMessageStatus() Initiated", DateTime.Now.ToString());

            IPeripheralMessenger.ICheckMessenger messenger = GetMessengerObject();

            IList<CheckStatusResponse> response = new List<CheckStatusResponse>();

            CheckStatusResponse checkStatus;

            Trace.WriteLine("CheckMessengerImpl:CheckMessageStatus() CheckList.Update started", DateTime.Now.ToString());

            CheckList.Update(channelPartnerId, tokenNo, employeeId, ticketNos);

            Trace.WriteLine("CheckMessengerImpl:CheckMessageStatus() CheckList.Update completed", DateTime.Now.ToString());

            foreach (PendingCheck check in CheckList.PendingChecks)
            {
                checkStatus = new CheckStatusResponse();

                checkStatus.TicketNo = check.TicketNo;

                if (IsNewMessageExists(messenger, check))
                {
                    checkStatus.HasNewMessage = true;
                }
                else
                {
                    checkStatus.HasNewMessage = false;
                }
                response.Add(checkStatus);
            }
            Trace.WriteLine("CheckMessengerImpl:CheckMessageStatus() completed", DateTime.Now.ToString());

            return response;
        }
                
        public IList<ChatMessage> CheckForMessages(int ticketNo)
        {
            Trace.WriteLine("CheckMessengerImpl:CheckForMessages() Initiated", DateTime.Now.ToString());

            IPeripheralMessenger.ICheckMessenger messenger = GetMessengerObject();

            IPeripheralMessengerData.CheckLogin checkLogin = GetCheckLogin(ticketNo);

            var checkResult = messenger.CheckForMessages(checkLogin, ticketNo);

            ThrowIfError(checkResult);

            Trace.WriteLine("CheckMessengerImpl:CheckForMessages() Completed", DateTime.Now.ToString());

            return ResponseMapper(messenger.ChatMessages);
        }

        public bool ComposeMessage(int ticketNo, string message)
        {
            Trace.WriteLine("CheckMessengerImpl:ComposeMessage() Initiated", DateTime.Now.ToString());

            IPeripheralMessenger.ICheckMessenger messenger = GetMessengerObject();

            IPeripheralMessengerData.CheckLogin checkLogin = GetCheckLogin(ticketNo);

            var checkResult = messenger.ComposeMessage(checkLogin, ticketNo, message);

            ThrowIfError(checkResult);

            Trace.WriteLine("CheckMessengerImpl:ComposeMessage() Completed", DateTime.Now.ToString());

            return true;
        }

        public HashSet<int> ConfirmAllMessages(int ticketNo, int lastMessageId)
        {
            Trace.WriteLine("CheckMessengerImpl:ConfirmAllMessages() Initiated", DateTime.Now.ToString());

            IPeripheralMessenger.ICheckMessenger messenger = GetMessengerObject();

            IPeripheralMessengerData.CheckLogin checkLogin = GetCheckLogin(ticketNo);

            var checkResult = messenger.ConfirmAllMessages(checkLogin, ticketNo, lastMessageId);

            ThrowIfError(checkResult);

            Trace.WriteLine("CheckMessengerImpl:ConfirmAllMessages() Completed", DateTime.Now.ToString());

            return messenger.MessageIds;
        }
        
        #endregion

        #region Private Methods

        private void ThrowIfError(IPeripheralMessengerData.CheckResult checkResult)
        {
            var errObject = new FaultInfo();

            if (checkResult.ErrorStatus)
            {
                Trace.WriteLine("Operation returned with Error Code : " + checkResult.ErrorCode + "," + checkResult.ErrorDescription, DateTime.Now.ToString());
                errObject.ErrorNo = checkResult.ErrorCode;
                errObject.ErrorMessage = checkResult.ErrorDescription;
                errObject.ErrorDetails = checkResult.ErrorDescription;
                throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
            }
        }
        
        private IPeripheralMessengerData.CheckLogin GetCheckLogin(int ticketNo)
        {
            var pendingCheck = CheckList.GetCheck(ticketNo);

            var checkLogin = new IPeripheralMessengerData.CheckLogin
                                 {
                                     CompanyToken = pendingCheck.TokenNo,
                                     EmployeeId = pendingCheck.EmployeeId,
                                     URL = Config.GetServiceUrl(pendingCheck.ChannelPartnerId)
                                 };


            return checkLogin;
        }

        private IPeripheralMessenger.ICheckMessenger GetMessengerObject()
        {
            Trace.WriteLine("Retrieving ContextRegistry:GetContext()", DateTime.Now.ToString());

            IApplicationContext ctx = ContextRegistry.GetContext();

            Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());
            Trace.WriteLine("Instantiating Ingo Chat Impl", DateTime.Now.ToString());

            IPeripheralMessenger.ICheckMessenger messenger = (IPeripheralMessenger.ICheckMessenger)ctx["IngoCheckMessenger"];

            Trace.WriteLine("Ingo Chat Impl Initiated", DateTime.Now.ToString());

            return messenger;
        }
        
        private bool IsNewMessageExists(IPeripheralMessenger.ICheckMessenger messenger, PendingCheck check)
        {
            var checkLogin = new IPeripheralMessengerData.CheckLogin
                                 {
                                     CompanyToken = check.TokenNo,
                                     EmployeeId = check.EmployeeId,
                                     URL = Config.GetServiceUrl(check.ChannelPartnerId)
                                 };

            var checkResult = messenger.CheckForMessages(checkLogin, check.TicketNo);

            ThrowIfError(checkResult);

            foreach (IPeripheralMessengerData.ChatMessage message in messenger.ChatMessages)
            {
                if (!message.IsRead)
                {
                    return true;
                }
            }
            return false;
        }

        private IList<ChatMessage> ResponseMapper(IList<IPeripheralMessengerData.ChatMessage> messages)
        {
            var chatMessages = new List<ChatMessage>();

            foreach (IPeripheralMessengerData.ChatMessage message in messages)
            {
                var chatMessage = new ChatMessage();

                //converts EST to local time
                var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var localTimeZone = TimeZoneInfo.Local;
                var localTime = TimeZoneInfo.ConvertTime(message.Date, easternTimeZone, localTimeZone);

                chatMessage.Date = localTime;
                chatMessage.IsRead = message.IsRead;
                chatMessage.MessageId = message.MessageId;
                chatMessage.Text = message.Text;
                chatMessage.UserName = message.UserName;
                chatMessage.IsIncoming = ( message.MessageType == MessageTypes.Incoming );

                chatMessages.Add(chatMessage);
            }
            return chatMessages;
        }

        #endregion

        #region Static data

        public static class Config
        {
            private static Dictionary<long, string> ChannelPartnerList = new Dictionary<long,string>();

            //private static Object thisLock = new Object();

            public static void Update(long channelPartnerId, string serviceUrl)
            {
                //lock (thisLock)
                //{
                    if (ChannelPartnerList.ContainsKey(channelPartnerId))
                    {
                        ChannelPartnerList[channelPartnerId] = serviceUrl;
                    }
                    else
                    {
                        ChannelPartnerList.Add(channelPartnerId, serviceUrl);                        
                    }
                //}
            }

            public static string GetServiceUrl(long channelPartnerId)
            {
                if (ChannelPartnerList.ContainsKey(channelPartnerId))
                {
                    return ChannelPartnerList[channelPartnerId];
                }
                else
                {
                    var errObject = new FaultInfo();

                    Trace.WriteLine("Ticket not found : Channel partner id : " + channelPartnerId.ToString(), DateTime.Now.ToString());
                    errObject.ErrorNo = -1000;
                    errObject.ErrorMessage = "Ticket not found";
                    errObject.ErrorDetails = "Ticket not found";
                    throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
                }
            }
        }

        public static class CheckList
        {
            public static List<PendingCheck> PendingChecks = new List<PendingCheck>();

            //private static Object thisLock = new Object();

            public static void Update(long channelPartnerId, string tokenNo, int employeeId, string ticketNos)
            {
                PendingCheck pendingCheck;

                //lock (thisLock)
                //{
                    PendingChecks.Clear();

                    if (!string.IsNullOrEmpty(ticketNos))
                    {
                        string[] tickets = ticketNos.Split(',');

                        foreach (string ticket in tickets)
                        {
                            pendingCheck = new PendingCheck();

                            pendingCheck.ChannelPartnerId = channelPartnerId;
                            pendingCheck.EmployeeId = employeeId;
                            pendingCheck.TokenNo = tokenNo;
                            pendingCheck.TicketNo = int.Parse(ticket); //can throw parse error

                            PendingChecks.Add(pendingCheck);
                        }
                    }
                //}
            }

            public static PendingCheck GetCheck(int ticketNo)
            {
                foreach (PendingCheck check in PendingChecks)
                {
                    if (check.TicketNo == ticketNo)
                        return check;
                }
                return new PendingCheck();
            }
        }
        #endregion
    }
}
