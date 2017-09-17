--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <04-10-2017>
-- Description:	This SP is used to get the WU transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_GetWUTransaction', N'P') IS NOT NULL
DROP PROC usp_GetWUTransaction
GO


CREATE PROCEDURE usp_GetWUTransaction
(
    @wuTrxId BIGINT
)
AS
BEGIN
	
BEGIN TRY
	
	DECLARE @transactionSubType INT
	DECLARE @wuTransactionId BIGINT
	DECLARE @mtcn VARCHAR(50)

	SELECT 
		@wuTransactionId = WUTrxID, @transactionSubType = TransactionSubType, @mtcn = Mtcn 
	FROM 
		tWUnion_Trx 
	WHERE 
		WUTrxID = @wuTrxId
		  
	IF @transactionSubType = 3
		BEGIN
			
			SELECT 
				@wuTransactionId = WUTrxID
			FROM 
				tWUnion_Trx 
			WHERE 
				Mtcn = @mtcn AND TransactionSubType = 1
		END

	SELECT 
	   @wuTrxId AS WUTrxId
	  ,twt.[OriginatorsPrincipalAmount]
	  ,twt.[OriginatingCountryCode]
	  ,twt.[OriginatingCurrencyCode]
	  ,twt.[TranascationType]
	  ,twt.[PromotionsCode]
	  ,twt.[ExchangeRate]
	  ,twt.[DestinationPrincipalAmount]
	  ,twt.[GrossTotalAmount]
	  ,twt.[Charges]
	  ,twt.[TaxAmount]
	  ,twt.[Mtcn]
	  ,twt.[DTTerminalCreate]
	  ,twt.[DTTerminalLastModified]
	  ,twt.[PromotionDiscount]
	  ,twt.[OtherCharges]
	  ,twt.[MoneyTransferKey]
	  ,twt.[AdditionalCharges]
	  ,twt.[DestinationCountryCode]
	  ,twt.[DestinationCurrencyCode]
	  ,twt.[DestinationState]
	  ,twt.[IsDomesticTransfer]
	  ,twt.[IsFixedOnSend]
	  ,twt.[PhoneNumber]
	  ,twt.[Url]
	  ,twt.[AgencyName]
	  ,twt.[ChannelPartnerId]
	  ,twt.[ProviderId]
	  ,twt.[TestQuestion]
	  ,twt.[TempMTCN]
	  ,twt.[ExpectedPayoutStateCode]
	  ,twt.[ExpectedPayoutCityName]
	  ,twt.[TestAnswer]
	  ,twt.[TestQuestionAvaliable]
	  ,twt.[GCNumber]
	  ,twt.[SenderName]
	  ,twt.[PdsRequiredFlag]
	  ,twt.[DfTransactionFlag]
	  ,twt.[DeliveryServiceName]
	  ,twt.[DTAvailableForPickup]
	  ,twt.[DTServerCreate]
	  ,twt.[DTServerLastModified]
	  ,twt.[RecieverFirstName]
	  ,twt.[RecieverLastName]
	  ,twt.[RecieverSecondLastName]
	  ,twt.[PromoCodeDescription]
	  ,twt.[PromoName]
	  ,twt.[PromoMessage]
	  ,twt.[PromotionError]
	  ,twt.[Sender_ComplianceDetails_ComplianceData_Buffer]
	  ,twt.[recordingCountryCode]
	  ,twt.[recordingCurrencyCode]
	  ,twt.[originating_city]
	  ,twt.[originating_state]
	  ,twt.[municipal_tax]
	  ,twt.[state_tax]
	  ,twt.[county_tax]
	  ,twt.[plus_charges_amount]
	  ,twt.[message_charge]
	  ,twt.[total_undiscounted_charges]
	  ,twt.[total_discount]
	  ,twt.[total_discounted_charges]
	  ,twt.[instant_notification_addl_service_charges]
	  ,twt.[PaySideCharges]
	  ,twt.[PaySideTax]
	  ,twt.[AmountToReceiver]
	  ,twt.[SMSNotificationFlag]
	  ,twt.[PersonalMessage]
	  ,twt.[DeliveryServiceDesc]
	  ,twt.[ReferenceNo]
	  ,twt.[pay_or_do_not_pay_indicator]
	  ,twt.[OriginalDestinationCountryCode]
	  ,twt.[OriginalDestinationCurrencyCode]
	  ,twt.[FilingDate]
	  ,twt.[FilingTime]
	  ,twt.[PaidDateTime]
	  ,twt.[AvailableForPickup]
	  ,twt.[DelayHours]
	  ,twt.[AvailableForPickupEST]
	  ,twt.[WUCard_TotalPointsEarned]
	  ,twt.[OriginalTransactionID]
	  ,@transactionSubType AS TransactionSubType
	  ,twt.[ReasonCode]
	  ,twt.[ReasonDescription]
	  ,twt.[Comments]
	  ,twt.[DeliveryOption]
	  ,twt.[DeliveryOptionDesc]
	  ,twt.[PromotionSequenceNo]
	  ,twt.[CounterId]
	  ,twt.[PrincipalAmount]
	  ,twt.[Receiver_unv_Buffer]
	  ,twt.[Sender_unv_Buffer]
	  ,twt.[TransalatedDeliveryServiceName]
	  ,twt.[MessageArea]
	  ,twt.[WUAccountID]
	  ,twt.[WUReceiverID]
	  ,twt.[Address]
	  ,twt.[State/Province] AS [State]
	  ,twt.[City]
	  ,twt.[ZipCode] AS [PostalCode]
	FROM [dbo].[tWUnion_Trx] twt
	LEFT JOIN dbo.[tWUnion_Receiver] twr ON twt.WUReceiverID = twr.WUReceiverID
	WHERE WUTrxID = @wuTransactionId
		
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
