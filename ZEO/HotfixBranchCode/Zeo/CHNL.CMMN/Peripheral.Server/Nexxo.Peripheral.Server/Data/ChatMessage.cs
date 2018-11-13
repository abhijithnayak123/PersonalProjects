using System;
using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int MessageId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public bool IsRead { get; set; }

        [DataMember]
        public bool IsIncoming { get; set; }
    }
}
