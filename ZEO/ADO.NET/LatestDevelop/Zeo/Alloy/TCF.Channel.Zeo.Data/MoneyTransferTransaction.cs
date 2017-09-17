using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class MoneyTransferTransaction
    {
        [DataMember]
        public long ReceiverId { get; set; }
        [DataMember]
        public long WUAccountId { get; set; }
        [DataMember]
        public decimal OriginatorsPrincipalAmount { get; set; }
        [DataMember]
        public string OriginatingCountryCode { get; set; }
        [DataMember]
        public string OriginatingCurrencyCode { get; set; }
        [DataMember]
        public string TranascationType { get; set; }
        [DataMember]
        public string PromotionsCode { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal DestinationPrincipalAmount { get; set; }
        [DataMember]
        public decimal GrossTotalAmount { get; set; }
        [DataMember]
        public decimal Charges { get; set; }
        // Fees
        [DataMember]
        public decimal TaxAmount { get; set; }
        [DataMember]
        public decimal PromotionDiscount { get; set; }
        [DataMember]
        public decimal OtherCharges { get; set; } // Other Fees
        [DataMember]
        public string MoneyTransferKey { get; set; }
        [DataMember]
        public string MTCN { get; set; }
        [DataMember]
        public string TempMTCN { get; set; }
        [DataMember]
        public decimal AdditionalCharges { get; set; }// Additional Fees
        [DataMember]
        public string DestinationCountryCode { get; set; }
        [DataMember]
        public string DestinationCurrencyCode { get; set; }
        [DataMember]
        public string DestinationState { get; set; }
        [DataMember]
        public bool IsDomesticTransfer { get; set; }
        [DataMember]
        public bool IsFixedOnSend { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string AgencyName { get; set; }
        [DataMember]
        public long ChannelPartnerId { get; set; }
        [DataMember]
        public int ProviderId { get; set; }
        [DataMember]
        public string ExpectedPayoutStateCode { get; set; }
        [DataMember]
        public string ExpectedPayoutCityName { get; set; }
        [DataMember]
        public string TestQuestion { get; set; }
        [DataMember]
        public string TestAnswer { get; set; }
        [DataMember]
        public string TestQuestionAvaliable { get; set; }
        [DataMember]
        public string GCNumber { get; set; }
        [DataMember]
        public string SenderName { get; set; }
        [DataMember]
        public bool PdsRequiredFlag { get; set; }
        [DataMember]
        public bool DfTransactionFlag { get; set; }
        [DataMember]
        public string DeliveryServiceName { get; set; }
        [DataMember]
        public Nullable<DateTime> DTAvailableForPickup { get; set; }
        [DataMember]
        public string ReceiverFirstName { get; set; }
        [DataMember]
        public string ReceiverLastName { get; set; }
        [DataMember]
        public string ReceiverSecondLastName { get; set; }
        [DataMember]
        public string PromoCodeDescription { get; set; }
        [DataMember]
        public string PromoName { get; set; }
        [DataMember]
        public string PromoMessage { get; set; }
        [DataMember]
        public string PromotionError { get; set; }
        [DataMember]
        public string SenderComplianceDetailsComplianceDataBuffer { get; set; }
        [DataMember]
        public decimal municipal_tax { get; set; }
        [DataMember]
        public decimal state_tax { get; set; }
        [DataMember]
        public decimal county_tax { get; set; }
        [DataMember]
        public decimal plus_charges_amount { get; set; }
        [DataMember]
        public decimal message_charge { get; set; }
        [DataMember]
        public decimal total_undiscounted_charges { get; set; }
        [DataMember]
        public decimal total_discount { get; set; }
        [DataMember]
        public decimal total_discounted_charges { get; set; }
        [DataMember]
        public string instant_notification_addl_service_charges { get; set; }
        [DataMember]
        public string recordingCountryCode { get; set; }
        [DataMember]
        public string recordingCurrencyCode { get; set; }
        [DataMember]
        public string originating_city { get; set; }
        [DataMember]
        public string originating_state { get; set; }
        [DataMember]
        public decimal PaySideCharges { get; set; }
        [DataMember]
        public decimal PaySideTax { get; set; }
        [DataMember]
        public decimal AmountToReceiver { get; set; }
        [DataMember]
        public string SMSNotificationFlag { get; set; }
        [DataMember]
        public string PersonalMessage { get; set; }
        [DataMember]
        public string DeliveryServiceDesc { get; set; }
        [DataMember]
        public string ReferenceNo { get; set; }
        [DataMember]
        public string WuCardTotalPointsEarned { get; set; }
        [DataMember]
        public long OriginalTransactionID { get; set; }
        [DataMember]
        public string TransactionSubType { get; set; }
        [DataMember]
        public string ReasonCode { get; set; }
        [DataMember]
        public string ReasonDescription { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string DeliveryOption { get; set; }
        [DataMember]
        public string DeliveryOptionDesc { get; set; }
        [DataMember]
        public string pay_or_do_not_pay_indicator { get; set; }
        [DataMember]
        public string OriginalDestinationCountryCode { get; set; }
        [DataMember]
        public string OriginalDestinationCurrencyCode { get; set; }
        [DataMember]
        public string FilingDate { get; set; }
        [DataMember]
        public string FilingTime { get; set; }
        [DataMember]
        public string PaidDateTime { get; set; }
        [DataMember]
        public string AvailableForPickup { get; set; }
        [DataMember]
        public string DelayHours { get; set; }
        [DataMember]
        public string AvailableForPickupEST { get; set; }
        [DataMember]
        public string PromotionSequenceNo { get; set; }
        [DataMember]
        public string CounterId { get; set; }
        [DataMember]
        public string Sender_unv_Buffer { get; set; }
        [DataMember]
        public string Receiver_unv_Buffer { get; set; }
        [DataMember]
        public decimal Principal_Amount { get; set; }
        [DataMember]
        public string TransalatedDeliveryServiceName { get; set; }
        [DataMember]
        public string MessageArea { get; set; }

        [DataMember]
        public bool IsModifiedOrRefunded { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public Dictionary<string, object> MetaData { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string Country { get; set; }
    }
}
