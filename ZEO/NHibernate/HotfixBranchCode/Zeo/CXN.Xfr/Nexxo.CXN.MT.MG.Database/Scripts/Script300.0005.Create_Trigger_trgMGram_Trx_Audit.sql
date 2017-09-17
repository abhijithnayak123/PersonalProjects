-- ============================================================
-- Author:		<Swarnalakshmi>
-- Create date: <07/29/2014>
-- Description:	<CREATE Trigger for MoneyGram Transfer Transaction Audit Table> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgMGram_Transfer_TrxAud]'))
DROP TRIGGER [dbo].[trgMGram_Transfer_TrxAud]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trgMGram_Transfer_TrxAud] 
ON [dbo].[tMGram_Transfer_Trx] 
AFTER INSERT, UPDATE, DELETE
AS
	SET NOCOUNT ON
	
	DECLARE @RevisionNo BIGINT

	SELECT 
		@RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 
	FROM 
		[tMGram_Transfer_Trx_Aud] 
	WHERE 
		Id = (SELECT Id FROM inserted)
             
   IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
   BEGIN
		INSERT INTO tMGram_Transfer_Trx_Aud(
	   [rowguid],[Id] ,[Amount] ,[FeeAmount] ,[DestinationCountry] ,[DestinationState]  ,[DeliveryOption]  ,[SenderFirstName]
      ,[SenderLastName]    ,[SenderAddress] ,[SenderCity] ,[SenderState]  ,[SenderZipCode]  ,[SenderCountry]  ,[SenderHomePhone]
      ,[ReceiverFirstName]  ,[ReceiverLastName],[ReceiverCountry],[PrimaryReceiptLanguage]  ,[SecondaryReceiptLanguage]
      ,[PromoCodeValues_PromoCode] ,[GrossTotalAmount],[TransactionType] ,[ChannelPartnerId],[ProviderId],[MgiTransactionSessionId]
      ,[ExchangeRate],[PromotionDiscount],[PromotionErrorCodeMessage_PrimaryLanguage] ,[PromotionErrorCode],[TaxAmount]
      ,[ReceiveAmount],[DTCreate],[DTLastMod],[DTServerCreate] ,[DTServerLastMod]  ,[EstimatedReceiveAmount],[EstimatedExchangeRate]
      ,[ReceiveAmountAltered]   ,[RevisedInformationalFee] ,[DeliveryOptionDesc]  ,[SendAmountAltered]
      ,[PromotionDiscountId]
      ,[PromotionCategoryId]
      ,[PromotionDiscountAmount]
      ,[TotalSendFees]
      ,[TotalDiscountAmount]
      ,[TotalSendTaxes]
      ,[TotalAmountToCollect]
      ,[DetailSendAmounts_mgiNonDiscountedSendFee]
      ,[DetailSendAmounts_totalNonDiscountedFees]
      ,[DetailSendAmounts_discountedMgiSendFee]
      ,[DetailSendAmounts_mgiSendTax]
      ,[DetailSendAmounts_totalMgiCollectedFeesAndTaxes]
      ,[DetailSendAmounts_totalAmountToMgi]
      ,[DetailSendAmounts_totalSendFeesAndTaxes]
      ,[DetailSendAmounts_totalNonMgiSendFeesAndTaxes]
      ,[DetailSendAmounts_nonMgiSendTax]
      ,[DetailSendAmounts_nonMgiSendFee]
      ,[ValidCurrencyIndicator]
      ,[PayoutCurrency]
      ,[TotalReceiveFees]
      ,[TotalReceiveTaxes]
      ,[TotalReceiveAmount]
      ,[ReceiveFeesAreEstimated]
      ,[ReceiveTaxesAreEstimated]
      ,[DetailReceiveAmounts_nonMgiReceiveFee]
      ,[DetailReceiveAmounts_nonMgiReceiveTax]
      ,[DetailReceiveAmounts_mgiReceiveFee]
      ,[DetailReceiveAmounts_mgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveTax]
      ,[AgentCustomerNumber]
      ,[ReceiveCurrency]
      ,[SenderDOB]
      ,[SendCurrency]
      ,[ConsumerId]
      ,[FormFreeStaging]
      ,[ReadyForCommit]
      ,[ReceiveAgentAddress]
      ,[ExchangeRateApplied]
      ,[ReceiveFeeDisclosureText]
      ,[ReceiveTaxDisclosureText]
      ,[ReferenceNumber]
      ,[PartnerConfirmationNumber]
      ,[FreePhoneCallPIN]
      ,[TollFreePhoneNumber]
      ,[ExpectedDateOfDelivery]
      ,[TransactionDateTime]
      ,[StateRegulatorVersion]
      ,[ReceiveAgentID]
      ,[AccountNumber]
      ,[CustomerReceiveNumber]
      ,[SenderMiddleName]
      ,[SenderLastName2]
      ,[SenderAddress2]
      ,[SenderAddress3]
      ,[ReceiverMiddleName]
      ,[ReceiverLastName2]
      ,[ReceiverAddress]
      ,[ReceiverAddress2]
      ,[ReceiverAddress3]
      ,[Direction1]
      ,[Direction2]
      ,[Direction3]
      ,[ReceiverCity]
      ,[ReceiverState]
      ,[ReceiverZipCode]
      ,[ReceiverPhone]
      ,[TestQuestion]
      ,[TestAnswer]
      ,[MessageField1]
      ,[MessageField2]
      ,[SenderPhotoIdType]
      ,[SenderPhotoIdNumber]
      ,[SenderPhotoIdState]
      ,[SenderPhotoIdCountry]
      ,[SenderLegalIdType]
      ,[SenderLegalIdNumber]
      ,[SenderOccupation]
      ,[SenderBirthCity]
      ,[SenderBirthCountry]
      ,[SenderPassportIssueDate]
      ,[SenderPassportIssueCity]
      ,[SenderPassportIssueCountry]
      ,[SenderLegalIdIssueCountry]
      ,[SenderEmailAddress]
      ,[SenderMobilePhone]
      ,[MarketingOptIn]
      ,[PcTerminalNumber]
      ,[AgentUseSendData]
      ,[SenderPhotoIdExpDate]
      ,[SenderPhotoIdIssueDate]
      ,[SenderPhotoIdStored]
      ,[SenderNationalityCountry]
      ,[SenderNationalityAtBirthCountry]
      ,[AgentTransactionId]
      ,[TimeToLive]
      ,[IsDomesticTransfer]
      ,[ReceiverId]
      ,[AccountId]
      ,[RequestResponseType]
      ,[AgentID]
      ,[AgentSequence]
      ,[Token]
      ,[RequestTimeStamp]
      ,[ApiVersion]
      ,[ClientSoftwareVersion]
      ,[DoCheckIn]
      ,[ResponseTimeStamp]
      ,[Flags]
      ,[PromotionalMessage_PrimaryLanguage]
      ,[PromotionalMessage_SecondaryLanguage]
      ,[PartnerName]
      ,[ReceiptTextInfo_PrimaryLanguage]
      ,[ReceiptTextInfo_SecondaryLanguage]
      ,[AccountNumberLastFour]
      ,[CustomerServiceMessage]
      ,[AccountNickname]
      ,[PromotionErrorCodeMessage_SecondaryLanguage]
      ,[DisclosureText_PrimaryLanguage]
      ,[DisclosureText_SecondaryLanguage]
      ,[ReceiveAgentName]
      ,[DynamicFields]
      ,[AuditEvent]
      ,[DTAudit]
      ,[RevisionNo])
       
       SELECT	[rowguid],[Id] ,[Amount] ,[FeeAmount] ,[DestinationCountry] ,[DestinationState]  ,[DeliveryOption]  ,[SenderFirstName]
      ,[SenderLastName]    ,[SenderAddress] ,[SenderCity] ,[SenderState]  ,[SenderZipCode]  ,[SenderCountry]  ,[SenderHomePhone]
      ,[ReceiverFirstName]  ,[ReceiverLastName],[ReceiverCountry],[PrimaryReceiptLanguage]  ,[SecondaryReceiptLanguage]
      ,[PromoCodeValues_PromoCode] ,[GrossTotalAmount],[TransactionType] ,[ChannelPartnerId],[ProviderId],[MgiTransactionSessionId]
      ,[ExchangeRate],[PromotionDiscount],[PromotionErrorCodeMessage_PrimaryLanguage] ,[PromotionErrorCode],[TaxAmount]
      ,[ReceiveAmount],[DTCreate],[DTLastMod],[DTServerCreate] ,[DTServerLastMod]  ,[EstimatedReceiveAmount],[EstimatedExchangeRate]
      ,[ReceiveAmountAltered]   ,[RevisedInformationalFee] ,[DeliveryOptionDesc]  ,[SendAmountAltered]
      ,[PromotionDiscountId]
      ,[PromotionCategoryId]
      ,[PromotionDiscountAmount]
      ,[TotalSendFees]
      ,[TotalDiscountAmount]
      ,[TotalSendTaxes]
      ,[TotalAmountToCollect]
      ,[DetailSendAmounts_mgiNonDiscountedSendFee]
      ,[DetailSendAmounts_totalNonDiscountedFees]
      ,[DetailSendAmounts_discountedMgiSendFee]
      ,[DetailSendAmounts_mgiSendTax]
      ,[DetailSendAmounts_totalMgiCollectedFeesAndTaxes]
      ,[DetailSendAmounts_totalAmountToMgi]
      ,[DetailSendAmounts_totalSendFeesAndTaxes]
      ,[DetailSendAmounts_totalNonMgiSendFeesAndTaxes]
      ,[DetailSendAmounts_nonMgiSendTax]
      ,[DetailSendAmounts_nonMgiSendFee]
      ,[ValidCurrencyIndicator]
      ,[PayoutCurrency]
      ,[TotalReceiveFees]
      ,[TotalReceiveTaxes]
      ,[TotalReceiveAmount]
      ,[ReceiveFeesAreEstimated]
      ,[ReceiveTaxesAreEstimated]
      ,[DetailReceiveAmounts_nonMgiReceiveFee]
      ,[DetailReceiveAmounts_nonMgiReceiveTax]
      ,[DetailReceiveAmounts_mgiReceiveFee]
      ,[DetailReceiveAmounts_mgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveTax]
      ,[AgentCustomerNumber]
      ,[ReceiveCurrency]
      ,[SenderDOB]
      ,[SendCurrency]
      ,[ConsumerId]
      ,[FormFreeStaging]
      ,[ReadyForCommit]
      ,[ReceiveAgentAddress]
      ,[ExchangeRateApplied]
      ,[ReceiveFeeDisclosureText]
      ,[ReceiveTaxDisclosureText]
      ,[ReferenceNumber]
      ,[PartnerConfirmationNumber]
      ,[FreePhoneCallPIN]
      ,[TollFreePhoneNumber]
      ,[ExpectedDateOfDelivery]
      ,[TransactionDateTime]
      ,[StateRegulatorVersion]
      ,[ReceiveAgentID]
      ,[AccountNumber]
      ,[CustomerReceiveNumber]
      ,[SenderMiddleName]
      ,[SenderLastName2]
      ,[SenderAddress2]
      ,[SenderAddress3]
      ,[ReceiverMiddleName]
      ,[ReceiverLastName2]
      ,[ReceiverAddress]
      ,[ReceiverAddress2]
      ,[ReceiverAddress3]
      ,[Direction1]
      ,[Direction2]
      ,[Direction3]
      ,[ReceiverCity]
      ,[ReceiverState]
      ,[ReceiverZipCode]
      ,[ReceiverPhone]
      ,[TestQuestion]
      ,[TestAnswer]
      ,[MessageField1]
      ,[MessageField2]
      ,[SenderPhotoIdType]
      ,[SenderPhotoIdNumber]
      ,[SenderPhotoIdState]
      ,[SenderPhotoIdCountry]
      ,[SenderLegalIdType]
      ,[SenderLegalIdNumber]
      ,[SenderOccupation]
      ,[SenderBirthCity]
      ,[SenderBirthCountry]
      ,[SenderPassportIssueDate]
      ,[SenderPassportIssueCity]
      ,[SenderPassportIssueCountry]
      ,[SenderLegalIdIssueCountry]
      ,[SenderEmailAddress]
      ,[SenderMobilePhone]
      ,[MarketingOptIn]
      ,[PcTerminalNumber]
      ,[AgentUseSendData]
      ,[SenderPhotoIdExpDate]
      ,[SenderPhotoIdIssueDate]
      ,[SenderPhotoIdStored]
      ,[SenderNationalityCountry]
      ,[SenderNationalityAtBirthCountry]
      ,[AgentTransactionId]
      ,[TimeToLive]
      ,[IsDomesticTransfer]
      ,[ReceiverId]
      ,[AccountId]
      ,[RequestResponseType]
      ,[AgentID]
      ,[AgentSequence]
      ,[Token]
      ,[RequestTimeStamp]
      ,[ApiVersion]
      ,[ClientSoftwareVersion]
      ,[DoCheckIn]
      ,[ResponseTimeStamp]
      ,[Flags]
      ,[PromotionalMessage_PrimaryLanguage]
      ,[PromotionalMessage_SecondaryLanguage]
      ,[PartnerName]
      ,[ReceiptTextInfo_PrimaryLanguage]
      ,[ReceiptTextInfo_SecondaryLanguage]
      ,[AccountNumberLastFour]
      ,[CustomerServiceMessage]
      ,[AccountNickname]
      ,[PromotionErrorCodeMessage_SecondaryLanguage]
      ,[DisclosureText_PrimaryLanguage]
      ,[DisclosureText_SecondaryLanguage]
      ,[ReceiveAgentName]
      ,[DynamicFields]
      , 2 AS AuditEvent, GETDATE(), @RevisionNo FROM inserted
       END       
   ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
   BEGIN
       INSERT INTO tMGram_Transfer_Trx_Aud( [rowguid],[Id] ,[Amount] ,[FeeAmount] ,[DestinationCountry] ,[DestinationState]  ,[DeliveryOption]  ,[SenderFirstName]
      ,[SenderLastName]    ,[SenderAddress] ,[SenderCity] ,[SenderState]  ,[SenderZipCode]  ,[SenderCountry]  ,[SenderHomePhone]
      ,[ReceiverFirstName]  ,[ReceiverLastName],[ReceiverCountry],[PrimaryReceiptLanguage]  ,[SecondaryReceiptLanguage]
      ,[PromoCodeValues_PromoCode] ,[GrossTotalAmount],[TransactionType] ,[ChannelPartnerId],[ProviderId],[MgiTransactionSessionId]
      ,[ExchangeRate],[PromotionDiscount],[PromotionErrorCodeMessage_PrimaryLanguage] ,[PromotionErrorCode],[TaxAmount]
      ,[ReceiveAmount],[DTCreate],[DTLastMod],[DTServerCreate] ,[DTServerLastMod]  ,[EstimatedReceiveAmount],[EstimatedExchangeRate]
      ,[ReceiveAmountAltered]   ,[RevisedInformationalFee],[DeliveryOptionDesc]  ,[SendAmountAltered]
      ,[PromotionDiscountId]
      ,[PromotionCategoryId]
      ,[PromotionDiscountAmount]
      ,[TotalSendFees]
      ,[TotalDiscountAmount]
      ,[TotalSendTaxes]
      ,[TotalAmountToCollect]
      ,[DetailSendAmounts_mgiNonDiscountedSendFee]
      ,[DetailSendAmounts_totalNonDiscountedFees]
      ,[DetailSendAmounts_discountedMgiSendFee]
      ,[DetailSendAmounts_mgiSendTax]
      ,[DetailSendAmounts_totalMgiCollectedFeesAndTaxes]
      ,[DetailSendAmounts_totalAmountToMgi]
      ,[DetailSendAmounts_totalSendFeesAndTaxes]
      ,[DetailSendAmounts_totalNonMgiSendFeesAndTaxes]
      ,[DetailSendAmounts_nonMgiSendTax]
      ,[DetailSendAmounts_nonMgiSendFee]
      ,[ValidCurrencyIndicator]
      ,[PayoutCurrency]
      ,[TotalReceiveFees]
      ,[TotalReceiveTaxes]
      ,[TotalReceiveAmount]
      ,[ReceiveFeesAreEstimated]
      ,[ReceiveTaxesAreEstimated]
      ,[DetailReceiveAmounts_nonMgiReceiveFee]
      ,[DetailReceiveAmounts_nonMgiReceiveTax]
      ,[DetailReceiveAmounts_mgiReceiveFee]
      ,[DetailReceiveAmounts_mgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveTax]
      ,[AgentCustomerNumber]
      ,[ReceiveCurrency]
      ,[SenderDOB]
      ,[SendCurrency]
      ,[ConsumerId]
      ,[FormFreeStaging]
      ,[ReadyForCommit]
      ,[ReceiveAgentAddress]
      ,[ExchangeRateApplied]
      ,[ReceiveFeeDisclosureText]
      ,[ReceiveTaxDisclosureText]
      ,[ReferenceNumber]
      ,[PartnerConfirmationNumber]
      ,[FreePhoneCallPIN]
      ,[TollFreePhoneNumber]
      ,[ExpectedDateOfDelivery]
      ,[TransactionDateTime]
      ,[StateRegulatorVersion]
      ,[ReceiveAgentID]
      ,[AccountNumber]
      ,[CustomerReceiveNumber]
      ,[SenderMiddleName]
      ,[SenderLastName2]
      ,[SenderAddress2]
      ,[SenderAddress3]
      ,[ReceiverMiddleName]
      ,[ReceiverLastName2]
      ,[ReceiverAddress]
      ,[ReceiverAddress2]
      ,[ReceiverAddress3]
      ,[Direction1]
      ,[Direction2]
      ,[Direction3]
      ,[ReceiverCity]
      ,[ReceiverState]
      ,[ReceiverZipCode]
      ,[ReceiverPhone]
      ,[TestQuestion]
      ,[TestAnswer]
      ,[MessageField1]
      ,[MessageField2]
      ,[SenderPhotoIdType]
      ,[SenderPhotoIdNumber]
      ,[SenderPhotoIdState]
      ,[SenderPhotoIdCountry]
      ,[SenderLegalIdType]
      ,[SenderLegalIdNumber]
      ,[SenderOccupation]
      ,[SenderBirthCity]
      ,[SenderBirthCountry]
      ,[SenderPassportIssueDate]
      ,[SenderPassportIssueCity]
      ,[SenderPassportIssueCountry]
      ,[SenderLegalIdIssueCountry]
      ,[SenderEmailAddress]
      ,[SenderMobilePhone]
      ,[MarketingOptIn]
      ,[PcTerminalNumber]
      ,[AgentUseSendData]
      ,[SenderPhotoIdExpDate]
      ,[SenderPhotoIdIssueDate]
      ,[SenderPhotoIdStored]
      ,[SenderNationalityCountry]
      ,[SenderNationalityAtBirthCountry]
      ,[AgentTransactionId]
      ,[TimeToLive]
      ,[IsDomesticTransfer]
      ,[ReceiverId]
      ,[AccountId]
      ,[RequestResponseType]
      ,[AgentID]
      ,[AgentSequence]
      ,[Token]
      ,[RequestTimeStamp]
      ,[ApiVersion]
      ,[ClientSoftwareVersion]
      ,[DoCheckIn]
      ,[ResponseTimeStamp]
      ,[Flags]
      ,[PromotionalMessage_PrimaryLanguage]
      ,[PromotionalMessage_SecondaryLanguage]
      ,[PartnerName]
      ,[ReceiptTextInfo_PrimaryLanguage]
      ,[ReceiptTextInfo_SecondaryLanguage]
      ,[AccountNumberLastFour]
      ,[CustomerServiceMessage]
      ,[AccountNickname]
      ,[PromotionErrorCodeMessage_SecondaryLanguage]
      ,[DisclosureText_PrimaryLanguage]
      ,[DisclosureText_SecondaryLanguage]
      ,[ReceiveAgentName]
      ,[DynamicFields]
      ,[AuditEvent]
      ,[DTAudit]
      ,[RevisionNo])
      
      SELECT  [rowguid],[Id] ,[Amount] ,[FeeAmount] ,[DestinationCountry] ,[DestinationState]  ,[DeliveryOption]  ,[SenderFirstName]
      ,[SenderLastName]    ,[SenderAddress] ,[SenderCity] ,[SenderState]  ,[SenderZipCode]  ,[SenderCountry]  ,[SenderHomePhone]
      ,[ReceiverFirstName]  ,[ReceiverLastName],[ReceiverCountry],[PrimaryReceiptLanguage]  ,[SecondaryReceiptLanguage]
      ,[PromoCodeValues_PromoCode] ,[GrossTotalAmount],[TransactionType] ,[ChannelPartnerId],[ProviderId],[MgiTransactionSessionId]
      ,[ExchangeRate],[PromotionDiscount],[PromotionErrorCodeMessage_PrimaryLanguage] ,[PromotionErrorCode],[TaxAmount]
      ,[ReceiveAmount],[DTCreate],[DTLastMod],[DTServerCreate] ,[DTServerLastMod]  ,[EstimatedReceiveAmount],[EstimatedExchangeRate]
      ,[ReceiveAmountAltered]   ,[RevisedInformationalFee],[DeliveryOptionDesc]  ,[SendAmountAltered]
      ,[PromotionDiscountId]
      ,[PromotionCategoryId]
      ,[PromotionDiscountAmount]
      ,[TotalSendFees]
      ,[TotalDiscountAmount]
      ,[TotalSendTaxes]
      ,[TotalAmountToCollect]
      ,[DetailSendAmounts_mgiNonDiscountedSendFee]
      ,[DetailSendAmounts_totalNonDiscountedFees]
      ,[DetailSendAmounts_discountedMgiSendFee]
      ,[DetailSendAmounts_mgiSendTax]
      ,[DetailSendAmounts_totalMgiCollectedFeesAndTaxes]
      ,[DetailSendAmounts_totalAmountToMgi]
      ,[DetailSendAmounts_totalSendFeesAndTaxes]
      ,[DetailSendAmounts_totalNonMgiSendFeesAndTaxes]
      ,[DetailSendAmounts_nonMgiSendTax]
      ,[DetailSendAmounts_nonMgiSendFee]
      ,[ValidCurrencyIndicator]
      ,[PayoutCurrency]
      ,[TotalReceiveFees]
      ,[TotalReceiveTaxes]
      ,[TotalReceiveAmount]
      ,[ReceiveFeesAreEstimated]
      ,[ReceiveTaxesAreEstimated]
      ,[DetailReceiveAmounts_nonMgiReceiveFee]
      ,[DetailReceiveAmounts_nonMgiReceiveTax]
      ,[DetailReceiveAmounts_mgiReceiveFee]
      ,[DetailReceiveAmounts_mgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveTax]
      ,[AgentCustomerNumber]
      ,[ReceiveCurrency]
      ,[SenderDOB]
      ,[SendCurrency]
      ,[ConsumerId]
      ,[FormFreeStaging]
      ,[ReadyForCommit]
      ,[ReceiveAgentAddress]
      ,[ExchangeRateApplied]
      ,[ReceiveFeeDisclosureText]
      ,[ReceiveTaxDisclosureText]
      ,[ReferenceNumber]
      ,[PartnerConfirmationNumber]
      ,[FreePhoneCallPIN]
      ,[TollFreePhoneNumber]
      ,[ExpectedDateOfDelivery]
      ,[TransactionDateTime]
      ,[StateRegulatorVersion]
      ,[ReceiveAgentID]
      ,[AccountNumber]
      ,[CustomerReceiveNumber]
      ,[SenderMiddleName]
      ,[SenderLastName2]
      ,[SenderAddress2]
      ,[SenderAddress3]
      ,[ReceiverMiddleName]
      ,[ReceiverLastName2]
      ,[ReceiverAddress]
      ,[ReceiverAddress2]
      ,[ReceiverAddress3]
      ,[Direction1]
      ,[Direction2]
      ,[Direction3]
      ,[ReceiverCity]
      ,[ReceiverState]
      ,[ReceiverZipCode]
      ,[ReceiverPhone]
      ,[TestQuestion]
      ,[TestAnswer]
      ,[MessageField1]
      ,[MessageField2]
      ,[SenderPhotoIdType]
      ,[SenderPhotoIdNumber]
      ,[SenderPhotoIdState]
      ,[SenderPhotoIdCountry]
      ,[SenderLegalIdType]
      ,[SenderLegalIdNumber]
      ,[SenderOccupation]
      ,[SenderBirthCity]
      ,[SenderBirthCountry]
      ,[SenderPassportIssueDate]
      ,[SenderPassportIssueCity]
      ,[SenderPassportIssueCountry]
      ,[SenderLegalIdIssueCountry]
      ,[SenderEmailAddress]
      ,[SenderMobilePhone]
      ,[MarketingOptIn]
      ,[PcTerminalNumber]
      ,[AgentUseSendData]
      ,[SenderPhotoIdExpDate]
      ,[SenderPhotoIdIssueDate]
      ,[SenderPhotoIdStored]
      ,[SenderNationalityCountry]
      ,[SenderNationalityAtBirthCountry]
      ,[AgentTransactionId]
      ,[TimeToLive]
      ,[IsDomesticTransfer]
      ,[ReceiverId]
      ,[AccountId]
      ,[RequestResponseType]
      ,[AgentID]
      ,[AgentSequence]
      ,[Token]
      ,[RequestTimeStamp]
      ,[ApiVersion]
      ,[ClientSoftwareVersion]
      ,[DoCheckIn]
      ,[ResponseTimeStamp]
      ,[Flags]
      ,[PromotionalMessage_PrimaryLanguage]
      ,[PromotionalMessage_SecondaryLanguage]
      ,[PartnerName]
      ,[ReceiptTextInfo_PrimaryLanguage]
      ,[ReceiptTextInfo_SecondaryLanguage]
      ,[AccountNumberLastFour]
      ,[CustomerServiceMessage]
      ,[AccountNickname]
      ,[PromotionErrorCodeMessage_SecondaryLanguage]
      ,[DisclosureText_PrimaryLanguage]
      ,[DisclosureText_SecondaryLanguage]
      ,[ReceiveAgentName]
      ,[DynamicFields]
      , 1 AS AuditEvent, GETDATE(), @RevisionNo FROM inserted
   END
   ELSE IF(SELECT COUNT(*) FROM DELETED)>0
   BEGIN
       INSERT INTO tMGram_Transfer_Trx_Aud(
	   [rowguid],[Id] ,[Amount] ,[FeeAmount] ,[DestinationCountry] ,[DestinationState]  ,[DeliveryOption]  ,[SenderFirstName]
      ,[SenderLastName]    ,[SenderAddress] ,[SenderCity] ,[SenderState]  ,[SenderZipCode]  ,[SenderCountry]  ,[SenderHomePhone]
      ,[ReceiverFirstName]  ,[ReceiverLastName],[ReceiverCountry],[PrimaryReceiptLanguage]  ,[SecondaryReceiptLanguage]
      ,[PromoCodeValues_PromoCode] ,[GrossTotalAmount],[TransactionType] ,[ChannelPartnerId],[ProviderId],[MgiTransactionSessionId]
      ,[ExchangeRate],[PromotionDiscount],[PromotionErrorCodeMessage_PrimaryLanguage] ,[PromotionErrorCode],[TaxAmount]
      ,[ReceiveAmount],[DTCreate],[DTLastMod],[DTServerCreate] ,[DTServerLastMod]  ,[EstimatedReceiveAmount],[EstimatedExchangeRate]
      ,[ReceiveAmountAltered]   ,[RevisedInformationalFee],[DeliveryOptionDesc]  ,[SendAmountAltered]
      ,[PromotionDiscountId]
      ,[PromotionCategoryId]
      ,[PromotionDiscountAmount]
      ,[TotalSendFees]
      ,[TotalDiscountAmount]
      ,[TotalSendTaxes]
      ,[TotalAmountToCollect]
      ,[DetailSendAmounts_mgiNonDiscountedSendFee]
      ,[DetailSendAmounts_totalNonDiscountedFees]
      ,[DetailSendAmounts_discountedMgiSendFee]
      ,[DetailSendAmounts_mgiSendTax]
      ,[DetailSendAmounts_totalMgiCollectedFeesAndTaxes]
      ,[DetailSendAmounts_totalAmountToMgi]
      ,[DetailSendAmounts_totalSendFeesAndTaxes]
      ,[DetailSendAmounts_totalNonMgiSendFeesAndTaxes]
      ,[DetailSendAmounts_nonMgiSendTax]
      ,[DetailSendAmounts_nonMgiSendFee]
      ,[ValidCurrencyIndicator]
      ,[PayoutCurrency]
      ,[TotalReceiveFees]
      ,[TotalReceiveTaxes]
      ,[TotalReceiveAmount]
      ,[ReceiveFeesAreEstimated]
      ,[ReceiveTaxesAreEstimated]
      ,[DetailReceiveAmounts_nonMgiReceiveFee]
      ,[DetailReceiveAmounts_nonMgiReceiveTax]
      ,[DetailReceiveAmounts_mgiReceiveFee]
      ,[DetailReceiveAmounts_mgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveTax]
      ,[AgentCustomerNumber]
      ,[ReceiveCurrency]
      ,[SenderDOB]
      ,[SendCurrency]
      ,[ConsumerId]
      ,[FormFreeStaging]
      ,[ReadyForCommit]
      ,[ReceiveAgentAddress]
      ,[ExchangeRateApplied]
      ,[ReceiveFeeDisclosureText]
      ,[ReceiveTaxDisclosureText]
      ,[ReferenceNumber]
      ,[PartnerConfirmationNumber]
      ,[FreePhoneCallPIN]
      ,[TollFreePhoneNumber]
      ,[ExpectedDateOfDelivery]
      ,[TransactionDateTime]
      ,[StateRegulatorVersion]
      ,[ReceiveAgentID]
      ,[AccountNumber]
      ,[CustomerReceiveNumber]
      ,[SenderMiddleName]
      ,[SenderLastName2]
      ,[SenderAddress2]
      ,[SenderAddress3]
      ,[ReceiverMiddleName]
      ,[ReceiverLastName2]
      ,[ReceiverAddress]
      ,[ReceiverAddress2]
      ,[ReceiverAddress3]
      ,[Direction1]
      ,[Direction2]
      ,[Direction3]
      ,[ReceiverCity]
      ,[ReceiverState]
      ,[ReceiverZipCode]
      ,[ReceiverPhone]
      ,[TestQuestion]
      ,[TestAnswer]
      ,[MessageField1]
      ,[MessageField2]
      ,[SenderPhotoIdType]
      ,[SenderPhotoIdNumber]
      ,[SenderPhotoIdState]
      ,[SenderPhotoIdCountry]
      ,[SenderLegalIdType]
      ,[SenderLegalIdNumber]
      ,[SenderOccupation]
      ,[SenderBirthCity]
      ,[SenderBirthCountry]
      ,[SenderPassportIssueDate]
      ,[SenderPassportIssueCity]
      ,[SenderPassportIssueCountry]
      ,[SenderLegalIdIssueCountry]
      ,[SenderEmailAddress]
      ,[SenderMobilePhone]
      ,[MarketingOptIn]
      ,[PcTerminalNumber]
      ,[AgentUseSendData]
      ,[SenderPhotoIdExpDate]
      ,[SenderPhotoIdIssueDate]
      ,[SenderPhotoIdStored]
      ,[SenderNationalityCountry]
      ,[SenderNationalityAtBirthCountry]
      ,[AgentTransactionId]
      ,[TimeToLive]
      ,[IsDomesticTransfer]
      ,[ReceiverId]
      ,[AccountId]
      ,[RequestResponseType]
      ,[AgentID]
      ,[AgentSequence]
      ,[Token]
      ,[RequestTimeStamp]
      ,[ApiVersion]
      ,[ClientSoftwareVersion]
      ,[DoCheckIn]
      ,[ResponseTimeStamp]
      ,[Flags]
      ,[PromotionalMessage_PrimaryLanguage]
      ,[PromotionalMessage_SecondaryLanguage]
      ,[PartnerName]
      ,[ReceiptTextInfo_PrimaryLanguage]
      ,[ReceiptTextInfo_SecondaryLanguage]
      ,[AccountNumberLastFour]
      ,[CustomerServiceMessage]
      ,[AccountNickname]
      ,[PromotionErrorCodeMessage_SecondaryLanguage]
      ,[DisclosureText_PrimaryLanguage]
      ,[DisclosureText_SecondaryLanguage]
      ,[ReceiveAgentName]
      ,[DynamicFields]
      ,[AuditEvent]
      ,[DTAudit]
      ,[RevisionNo])

       SELECT  [rowguid],[Id] ,[Amount] ,[FeeAmount] ,[DestinationCountry] ,[DestinationState]  ,[DeliveryOption]  ,[SenderFirstName]
      ,[SenderLastName]    ,[SenderAddress] ,[SenderCity] ,[SenderState]  ,[SenderZipCode]  ,[SenderCountry]  ,[SenderHomePhone]
      ,[ReceiverFirstName]  ,[ReceiverLastName],[ReceiverCountry],[PrimaryReceiptLanguage]  ,[SecondaryReceiptLanguage]
      ,[PromoCodeValues_PromoCode] ,[GrossTotalAmount],[TransactionType] ,[ChannelPartnerId],[ProviderId],[MgiTransactionSessionId]
      ,[ExchangeRate],[PromotionDiscount],[PromotionalMessage_PrimaryLanguage] ,[PromotionErrorCode],[TaxAmount]
      ,[ReceiveAmount],[DTCreate],[DTLastMod],[DTServerCreate] ,[DTServerLastMod]  ,[EstimatedReceiveAmount],[EstimatedExchangeRate]
      ,[ReceiveAmountAltered]   ,[RevisedInformationalFee],[DeliveryOptionDesc]  ,[SendAmountAltered]
      ,[PromotionDiscountId]
      ,[PromotionCategoryId]
      ,[PromotionDiscountAmount]
      ,[TotalSendFees]
      ,[TotalDiscountAmount]
      ,[TotalSendTaxes]
      ,[TotalAmountToCollect]
      ,[DetailSendAmounts_mgiNonDiscountedSendFee]
      ,[DetailSendAmounts_totalNonDiscountedFees]
      ,[DetailSendAmounts_discountedMgiSendFee]
      ,[DetailSendAmounts_mgiSendTax]
      ,[DetailSendAmounts_totalMgiCollectedFeesAndTaxes]
      ,[DetailSendAmounts_totalAmountToMgi]
      ,[DetailSendAmounts_totalSendFeesAndTaxes]
      ,[DetailSendAmounts_totalNonMgiSendFeesAndTaxes]
      ,[DetailSendAmounts_nonMgiSendTax]
      ,[DetailSendAmounts_nonMgiSendFee]
      ,[ValidCurrencyIndicator]
      ,[PayoutCurrency]
      ,[TotalReceiveFees]
      ,[TotalReceiveTaxes]
      ,[TotalReceiveAmount]
      ,[ReceiveFeesAreEstimated]
      ,[ReceiveTaxesAreEstimated]
      ,[DetailReceiveAmounts_nonMgiReceiveFee]
      ,[DetailReceiveAmounts_nonMgiReceiveTax]
      ,[DetailReceiveAmounts_mgiReceiveFee]
      ,[DetailReceiveAmounts_mgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_nonMgiReceiveTax]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveFee]
      ,[DetailEstimatedReceiveAmounts_mgiReceiveTax]
      ,[AgentCustomerNumber]
      ,[ReceiveCurrency]
      ,[SenderDOB]
      ,[SendCurrency]
      ,[ConsumerId]
      ,[FormFreeStaging]
      ,[ReadyForCommit]
      ,[ReceiveAgentAddress]
      ,[ExchangeRateApplied]
      ,[ReceiveFeeDisclosureText]
      ,[ReceiveTaxDisclosureText]
      ,[ReferenceNumber]
      ,[PartnerConfirmationNumber]
      ,[FreePhoneCallPIN]
      ,[TollFreePhoneNumber]
      ,[ExpectedDateOfDelivery]
      ,[TransactionDateTime]
      ,[StateRegulatorVersion]
      ,[ReceiveAgentID]
      ,[AccountNumber]
      ,[CustomerReceiveNumber]
      ,[SenderMiddleName]
      ,[SenderLastName2]
      ,[SenderAddress2]
      ,[SenderAddress3]
      ,[ReceiverMiddleName]
      ,[ReceiverLastName2]
      ,[ReceiverAddress]
      ,[ReceiverAddress2]
      ,[ReceiverAddress3]
      ,[Direction1]
      ,[Direction2]
      ,[Direction3]
      ,[ReceiverCity]
      ,[ReceiverState]
      ,[ReceiverZipCode]
      ,[ReceiverPhone]
      ,[TestQuestion]
      ,[TestAnswer]
      ,[MessageField1]
      ,[MessageField2]
      ,[SenderPhotoIdType]
      ,[SenderPhotoIdNumber]
      ,[SenderPhotoIdState]
      ,[SenderPhotoIdCountry]
      ,[SenderLegalIdType]
      ,[SenderLegalIdNumber]
      ,[SenderOccupation]
      ,[SenderBirthCity]
      ,[SenderBirthCountry]
      ,[SenderPassportIssueDate]
      ,[SenderPassportIssueCity]
      ,[SenderPassportIssueCountry]
      ,[SenderLegalIdIssueCountry]
      ,[SenderEmailAddress]
      ,[SenderMobilePhone]
      ,[MarketingOptIn]
      ,[PcTerminalNumber]
      ,[AgentUseSendData]
      ,[SenderPhotoIdExpDate]
      ,[SenderPhotoIdIssueDate]
      ,[SenderPhotoIdStored]
      ,[SenderNationalityCountry]
      ,[SenderNationalityAtBirthCountry]
      ,[AgentTransactionId]
      ,[TimeToLive]
      ,[IsDomesticTransfer]
      ,[ReceiverId]
      ,[AccountId]
      ,[RequestResponseType]
      ,[AgentID]
      ,[AgentSequence]
      ,[Token]
      ,[RequestTimeStamp]
      ,[ApiVersion]
      ,[ClientSoftwareVersion]
      ,[DoCheckIn]
      ,[ResponseTimeStamp]
      ,[Flags]
      ,[PromotionalMessage_PrimaryLanguage]
      ,[PromotionalMessage_SecondaryLanguage]
      ,[PartnerName]
      ,[ReceiptTextInfo_PrimaryLanguage]
      ,[ReceiptTextInfo_SecondaryLanguage]
      ,[AccountNumberLastFour]
      ,[CustomerServiceMessage]
      ,[AccountNickname]
      ,[PromotionErrorCodeMessage_SecondaryLanguage]
      ,[DisclosureText_PrimaryLanguage]
      ,[DisclosureText_SecondaryLanguage]
      ,[ReceiveAgentName]
      ,[DynamicFields]
      , 3 AS AuditEvent, GETDATE(), @RevisionNo FROM deleted
    END
GO
