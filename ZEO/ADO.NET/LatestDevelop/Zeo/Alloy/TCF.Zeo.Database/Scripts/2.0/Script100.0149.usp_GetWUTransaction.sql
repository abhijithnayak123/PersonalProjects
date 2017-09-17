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
	   [WUTrxId]
	  ,[OriginatorsPrincipalAmount]
      ,[OriginatingCountryCode]
      ,[OriginatingCurrencyCode]
      ,[TranascationType]
      ,[PromotionsCode]
      ,[ExchangeRate]
      ,[DestinationPrincipalAmount]
      ,[GrossTotalAmount]
      ,[Charges]
      ,[TaxAmount]
      ,[Mtcn]
      ,[DTTerminalCreate]
      ,[DTTerminalLastModified]
      ,[PromotionDiscount]
      ,[OtherCharges]
      ,[MoneyTransferKey]
      ,[AdditionalCharges]
      ,[DestinationCountryCode]
      ,[DestinationCurrencyCode]
      ,[DestinationState]
      ,[IsDomesticTransfer]
      ,[IsFixedOnSend]
      ,[PhoneNumber]
      ,[Url]
      ,[AgencyName]
      ,[ChannelPartnerId]
      ,[ProviderId]
      ,[TestQuestion]
      ,[TempMTCN]
      ,[ExpectedPayoutStateCode]
      ,[ExpectedPayoutCityName]
      ,[TestAnswer]
      ,[TestQuestionAvaliable]
      ,[GCNumber]
      ,[SenderName]
      ,[PdsRequiredFlag]
      ,[DfTransactionFlag]
      ,[DeliveryServiceName]
      ,[DTAvailableForPickup]
      ,[DTServerCreate]
      ,[DTServerLastModified]
      ,[RecieverFirstName]
      ,[RecieverLastName]
      ,[RecieverSecondLastName]
      ,[PromoCodeDescription]
      ,[PromoName]
      ,[PromoMessage]
      ,[PromotionError]
      ,[Sender_ComplianceDetails_ComplianceData_Buffer]
      ,[recordingCountryCode]
      ,[recordingCurrencyCode]
      ,[originating_city]
      ,[originating_state]
      ,[municipal_tax]
      ,[state_tax]
      ,[county_tax]
      ,[plus_charges_amount]
      ,[message_charge]
      ,[total_undiscounted_charges]
      ,[total_discount]
      ,[total_discounted_charges]
      ,[instant_notification_addl_service_charges]
      ,[PaySideCharges]
      ,[PaySideTax]
      ,[AmountToReceiver]
      ,[SMSNotificationFlag]
      ,[PersonalMessage]
      ,[DeliveryServiceDesc]
      ,[ReferenceNo]
      ,[pay_or_do_not_pay_indicator]
      ,[OriginalDestinationCountryCode]
      ,[OriginalDestinationCurrencyCode]
      ,[FilingDate]
      ,[FilingTime]
      ,[PaidDateTime]
      ,[AvailableForPickup]
      ,[DelayHours]
      ,[AvailableForPickupEST]
      ,[WUCard_TotalPointsEarned]
      ,[OriginalTransactionID]
      ,[TransactionSubType]
      ,[ReasonCode]
      ,[ReasonDescription]
      ,[Comments]
      ,[DeliveryOption]
      ,[DeliveryOptionDesc]
      ,[PromotionSequenceNo]
      ,[CounterId]
      ,[PrincipalAmount]
      ,[Receiver_unv_Buffer]
      ,[Sender_unv_Buffer]
      ,[TransalatedDeliveryServiceName]
      ,[MessageArea]
      ,[WUAccountID]
      ,[WUReceiverID]
	FROM [dbo].[tWUnion_Trx]
	WHERE WUTrxID = @wuTrxId

END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
