using System.Collections.Generic;
namespace MGI.Biz.Partner.Data
{
	public class MoneyTransferTransaction
	{
        public Account Account { get; set; }
        public string Id { get; set; }
        public int TransferType { get; set; } // Sendmoney,ReceiveMoney type 
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountToReceiver { get; set; } // Use this only for receivemoney 
        public decimal Fee { get; set; }
        public string ConfirmationNumber { get; set; } //MTCN No
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverSecondLastName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverNickName { get; set; }
        public string ReceiverPhone { get; set; }
        
        public int ProviderId { get; set; }
        public string TestQuestion { get; set; }
        public string TestAnswer { get; set; }
        public string SenderName { get; set; }
        public string PromotionsCode { get; set; }
        public decimal PromotionDiscount { get; set; }
        public long OriginalTransactionID { get; set; }
        public string TransactionSubType { get; set; }
        public bool IsModifiedOrRefunded { get; set; }
        public decimal GrossTotalAmount { get; set; }
        public string ExpectedPayoutCityName { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverZipCode { get; set; }
		public string LoyaltyCardNumber { get; set; }
		public string LoyaltyCardPoints { get; set; }
		public Dictionary<string, object> MetaData { get; set; }

		public string DestinationCountryCode { get; set; }
		public string DestinationCurrencyCode { get; set; }
		public string DestinationState { get; set; }
		public string DeliveryServiceName { get; set; }
		public string DeliveryServiceDesc { get; set; }
		public bool IsDomesticTransfer { get; set; }
		public string PersonalMessage { get; set; }
		public long ReceiverId { get; set; }
		public bool IsReceiverHasPhotoId { get; set; }
		public string ReceiverSecurityQuestion { get; set; }
		public string ReceiverSecurityAnswer { get; set; }
    }
}
