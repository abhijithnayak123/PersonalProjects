using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Peripheral.Check.Messenger.Data
{
    public class ChatMessage
    {
        public DateTime Date { get; set; }
        public int MessageId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public MessageTypes MessageType { get; set; }
        public bool IsRead { get; set; }
    }

    public enum MessageTypes
    {
        Incoming = 1,
        OutGoing = 2
    }
}
