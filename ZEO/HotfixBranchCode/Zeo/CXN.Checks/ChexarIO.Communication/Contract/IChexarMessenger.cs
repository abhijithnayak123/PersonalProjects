using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChexarIO.Communication.Data;

namespace ChexarIO.Communication.Contract
{
    public interface IChexarMessenger
    {
        ChexarResult CheckForMessage(ChexarLogin login, int ticketNo);

        ChexarResult ComposeMessage(ChexarLogin login, int ticketNo, string text);

        ChexarResult ConfirmMessage(ChexarLogin login, int messageId);

        ChexarResult ConfirmAllMessages(ChexarLogin login, int ticketNo, int lastMessageId);
    }
}
