using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class PastTransaction
    {
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public string TransactionType { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public DateTime DTLastMod { get; set; }

        //BillPay
        [DataMember]
        public string BillerName { get; set; }
        [DataMember]
        public string BillerCode { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }

        //Money Transfer
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string ReceiverFirstName { get; set; }
        [DataMember]
        public string ReceiverLastName { get; set; }

        override public string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("TransactionId = {0} \r\n", TransactionId );
            stringBuilder.AppendFormat("TransactionType = {0} \r\n", TransactionType );
            stringBuilder.AppendFormat("Amount = {0} \r\n", Amount );
            stringBuilder.AppendFormat("Fee = {0} \r\n", Fee );
            stringBuilder.AppendFormat("DTLastMod = {0} \r\n", DTLastMod);
            stringBuilder.AppendFormat("BillerName = {0} \r\n", BillerName);
            stringBuilder.AppendFormat("BillerCode = {0} \r\n", BillerCode);
            stringBuilder.AppendFormat("AccountNumber = {0} \r\n", AccountNumber);
            stringBuilder.AppendFormat("Country = {0} \r\n", Country);
            stringBuilder.AppendFormat("State = {0} \r\n", State);
            stringBuilder.AppendFormat("Amount = {0} \r\n", Amount);
            stringBuilder.AppendFormat("ReceiverFirstName = {0} \r\n", ReceiverFirstName);
            stringBuilder.AppendFormat("ReceiverLastName = {0} \r\n", ReceiverLastName);
            return stringBuilder.ToString();
        }
    }
}
