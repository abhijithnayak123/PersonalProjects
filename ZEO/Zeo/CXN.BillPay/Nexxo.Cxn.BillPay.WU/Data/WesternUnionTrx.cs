using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.BillPay.WU.Data
{
	public class WesternUnionTrx : NexxoModel
	{
		public virtual int ChannelParterId { get; set; }
		public virtual WesternUnionAccount WesternUnionAccount { get; set; }
		public virtual int ProviderId { get; set; }
		public virtual string ChannelType { get; set; }
		public virtual string ChannelName { get; set; }
		public virtual string ChannelVersion { get; set; }
		public virtual string SenderFirstName { get; set; }
		public virtual string SenderLastname { get; set; }
		public virtual string SenderAddressLine1 { get; set; }
		public virtual string SenderCity { get; set; }
		public virtual string SenderState { get; set; }
		public virtual string SenderPostalCode { get; set; }
		public virtual string SenderCountryCode { get; set; }
		public virtual string SenderCurrencyCode { get; set; }
		public virtual string SenderAddressLine2 { get; set; }
		public virtual string SenderStreet { get; set; }
		public virtual string WesternUnionCardNumber { get; set; }
		public virtual string LevelCode { get; set; }
		public virtual string SenderEmail { get; set; }
		public virtual string SenderContactPhone { get; set; }
		public virtual string SenderDateOfBirth { get; set; }
		public virtual string BillerName { get; set; }
		public virtual string BillerCityCode { get; set; }
		public virtual string CustomerAccountNumber { get; set; }
		public virtual string CountryCode { get; set; }
		public virtual string CurrencyCode { get; set; }
		public virtual string FinancialsMunicipalTax { get; set; }
		public virtual string FinancialsStateTax { get; set; }
		public virtual string FinancialsCountTax { get; set; }
		public virtual decimal FinancialsOriginatorsPrincipalAmount { get; set; }
		public virtual decimal FinancialsDestinationPrincipalAmount { get; set; }
		public virtual decimal FinancialsFee { get; set; }
		public virtual decimal FinancialsGrossTotalAmount { get; set; }
		public virtual decimal FinancialsTotal { get; set; }
		public virtual decimal FinancialsUndiscountedCharges { get; set; }
		public virtual decimal FinancialsDiscountedCharges { get; set; }
		public virtual System.Nullable<decimal> FinancialsPlusChargesAmount { get; set; }
		public virtual System.Nullable<decimal> FinancialsTotalDiscount { get; set; }

		public virtual string PaymentDetailsRecordingCountryCode { get; set; }
		public virtual string PaymentDetailsRecordingCountryCurrency { get; set; }
		public virtual string PaymentDetailsDestinationCountryCode { get; set; }
		public virtual string PaymentDetailsDestinationCountryCurrency { get; set; }
		public virtual string PaymentDetailsOriginatingCountryCode { get; set; }
		public virtual string PaymentDetailsOriginatingCountryCurrency { get; set; }
		public virtual string PaymentDetailsOriginatingCity { get; set; }
		public virtual string PaymentDetailsOriginatingState { get; set; }
		public virtual string PaymentDetailsTransactionType { get; set; }
		public virtual string PaymentDetailsPaymentType { get; set; }
		public virtual double PaymentDetailsExchangeRate { get; set; }
		public virtual string PaymentDetailsFixOnSend { get; set; }
		public virtual string PaymentDetailsReceiptOptOut { get; set; }
		public virtual string PaymentDetailsAuthStatus { get; set; }

		public virtual string PromotionsCouponsPromotions { get; set; }
		public virtual string PromotionsPromoCodeDescription { get; set; }
		public virtual string PromotionsPromoSequenceNo { get; set; }
		public virtual string PromotionsPromoName { get; set; }
		public virtual string PromotionsPromoMessage { get; set; }
		public virtual decimal PromotionsPromoDiscountAmount { get; set; }
		public virtual string PromotionsPromotionError { get; set; }
		public virtual string PromotionsSenderPromoCode { get; set; }

		public virtual string SenderComplianceDetailsTemplateID { get; set; }
		public virtual string SenderComplianceDetailsIdDetailsIdType { get; set; }
		public virtual string SenderComplianceDetailsIdDetailsIdCountryOfIssue { get; set; }
		public virtual string SenderComplianceDetailsIdDetailsIdPlaceOfIssue { get; set; }
		public virtual string SenderComplianceDetailsIdDetailsIdNumber { get; set; }
		public virtual string SenderComplianceDetailsSecondIDIdType { get; set; }
		public virtual string SenderComplianceDetailsSecondIDIdCountryOfIssue { get; set; }
		public virtual string SenderComplianceDetailsSecondIDIdNumber { get; set; }
		public virtual string SenderComplianceDetailsDateOfBirth { get; set; }
		public virtual string SenderComplianceDetailsOccupation { get; set; }
		public virtual string SenderComplianceDetailsCurrentAddressAddrLine1 { get; set; }
		public virtual string SenderComplianceDetailsCurrentAddressAddrLine2 { get; set; }
		public virtual string SenderComplianceDetailsCurrentAddressCity { get; set; }
		public virtual string SenderComplianceDetailsCurrentAddressStateCode { get; set; }
		public virtual string SenderComplianceDetailsCurrentAddressPostalCode { get; set; }
		public virtual string SenderComplianceDetailsContactPhone { get; set; }
		public virtual string SenderComplianceDetailsIActOnMyBehalf { get; set; }
		public virtual string SenderComplianceDetailsAckFlag { get; set; }
		public virtual string SenderComplianceDetailsComplianceDataBuffer { get; set; }
		public virtual string QPCompanyDepartment { get; set; }
		public virtual string FillingDate { get; set; }
		public virtual string FillingTime { get; set; }
		public virtual string Mtcn { get; set; }
		public virtual string NewMTCN { get; set; }
		public virtual string DfFieldsPdsrequiredflag { get; set; }
		public virtual string DfFieldsTransactionFlag { get; set; }
		public virtual string DfFieldsDeliveryServiceName { get; set; }
		public virtual string DeliveryCode { get; set; }
		public virtual string FusionScreen { get; set; }
		public virtual string ConvSessionCookie { get; set; }
		public virtual string ForeignRemoteSystemIdentifier { get; set; }
		public virtual string ForeignRemoteSystemReferenceNo { get; set; }
		public virtual string ForeignRemoteSystemCounterId { get; set; }
		public virtual string InstantNotificationAddlServiceCharges { get; set; }
		public virtual short InstantNotificationAddlServiceLength { get; set; }

		public virtual string RequestXml { get; set; }
		public virtual string ResponseXml { get; set; }
		public virtual string WuCardTotalPointsEarned { get; set; }
		public virtual string MessageArea { get; set; }
	}
}
