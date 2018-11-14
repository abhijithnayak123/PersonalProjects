using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class WUTransaction : ZeoModel
    {
        public long ReceiverId { get; set; }
        public long WUAccountId { get; set; }
        public decimal OriginatorsPrincipalAmount { get; set; }
        public string OriginatingCountryCode { get; set; }
        public string OriginatingCurrencyCode { get; set; }
        public string TranascationType { get; set; }
        public string PromotionsCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal DestinationPrincipalAmount { get; set; }
        public decimal GrossTotalAmount { get; set; }
        public decimal Charges { get; set; }  // Fees
        public decimal TaxAmount { get; set; }
        public decimal PromotionDiscount { get; set; }
        public decimal OtherCharges { get; set; } // Other Fees
        public string MoneyTransferKey { get; set; }
        public string MTCN { get; set; }
        public string TempMTCN { get; set; }
        public decimal AdditionalCharges { get; set; }// Additional Fees
        public string DestinationCountryCode { get; set; }
        public string DestinationCurrencyCode { get; set; }
        public string DestinationState { get; set; }
        public bool IsDomesticTransfer { get; set; }
        public bool IsFixedOnSend { get; set; }
        public string PhoneNumber { get; set; }
        public string Url { get; set; }
        public string AgencyName { get; set; }
        public long ChannelPartnerId { get; set; }
        public int ProviderId { get; set; }
        public string ExpectedPayoutStateCode { get; set; }
        public string ExpectedPayoutCityName { get; set; }
        public string TestQuestion { get; set; }
        public string TestAnswer { get; set; }
        public string TestQuestionAvaliable { get; set; }
        public string GCNumber { get; set; }
        public string SenderName { get; set; }
        public bool PdsRequiredFlag { get; set; }
        public bool DfTransactionFlag { get; set; }
        public string DeliveryServiceName { get; set; }
        public Nullable<DateTime> DTAvailableForPickup { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverSecondLastName { get; set; }
        public string PromoCodeDescription { get; set; }
        public string PromoName { get; set; }
        public string PromoMessage { get; set; }
        public string PromotionError { get; set; }
        public string SenderComplianceDetailsComplianceDataBuffer { get; set; }
        public decimal municipal_tax { get; set; }
        public decimal state_tax { get; set; }
        public decimal county_tax { get; set; }
        public decimal plus_charges_amount { get; set; }
        public decimal message_charge { get; set; }
        public decimal total_undiscounted_charges { get; set; }
        public decimal total_discount { get; set; }
        public decimal total_discounted_charges { get; set; }
        public string instant_notification_addl_service_charges { get; set; }
        public string recordingCountryCode { get; set; }
        public string recordingCurrencyCode { get; set; }
        public string originating_city { get; set; }
        public string originating_state { get; set; }
        public decimal PaySideCharges { get; set; }
        public decimal PaySideTax { get; set; }
        public decimal AmountToReceiver { get; set; }
        public string SMSNotificationFlag { get; set; }
        public string PersonalMessage { get; set; }
        public string DeliveryServiceDesc { get; set; }
        public string ReferenceNo { get; set; }
        public string WuCardTotalPointsEarned { get; set; }
        public long OriginalTransactionID { get; set; }
        public string TransactionSubType { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDescription { get; set; }
        public string Comments { get; set; }
        public string DeliveryOption { get; set; }
        public string DeliveryOptionDesc { get; set; }
        public string pay_or_do_not_pay_indicator { get; set; }
        public string OriginalDestinationCountryCode { get; set; }
        public string OriginalDestinationCurrencyCode { get; set; }
        public string FilingDate { get; set; }
        public string FilingTime { get; set; }
        public string PaidDateTime { get; set; }
        public string AvailableForPickup { get; set; }
        public string DelayHours { get; set; }
        public string AvailableForPickupEST { get; set; }
        public string PromotionSequenceNo { get; set; }
        public string CounterId { get; set; }
        public string Sender_unv_Buffer { get; set; }
        public string Receiver_unv_Buffer { get; set; }
        public decimal Principal_Amount { get; set; }
        public string TransalatedDeliveryServiceName { get; set; }
        public string MessageArea { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ConsumerFraudPromptQuestion { get; set; }
    }
}
