using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Cxn.MoneyTransfer.Data;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Data
{
    public class CommitRequest : BaseRequest
    {
        public string ReferenceNo { get; set; }
        public string TranascationType { get; set; }
        public string MTCN { get; set; }
        public string TempMTCN { get; set; }
        public string MoneyTransferKey { get; set; }
        public string SenderComplianceDetailsComplianceDataBuffer { get; set; }
        public string TestQuestion { get; set; }
        public string TestAnswer { get; set; }
        public decimal GrossTotalAmount { get; set; }
        public decimal AmountToReceiver { get; set; }
        public decimal DestinationPrincipalAmount { get; set; }
        public decimal Charges { get; set; }
        public string DestinationCountryCode { get; set; }
        public string DestinationCurrencyCode { get; set; }
        public string ExpectedPayoutStateCode { get; set; }
        public string ExpectedPayoutCityName { get; set; }
        public decimal ExchangeRate { get; set; }
        public string originating_city { get; set; }
        public string ReceiverSecondLastName { get; set; }
        public string PromotionSequenceNo { get; set; }
        public string PromotionsCode { get; set; }
        public string PromoCodeDescription { get; set; }
        public string PromoName { get; set; }
        public decimal PromotionDiscount { get; set; }
        public string PromoMessage { get; set; }
        public string DeliveryOption { get; set; }
        public string DeliveryServiceName { get; set; }
        public decimal municipal_tax { get; set; }
        public decimal state_tax { get; set; }
        public decimal county_tax { get; set; }
        public decimal plus_charges_amount { get; set; }
        public decimal PaySideCharges { get; set; }
        public decimal PaySideTax { get; set; }
        public string DeliveryServiceDesc { get; set; }
        public bool PdsRequiredFlag { get; set; }
        public bool DfTransactionFlag { get; set; }
        public decimal message_charge { get; set; }
        public decimal OriginatorsPrincipalAmount { get; set; }
        public decimal total_discount { get; set; }
        public decimal total_discounted_charges { get; set; }
        public decimal total_undiscounted_charges { get; set; }
        public string SmsNotificationFlag { get; set; }
        public string PersonalMessage { get; set; }
        public string instant_notification_addl_service_charges { get; set; }
        public string PreferredCustomerAccountNumber { get; set; }
        public string GCNumber { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverContactNumber { get; set; }
    }
}
