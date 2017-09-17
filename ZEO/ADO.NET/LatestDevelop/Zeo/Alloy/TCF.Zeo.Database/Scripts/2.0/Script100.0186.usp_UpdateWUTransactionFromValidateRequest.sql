--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to update the WU transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateWUTransactionFromValidateRequest', N'P') IS NOT NULL
DROP PROC usp_UpdateWUTransactionFromValidateRequest
GO


CREATE PROCEDURE usp_UpdateWUTransactionFromValidateRequest
(
	 @wuTrxId BIGINT
	,@mtcn VARCHAR(50)
	,@tempMTCN VARCHAR(100)
	,@deliveryServiceDesc VARCHAR(100)
	,@pdsRequiredFlag BIT
	,@dfTransactionFlag BIT
	,@paySideTax DECIMAL(18,2)
	,@paySideCharges DECIMAL(18,2)
	,@amountToReceiver DECIMAL(18,2)
	,@municipal_tax DECIMAL(18,2)
    ,@state_tax DECIMAL(18,2)
    ,@county_tax DECIMAL(18,2)
	,@plus_charges_amount DECIMAL(18,2)
	,@message_charge DECIMAL(18,2)
	,@total_discount DECIMAL(18,2)
	,@total_discounted_charges DECIMAL(18,2)
	,@total_undiscounted_charges DECIMAL(18,2)
	,@promoCodeDescription NVARCHAR(80)
    ,@promoName NVARCHAR(80)
    ,@promoMessage NVARCHAR(80)
	,@promotionDiscount DECIMAL(18,2)
	,@promotionsCode VARCHAR(50)
	,@promotionSequenceNo VARCHAR(20)
	,@grossTotalAmount DECIMAL(18,2)
	,@smsNotificationFlag VARCHAR(10)
	,@additionalCharges decimal(18,2)
	,@personalMessage NVARCHAR(1000)
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
	,@dtServerLastModified DATETIME
	,@dtTerminalLastModified DATETIME
)
AS
BEGIN
	
BEGIN TRY
	
		UPDATE [dbo].[tWUnion_Trx]
		SET 
			MTCN = @mtcn
			,TempMTCN = @tempMTCN
			,DeliveryServiceDesc = @deliveryServiceDesc
			,PdsRequiredFlag = @pdsRequiredFlag
			,DfTransactionFlag = @dfTransactionFlag
			,PaySideTax = @paySideTax
			,PaySideCharges = @paySideCharges
			,AmountToReceiver = @amountToReceiver
			,municipal_tax = @municipal_tax
			,state_tax = @state_tax
            ,county_tax = @county_tax
			,plus_charges_amount = @plus_charges_amount
			,message_charge = @message_charge
			,total_discount = @total_discount
			,total_discounted_charges = @total_discounted_charges
			,total_undiscounted_charges = @total_undiscounted_charges
			,PromoCodeDescription = @promoCodeDescription
			,PromoName = @promoName 
			,PromoMessage = @promoMessage
			,PromotionDiscount = @promotionDiscount
			,PromotionsCode = @promotionsCode
			,PromotionSequenceNo = @promotionSequenceNo
			--,GrossTotalAmount = @grossTotalAmount
			,SMSNotificationFlag = @smsNotificationFlag
			,AdditionalCharges = @additionalCharges
			,PersonalMessage = @personalMessage
			,FilingDate = @filingDate
			,FilingTime = @filingTime
			,instant_notification_addl_service_charges = @instant_notification_addl_service_charges
			,originating_city = @originating_city
			,originating_state = @originating_state
			,ExpectedPayoutCityName = @expectedPayoutCityName
			,ExpectedPayoutStateCode = @expectedPayoutStateCode
			,IsFixedOnSend = @isFixedOnSend
			,DeliveryOption = @deliveryOption
			,TransalatedDeliveryServiceName = @transalatedDeliveryServiceName
			,Sender_ComplianceDetails_ComplianceData_Buffer = @sender_ComplianceDetails_ComplianceData_Buffer
			,TaxAmount = @taxAmount
			,TestQuestion = @testQuestion
			,TestAnswer = @testAnswer
			,DTTerminalLastModified = @dtTerminalLastModified
			,DTServerLastModified = @dtServerLastModified
	    WHERE WUTrxId = @wuTrxId

END TRY
BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
