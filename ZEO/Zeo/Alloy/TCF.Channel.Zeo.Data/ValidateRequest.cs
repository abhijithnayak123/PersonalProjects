using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class ValidateRequest
    {
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal OtherFee { get; set; }
        [DataMember]
        public decimal MessageFee { get; set; }

        [DataMember]
        public long TransactionId { get; set; }

        [DataMember]
        public long ReceiverId { get; set; }

        [DataMember]
        public MoneyTransferType TransferType { get; set; }
        [DataMember]
        public string ReferenceNumber { get; set; }
        [DataMember]
        public string PersonalMessage { get; set; }
        [DataMember]
        public string PromoCode { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string DeliveryService { get; set; }
        [DataMember]
        public string ReceiveCurrency { get; set; }
        [DataMember]
        public string ReceiverFirstName { get; set; }
        [DataMember]
        public string ReceiverLastName { get; set; }
        [DataMember]
        public string ReceiverSecondLastName { get; set; }

        [DataMember]
        public string IdentificationQuestion { get; set; }
        [DataMember]
        public string IdentificationAnswer { get; set; }

        [DataMember]
        public Dictionary<string, object> MetaData { get; set; }

        [DataMember]
        public Dictionary<string, string> FieldValues { get; set; }

        [DataMember]
        public bool ConsumerFraudPromptQuestion { get; set; }

        override public string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Amount = {0} \r\n", Amount);
            stringBuilder.AppendFormat("Fee = {0} \r\n", Fee);
            stringBuilder.AppendFormat("Tax = {0} \r\n", Tax);
            stringBuilder.AppendFormat("OtherFee = {0} \r\n", OtherFee);
            stringBuilder.AppendFormat("MessageFee = {0} \r\n", MessageFee);
            stringBuilder.AppendFormat("TransactionId = {0} \r\n", TransactionId);
            stringBuilder.AppendFormat("ReceiverId = {0} \r\n", ReceiverId);
            stringBuilder.AppendFormat("TransferType = {0} \r\n", TransferType);
            stringBuilder.AppendFormat("ReferenceNumber = {0} \r\n", ReferenceNumber);
            stringBuilder.AppendFormat("PersonalMessage = {0} \r\n", PersonalMessage);
            stringBuilder.AppendFormat("PromoCode = {0} \r\n", PromoCode);
            stringBuilder.AppendFormat("State = {0} \r\n", State);
            stringBuilder.AppendFormat("DeliveryService = {0} \r\n", DeliveryService);
            stringBuilder.AppendFormat("ReceiverFirstName = {0} \r\n", ReceiverFirstName);
            stringBuilder.AppendFormat("ReceiverLastName = {0} \r\n", ReceiverLastName);
            stringBuilder.AppendFormat("ReceiverSecondLastName = {0} \r\n", ReceiverSecondLastName);
            stringBuilder.AppendFormat("IdentificationQuestion = {0} \r\n", IdentificationQuestion);
            stringBuilder.AppendFormat("IdentificationAnswer = {0} \r\n", IdentificationAnswer);
            stringBuilder.AppendFormat("ConsumerFraudPromptQuestion = {0} \r\n", ConsumerFraudPromptQuestion);

            if (MetaData != null)
            {
                foreach (KeyValuePair<string, object> meta in MetaData)
                {
                    stringBuilder.AppendFormat("{0} = {1} \r\n", meta.Key, meta.Value);
                }
            }

            if (FieldValues != null)
            {
                foreach (KeyValuePair<string, string> field in FieldValues)
                {
                    stringBuilder.AppendFormat("{0} = {1} \r\n", field.Key, field.Value);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
