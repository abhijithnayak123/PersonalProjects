using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChexarIO.Communication.Data
{
    public class ChatMessage
    {
        public string UserName { get; set; }
        public DateTime Datetime { get; set; }
        public string Text { get; set; }
        public int MessageId { get; set; }
        public MessageTypes MessageType { get; set; }
        public bool IsRead { get; set; }
    }
}
