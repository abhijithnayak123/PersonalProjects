using TCF.Zeo.Common.Data;
using System;

namespace TCF.Zeo.Cxn.BillPay.WU.Data
{
    public class WUTransaction : ZeoModel
    {
        public long WUAccountId { get; set; }
        public int ProviderId { get; set; }
        public string ChannelType { get; set; }
        public string ChannelName { get; set; }
        public string ChannelVersion { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastname { get; set; }
        public string SenderAddressLine1 { get; set; }
        public string SenderCity { get; set; }
        public string SenderState { get; set; }
        public string SenderPostalCode { get; set; }
        public string SenderCountryCode { get; set; }
        public string SenderCurrencyCode { get; set; }
        public string SenderAddressLine2 { get; set; }
        public string SenderStreet { get; set; }
        public string WesternUnionCardNumber { get; set; }
        public string LevelCode { get; set; }
        public string SenderEmail { get; set; }
        public string SenderContactPhone { get; set; }
        public string SenderDateOfBirth { get; set; }
        public string BillerName { get; set; }
        public string BillerCityCode { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string FinancialsMunicipalTax { get; set; }
        public string FinancialsStateTax { get; set; }
        public string FinancialsCountTax { get; set; }
        public decimal FinancialsOriginatorsPrincipalAmount { get; set; }
        public decimal FinancialsDestinationPrincipalAmount { get; set; }
        public decimal FinancialsFee { get; set; }
        public decimal FinancialsGrossTotalAmount { get; set; }
        public decimal FinancialsTotal { get; set; }
        public decimal FinancialsUndiscountedCharges { get; set; }
        public decimal FinancialsDiscountedCharges { get; set; }
        public Nullable<decimal> FinancialsPlusChargesAmount { get; set; }
        public Nullable<decimal> FinancialsTotalDiscount { get; set; }

        public string PaymentDetailsRecordingCountryCode { get; set; }
        public string PaymentDetailsRecordingCountryCurrency { get; set; }
        public string PaymentDetailsDestinationCountryCode { get; set; }
        public string PaymentDetailsDestinationCountryCurrency { get; set; }
        public string PaymentDetailsOriginatingCountryCode { get; set; }
        public string PaymentDetailsOriginatingCountryCurrency { get; set; }
        public string PaymentDetailsOriginatingCity { get; set; }
        public string PaymentDetailsOriginatingState { get; set; }
        public string PaymentDetailsTransactionType { get; set; }
        public string PaymentDetailsPaymentType { get; set; }
        public double PaymentDetailsExchangeRate { get; set; }
        public string PaymentDetailsFixOnSend { get; set; }
        public string PaymentDetailsReceiptOptOut { get; set; }
        public string PaymentDetailsAuthStatus { get; set; }
        public string PromotionsCouponsPromotions { get; set; }
        public string PromotionsPromoCodeDescription { get; set; }
        public string PromotionsPromoSequenceNo { get; set; }
        public string PromotionsPromoName { get; set; }
        public string PromotionsPromoMessage { get; set; }
        public decimal PromotionsPromoDiscountAmount { get; set; }
        public string PromotionsPromotionError { get; set; }
        public string PromotionsSenderPromoCode { get; set; }
        public string SenderComplianceDetailsTemplateID { get; set; }
        public string SenderComplianceDetailsIdDetailsIdType { get; set; }
        public string SenderComplianceDetailsIdDetailsIdCountryOfIssue { get; set; }
        public string SenderComplianceDetailsIdDetailsIdPlaceOfIssue { get; set; }
        public string SenderComplianceDetailsIdDetailsIdNumber { get; set; }
        public string SenderComplianceDetailsSecondIDIdType { get; set; }
        public string SenderComplianceDetailsSecondIDIdCountryOfIssue { get; set; }
        public string SenderComplianceDetailsSecondIDIdNumber { get; set; }
        public string SenderComplianceDetailsDateOfBirth { get; set; }
        public string SenderComplianceDetailsOccupation { get; set; }
        public string SenderComplianceDetailsCurrentAddressAddrLine1 { get; set; }
        public string SenderComplianceDetailsCurrentAddressAddrLine2 { get; set; }
        public string SenderComplianceDetailsCurrentAddressCity { get; set; }
        public string SenderComplianceDetailsCurrentAddressStateCode { get; set; }
        public string SenderComplianceDetailsCurrentAddressPostalCode { get; set; }
        public string SenderComplianceDetailsContactPhone { get; set; }
        public string SenderComplianceDetailsIActOnMyBehalf { get; set; }
        public string SenderComplianceDetailsAckFlag { get; set; }
        public string SenderComplianceDetailsComplianceDataBuffer { get; set; }
        public string QPCompanyDepartment { get; set; }
        public string FillingDate { get; set; }
        public string FillingTime { get; set; }
        public string Mtcn { get; set; }
        public string NewMTCN { get; set; }
        public string DfFieldsPdsrequiredflag { get; set; }
        public string DfFieldsTransactionFlag { get; set; }
        public string DfFieldsDeliveryServiceName { get; set; }
        public string DeliveryCode { get; set; }
        public string FusionScreen { get; set; }
        public string ConvSessionCookie { get; set; }
        public string ForeignRemoteSystemIdentifier { get; set; }
        public string ForeignRemoteSystemReferenceNo { get; set; }
        public string ForeignRemoteSystemCounterId { get; set; }
        public string InstantNotificationAddlServiceCharges { get; set; }
        public short InstantNotificationAddlServiceLength { get; set; }
        public string RequestXml { get; set; }
        public string ResponseXml { get; set; }
        public string WuCardTotalPointsEarned { get; set; }
        public string MessageArea { get; set; }
    }
}
