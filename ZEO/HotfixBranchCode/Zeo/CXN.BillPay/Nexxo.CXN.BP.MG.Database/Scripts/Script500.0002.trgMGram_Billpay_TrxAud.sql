--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[trgMGram_Billpay_TrxAud] 
ON [dbo].[tMGram_BillPay_Trx] 
AFTER INSERT, UPDATE, DELETE
AS
	SET NOCOUNT ON
	
	DECLARE @RevisionNo BIGINT

	SELECT 
		@RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 
	FROM 
		[tMGram_BillPay_Trx_Aud] 
	WHERE 
		MGBillPayTrxID = (SELECT MGBillPayTrxID FROM inserted)
             
	IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
	BEGIN
		INSERT INTO [dbo].[tMGram_BillPay_Trx_Aud]
           ([MGBillPayTrxPK]
           ,[MGBillPayTrxID]
           ,[AgentID]
           ,[AgentSequence]
           ,[Token]
           ,[ApiVersion]
           ,[ClientSoftwareVersion]
           ,[RequestResponseType]
           ,[ProductVariant]
           ,[ReceiveCountry]
           ,[ReceiveCode]
           ,[ReceiveAgentID]
           ,[ReceiveCurrency]
           ,[SendCurrency]
           ,[PromoCodeValuesPromoCode]
           ,[DoCheckIn]
           ,[TimeStamp]
           ,[Flags]
           ,[ValidReceiveAmount]
           ,[ValidReceiveCurrency]
           ,[ValidExchangeRate]
           ,[TotalAmount]
           ,[ReceiveAmountAltered]
           ,[RevisedInformationalFee]
           ,[DeliveryOptId]
           ,[DeliveryOptDisplayName]
           ,[ReceiveAgentName]
           ,[MgiTransactionSessionID]
           ,[SendAmountAltered]
           ,[SendAmount]
           ,[TotalSendFees]
           ,[TotalDiscountAmount]
           ,[TotalSendTaxes]
           ,[TotalAmountToCollect]
           ,[ReceiveAmount]
           ,[ValidCurrencyIndicator]
           ,[PayoutCurrency]
           ,[TotalReceiveFees]
           ,[TotalReceiveTaxes]
           ,[TotalReceiveAmount]
           ,[ReceiveFeesAreEstimated]
           ,[ReceiveTaxesAreEstimated]
           ,[DTCreate]
           ,[DTLastMod]
           ,[DTServerCreate]
           ,[DTServerLastMod]
           ,[AccountPK]
           ,[AccountNumberRetryCount]
           ,[SenderFirstName]
           ,[SenderLastName]
           ,[SenderAddress]
           ,[SenderCity]
           ,[SenderState]
           ,[SenderZipCode]
           ,[SenderCountry]
           ,[SenderHomePhone]
           ,[ReceiverFirstName]
           ,[ReceiverLastName]
           ,[ServiceOfferingID]
           ,[BillerWebsite]
           ,[BillerPhone]
           ,[BillerCutoffTime]
           ,[BillerAddress]
           ,[BillerAddress2]
           ,[BillerAddress3]
           ,[BillerCity]
           ,[BillerState]
           ,[BillerZip]
           ,[PrintMGICustomerServiceNumber]
           ,[AgentTransactionId]
           ,[ReadyForCommit]
           ,[ProcessingFee]
           ,[InfoFeeIndicator]
           ,[ExchangeRateApplied]
           ,[ReferenceNumber]
           ,[PartnerConfirmationNumber]
           ,[PartnerName]
           ,[FreePhoneCallPin]
           ,[TollFreePhoneNumber]
           ,[ExpectedDateOfDelivery]
           ,[TransactionDateTime]
           ,[AccountNumber]
           ,[SenderMiddleName]
           ,[SenderLastName2]
           ,[MessageField1]
           ,[MessageField2]
           ,[SenderDOB]
           ,[SenderOccupation]
           ,[SenderLegalIdNumber]
           ,[SenderLegalIdType]
           ,[SenderPhotoIdCountry]
           ,[SenderPhotoIdState]
           ,[SenderPhotoIdNumber]
           ,[SenderPhotoIdType]
           ,[BillerName]
           ,[TextTranslation]
           ,[ReceiverMiddleName]
           ,[ReceiverLastName2]
           ,[PurposeOfFund]
           ,[TotalSendAmount]
           ,[MgiRewardsNumber]
           ,[ValidateAccountNumber]
           ,[CardExpirationMonth]
           ,[CardExpirationYear]
           ,[AuditEvent]
           ,[DTAudit]
           ,[RevisionNo]
           ,[IsValidateAccNumberRequired]
		   ,[ExpectedPostingTimeFrame]
		   ,[ExpectedPostingTimeFrameSecondary]
		   ,[CustomerTipTextTranslation])
		SELECT
			[MGBillPayTrxPK]
           ,[MGBillPayTrxID]
           ,[AgentID]
           ,[AgentSequence]
           ,[Token]
           ,[ApiVersion]
           ,[ClientSoftwareVersion]
           ,[RequestResponseType]
           ,[ProductVariant]
           ,[ReceiveCountry]
           ,[ReceiveCode]
           ,[ReceiveAgentID]
           ,[ReceiveCurrency]
           ,[SendCurrency]
           ,[PromoCodeValuesPromoCode]
           ,[DoCheckIn]
           ,[TimeStamp]
           ,[Flags]
           ,[ValidReceiveAmount]
           ,[ValidReceiveCurrency]
           ,[ValidExchangeRate]
           ,[TotalAmount]
           ,[ReceiveAmountAltered]
           ,[RevisedInformationalFee]
           ,[DeliveryOptId]
           ,[DeliveryOptDisplayName]
           ,[ReceiveAgentName]
           ,[MgiTransactionSessionID]
           ,[SendAmountAltered]
           ,[SendAmount]
           ,[TotalSendFees]
           ,[TotalDiscountAmount]
           ,[TotalSendTaxes]
           ,[TotalAmountToCollect]
           ,[ReceiveAmount]
           ,[ValidCurrencyIndicator]
           ,[PayoutCurrency]
           ,[TotalReceiveFees]
           ,[TotalReceiveTaxes]
           ,[TotalReceiveAmount]
           ,[ReceiveFeesAreEstimated]
           ,[ReceiveTaxesAreEstimated]
           ,[DTCreate]
           ,[DTLastMod]
           ,[DTServerCreate]
           ,[DTServerLastMod]
           ,[AccountPK]
           ,[AccountNumberRetryCount]
           ,[SenderFirstName]
           ,[SenderLastName]
           ,[SenderAddress]
           ,[SenderCity]
           ,[SenderState]
           ,[SenderZipCode]
           ,[SenderCountry]
           ,[SenderHomePhone]
           ,[ReceiverFirstName]
           ,[ReceiverLastName]
           ,[ServiceOfferingID]
           ,[BillerWebsite]
           ,[BillerPhone]
           ,[BillerCutoffTime]
           ,[BillerAddress]
           ,[BillerAddress2]
           ,[BillerAddress3]
           ,[BillerCity]
           ,[BillerState]
           ,[BillerZip]
           ,[PrintMGICustomerServiceNumber]
           ,[AgentTransactionId]
           ,[ReadyForCommit]
           ,[ProcessingFee]
           ,[InfoFeeIndicator]
           ,[ExchangeRateApplied]
           ,[ReferenceNumber]
           ,[PartnerConfirmationNumber]
           ,[PartnerName]
           ,[FreePhoneCallPin]
           ,[TollFreePhoneNumber]
           ,[ExpectedDateOfDelivery]
           ,[TransactionDateTime]
           ,[AccountNumber]
           ,[SenderMiddleName]
           ,[SenderLastName2]
           ,[MessageField1]
           ,[MessageField2]
           ,[SenderDOB]
           ,[SenderOccupation]
           ,[SenderLegalIdNumber]
           ,[SenderLegalIdType]
           ,[SenderPhotoIdCountry]
           ,[SenderPhotoIdState]
           ,[SenderPhotoIdNumber]
           ,[SenderPhotoIdType]
           ,[BillerName]
           ,[TextTranslation]
           ,[ReceiverMiddleName]
           ,[ReceiverLastName2]
           ,[PurposeOfFund]
           ,[TotalSendAmount]
           ,[MgiRewardsNumber]
           ,[ValidateAccountNumber]
           ,[CardExpirationMonth]
           ,[CardExpirationYear]	
		   , 2 AS AuditEvent
		   , GETDATE()
		   , @RevisionNo
		   ,[IsValidateAccNumberRequired]
		   ,[ExpectedPostingTimeFrame]
		   ,[ExpectedPostingTimeFrameSecondary]
		   ,[CustomerTipTextTranslation]
		FROM 
			inserted
	END       
	ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
	BEGIN
		INSERT INTO [dbo].[tMGram_BillPay_Trx_Aud]
           ([MGBillPayTrxPK]
           ,[MGBillPayTrxID]
           ,[AgentID]
           ,[AgentSequence]
           ,[Token]
           ,[ApiVersion]
           ,[ClientSoftwareVersion]
           ,[RequestResponseType]
           ,[ProductVariant]
           ,[ReceiveCountry]
           ,[ReceiveCode]
           ,[ReceiveAgentID]
           ,[ReceiveCurrency]
           ,[SendCurrency]
           ,[PromoCodeValuesPromoCode]
           ,[DoCheckIn]
           ,[TimeStamp]
           ,[Flags]
           ,[ValidReceiveAmount]
           ,[ValidReceiveCurrency]
           ,[ValidExchangeRate]
           ,[TotalAmount]
           ,[ReceiveAmountAltered]
           ,[RevisedInformationalFee]
           ,[DeliveryOptId]
           ,[DeliveryOptDisplayName]
           ,[ReceiveAgentName]
           ,[MgiTransactionSessionID]
           ,[SendAmountAltered]
           ,[SendAmount]
           ,[TotalSendFees]
           ,[TotalDiscountAmount]
           ,[TotalSendTaxes]
           ,[TotalAmountToCollect]
           ,[ReceiveAmount]
           ,[ValidCurrencyIndicator]
           ,[PayoutCurrency]
           ,[TotalReceiveFees]
           ,[TotalReceiveTaxes]
           ,[TotalReceiveAmount]
           ,[ReceiveFeesAreEstimated]
           ,[ReceiveTaxesAreEstimated]
           ,[DTCreate]
           ,[DTLastMod]
           ,[DTServerCreate]
           ,[DTServerLastMod]
           ,[AccountPK]
           ,[AccountNumberRetryCount]
           ,[SenderFirstName]
           ,[SenderLastName]
           ,[SenderAddress]
           ,[SenderCity]
           ,[SenderState]
           ,[SenderZipCode]
           ,[SenderCountry]
           ,[SenderHomePhone]
           ,[ReceiverFirstName]
           ,[ReceiverLastName]
           ,[ServiceOfferingID]
           ,[BillerWebsite]
           ,[BillerPhone]
           ,[BillerCutoffTime]
           ,[BillerAddress]
           ,[BillerAddress2]
           ,[BillerAddress3]
           ,[BillerCity]
           ,[BillerState]
           ,[BillerZip]
           ,[PrintMGICustomerServiceNumber]
           ,[AgentTransactionId]
           ,[ReadyForCommit]
           ,[ProcessingFee]
           ,[InfoFeeIndicator]
           ,[ExchangeRateApplied]
           ,[ReferenceNumber]
           ,[PartnerConfirmationNumber]
           ,[PartnerName]
           ,[FreePhoneCallPin]
           ,[TollFreePhoneNumber]
           ,[ExpectedDateOfDelivery]
           ,[TransactionDateTime]
           ,[AccountNumber]
           ,[SenderMiddleName]
           ,[SenderLastName2]
           ,[MessageField1]
           ,[MessageField2]
           ,[SenderDOB]
           ,[SenderOccupation]
           ,[SenderLegalIdNumber]
           ,[SenderLegalIdType]
           ,[SenderPhotoIdCountry]
           ,[SenderPhotoIdState]
           ,[SenderPhotoIdNumber]
           ,[SenderPhotoIdType]
           ,[BillerName]
           ,[TextTranslation]
           ,[ReceiverMiddleName]
           ,[ReceiverLastName2]
           ,[PurposeOfFund]
           ,[TotalSendAmount]
           ,[MgiRewardsNumber]
           ,[ValidateAccountNumber]
           ,[CardExpirationMonth]
           ,[CardExpirationYear]
           ,[AuditEvent]
           ,[DTAudit]
		   ,[RevisionNo]
		   ,[IsValidateAccNumberRequired]
		   ,[ExpectedPostingTimeFrame]
		   ,[ExpectedPostingTimeFrameSecondary]
		   ,[CustomerTipTextTranslation])
		SELECT
			[MGBillPayTrxPK]
           ,[MGBillPayTrxID]
           ,[AgentID]
           ,[AgentSequence]
           ,[Token]
           ,[ApiVersion]
           ,[ClientSoftwareVersion]
           ,[RequestResponseType]
           ,[ProductVariant]
           ,[ReceiveCountry]
           ,[ReceiveCode]
           ,[ReceiveAgentID]
           ,[ReceiveCurrency]
           ,[SendCurrency]
           ,[PromoCodeValuesPromoCode]
           ,[DoCheckIn]
           ,[TimeStamp]
           ,[Flags]
           ,[ValidReceiveAmount]
           ,[ValidReceiveCurrency]
           ,[ValidExchangeRate]
           ,[TotalAmount]
           ,[ReceiveAmountAltered]
           ,[RevisedInformationalFee]
           ,[DeliveryOptId]
           ,[DeliveryOptDisplayName]
           ,[ReceiveAgentName]
           ,[MgiTransactionSessionID]
           ,[SendAmountAltered]
           ,[SendAmount]
           ,[TotalSendFees]
           ,[TotalDiscountAmount]
           ,[TotalSendTaxes]
           ,[TotalAmountToCollect]
           ,[ReceiveAmount]
           ,[ValidCurrencyIndicator]
           ,[PayoutCurrency]
           ,[TotalReceiveFees]
           ,[TotalReceiveTaxes]
           ,[TotalReceiveAmount]
           ,[ReceiveFeesAreEstimated]
           ,[ReceiveTaxesAreEstimated]
           ,[DTCreate]
           ,[DTLastMod]
           ,[DTServerCreate]
           ,[DTServerLastMod]
           ,[AccountPK]
           ,[AccountNumberRetryCount]
           ,[SenderFirstName]
           ,[SenderLastName]
           ,[SenderAddress]
           ,[SenderCity]
           ,[SenderState]
           ,[SenderZipCode]
           ,[SenderCountry]
           ,[SenderHomePhone]
           ,[ReceiverFirstName]
           ,[ReceiverLastName]
           ,[ServiceOfferingID]
           ,[BillerWebsite]
           ,[BillerPhone]
           ,[BillerCutoffTime]
           ,[BillerAddress]
           ,[BillerAddress2]
           ,[BillerAddress3]
           ,[BillerCity]
           ,[BillerState]
           ,[BillerZip]
           ,[PrintMGICustomerServiceNumber]
           ,[AgentTransactionId]
           ,[ReadyForCommit]
           ,[ProcessingFee]
           ,[InfoFeeIndicator]
           ,[ExchangeRateApplied]
           ,[ReferenceNumber]
           ,[PartnerConfirmationNumber]
           ,[PartnerName]
           ,[FreePhoneCallPin]
           ,[TollFreePhoneNumber]
           ,[ExpectedDateOfDelivery]
           ,[TransactionDateTime]
           ,[AccountNumber]
           ,[SenderMiddleName]
           ,[SenderLastName2]
           ,[MessageField1]
           ,[MessageField2]
           ,[SenderDOB]
           ,[SenderOccupation]
           ,[SenderLegalIdNumber]
           ,[SenderLegalIdType]
           ,[SenderPhotoIdCountry]
           ,[SenderPhotoIdState]
           ,[SenderPhotoIdNumber]
           ,[SenderPhotoIdType]
           ,[BillerName]
           ,[TextTranslation]
           ,[ReceiverMiddleName]
           ,[ReceiverLastName2]
           ,[PurposeOfFund]
           ,[TotalSendAmount]
           ,[MgiRewardsNumber]
           ,[ValidateAccountNumber]
           ,[CardExpirationMonth]
           ,[CardExpirationYear]
		   , 1 AS AuditEvent
		   , GETDATE()
		   , @RevisionNo
		   ,[IsValidateAccNumberRequired]
		   ,[ExpectedPostingTimeFrame]
		   ,[ExpectedPostingTimeFrameSecondary]
		   ,[CustomerTipTextTranslation] 
		FROM 
			inserted
	END
	ELSE IF(SELECT COUNT(*) FROM DELETED)>0
	BEGIN
		INSERT INTO [dbo].[tMGram_BillPay_Trx_Aud]
           ([MGBillPayTrxPK]
           ,[MGBillPayTrxID]
           ,[AgentID]
           ,[AgentSequence]
           ,[Token]
           ,[ApiVersion]
           ,[ClientSoftwareVersion]
           ,[RequestResponseType]
           ,[ProductVariant]
           ,[ReceiveCountry]
           ,[ReceiveCode]
           ,[ReceiveAgentID]
           ,[ReceiveCurrency]
           ,[SendCurrency]
           ,[PromoCodeValuesPromoCode]
           ,[DoCheckIn]
           ,[TimeStamp]
           ,[Flags]
           ,[ValidReceiveAmount]
           ,[ValidReceiveCurrency]
           ,[ValidExchangeRate]
           ,[TotalAmount]
           ,[ReceiveAmountAltered]
           ,[RevisedInformationalFee]
           ,[DeliveryOptId]
           ,[DeliveryOptDisplayName]
           ,[ReceiveAgentName]
           ,[MgiTransactionSessionID]
           ,[SendAmountAltered]
           ,[SendAmount]
           ,[TotalSendFees]
           ,[TotalDiscountAmount]
           ,[TotalSendTaxes]
           ,[TotalAmountToCollect]
           ,[ReceiveAmount]
           ,[ValidCurrencyIndicator]
           ,[PayoutCurrency]
           ,[TotalReceiveFees]
           ,[TotalReceiveTaxes]
           ,[TotalReceiveAmount]
           ,[ReceiveFeesAreEstimated]
           ,[ReceiveTaxesAreEstimated]
           ,[DTCreate]
           ,[DTLastMod]
           ,[DTServerCreate]
           ,[DTServerLastMod]
           ,[AccountPK]
           ,[AccountNumberRetryCount]
           ,[SenderFirstName]
           ,[SenderLastName]
           ,[SenderAddress]
           ,[SenderCity]
           ,[SenderState]
           ,[SenderZipCode]
           ,[SenderCountry]
           ,[SenderHomePhone]
           ,[ReceiverFirstName]
           ,[ReceiverLastName]
           ,[ServiceOfferingID]
           ,[BillerWebsite]
           ,[BillerPhone]
           ,[BillerCutoffTime]
           ,[BillerAddress]
           ,[BillerAddress2]
           ,[BillerAddress3]
           ,[BillerCity]
           ,[BillerState]
           ,[BillerZip]
           ,[PrintMGICustomerServiceNumber]
           ,[AgentTransactionId]
           ,[ReadyForCommit]
           ,[ProcessingFee]
           ,[InfoFeeIndicator]
           ,[ExchangeRateApplied]
           ,[ReferenceNumber]
           ,[PartnerConfirmationNumber]
           ,[PartnerName]
           ,[FreePhoneCallPin]
           ,[TollFreePhoneNumber]
           ,[ExpectedDateOfDelivery]
           ,[TransactionDateTime]
           ,[AccountNumber]
           ,[SenderMiddleName]
           ,[SenderLastName2]
           ,[MessageField1]
           ,[MessageField2]
           ,[SenderDOB]
           ,[SenderOccupation]
           ,[SenderLegalIdNumber]
           ,[SenderLegalIdType]
           ,[SenderPhotoIdCountry]
           ,[SenderPhotoIdState]
           ,[SenderPhotoIdNumber]
           ,[SenderPhotoIdType]
           ,[BillerName]
           ,[TextTranslation]
           ,[ReceiverMiddleName]
           ,[ReceiverLastName2]
           ,[PurposeOfFund]
           ,[TotalSendAmount]
           ,[MgiRewardsNumber]
           ,[ValidateAccountNumber]
           ,[CardExpirationMonth]
           ,[CardExpirationYear]
           ,[AuditEvent]
           ,[DTAudit]
		   ,[RevisionNo]
		   ,[IsValidateAccNumberRequired]
		   ,[ExpectedPostingTimeFrame]
		   ,[ExpectedPostingTimeFrameSecondary]
		   ,[CustomerTipTextTranslation])
		SELECT
			[MGBillPayTrxPK]
           ,[MGBillPayTrxID]
           ,[AgentID]
           ,[AgentSequence]
           ,[Token]
           ,[ApiVersion]
           ,[ClientSoftwareVersion]
           ,[RequestResponseType]
           ,[ProductVariant]
           ,[ReceiveCountry]
           ,[ReceiveCode]
           ,[ReceiveAgentID]
           ,[ReceiveCurrency]
           ,[SendCurrency]
           ,[PromoCodeValuesPromoCode]
           ,[DoCheckIn]
           ,[TimeStamp]
           ,[Flags]
           ,[ValidReceiveAmount]
           ,[ValidReceiveCurrency]
           ,[ValidExchangeRate]
           ,[TotalAmount]
           ,[ReceiveAmountAltered]
           ,[RevisedInformationalFee]
           ,[DeliveryOptId]
           ,[DeliveryOptDisplayName]
           ,[ReceiveAgentName]
           ,[MgiTransactionSessionID]
           ,[SendAmountAltered]
           ,[SendAmount]
           ,[TotalSendFees]
           ,[TotalDiscountAmount]
           ,[TotalSendTaxes]
           ,[TotalAmountToCollect]
           ,[ReceiveAmount]
           ,[ValidCurrencyIndicator]
           ,[PayoutCurrency]
           ,[TotalReceiveFees]
           ,[TotalReceiveTaxes]
           ,[TotalReceiveAmount]
           ,[ReceiveFeesAreEstimated]
           ,[ReceiveTaxesAreEstimated]
           ,[DTCreate]
           ,[DTLastMod]
           ,[DTServerCreate]
           ,[DTServerLastMod]
           ,[AccountPK]
           ,[AccountNumberRetryCount]
           ,[SenderFirstName]
           ,[SenderLastName]
           ,[SenderAddress]
           ,[SenderCity]
           ,[SenderState]
           ,[SenderZipCode]
           ,[SenderCountry]
           ,[SenderHomePhone]
           ,[ReceiverFirstName]
           ,[ReceiverLastName]
           ,[ServiceOfferingID]
           ,[BillerWebsite]
           ,[BillerPhone]
           ,[BillerCutoffTime]
           ,[BillerAddress]
           ,[BillerAddress2]
           ,[BillerAddress3]
           ,[BillerCity]
           ,[BillerState]
           ,[BillerZip]
           ,[PrintMGICustomerServiceNumber]
           ,[AgentTransactionId]
           ,[ReadyForCommit]
           ,[ProcessingFee]
           ,[InfoFeeIndicator]
           ,[ExchangeRateApplied]
           ,[ReferenceNumber]
           ,[PartnerConfirmationNumber]
           ,[PartnerName]
           ,[FreePhoneCallPin]
           ,[TollFreePhoneNumber]
           ,[ExpectedDateOfDelivery]
           ,[TransactionDateTime]
           ,[AccountNumber]
           ,[SenderMiddleName]
           ,[SenderLastName2]
           ,[MessageField1]
           ,[MessageField2]
           ,[SenderDOB]
           ,[SenderOccupation]
           ,[SenderLegalIdNumber]
           ,[SenderLegalIdType]
           ,[SenderPhotoIdCountry]
           ,[SenderPhotoIdState]
           ,[SenderPhotoIdNumber]
           ,[SenderPhotoIdType]
           ,[BillerName]
           ,[TextTranslation]
           ,[ReceiverMiddleName]
           ,[ReceiverLastName2]
           ,[PurposeOfFund]
           ,[TotalSendAmount]
           ,[MgiRewardsNumber]
           ,[ValidateAccountNumber]
           ,[CardExpirationMonth]
           ,[CardExpirationYear]
		   , 3 AS AuditEvent
		   , GETDATE()
		   , @RevisionNo
		   ,[IsValidateAccNumberRequired]
		   ,[ExpectedPostingTimeFrame]
		   ,[ExpectedPostingTimeFrameSecondary]
		   ,[CustomerTipTextTranslation] 
		FROM 
		   deleted
    END
GO


