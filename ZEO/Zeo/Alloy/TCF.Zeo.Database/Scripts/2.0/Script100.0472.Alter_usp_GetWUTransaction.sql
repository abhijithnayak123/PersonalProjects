--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
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
	
	SELECT 
	   twt.[WUTrxId]
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
      ,twt.[TransactionSubType]
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
	  ,twr.[Address]
	  ,twr.[State/Province] AS [State]
	  ,twr.[City]
	  ,twr.[ZipCode] AS [PostalCode]
	FROM [dbo].[tWUnion_Trx] twt
	LEFT JOIN dbo.[tWUnion_Receiver] twr ON twt.WUReceiverID = twr.WUReceiverID
	WHERE WUTrxID = @wuTrxId

END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
