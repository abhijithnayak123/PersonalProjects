using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class PaymentDetails
    {
        public decimal OriginatorsPrincipalAmount { get; set; }
        public decimal DestinationPrincipalAmount { get; set; }
        public string ExpectedPayoutLocCity { get; set; }
        public string RecordingcountrycurrencyCountryCode { get; set; }
        public string RecordingcountrycurrencyCurrencyCode { get; set; }
        public string DestinationCountryCode { get; set; }
        public string DestinationCurrencyCode { get; set; }
        public string OriginatingCountryCode { get; set; }
        public string OriginatingCurrencyCode { get; set; }
        public string ExpectedPayoutStateCode { get; set; }
        public string ExpectedPayoutCityCode { get; set; }
        public string TranascationType { get; set; }
        public string PaymentType { get; set; }
        public string PromotionsCode { get; set; }
        public long TransactionId { get; set; }

        public string TestQuestionAvailable { get; set; }
        public string TestQuestion { get; set; }
        public string TestAnswer { get; set; }
        public decimal Fee { get; set; }
        public decimal OtherFees {get; set;}
        public decimal ExchangeRate { get; set; }
        public string DestinationState { get; set; }
        public decimal PromotionDiscount { get; set; }
        public bool IsDomesticTransfer { get; set; }
        public bool IsFixedOnSend { get; set; }		
		public string RecieverFirstName { get; set; }		
		public string RecieverLastName { get; set; }		
		public string RecieverSecondLastName { get; set; }

		public string PromoCodeDescription { get; set; }
		public string PromoName { get; set; }
		public string PromoMessage { get; set; }
		public string PromotionError { get; set; }

	    public string DeliveryMethod { get; set; }
	    public string DeliveryServiceDesc { get; set; }
        public string ReferenceNo { get; set; }
	    public string PersonalMessages { get; set; }
        public decimal MessageCharge { get; set; }

        public string TransactionSubType { get; set; }
        public long OriginalTransactionId { get; set; }
        public string DeliveryOption { get; set; }
        public string DeliveryOptionDesc { get; set; }
		public bool ProceedWithLPMTError { get; set; }
	}
}
