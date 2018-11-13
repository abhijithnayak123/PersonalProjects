using MGI.Common.DataAccess.Data;
using System;


namespace MGI.Cxn.MoneyTransfer.MG.Data
{
    public class Transaction : NexxoModel
    {
        public virtual Receiver Recipient { get; set; }

        public virtual decimal Amount { get; set; }
        public virtual decimal FeeAmount { get; set; }
        public virtual string DestinationCountry { get; set; }
        public virtual string DestinationState { get; set; }
        public virtual string DeliveryOption { get; set; }

        //Sender Details
        public virtual string SenderFirstName { get; set; }
        public virtual string SenderLastName { get; set; }
        public virtual string SenderAddress { get; set; }
        public virtual string SenderCity { get; set; }
        public virtual string SenderState { get; set; }
        public virtual string SenderZipCode { get; set; }
        public virtual string SenderCountry { get; set; }
        public virtual string SenderHomePhone { get; set; }

        //Receiver Details
        public virtual string ReceiverFirstName { get; set; }
        public virtual string ReceiverLastName { get; set; }
        public virtual string ReceiverCountry { get; set; }
        public virtual string ReceiverNickName { get; set; }
        public virtual string PrimaryReceiptLanguage { get; set; }
        public virtual string SecondaryReceiptLanguage { get; set; }
        public virtual string PromoCodeValues_PromoCode { get; set; }

        public virtual decimal GrossTotalAmount { get; set; }
        public virtual string TransactionType { get; set; }

        public virtual string MgiTransactionSessionId { get; set; }

        public virtual long ChannelPartnerId { get; set; }
        public virtual int ProviderId { get; set; }

        


        //For Feeinquiry purpose has to revisit again 
        public virtual decimal TaxAmount { get; set; }
        public virtual decimal ReceiveAmount { get; set; }
        public virtual decimal ExchangeRate { get; set; }
        public virtual decimal PromotionDiscount { get; set; }
        public virtual string PromotionErrorCode { get; set; }

        public virtual decimal EstimatedReceiveAmount { get; set; }
        public virtual decimal EstimatedExchangeRate { get; set; }
        public virtual bool ReceiveAmountAltered { get; set; }
        public virtual bool RevisedInformationalFee { get; set; }
        public virtual string DeliveryOptionDesc { get; set; }
        public virtual bool SendAmountAltered { get; set; }
        public virtual string PromotionDiscountId { get; set; }
        public virtual string PromotionCategoryId { get; set; }
        public virtual decimal PromotionDiscountAmount { get; set; }
        public virtual decimal TotalSendFees { get; set; }
        public virtual decimal TotalDiscountAmount { get; set; }
        public virtual decimal TotalSendTaxes { get; set; }
        public virtual decimal TotalAmountToCollect { get; set; }
        public virtual decimal DetailSendAmounts_mgiNonDiscountedSendFee { get; set; }
        public virtual decimal DetailSendAmounts_totalNonDiscountedFees { get; set; }
        public virtual decimal DetailSendAmounts_discountedMgiSendFee { get; set; }
        public virtual decimal DetailSendAmounts_mgiSendTax { get; set; }
        public virtual decimal DetailSendAmounts_totalMgiCollectedFeesAndTaxes { get; set; }
        public virtual decimal DetailSendAmounts_totalAmountToMgi { get; set; }
        public virtual decimal DetailSendAmounts_totalSendFeesAndTaxes { get; set; }
        public virtual decimal DetailSendAmounts_totalNonMgiSendFeesAndTaxes { get; set; }
        public virtual decimal DetailSendAmounts_nonMgiSendTax { get; set; }
        public virtual decimal DetailSendAmounts_nonMgiSendFee { get; set; }
        public virtual bool ValidCurrencyIndicator { get; set; }
        public virtual string PayoutCurrency { get; set; }
        public virtual decimal TotalReceiveFees { get; set; }
        public virtual decimal TotalReceiveTaxes { get; set; }
        public virtual decimal TotalReceiveAmount { get; set; }
        public virtual bool ReceiveFeesAreEstimated { get; set; }
        public virtual bool ReceiveTaxesAreEstimated { get; set; }

        public virtual decimal DetailReceiveAmounts_nonMgiReceiveFee { get; set; }
        public virtual decimal DetailReceiveAmounts_nonMgiReceiveTax { get; set; }
        public virtual decimal DetailReceiveAmounts_mgiReceiveFee { get; set; }
        public virtual decimal DetailReceiveAmounts_mgiReceiveTax { get; set; }

        public virtual decimal DetailEstimatedReceiveAmounts_nonMgiReceiveFee { get; set; }
        public virtual decimal DetailEstimatedReceiveAmounts_nonMgiReceiveTax { get; set; }
        public virtual decimal DetailEstimatedReceiveAmounts_mgiReceiveFee { get; set; }
        public virtual decimal DetailEstimatedReceiveAmounts_mgiReceiveTax { get; set; }

        public virtual string AgentCustomerNumber { get; set; }
        public virtual string ReceiveCurrency { get; set; }
        public virtual DateTime? SenderDOB { get; set; }
        public virtual string SendCurrency { get; set; }
        public virtual string ConsumerId { get; set; }
        public virtual bool FormFreeStaging { get; set; }
        public virtual bool ReadyForCommit { get; set; }
        public virtual string ReceiveAgentAddress { get; set; }
        public virtual decimal ExchangeRateApplied { get; set; }
        public virtual bool ReceiveFeeDisclosureText { get; set; }
        public virtual bool ReceiveTaxDisclosureText { get; set; }

        public virtual string ReferenceNumber { get; set; }
        public virtual string PartnerConfirmationNumber { get; set; }
        public virtual string FreePhoneCallPIN { get; set; }
        public virtual string TollFreePhoneNumber { get; set; }
        public virtual DateTime? ExpectedDateOfDelivery { get; set; }
        public virtual DateTime? TransactionDateTime { get; set; }
        public virtual string StateRegulatorVersion { get; set; }

        public virtual string ReceiveAgentID { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string CustomerReceiveNumber { get; set; }
        public virtual string SenderMiddleName { get; set; }
        public virtual string SenderLastName2 { get; set; }
        public virtual string SenderAddress2 { get; set; }
        public virtual string SenderAddress3 { get; set; }
        public virtual string ReceiverMiddleName { get; set; }
        public virtual string ReceiverLastName2 { get; set; }
        public virtual string ReceiverAddress { get; set; }
        public virtual string ReceiverAddress2 { get; set; }
        public virtual string ReceiverAddress3 { get; set; }

        public virtual string Direction1 { get; set; }
        public virtual string Direction2 { get; set; }
        public virtual string Direction3 { get; set; }
        public virtual string ReceiverCity { get; set; }
        public virtual string ReceiverState { get; set; }
        public virtual string ReceiverZipCode { get; set; }
        public virtual string ReceiverPhone { get; set; }
        public virtual string TestQuestion { get; set; }
        public virtual string TestAnswer { get; set; }
        public virtual string MessageField1 { get; set; }
        public virtual string MessageField2 { get; set; }
        public virtual string SenderPhotoIdType { get; set; }
        public virtual string SenderPhotoIdNumber { get; set; }
        public virtual string SenderPhotoIdState { get; set; }
        public virtual string SenderPhotoIdCountry { get; set; }
        public virtual string SenderLegalIdType { get; set; }
        public virtual string SenderLegalIdNumber { get; set; }
        public virtual string SenderOccupation { get; set; }
        public virtual string SenderBirthCity { get; set; }
        public virtual string SenderBirthCountry { get; set; }
        public virtual string SenderPassportIssueDate { get; set; }
        public virtual string SenderPassportIssueCity { get; set; }
        public virtual string SenderPassportIssueCountry { get; set; }
        public virtual string SenderLegalIdIssueCountry { get; set; }
        public virtual string SenderEmailAddress { get; set; }
        public virtual string SenderMobilePhone { get; set; }
        public virtual string MarketingOptIn { get; set; }
        public virtual string PcTerminalNumber { get; set; }
        public virtual string AgentUseSendData { get; set; }
        public virtual DateTime? SenderPhotoIdExpDate { get; set; }
        public virtual DateTime? SenderPhotoIdIssueDate { get; set; }
        public virtual bool SenderPhotoIdStored { get; set; }
        public virtual string SenderNationalityCountry { get; set; }
        public virtual string SenderNationalityAtBirthCountry { get; set; }
        public virtual string AgentTransactionId { get; set; }
        public virtual string TimeToLive { get; set; }


        public virtual bool IsDomesticTransfer { get; set; }
        public virtual long ReceiverId { get; set; }
        public virtual long AccountId { get; set; }
        public virtual Int16 RequestResponseType { get; set; }

        public virtual string AgentID { get; set; }
        public virtual string AgentSequence { get; set; }
        public virtual string Token { get; set; }
        public virtual System.Nullable<System.DateTime> RequestTimeStamp { get; set; }
        public virtual string ApiVersion { get; set; }
        public virtual string ClientSoftwareVersion { get; set; }
        public virtual System.Nullable<bool> DoCheckIn { get; set; }
        public virtual System.Nullable<System.DateTime> ResponseTimeStamp { get; set; }
        public virtual System.Nullable<int> Flags { get; set; }
        public virtual string PromotionalMessage_PrimaryLanguage { get; set; }
        public virtual string PromotionalMessage_SecondaryLanguage { get; set; }

        public virtual string PartnerName { get; set; }
        public virtual string ReceiptTextInfo_PrimaryLanguage { get; set; }
        public virtual string ReceiptTextInfo_SecondaryLanguage { get; set; }

        public virtual string AccountNumberLastFour { get; set; }
        public virtual string CustomerServiceMessage { get; set; }
        public virtual string AccountNickname { get; set; }
        public virtual string PromotionErrorCodeMessage_PrimaryLanguage { get; set; }
        public virtual string PromotionErrorCodeMessage_SecondaryLanguage { get; set; }
        public virtual string DisclosureText_PrimaryLanguage { get; set; }
        public virtual string DisclosureText_SecondaryLanguage { get; set; }
        public virtual string ReceiveAgentName { get; set; }
        public virtual string DynamicFields { get; set; }

        //additional fields For Recieve Money 

        public virtual string SenderAddress4 { get; set; }
        public virtual string ReceiverAddress4 { get; set; }
        public virtual string AgentCheckNumber { get; set; }
        public virtual decimal AgentCheckAmount { get; set; }
        public virtual string AgentCheckAuthorizationNumber { get; set; }
        public virtual string CustomerCheckNumber { get; set; }
        public virtual decimal CustomerCheckAmount { get; set; }
        public virtual bool OkForAgent { get; set; }
        public virtual string TransactionStatus { get; set; }
        public virtual DateTime? DateTimeSent { get; set; }
        public virtual string OriginatingCountry { get; set; }
        public virtual bool ValidIndicator { get; set; }
        public virtual decimal IndicativeReceiveAmount { get; set; }
        public virtual string IndicativeReceiveCurrency { get; set; }
        public virtual decimal IndicativeExchangeRate { get; set; }
        public virtual string ReceiveAgentAbbreviation { get; set; }
        public virtual decimal OriginalSendAmount { get; set; }
        public virtual string OriginalSendCurrency { get; set; }
        public virtual decimal OriginalSendFee { get; set; }
        public virtual decimal OriginalExchangeRate { get; set; }
        public virtual bool RedirectIndicator { get; set; }
        public virtual decimal RedirectInfoOriginalSendAmount { get; set; }
        public virtual string RedirectInfoOriginalSendCurrency { get; set; }
        public virtual decimal RedirectInfoOriginalSendFee { get; set; }
        public virtual decimal RedirectInfoOriginalExchangeRate { get; set; }
        public virtual decimal RedirectInfoOriginalReceiveAmount { get; set; }
        public virtual string RedirectInfoOriginalReceiveCurrency { get; set; }
        public virtual string RedirectInfoOriginalReceiveCountry { get; set; }
        public virtual decimal RedirectInfoNewExchangeRate { get; set; }
        public virtual decimal RedirectInfoNewReceiveAmount { get; set; }
        public virtual string RedirectInfoNewReceiveCurrency { get; set; }
        public virtual string RedirectInfoRedirectType { get; set; }
        public virtual bool OkForPickup { get; set; }
        public virtual string NotOkForPickupReasonCode { get; set; }
        public virtual string NotOkForPickupReasonDescription { get; set; }
        public virtual string MinutesUntilOkForPickup { get; set; }

        public virtual string ReceiveMoneySearchRefNo { get; set; }
        public virtual string TransactionSubType { get; set; }
        public virtual long OriginalTransactionID { get; set; } 

        public virtual string ReceiverPhotoIdType { get; set; }
        public virtual string ReceiverPhotoIdNumber { get; set; }
        public virtual string ReceiverPhotoIdState { get; set; }
        public virtual string ReceiverPhotoIdCountry { get; set; }
        public virtual string ReceiverLegalIdType { get; set; }
        public virtual string ReceiverLegalIdNumber { get; set; }
        public virtual DateTime? ReceiverDOB { get; set; }
        public virtual string ReceiverOccupation { get; set; }
        public virtual string ReceiverBirthCountry { get; set; }

        public virtual bool TransactionSucceeded { get; set; }

        public virtual string IsTestQusAndAnsRequired { get; set; }

        public virtual bool IsReceiverHasPhotoId { get; set; }

        //Refund Related Cols
        public virtual int SendReversalType { get; set; }
	    public virtual int? SendReversalReason { get; set; }
	    public virtual string FeeRefund { get; set; }
	    public virtual string OperatorName { get; set; }
	    public virtual decimal RefundTotalAmount { get; set; }
	    public virtual decimal RefundFaceAmount { get; set; }
        public virtual decimal RefundFeeAmount { get; set; }
    }
}
