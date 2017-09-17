using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;


namespace MGI.Cxn.MoneyTransfer.WU.Data
{
	public class WUTransaction : NexxoModel
	{
		public virtual WUReceiver WUnionRecipient { get; set; }

		public virtual WUAccount WUnionAccount { get; set; }

		public virtual decimal OriginatorsPrincipalAmount { get; set; }
		public virtual string OriginatingCountryCode { get; set; }
		public virtual string OriginatingCurrencyCode { get; set; }
		public virtual string TranascationType { get; set; }
		public virtual string PromotionsCode { get; set; }
		public virtual decimal ExchangeRate { get; set; }
		public virtual decimal DestinationPrincipalAmount { get; set; }
		public virtual decimal GrossTotalAmount { get; set; }
		public virtual decimal Charges { get; set; }  // Fees
		public virtual decimal TaxAmount { get; set; }
		public virtual decimal PromotionDiscount { get; set; }

		public virtual decimal OtherCharges { get; set; } // Other Fees
		public virtual string MoneyTransferKey { get; set; }
		public virtual string MTCN { get; set; }
		public virtual string TempMTCN { get; set; }

		public virtual decimal AdditionalCharges { get; set; }// Additional Fees
		public virtual string DestinationCountryCode { get; set; }
		public virtual string DestinationCurrencyCode { get; set; }
		public virtual string DestinationState { get; set; }

		public virtual bool IsDomesticTransfer { get; set; }
		public virtual bool IsFixedOnSend { get; set; }


		public virtual string PhoneNumber { get; set; }
		public virtual string Url { get; set; }
		public virtual string AgencyName { get; set; }
		public virtual long ChannelPartnerId { get; set; }
		public virtual int ProviderId { get; set; }
		public virtual string ExpectedPayoutStateCode { get; set; }
		public virtual string ExpectedPayoutCityName { get; set; }

		public virtual string TestQuestion { get; set; }
		public virtual string TestAnswer { get; set; }
		public virtual string TestQuestionAvaliable { get; set; }
		public virtual string GCNumber { get; set; }
		public virtual string SenderName { get; set; }

		public virtual bool PdsRequiredFlag { get; set; }
		public virtual bool DfTransactionFlag { get; set; }
		public virtual string DeliveryServiceName { get; set; }
		public virtual Nullable<DateTime> DTAvailableForPickup { get; set; }

		public virtual string RecieverFirstName { get; set; }
		public virtual string RecieverLastName { get; set; }
		public virtual string RecieverSecondLastName { get; set; }

		public virtual string PromoCodeDescription { get; set; }
		public virtual string PromoName { get; set; }
		public virtual string PromoMessage { get; set; }
		public virtual string PromotionError { get; set; }

		public virtual string SenderComplianceDetailsComplianceDataBuffer { get; set; }

		public virtual decimal municipal_tax { get; set; }
		public virtual decimal state_tax { get; set; }
		public virtual decimal county_tax { get; set; }
		public virtual decimal plus_charges_amount { get; set; }
		public virtual decimal message_charge { get; set; }
		public virtual decimal total_undiscounted_charges { get; set; }
		public virtual decimal total_discount { get; set; }
		public virtual decimal total_discounted_charges { get; set; }
		public virtual string instant_notification_addl_service_charges { get; set; }

		public virtual string recordingCountryCode { get; set; }
		public virtual string recordingCurrencyCode { get; set; }
		public virtual string originating_city { get; set; }
		public virtual string originating_state { get; set; }

		public virtual decimal PaySideCharges { get; set; }
		public virtual decimal PaySideTax { get; set; }
		public virtual decimal AmountToReceiver { get; set; }
		public virtual string SMSNotificationFlag { get; set; }

		public virtual string PersonalMessage { get; set; }
		public virtual string DeliveryServiceDesc { get; set; }
		public virtual string ReferenceNo { get; set; }
		public virtual string WuCardTotalPointsEarned { get; set; }
		public virtual long OriginalTransactionID { get; set; }
		public virtual string TransactionSubType { get; set; }
		public virtual string ReasonCode { get; set; }
		public virtual string ReasonDescription { get; set; }
		public virtual string Comments { get; set; }
		public virtual string DeliveryOption { get; set; }
		public virtual string DeliveryOptionDesc { get; set; }
		public virtual string pay_or_do_not_pay_indicator { get; set; }
		public virtual string OriginalDestinationCountryCode { get; set; }
		public virtual string OriginalDestinationCurrencyCode { get; set; }
		public virtual string FilingDate { get; set; }
		public virtual string FilingTime { get; set; }
		public virtual string PaidDateTime { get; set; }
		public virtual string AvailableForPickup { get; set; }
		public virtual string DelayHours { get; set; }
		public virtual string AvailableForPickupEST { get; set; }
		public virtual string PromotionSequenceNo { get; set; }
		public virtual string CounterId { get; set; }
		public virtual string Sender_unv_Buffer { get; set; }
		public virtual string Receiver_unv_Buffer { get; set; }
        public virtual decimal Principal_Amount { get; set; }
        public virtual string TransalatedDeliveryServiceName { get; set; }
		public virtual string MessageArea { get; set; }
	}
}
