using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Peripheral.Check.Messenger.Data;

namespace MGI.Peripheral.Check.Messenger.Contract
{
    public interface ICheckMessenger
    {
        IList<Data.ChatMessage> ChatMessages { get; set; }

        HashSet<int> MessageIds { get; set; }

        CheckResult CheckForMessages(CheckLogin login, int ticketNo);
        
        CheckResult ComposeMessage(CheckLogin login, int ticketNo, string message);

        CheckResult ConfirmMessage(CheckLogin login, int messageId);

        CheckResult ConfirmAllMessages(CheckLogin login, int ticketNo, int lastMessageId);
    }
}
