--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to update the WU transaction.
-- Jira ID:		<AL-8324>

/*
EXEC usp_SendMoneyModifyorRefund 1000000008,1000000000000010,'LEONARD','RAMOS','','','','2016122002354389',0,0,null,NULL,NULL,NULL,2,'12/20/2016','12/20/2016'
*/
-- ================================================================================

IF OBJECT_ID(N'usp_SendMoneyModifyorRefund', N'P') IS NOT NULL
DROP PROC usp_SendMoneyModifyorRefund
GO


CREATE PROCEDURE usp_SendMoneyModifyorRefund
(
     @wuTrxId BIGINT
	,@customerId BIGINT
	,@receiverFname VARCHAR(100) = NULL
	,@receiverLname VARCHAR(100) = NULL
	,@receiverSecondLname VARCHAR(100) = NULL
	,@testQuestion VARCHAR(100) = NULL
    ,@testAnswer VARCHAR(100) = NULL
	,@referenceNo VARCHAR(50)= NULL
	--For Refund transaction
	,@counterId VARCHAR(100) = NULL
	,@reasonCode VARCHAR(20) = NULL
	,@reasonDescription VARCHAR(255) = NULL
	,@comments VARCHAR(50) = NULL
	,@transactionSubType INT -- Either modify or refund. Pass it from the application. //2-Modify, 3-Refund
	,@dtTerminalLastModified DATETIME
	,@dtServerLastModified DATETIME
)
AS
BEGIN
BEGIN TRY

		DECLARE @cancelTranSubType VARCHAR(20) = '1' 
		DECLARE @cancelWUTrxId BIGINT
		DECLARE @modifyorRefundWUTrxId BIGINT
		
		--Check the receiver already existing in the database with the given name.
		--Check whether the other receiver exists for the given customerId and for given receiver details.
		-- There should be only one receiver with the given details.
		DECLARE @isOtherReceiverExists BIT = 
		(
			SELECT dbo.ufn_IsOtherReceiverExists(@customerId, @receiverFname, @receiverLname)
		)
		
		IF @isOtherReceiverExists = 0
		BEGIN
			
			--IF @transactionSubType = 2
			--BEGIN
			--	DECLARE @receiverId BIGINT

			--	SELECT @receiverId = WUReceiverID 
			--	FROM tWUnion_Trx 
									--JOIN tTxn_MoneyTransfer mt ON mt.CXNId = wt.WUTrxId
			--	WHERE WUTrxID = @wuTrxId

			--	UPDATE tWUnion_Receiver
			--		SET 
			--			FirstName = @receiverFname,
			--			LastName = @receiverLname,
			--			SecondLastName = @receiverSecondLname
			--		WHERE 
			--			WUReceiverID = @receiverId
			--END

		   -- Create a record in WU transaction table from the existing transaction from transaction Id with Cancel status.  	
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
			   ,[WUReceiverID]
		 )
		 SELECT 
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
			,WUTrxID  -- Id need to be assigned for OriginalTransactionId field.
			,@cancelTranSubType
			,[ReasonCode]
			,[ReasonDescription]
			,[Comments]
			,[DeliveryOption]
			,[DeliveryOptionDesc]
			,[PromotionSequenceNo]
			,CASE
				WHEN @transactionSubType= 2 --If modify then take from existing transaction.
				THEN [CounterId]
				ELSE @counterId
			 END
			,[PrincipalAmount]
			,[Receiver_unv_Buffer]
			,[Sender_unv_Buffer]
			,[TransalatedDeliveryServiceName]
			,[MessageArea]
			,[WUAccountID]
			,[WUReceiverID]
		 FROM tWUnion_Trx
		 WHERE WUTrxId = @WUTrxId

		 SET @cancelWUTrxId = CAST(SCOPE_IDENTITY() AS BIGINT)

		   -- Create a record in WU transaction table from the existing transaction from transaction Id with Modify status. 
		   -- Update the receiver details from receiver id of the transaction.
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
			   ,[WUReceiverID]
		  )
		  SELECT 
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
			,CASE
				WHEN @transactionSubType= 2 --If modify then take the value from parameter.
				THEN @testQuestion
				ELSE TestQuestion
			 END
			,[TempMTCN]
			,[ExpectedPayoutStateCode]
			,[ExpectedPayoutCityName]
			,CASE
				WHEN @transactionSubType= 2 --If modify then take the value from parameter.
				THEN @testAnswer
				ELSE TestAnswer
			 END
			,[TestQuestionAvaliable]
			,[GCNumber]
			,[SenderName]
			,[PdsRequiredFlag]
			,[DfTransactionFlag]
			,[DeliveryServiceName]
			,[DTAvailableForPickup]
			,[DTServerCreate]
			--,[DTServerLastModified]
			,CASE
				WHEN @transactionSubType= 2 --If modify then take the value from parameter.
				THEN @receiverFname
				ELSE RecieverFirstName
			 END
			,CASE
				WHEN @transactionSubType= 2 --If modify then take the value from parameter.
				THEN @receiverLname
				ELSE RecieverLastName
			 END
			,CASE
				WHEN @transactionSubType= 2 --If modify then take the value from parameter.
				THEN @receiverSecondLname
				ELSE RecieverSecondLastName
			 END
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
			,@referenceNo  --For both Modify and Refund we are passing reference number.
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
			,WUTrxID  -- Id need to be assigned for OriginalTransactionId field.
			,@transactionSubType
			,CASE
				WHEN @transactionSubType= 3 --If refund then take the value from parameter.
				THEN @reasonCode
				ELSE ReasonCode
			 END
			,CASE
				WHEN @transactionSubType= 3 --If refund then take the value from parameter.
				THEN @reasonDescription
				ELSE ReasonDescription
			 END
			,CASE
				WHEN @transactionSubType= 3 --If refund then take the value from parameter.
				THEN @comments
				ELSE Comments
			 END
			,[DeliveryOption]
			,[DeliveryOptionDesc]
			,[PromotionSequenceNo]
			,CASE
				WHEN @transactionSubType= 3 --If refund then take the value from parameter.
				THEN @counterId
				ELSE CounterId
			 END
			,[PrincipalAmount]
			,[Receiver_unv_Buffer]
			,[Sender_unv_Buffer]
			,[TransalatedDeliveryServiceName]
			,[MessageArea]
			,[WUAccountID]
			,[WUReceiverID]
		 FROM tWUnion_Trx
		 WHERE WUTrxId = @WUTrxId

		 SET @modifyorRefundWUTrxId = CAST(SCOPE_IDENTITY() AS BIGINT)

	  END

	  SELECT
			@cancelWUTrxId AS CancelTransactionId
			,@modifyorRefundWUTrxId AS ModifyorRefundTransactionId 

END TRY
BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH
END
GO
