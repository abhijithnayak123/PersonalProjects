--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- modified By : Manikandan Govindraj
-- Modified Date: 10-25-2017
-- Modified Reason : When teller select currency type as USD - Updated destination currency code for fee enquiry request.
-- Description:	This SP is used to update the WU transaction.
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateWUTransaction', N'P') IS NOT NULL
DROP PROC usp_UpdateWUTransaction
GO


CREATE PROCEDURE [dbo].[usp_UpdateWUTransaction]
(
    @wuTrxId BIGINT
	,@mtcn VARCHAR(50)
	,@tempMtcn VARCHAR(100)
	,@deliveryServiceDesc VARCHAR(100)
	,@pdsRequiredFlag BIT
	,@dfTransactionFlag BIT
	,@paySideTax DECIMAL(18,2)
	,@paySideCharges DECIMAL(18,2)
	,@amountToReceiver DECIMAL(18,2)
	,@municipal_tax DECIMAL(18,2)
	,@state_tax DECIMAL(18,2)
	,@country_tax DECIMAL(18,2)
	,@plus_charges_amount DECIMAL(18,2)
	,@message_charge DECIMAL(18,2)
	,@total_discount DECIMAL(18,2)
	,@total_discounted_charges DECIMAL(18,2)
	,@total_undiscounted_charges DECIMAL(18,2)
	,@promoCodeDescription NVARCHAR(80)
	,@promoName NVARCHAR(80)
	,@promoMsg NVARCHAR(80)
	,@promoDiscount DECIMAL(18,2)
	,@promoSeqNo VARCHAR(20)
	,@grossTotAmt DECIMAL(18,2)
	,@smsNotificationFlag VARCHAR(10)
	,@additionalCharges DECIMAL(18,2)
	,@personalMsg NVARCHAR(1000)
	,@filingDate VARCHAR(10)
	,@filingTime VARCHAR(10)
	,@instant_notification_addl_service_charges NVARCHAR(300)
	,@originating_city NVARCHAR(100)
	,@originating_state NVARCHAR(100)
	,@expectedPayoutCityName VARCHAR(100)
	,@expectedPayoutStateCode VARCHAR(100)
	,@isFixedOnSend BIT
	,@deliveryOption VARCHAR(20)
	,@transalatedDeliveryServiceName VARCHAR(200)
	,@sender_ComplianceDetails_ComplianceData_Buffer VARCHAR(500)
	,@taxAmount DECIMAL(18,2)
	,@testQuestion VARCHAR(100)
	,@testAnswer VARCHAR(100)
	,@paidDateTime NVARCHAR(50)
	,@messageArea NVARCHAR(MAX)
	,@personalMessage NVARCHAR(1000)
	,@promotionsCode VARCHAR(50)
	,@gcNumber VARCHAR(20)
	,@destinationCountryCode VARCHAR(100)
	,@destinationCurrencyCode VARCHAR(100)
	,@recordingCountryCode NVARCHAR(20)
	,@recordingCurrencyCode NVARCHAR(20)
	,@recieverFirstName VARCHAR(100)
	,@recieverLastName VARCHAR(100)
	,@destinationPrincipalAmount DECIMAL(18,2)
	,@originatorsPrincipalAmount DECIMAL(18,2)
	,@testQuestionAvaliable VARCHAR(5)
	,@destinationState VARCHAR(100)
	,@exchangeRate DECIMAL(18,4)
	,@charges DECIMAL(18,2)
	,@availableForPickup NVARCHAR(50)
	,@dtAvailableForPickup DATETIME
	,@delayHours VARCHAR(10)
	,@deliveryServiceName VARCHAR(100)
	,@agencyName VARCHAR(200)
	,@url VARCHAR(100)
	,@phoneNumber VARCHAR(20)
	,@wuCardTotalPointsEarned VARCHAR(50)
	,@fee MONEY
	,@dtTerminalLastModified DATETIME
	,@dtServerLastModified DATETIME
	,@requestName VARCHAR(50)
	,@transferType VARCHAR(50) --1 - Send Money, 2 - Receive Money
	,@payordonotpayindicator VARCHAR(10)
)
AS
BEGIN
	
BEGIN TRY

	IF UPPER(@transferType) = 'SEND' 
	BEGIN

		IF LOWER(@requestName) = 'feeinquiryrequest'
		BEGIN 
			EXEC usp_UpdateWUTransactionFromFeeInquiryRequest
				@wuTrxId
				,@recieverFirstName 
				,@recieverLastName 
				,@destinationPrincipalAmount 
				,@originatorsPrincipalAmount 
				,@destinationCountryCode 
				,@destinationCurrencyCode 
				,@recordingCountryCode 
				,@recordingCurrencyCode 
				,@isFixedOnSend
				,@deliveryOption 
				,@personalMessage 
				,@promotionsCode 
				,@gcNumber 
				,@grossTotAmt 
				,@taxAmount 
				,@dtServerLastModified 
				,@dtTerminalLastModified 
		END
		ELSE IF LOWER(@requestName) = 'feerequest'
		BEGIN
			EXEC usp_UpdateWUTransactionFromFeeRequest
				 @wuTrxId
				,@grossTotAmt	
				,@destinationPrincipalAmount 
				,@originatorsPrincipalAmount 
				,@message_charge
				,@promotionsCode
				,@charges 
				,@exchangeRate 
				,@plus_charges_amount 
				,@municipal_tax 
				,@state_tax 
				,@country_tax 
				,@taxAmount 
				,@destinationState
				,@testQuestionAvaliable
				,@dtServerLastModified
				,@dtTerminalLastModified
				,@destinationCurrencyCode
				--Update the Fee based on TransactionId in PTNR transaction table.
			UPDATE mt
			SET mt.Fee = @fee, 
			mt.State = 2,
			mt.Amount = @originatorsPrincipalAmount,
			[DTTerminalLastModified] = @dtTerminalLastModified,
			[DTServerLastModified] = @dtServerLastModified
			FROM tTxn_MoneyTransfer mt
			WHERE mt.CXNId = @WUTrxId

		END
		ELSE IF LOWER(@requestName) = 'sendmoneystore'
		BEGIN
			EXEC usp_UpdateWUTransactionFromSendMoneyStore
				@WUTrxId 
				,@mtcn 
				,@tempMTCN 
				,@amountToReceiver 
				,@availableForPickup
				,@dtAvailableForPickup 
				,@dfTransactionFlag 
				,@pdsRequiredFlag 
				,@paySideCharges 
				,@paySideTax 
				,@delayHours 
				,@deliveryServiceName 
				,@agencyName 
				,@url 
				,@phoneNumber 
				,@messageArea 
				,@filingDate 
				,@filingTime 
				,@wuCardTotalPointsEarned 
				,@dtServerLastModified 
				,@dtTerminalLastModified 
		END
		ELSE IF LOWER(@requestName) = 'validaterequest'
		BEGIN
			EXEC usp_UpdateWUTransactionFromValidateRequest
				@WUTrxId 
				,@mtcn 
				,@tempMTCN 
				,@deliveryServiceDesc 
				,@pdsRequiredFlag 
				,@dfTransactionFlag 
				,@paySideTax 
				,@paySideCharges 
				,@amountToReceiver 
				,@municipal_tax 
				,@state_tax 
				,@country_tax 
				,@plus_charges_amount 
				,@message_charge 
				,@total_discount 
				,@total_discounted_charges 
				,@total_undiscounted_charges 
				,@promoCodeDescription 
				,@promoName 
				,@promoMsg 
				,@promoDiscount 
				,@promotionsCode 
				,@promoSeqNo 
				,@grossTotAmt 
				,@smsNotificationFlag 
				,@additionalCharges 
				,@personalMessage 
				,@filingDate 
				,@filingTime
				,@instant_notification_addl_service_charges 
				,@originating_city 
				,@originating_state 
				,@expectedPayoutCityName
				,@expectedPayoutStateCode 
				,@isFixedOnSend
				,@deliveryOption
				,@transalatedDeliveryServiceName
				,@sender_ComplianceDetails_ComplianceData_Buffer
				,@taxAmount 
				,@testQuestion
				,@testAnswer 
				,@dtServerLastModified
				,@dtTerminalLastModified

			--Update the Fee based on TransactionId in PTNR transaction table.
			UPDATE mt
			SET mt.Fee = @fee, 
			mt.State = 2,
			[DTTerminalLastModified] = @dtTerminalLastModified,
			[DTServerLastModified] = @dtServerLastModified
			FROM tTxn_MoneyTransfer mt
			JOIN tWUnion_Trx wt ON mt.CXNId = wt.WUTrxId
			WHERE wt.WUTrxId = @WUTrxId
		END

	END
	ELSE IF UPPER(@transferType) = 'RECEIVE' 
	BEGIN
		IF LOWER(@requestName) = 'validaterequest'
		BEGIN
			UPDATE tWUnion_Trx
			SET 
			  Mtcn = @mtcn,
			  DTTerminalLastModified = @dtTerminalLastModified,
			  DTServerLastModified = @dtServerLastModified,
			  PaidDateTime = @paidDateTime,
			  MessageArea = @messageArea,
			  Sender_ComplianceDetails_ComplianceData_Buffer = @sender_ComplianceDetails_ComplianceData_Buffer,
			  pay_or_do_not_pay_indicator = @payordonotpayindicator
			WHERE WUTrxId = @WUTrxId

			UPDATE mt
			SET 
				--mt.Amount = @destinationPrincipalAmount,
				mt.State = 2,
				[DTTerminalLastModified] = @dtTerminalLastModified,
				[DTServerLastModified] = @dtServerLastModified
			FROM 
				tTxn_MoneyTransfer mt
				JOIN tWUnion_Trx wt ON mt.CXNId = wt.WUTrxId
			WHERE 
				wt.WUTrxId = @WUTrxId
		END

		ELSE IF LOWER(@requestName) = 'sendmoneystore'
		BEGIN
			UPDATE tWUnion_Trx
			SET 
			  Mtcn = @mtcn,
			  DTTerminalLastModified = @dtTerminalLastModified,
			  DTServerLastModified = @dtServerLastModified,
			  PaidDateTime = @paidDateTime,
			  MessageArea = @messageArea
			WHERE WUTrxId = @WUTrxId
		END
	END 
	

END TRY
BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	
END CATCH
END
GO
