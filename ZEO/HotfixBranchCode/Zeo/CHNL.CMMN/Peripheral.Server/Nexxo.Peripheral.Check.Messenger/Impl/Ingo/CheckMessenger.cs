using System.Collections.Generic;
using System.Linq;
using ChexarIO.Communication.Data;
using MGI.Peripheral.Check.Messenger.Contract;
using MGI.Peripheral.Check.Messenger.Data;
using ChexarIO.Communication.Impl;
using ChatMessage = MGI.Peripheral.Check.Messenger.Data.ChatMessage;
using MessageTypes = ChexarIO.Communication.Data.MessageTypes;

namespace MGI.Peripheral.Check.Messenger.Ingo.Impl
{
    public class CheckMessenger : ICheckMessenger
    {
        public IList<Data.ChatMessage> ChatMessages { get; set; }

        public HashSet<int> MessageIds { get; set; }

        public CheckResult CheckForMessages(CheckLogin login, int ticketNo)
        {
            var chexarLogin = ChexarLoginRequestMapper(login);

            var messengerGateway = new ChexarMessenger();

            ChexarResult chexarResult = messengerGateway.CheckForMessage(chexarLogin, ticketNo);

            ResponseMessageMapper(messengerGateway.ChatMessages);

            return ResponseResultMapper(chexarResult);
        }

        public CheckResult ComposeMessage(CheckLogin login, int ticketNo, string message)
        {
            ChexarLogin chexarLogin = ChexarLoginRequestMapper(login);

            var messengerGateway = new ChexarMessenger();

            ChexarResult chexarResult = messengerGateway.ComposeMessage(chexarLogin, ticketNo, message);

            return ResponseResultMapper(chexarResult);
        }

        public CheckResult ConfirmAllMessages(CheckLogin login, int ticketNo, int lastMessageId)
        {
            ChexarLogin chexarLogin = ChexarLoginRequestMapper(login);

            var messengerGateway = new ChexarMessenger();

            ChexarResult chexarResult = messengerGateway.ConfirmAllMessages(chexarLogin, ticketNo, lastMessageId);

            MessageIds = messengerGateway.MessageIds;

            return ResponseResultMapper(chexarResult);

        }

        public CheckResult ConfirmMessage(CheckLogin login, int messageId)
        {
            ChexarLogin chexarLogin = ChexarLoginRequestMapper(login);

            var messengerGateway = new ChexarMessenger();

            ChexarResult chexarResult = messengerGateway.ConfirmMessage(chexarLogin, messageId);

            return ResponseResultMapper(chexarResult);
        }

        private void ResponseMessageMapper(IEnumerable<ChexarIO.Communication.Data.ChatMessage> list)
        {
            ChatMessages = list.Select(chat => new ChatMessage
                                           {
                                               MessageType = (chat.MessageType == MessageTypes.Incoming) ? Data.MessageTypes.Incoming : Data.MessageTypes.OutGoing, 
                                               IsRead = chat.IsRead, 
                                               UserName = chat.UserName, 
                                               MessageId = chat.MessageId,
                                               Date = chat.Datetime,
                                               Text =  chat.Text
                                           }).ToList();
        }

        private CheckResult ResponseResultMapper(ChexarResult chexarResult)
        {
            var checkResult = new CheckResult
            {
                ErrorCode = chexarResult.ErrorCode,
                ErrorDescription = chexarResult.ErrorDescription,
                ErrorStatus = chexarResult.ErrorStatus
            };
            return checkResult;
        }

        private ChexarLogin ChexarLoginRequestMapper(CheckLogin login)
        {
            var chexarLogin = new ChexarLogin
                                  {     CompanyToken = login.CompanyToken, 
                                        EmployeeId = login.EmployeeId, 
                                        URL = login.URL
                                  };
            return chexarLogin;
        }

    }
}
