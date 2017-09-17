using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class AgentMessage
    {
        [DataMember]
        public string CustomerFirstName { get; set; }
        [DataMember]
        public string CustomerLastName { get; set; }
        [DataMember]
        public string Amount { get; set; }
        [DataMember]
        public string TransactionState { get; set; }
        [DataMember]
        public string TicketNumber { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public string DeclineMessage { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "CustomerFirstName = ", CustomerFirstName, "\r\n");
            str = string.Concat(str, "CustomerLastName = ", CustomerLastName, "\r\n");
            str = string.Concat(str, "Amount = ", Amount, "\r\n");
            str = string.Concat(str, "TransactionState = ", TransactionState, "\r\n");
            str = string.Concat(str, "TicketNumber = ", TicketNumber, "\r\n");
            str = string.Concat(str, "TransactionId = ", TransactionId, "\r\n");
            str = string.Concat(str, "DeclineMessage = ", DeclineMessage, "\r\n");
            return str;
        }
    }
}
