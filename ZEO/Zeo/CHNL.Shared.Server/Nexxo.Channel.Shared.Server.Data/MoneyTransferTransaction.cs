using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class MoneyTransferTransaction
    {
        [DataMember]
        public Account Account { get; set; }
        [DataMember]
        public Receiver Receiver { get; set; }
        [DataMember]
        public decimal TransactionAmount { get; set; }
        [DataMember]
        public string OriginatingCountryCode { get; set; }
        [DataMember]
        public string OriginatingCurrencyCode { get; set; }
        [DataMember]
        public string TransactionType { get; set; }
        [DataMember]
        public string PromotionsCode { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal DestinationPrincipalAmount { get; set; }
        [DataMember]
        public decimal GrossTotalAmount { get; set; }
        [DataMember]
        public decimal Fee { get; set; }  // Fees
        [DataMember]
        public decimal TaxAmount { get; set; }
        [DataMember]
        public decimal PromotionDiscount { get; set; }
        [DataMember]
        public string ConfirmationNumber { get; set; }
        [DataMember]
        public string DestinationCountryCode { get; set; }
        [DataMember]
        public string DestinationCurrencyCode { get; set; }
        [DataMember]
        public string DestinationState { get; set; }
        [DataMember]
        public bool IsDomesticTransfer { get; set; }
        [DataMember]
        public string TestQuestion { get; set; }
        [DataMember]
        public string TestAnswer { get; set; }
        [DataMember]
        public string DeliveryServiceName { get; set; }
        [DataMember]
        public string DeliveryServiceDesc { get; set; }
        [DataMember]
        public string TransactionID { get; set; }
        [DataMember]
        public string SenderName { get; set; }
        [DataMember]
        public string ExpectedPayoutStateCode { get; set; }
        [DataMember]
        public string ReceiverFirstName { get; set; }
        [DataMember]
        public string ReceiverLastName { get; set; }
        [DataMember]
        public string ReceiverSecondLastName { get; set; }
        [DataMember]
        public decimal AmountToReceiver { get; set; }
        [DataMember]
        public string TransactionSubType { get; set; }
        [DataMember]
        public long OriginalTransactionId { get; set; }
        [DataMember]
        public string ReceiverAddress { get; set; }
        [DataMember]
        public string ReceiverCity { get; set; }
        [DataMember]
        public string ReceiverState { get; set; }
        [DataMember]
        public string ReceiverCountry { get; set; }
        [DataMember]
        public string ReceiverZipCode { get; set; }
        [DataMember]
        public int ProviderId { get; set; }
        [DataMember]
        public bool IsModifiedOrRefunded { get; set; }
        [DataMember]
        public string LoyaltyCardNumber { get; set; }
        [DataMember]
        public string LoyaltyCardPoints { get; set; }
        [DataMember]
        public Dictionary<string, object> MetaData { get; set; }
        [DataMember]
        public string PersonalMessage { get; set; }
        [DataMember]
        public string ReceiveAgentID { get; set; }
        [DataMember]
        public string ReceiverCountryName { get; set; }
        [DataMember]
        public string ReceiverStateName { get; set; }
        [DataMember]
        public Nullable<DateTime> DTAvailableForPickup { get; set; }
		[DataMember]
		public decimal TransferTax{get;set;}

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "TransactionAmount = ", TransactionAmount, "\r\n");
            str = string.Concat(str, "ConfirmationNumber = ", ConfirmationNumber, "\r\n");
            str = string.Concat(str, "ReceiverFirstName = ", ReceiverFirstName, "\r\n");
            str = string.Concat(str, "RecieverLastName = ", ReceiverLastName, "\r\n");
            str = string.Concat(str, "RecieverSecondLastName = ", ReceiverSecondLastName, "\r\n");
            str = string.Concat(str, "ReceiverAddress = ", ReceiverAddress, "\r\n");
            str = string.Concat(str, "ProviderName = ", ProviderId, "\r\n");
            str = string.Concat(str, "TestQuestion = ", TestQuestion, "\r\n");
            str = string.Concat(str, "TestAnswer = ", TestAnswer, "\r\n");
            str = string.Concat(str, "SenderName = ", SenderName, "\r\n");
            str = string.Concat(str, "ReceiverCity= ", ReceiverCity, "\r\n");
            str = string.Concat(str, "ReceiverState= ", ReceiverState, "\r\n");
            str = string.Concat(str, "ReceiverCountry= ", ReceiverCountry, "\r\n");
            str = string.Concat(str, "ReceiverZipCode= ", ReceiverZipCode, "\r\n");
            str = string.Concat(str, "DTAvailableForPickup= ", DTAvailableForPickup, "\r\n");
            return str;
        }
    }
}
