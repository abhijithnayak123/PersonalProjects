using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using ChexarIO.Communication.Contract;
using ChexarIO.Communication.Data;
using System.IO;


namespace ChexarIO.Communication.Impl
{
    public class ChexarMessenger : IChexarMessenger
    {
        public List<ChatMessage> ChatMessages;
        public HashSet<int> MessageIds;

        public ChexarResult CheckForMessage(ChexarLogin login, int ticketNo)
        {
            var xmlDoc = new XmlDocument();

            ChatMessages = new List<ChatMessage>();

            string result = getCallCenterSvc(login.URL).CheckForMessage(login.CompanyToken, ticketNo);

            ChexarResult chexarResult = ParseError(result);

            if(chexarResult.ErrorStatus != false)
                return chexarResult;
            
            xmlDoc.Load(new StringReader(result));

            XmlNodeList messagesNodes = xmlDoc.SelectNodes("/callcenter/messages/message");

            foreach (XmlNode messagesNode in messagesNodes)
            {
                var chatMsg = new ChatMessage();

                chatMsg.MessageId = Convert.ToInt32(messagesNode["messageid"].InnerText);

                FormatChatText(chatMsg, messagesNode["text"].InnerText);

                if (messagesNode["msgread"] != null)
                {
                    chatMsg.MessageType = MessageTypes.Incoming;

                    chatMsg.IsRead = messagesNode["msgread"].InnerText == "0" ? false : true;
                }
                else
                {
                    chatMsg.IsRead = true;

                    chatMsg.MessageType = MessageTypes.OutGoing;
                }
                ChatMessages.Add(chatMsg);
            }
            
            return chexarResult;
        }

        public ChexarResult ComposeMessage(ChexarLogin login, int ticketNo, string text)
        {
            return ParseError(getCallCenterSvc(login.URL).ComposeMessage(login.CompanyToken, ticketNo, login.EmployeeId, text));
        }

        public ChexarResult ConfirmMessage(ChexarLogin login, int messageId)
        {
            return ParseError(getCallCenterSvc(login.URL).ConfirmMessage(login.CompanyToken, messageId));
        }

        public ChexarResult ConfirmAllMessages(ChexarLogin login, int ticketNo, int lastMessageId)
        {
            MessageIds = new HashSet<int>();

            var chexarResult = new ChexarResult();

            CheckForMessage(login, ticketNo);

            foreach (var chatMessage in ChatMessages)
            {
                if (chatMessage.MessageType != MessageTypes.Incoming || chatMessage.IsRead != false) continue;

                chexarResult = ConfirmMessage(login, chatMessage.MessageId);

                if (chexarResult.ErrorStatus)
                    return chexarResult;

                MessageIds.Add(chatMessage.MessageId);
            }
            return chexarResult;
        }

        #region Private

        private ChexarResult ParseError(string result)
        {
            var chexarResult = new ChexarResult();

            chexarResult.ParseResult(result);

            return chexarResult;
        }
        
        private void FormatChatText(ChatMessage chat, string text)
        {
            string[] splitLevel1 = text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

            text = text.Substring(text.IndexOf(" - ") + 3);

            string[] splitLevel2 = text.Split(new string[] { " Wrote:" }, StringSplitOptions.RemoveEmptyEntries);

            text = text.Substring(text.IndexOf(" Wrote:") + 8);

            var enUS = new CultureInfo("en-US");

            chat.Datetime = Convert.ToDateTime(splitLevel1[0], enUS);

            chat.UserName = splitLevel2[0];

            chat.Text = text;
        }

        private ChexarIO.Communication.CallCenterService.CallCenterService getCallCenterSvc(string url)
        {
            var callCenterSvc = new CallCenterService.CallCenterService {Url = url};

            return callCenterSvc;
        }
        
        private XDocument ParseXDocument(string x)
        {
            return XDocument.Parse(x.Replace("&", "&amp;"));
        }

        #endregion
    }
}
