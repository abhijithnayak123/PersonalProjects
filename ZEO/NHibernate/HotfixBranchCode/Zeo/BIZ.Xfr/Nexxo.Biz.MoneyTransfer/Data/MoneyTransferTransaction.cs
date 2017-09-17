using System;
using System.Collections.Generic;

namespace MGI.Biz.MoneyTransfer.Data
{
	public class MoneyTransferTransaction
	{
		public Account Account { get; set; }
		public Receiver Receiver { get; set; }
		public decimal TransactionAmount { get; set; }
		public string OriginatingCountryCode { get; set; }
		public string OriginatingCurrencyCode { get; set; }
		public string TransactionType { get; set; }
		public string PromotionsCode { get; set; }
		public decimal ExchangeRate { get; set; }
		public decimal DestinationPrincipalAmount { get; set; }
		public decimal GrossTotalAmount { get; set; }
		public decimal Fee { get; set; }  // Fees
		public decimal TaxAmount { get; set; }
		public decimal PromotionDiscount { get; set; }
		public string ConfirmationNumber { get; set; }
		public string DestinationCountryCode { get; set; }
		public string DestinationCurrencyCode { get; set; }
		public string DestinationState { get; set; }
		public int TransferType { get; set; }
		public string DeliveryServiceName { get; set; }
		public string DeliveryServiceDesc { get; set; }
		public bool IsDomesticTransfer { get; set; }
		public string TransactionID { get; set; }
		public string SenderName { get; set; }
		public string ExpectedPayoutStateCode { get; set; }
		public Nullable<DateTime> DTAvailableForPickup { get; set; }
		public string ReceiverFirstName { get; set; }
		public string ReceiverLastName { get; set; }
		public string ReceiverSecondLastName { get; set; }
		public decimal AmountToReceiver { get; set; }
		public string ReferenceNo { get; set; }
		public string TransactionSubType { get; set; }
		public long OriginalTransactionId { get; set; }
		public bool IsModifiedOrRefunded { get; set; }
		public int ProviderId { get; set; }
		public string LoyaltyCardNumber { get; set; }
		public string LoyaltyCardPoints { get; set; }
		public string TestQuestion { get; set; }
		public string TestAnswer { get; set; }
		public Dictionary<string, object> MetaData { get; set; }
		public string PersonalMessage { get; set; }
        public string ReceiveAgentID { get; set; }
        
	}
}
