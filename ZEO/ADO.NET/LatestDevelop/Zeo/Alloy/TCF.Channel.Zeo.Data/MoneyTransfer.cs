using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class MoneyTransfer
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public long ReceiverId { get; set; }
        [DataMember]
        public bool AcceptedFee { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public decimal MoneyTransferTotal { get; set; }
        [DataMember]
        public string PickupLocation { get; set; }
        [DataMember]
        public string PickupMethod { get; set; }
        [DataMember]
        public string PickupOptions { get; set; }
        [DataMember]
        public decimal TransferTax { get; set; }
        [DataMember]
        public decimal OtherFee { get; set; }
        [DataMember]
        public decimal OtherTax { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public string ReceiverFirstName { get; set; }
        [DataMember]
        public string ReceiverLastName { get; set; }
        [DataMember]
        public string ReceiverAddress { get; set; }
        [DataMember]
        public string ReceiverCity { get; set; }
        [DataMember]
        public string ReceiverState { get; set; }
        [DataMember]
        public string ReceiverAccount { get; set; }
        [DataMember]
        public string SenderFirstName { get; set; }
        [DataMember]
        public string SenderLastName { get; set; }
        [DataMember]
        public string SenderMiddleName { get; set; }
        [DataMember]
        public string SenderSecondLastName { get; set; }
        [DataMember]
        public string SenderAddress { get; set; }
        [DataMember]
        public string SenderCity { get; set; }
        [DataMember]
        public string SenderState { get; set; }
        [DataMember]
        public string DestinationCountry { get; set; }
        [DataMember]
        public string DestinationCurrency { get; set; }
        [DataMember]
        public string SourceCountry { get; set; }
        [DataMember]
        public string SourceCurrency { get; set; }
        [DataMember]
        public decimal SourceAmount { get; set; }
        [DataMember]
        public decimal DestinationAmount { get; set; }
        [DataMember]
        public int TransferType { get; set; }
        [DataMember]
        public string TransactionSubType { get; set; }
        [DataMember]
        public long OriginalTransactionId { get; set; }
        [DataMember]
        public string ConfirmationNumber { get; set; }
        [DataMember]
        public Dictionary<string, object> MetaData { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MoneyTransfer:");
            sb.AppendLine(string.Format("   Id: {0}", Id));
            sb.AppendLine(string.Format("	Amount: {0}", Amount.ToString("c2")));
            sb.AppendLine(string.Format("	Status: {0}", Status));
            sb.AppendLine(string.Format("	Status Description: {0}", StatusDescription));
            sb.AppendLine(string.Format("	Status Message: {0}", StatusMessage));
            sb.AppendLine(string.Format("	ReceiverId: {0}", ReceiverId.ToString()));
            sb.AppendLine(string.Format("	AcceptedFee: {0}", AcceptedFee.ToString()));
            sb.AppendLine(string.Format("	Fee: {0}", Fee.ToString()));
            sb.AppendLine(string.Format("	MoneyTransferTotal: {0}", MoneyTransferTotal.ToString()));
            sb.AppendLine(string.Format("	PickupLocation: {0}", PickupLocation));
            sb.AppendLine(string.Format("	PickupMethod: {0}", PickupMethod));
            sb.AppendLine(string.Format("	PickupOptions: {0}", PickupOptions));
            sb.AppendLine(string.Format("	TransferTax: {0}", TransferTax));
            sb.AppendLine(string.Format("	OtherFee: {0}", OtherFee));
            sb.AppendLine(string.Format("	OtherTax: {0}", OtherTax));
            sb.AppendLine(string.Format("	ReceiverFirstName: {0}", ReceiverFirstName));
            sb.AppendLine(string.Format("	ReceiverLastName: {0}", ReceiverLastName));
            sb.AppendLine(string.Format("	ReceiverAddress: {0}", ReceiverAddress));
            sb.AppendLine(string.Format("	ReceiverCity: {0}", ReceiverCity));
            sb.AppendLine(string.Format("	ReceiverState: {0}", ReceiverState));
            sb.AppendLine(string.Format("	ReceiverAccount: {0}", ReceiverAccount));
            sb.AppendLine(string.Format("	SenderFirstName: {0}", SenderFirstName));
            sb.AppendLine(string.Format("	SenderLastName: {0}", SenderLastName));
            sb.AppendLine(string.Format("	SenderMiddleName: {0}", SenderMiddleName));
            sb.AppendLine(string.Format("	SenderSecondLastName: {0}", SenderSecondLastName));
            sb.AppendLine(string.Format("	SenderAddress: {0}", SenderAddress));
            sb.AppendLine(string.Format("	SenderCity: {0}", SenderCity));
            sb.AppendLine(string.Format("	SenderState: {0}", SenderState));
            sb.AppendLine(string.Format("	DestinationCountry: {0}", DestinationCountry));
            sb.AppendLine(string.Format("	DestinationCurrency: {0}", DestinationCurrency));
            sb.AppendLine(string.Format("	SourceCountry: {0}", SourceCountry));
            sb.AppendLine(string.Format("	SourceCurrency: {0}", SourceCurrency));
            sb.AppendLine(string.Format("	SourceAmount: {0}", SourceAmount));
            sb.AppendLine(string.Format("	DestinationAmount: {0}", DestinationAmount));
            sb.AppendLine(string.Format("   TransactionSubType: {0}", TransactionSubType));
            sb.AppendLine(string.Format("   OriginalTransactionId: {0}", OriginalTransactionId.ToString()));
            sb.AppendLine(string.Format("   ConfirmationNumber: {0}", ConfirmationNumber));

            return sb.ToString();
        }
    }
}
