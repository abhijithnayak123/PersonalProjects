using System;
using System.Collections.Generic;
namespace MGI.Cxn.MoneyTransfer.Data
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
		public decimal OtherFees { get; set; }
		public decimal TransferTax { get; set; }
		public decimal ExchangeRate { get; set; }
		public bool isDomesticTransfer { get; set; }
		public string DestinationState { get; set; }
		public decimal PromotionDiscount { get; set; }
		public bool IsFixedOnSend { get; set; }
		public string RecieverFirstName { get; set; }
		public string RecieverLastName { get; set; }
		public string RecieverSecondLastName { get; set; }
		public string PromoCodeDescription { get; set; }
		public string PromoName { get; set; }
		public string PromoMessage { get; set; }
		public string PromotionError { get; set; }
		public string PromotionSequenceNo { get; set; }
		public string PrimaryIdType { get; set; }
		public string PrimaryIdNumber { get; set; }
		public string PrimaryIdCountryOfIssue { get; set; }
		public string PrimaryCountryOfIssue { get; set; }
		public string PrimaryIdPlaceOfIssue { get; set; }
		public string SecondIdType { get; set; }
		public string SecondIdNumber { get; set; }
		public string SecondIdCountryOfIssue { get; set; }
		public string Occupation { get; set; }
		public string DateOfBirth { get; set; }
		public string SenderComplianceDetailsComplianceDataBuffer { get; set; }
		public string recordingCountryCode { get; set; }
		public string recordingCurrencyCode { get; set; }
		public string originating_city { get; set; }
		public string originating_state { get; set; }
		public string instant_notification_addl_service_charges { get; set; }

		public string DeliveryMethod { get; set; }
		public string CountryOfBirth { get; set; }
		public string DeliveryServiceDesc { get; set; }
		public string ReferenceNo { get; set; }
		public string PersonalMessages { get; set; }
		public decimal MessageCharge { get; set; }
		public string DeliveryOption { get; set; }
		public string DeliveryOptionDesc { get; set; }
		public bool ProceedWithLPMTError { get; set; }
		public decimal MunicipalTax { get; set; }
		public decimal StateTax { get; set; }
		public decimal CountyTax { get; set; }
	}
}
