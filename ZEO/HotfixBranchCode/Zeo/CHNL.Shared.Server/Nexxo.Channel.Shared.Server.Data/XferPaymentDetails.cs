using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class XferPaymentDetails
    {
        public XferPaymentDetails()
        {
        }
        [DataMember]
        public decimal OriginatorsPrincipalAmount { get; set; }
        [DataMember]
        public decimal DestinationPrincipalAmount { get; set; }
        [DataMember]
        public string ExpectedPayoutLocCity { get; set; }
        [DataMember]
        public string RecordingcountrycurrencyCountryCode { get; set; }
        [DataMember]
        public string RecordingcountrycurrencyCurrencyCode { get; set; }
        [DataMember]
        public string DestinationCountryCode { get; set; }
        [DataMember]
        public string DestinationCurrencyCode { get; set; }
        [DataMember]
        public string OriginatingCountryCode { get; set; }
        [DataMember]
        public string OriginatingCurrencyCode { get; set; }
        [DataMember]
        public string ExpectedPayoutStateCode { get; set; }
        [DataMember]
        public string ExpectedPayoutCityCode { get; set; }
        [DataMember]
		public string TranascationType { get; set; }
        [DataMember]
        public string PaymentType { get; set; }
        [DataMember]
        public string PromotionsCode { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public string TestQuestionAvailable { get; set; }
        [DataMember]
        public string TestQuestion { get; set; }
        [DataMember]
        public string TestAnswer { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public decimal OtherFees { get; set; }
        [DataMember]
        public decimal TransferTax { get; set; }
        [DataMember]
        public bool IsDomesticTransfer { get; set; }
        [DataMember]
        public string DestinationState { get; set; }
        [DataMember]
        public decimal PromotionDiscount { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public string ReceiverFirstName { get; set; }
        [DataMember]
        public string ReceiverLastName { get; set; }
        [DataMember]
        public string ReceiverSecondLastName { get; set; }
        [DataMember]
        public bool IsFixedOnSend { get; set; }
        [DataMember]
        public string PromoCodeDescription { get; set; }
        [DataMember]
        public string PromoName { get; set; }
        [DataMember]
        public string PromoMessage { get; set; }
        [DataMember]
        public string PromotionError { get; set; }
        [DataMember]
        public string DeliveryMethod { get; set; }
        [DataMember]
        public string DeliveryServiceDesc { get; set; }
        [DataMember]
        public string ReferenceNo { get; set; }
        //Added for User Story # 1684
        [DataMember]
        public string PersonalMessages { get; set; }
        [DataMember]
        public decimal MessageCharge { get; set; }
        [DataMember]
        public string DeliveryOption { get; set; }
        [DataMember]
        public string DeliveryOptionDesc { get; set; }
		[DataMember]
		public bool ProceedWithLPMTError { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "OriginatorsPrincipalAmount = ", OriginatorsPrincipalAmount, "\r\n");
			str = string.Concat(str, "DestinationPrincipalAmount = ", DestinationPrincipalAmount, "\r\n");
			str = string.Concat(str, "ExpectedPayoutLocCity = ", ExpectedPayoutLocCity, "\r\n");
			str = string.Concat(str, "RecordingcountrycurrencyCountryCode = ", RecordingcountrycurrencyCountryCode, "\r\n");
			str = string.Concat(str, "RecordingcountrycurrencyCurrencyCode = ", RecordingcountrycurrencyCurrencyCode, "\r\n");
			str = string.Concat(str, "DestinationCountryCode = ", DestinationCountryCode, "\r\n");
			str = string.Concat(str, "DestinationCurrencyCode = ", DestinationCurrencyCode, "\r\n");
			str = string.Concat(str, "OriginatingCountryCode = ", OriginatingCountryCode, "\r\n");
			str = string.Concat(str, "OriginatingCurrencyCode = ", OriginatingCurrencyCode, "\r\n");
			str = string.Concat(str, "ExpectedPayoutStateCode = ", ExpectedPayoutStateCode, "\r\n");
			str = string.Concat(str, "ExpectedPayoutCityCode = ", ExpectedPayoutCityCode, "\r\n");
			str = string.Concat(str, "TranascationType = ", TranascationType, "\r\n");
			str = string.Concat(str, "PaymentType = ", PaymentType, "\r\n");
			str = string.Concat(str, "PromotionsCode = ", PromotionsCode, "\r\n");
			str = string.Concat(str, "TransactionId = ", TransactionId, "\r\n");
			str = string.Concat(str, "TestQuestionAvailable = ", TestQuestionAvailable, "\r\n");
			str = string.Concat(str, "TestQuestion = ", TestQuestion, "\r\n");
			str = string.Concat(str, "TestAnswer = ", TestAnswer, "\r\n");
			str = string.Concat(str, "Fee = ", Fee, "\r\n");
			str = string.Concat(str, "OtherFees = ", OtherFees, "\r\n");
			str = string.Concat(str, "TransferTax = ", TransferTax, "\r\n");
			str = string.Concat(str, "IsDomesticTransfer = ", IsDomesticTransfer, "\r\n");
			str = string.Concat(str, "DestinationState = ", DestinationState, "\r\n");
			str = string.Concat(str, "PromotionDiscount = ", PromotionDiscount, "\r\n");
			str = string.Concat(str, "ExchangeRate = ", ExchangeRate, "\r\n");
			str = string.Concat(str, "ReceiverFirstName = ", ReceiverFirstName, "\r\n");
			str = string.Concat(str, "ReceiverLastName = ", ReceiverLastName, "\r\n");
			str = string.Concat(str, "ReceiverSecondLastName = ", ReceiverSecondLastName, "\r\n");
			str = string.Concat(str, "IsFixedOnSend = ", IsFixedOnSend, "\r\n");
			str = string.Concat(str, "PromoCodeDescription = ", PromoCodeDescription, "\r\n");
			str = string.Concat(str, "PromoName = ", PromoName, "\r\n");
			str = string.Concat(str, "PromoMessage = ", PromoMessage, "\r\n");
			str = string.Concat(str, "PromotionError = ", PromotionError, "\r\n");
			str = string.Concat(str, "DeliveryMethod = ", DeliveryMethod, "\r\n");
			str = string.Concat(str, "DeliveryServiceDesc = ", DeliveryServiceDesc, "\r\n");
			str = string.Concat(str, "ReferenceNo = ", ReferenceNo, "\r\n");
			str = string.Concat(str, "PersonalMessages = ", PersonalMessages, "\r\n");
			str = string.Concat(str, "MessageCharge = ", MessageCharge, "\r\n");
			str = string.Concat(str, "DeliveryOption = ", DeliveryOption, "\r\n");
			str = string.Concat(str, "DeliveryOptionDesc = ", DeliveryOptionDesc, "\r\n");
			str = string.Concat(str, "ProceedWithLPMTError = ", ProceedWithLPMTError, "\r\n");
			return str;
		}
    }
}
