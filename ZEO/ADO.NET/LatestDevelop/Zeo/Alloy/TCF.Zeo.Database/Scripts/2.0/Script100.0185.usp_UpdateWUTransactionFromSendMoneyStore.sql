--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to update the WU transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateWUTransactionFromSendMoneyStore', N'P') IS NOT NULL
DROP PROC usp_UpdateWUTransactionFromSendMoneyStore
GO


CREATE PROCEDURE usp_UpdateWUTransactionFromSendMoneyStore
(
	 @wuTrxId BIGINT
	,@mtcn VARCHAR(50)
	,@tempMTCN VARCHAR(100)
	,@amountToReceiver DECIMAL(18,2)
	,@availableForPickup NVARCHAR(50)
	,@dtAvailableForPickup DATETIME
	,@dfTransactionFlag BIT
	,@pdsRequiredFlag BIT
	,@paySideCharges DECIMAL(18,2)
	,@paySideTax DECIMAL(18,2)
	,@delayHours VARCHAR(10)
	,@deliveryServiceName VARCHAR(100)
	,@agencyName VARCHAR(200)
	,@url VARCHAR(100)
	,@phoneNumber VARCHAR(20)
	,@messageArea NVARCHAR(MAX)
	,@filingDate VARCHAR(10)
	,@filingTime VARCHAR(10)
	,@wuCardTotalPointsEarned VARCHAR(50)
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
			,AmountToReceiver = @amountToReceiver
			,AvailableForPickup = @availableForPickup
			,DTAvailableForPickup = @dtAvailableForPickup
			,DfTransactionFlag = @dfTransactionFlag
			,PdsRequiredFlag = @pdsRequiredFlag
			,PaySideCharges = @paySideCharges
			,PaySideTax = @paySideTax
			,DelayHours = @delayHours
			,DeliveryServiceName = @deliveryServiceName
			,AgencyName = @agencyName
			,Url = @url
			,PhoneNumber = @phoneNumber
			,MessageArea = @messageArea
			,FilingDate = @filingDate
			,FilingTime = @filingTime
			,WUCard_TotalPointsEarned = @wuCardTotalPointsEarned
			,DTTerminalLastModified = @dtTerminalLastModified
			,DTServerLastModified = @dtServerLastModified
	    WHERE WUTrxId = @wuTrxId

END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
