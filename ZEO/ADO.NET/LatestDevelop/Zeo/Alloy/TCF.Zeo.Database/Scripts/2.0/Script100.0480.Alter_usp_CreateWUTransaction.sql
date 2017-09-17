--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <04-10-2017>
-- Description:	This SP is used to create a WU transaction.
-- Jira ID:		<>
-- ================================================================================

IF OBJECT_ID(N'usp_CreateWUTransaction', N'P') IS NOT NULL
DROP PROC usp_CreateWUTransaction
GO


CREATE PROCEDURE usp_CreateWUTransaction
(
	@originatorsPrincipalAmount DECIMAL(18,2)
	,@originatingCountryCode VARCHAR(20)
	,@originatingCurrencyCode VARCHAR(20)
	,@tranascationType VARCHAR(20)
	,@promoCode VARCHAR(50)
	,@exchangeRate DECIMAL(18,4)
	,@destinationPrincipalAmount DECIMAL(18,4)
	,@grossTotAmt DECIMAL(18,2)
	,@charges DECIMAL(18,2)
	,@taxAmt DECIMAL(18,2)
	,@mtcn VARCHAR(50)
	,@dtTerminalCreate DATETIME
	,@promoDiscount DECIMAL(18,2)
	,@otherCharges DECIMAL(18,2)
	,@moneyTranKey VARCHAR(100)
	,@additionalCharges DECIMAL(18,2)
	,@destinationCountryCode VARCHAR(100)
	,@destinationCurrencyCode VARCHAR(100)
	,@destinationState VARCHAR(100)
	,@isDomesticTransfer BIT
	,@isFixedOnSend BIT
	,@phoneNumber VARCHAR(20)
	,@Url VARCHAR(100)
	,@agencyName VARCHAR(200)
	,@channelPartnerId BIGINT
	,@providerId INT
	,@testQuestion VARCHAR(100)
	,@tempMtcn VARCHAR(100)
	,@expectedPayoutStateCode VARCHAR(100)
	,@expectedPayoutCityName VARCHAR(100)
	,@testAnswer VARCHAR(100)
	,@testQuestionAvaliable VARCHAR(5)
	,@gcNumber VARCHAR(20)
	,@senderName VARCHAR(50)
	,@pdsRequiredFlag BIT
	,@DfTransactionFlag BIT
	,@deliveryServiceName VARCHAR(100)
	,@DtAvailableForPickup DATETIME
	,@DtServerCreate DATETIME
	,@receiverFirstName VARCHAR(100)
	,@receiverLastName VARCHAR(100)
	,@receiverSecondLastName VARCHAR(100)
	,@promoCodeDescription NVARCHAR(80)
	,@promoName NVARCHAR(80)
	,@promoMsg NVARCHAR(80)
	,@promoError NVARCHAR(80)
	,@sender_ComplianceDetails_ComplianceData_Buffer VARCHAR(500)
	,@recordingCountryCode NVARCHAR(20)
	,@recordingCurrencyCode NVARCHAR(20)
	,@originating_city NVARCHAR(100)
	,@originating_state NVARCHAR(100)
	,@municipal_tax DECIMAL(18,2)
	,@state_tax DECIMAL(18,2)
	,@country_tax DECIMAL(18,2)
	,@plus_charges_amount DECIMAL(18,2)
	,@message_charge DECIMAL(18,2)
	,@total_undiscounted_charges DECIMAL(18,2)
	,@total_discount DECIMAL(18,2)
	,@total_discounted_charges DECIMAL(18,2)
	,@instant_notification_addl_service_charges NVARCHAR(300)
	,@paySideCharges DECIMAL(18,2)
	,@paySideTax DECIMAL(18,2)
	,@amountToReceiver DECIMAL(18,2)
	,@smsNotificationFlag VARCHAR(10)
	,@personalMsg NVARCHAR(1000)
	,@deliveryServiceDesc VARCHAR(100)
	,@referenceNo VARCHAR(50)
	,@pay_or_do_not_pay_indicator VARCHAR(10)
	,@originalDestinationCountryCode VARCHAR(20)
	,@originalDestinationCurrencyCode VARCHAR(20)
	,@fillingDate VARCHAR(10)
	,@fillingTime VARCHAR(10)
	,@paidDateTime NVARCHAR(50)
	,@availableForPickup NVARCHAR(50)
	,@delayHrs VARCHAR(10)
	,@availableForPickupEST VARCHAR(10)
	,@wuCardTotalPointsEarned VARCHAR(50)
	,@originalTransactionID BIGINT
	,@transactionSubType VARCHAR(20)
	,@reasonCode VARCHAR(20)
	,@reasonDesc VARCHAR(255)
	,@comments VARCHAR(50)
	,@deliveryOption VARCHAR(20)
	,@deliveryOptionDesc VARCHAR(100)
	,@promoSeqNo VARCHAR(20)
	,@counterId VARCHAR(100)
	,@principalAmt DECIMAL(18,2)
	,@receiver_unv_Buffer VARCHAR(300)
	,@sender_unv_Buffer VARCHAR(300)
	,@transalatedDeliveryServiceName VARCHAR(200)
	,@messageArea NVARCHAR(MAX)
	--,@wuTransactionId BIGINT
	,@accountId BIGINT
	,@receiverId BIGINT
)
AS
BEGIN
	
BEGIN TRY
	--- 
	SELECT 
		@gcNumber = PreferredCustomerAccountNumber
	FROM 
		tWUnion_Account
	WHERE 
		WUAccountID = @accountId

	--IF NOT EXISTS
	--(
	--	SELECT 1
	--	FROM tWUnion_Trx
	--	WHERE WUTrxID = @wuTransactionId
	--)
	--BEGIN 	
		INSERT INTO [dbo].[tWUnion_Trx]
           (
            [OriginatorsPrincipalAmount]
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
          -- ,[DTTerminalLastModified]
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
           --,[DTServerLastModified]
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
           ,[WUReceiverID])
     VALUES
           (
             @originatorsPrincipalAmount 
			,@originatingCountryCode 
			,@originatingCurrencyCode 
			,@tranascationType 
			,@promoCode 
			,@exchangeRate 
			,@destinationPrincipalAmount 
			,@grossTotAmt 
			,@charges 
			,@taxAmt 
			,@mtcn 
			,@dtTerminalCreate 
			,@promoDiscount 
			,@otherCharges 
			,@moneyTranKey 
			,@additionalCharges 
			,@destinationCountryCode 
			,@destinationCurrencyCode 
			,@destinationState
			,@isDomesticTransfer 
			,@isFixedOnSend 
			,@phoneNumber 
			,@Url 
			,@agencyName 
			,@channelPartnerId 
			,@providerId 
			,@testQuestion 
			,@tempMtcn 
			,@expectedPayoutStateCode 
			,@expectedPayoutCityName 
			,@testAnswer 
			,@testQuestionAvaliable 
			,@gcNumber 
			,@senderName 
			,@pdsRequiredFlag 
			,@DfTransactionFlag 
			,@deliveryServiceName 
			,@DtAvailableForPickup 
			,@DtServerCreate 
			,@receiverFirstName 
			,@receiverLastName 
			,@receiverSecondLastName 
			,@promoCodeDescription 
			,@promoName 
			,@promoMsg 
			,@promoError 
			,@sender_ComplianceDetails_ComplianceData_Buffer 
			,@recordingCountryCode
			,@recordingCurrencyCode
			,@originating_city
			,@originating_state
			,@municipal_tax 
			,@state_tax 
			,@country_tax 
			,@plus_charges_amount 
			,@message_charge 
			,@total_undiscounted_charges 
			,@total_discount 
			,@total_discounted_charges 
			,@instant_notification_addl_service_charges 
			,@paySideCharges 
			,@paySideTax 
			,@amountToReceiver 
			,@smsNotificationFlag 
			,@personalMsg 
			,@deliveryServiceDesc 
			,@referenceNo 
			,@pay_or_do_not_pay_indicator 
			,@originalDestinationCountryCode 
			,@originalDestinationCurrencyCode 
			,@fillingDate 
			,@fillingTime 
			,@paidDateTime
			,@availableForPickup
			,@delayHrs 
			,@availableForPickupEST 
			,@wuCardTotalPointsEarned 
			,@originalTransactionID 
			,@transactionSubType 
			,@reasonCode 
			,@reasonDesc 
			,@comments 
			,@deliveryOption 
			,@deliveryOptionDesc 
			,@promoSeqNo 
			,@counterId 
			,@principalAmt 
			,@receiver_unv_Buffer 
			,@sender_unv_Buffer 
			,@transalatedDeliveryServiceName 
			,@messageArea
			,@accountId 
			,@receiverId )

		SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS WUTransactionId
	--END
	
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
