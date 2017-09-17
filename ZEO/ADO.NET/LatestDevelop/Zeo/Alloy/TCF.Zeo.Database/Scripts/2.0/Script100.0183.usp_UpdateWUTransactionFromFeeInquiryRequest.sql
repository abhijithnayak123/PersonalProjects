--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to update the WU transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateWUTransactionFromFeeInquiryRequest', N'P') IS NOT NULL
DROP PROC usp_UpdateWUTransactionFromFeeInquiryRequest
GO


CREATE PROCEDURE usp_UpdateWUTransactionFromFeeInquiryRequest
(
     @WUTrxId BIGINT
	,@recieverFirstName VARCHAR(100)
	,@recieverLastName VARCHAR(100)
	,@destinationPrincipalAmount DECIMAL(18,2)
	,@originatorsPrincipalAmount DECIMAL(18,2)
	,@destinationCountryCode VARCHAR(100)
	,@destinationCurrencyCode VARCHAR(100)
	,@recordingCountryCode NVARCHAR(20)
	,@recordingCurrencyCode NVARCHAR(20)
	,@isFixedOnSend BIT
	,@deliveryOption VARCHAR(20)
	,@personalMessage NVARCHAR(1000)
	,@promotionsCode VARCHAR(50)
	,@gcNumber VARCHAR(20)
	,@grossTotalAmount DECIMAL(18,2)
	,@taxAmount DECIMAL(18,2)
	,@dtServerLastModified DATETIME
	,@dtTerminalLastModified DATETIME
)
AS
BEGIN
	
BEGIN TRY
	
		UPDATE [dbo].[tWUnion_Trx]
		SET 
		  RecieverFirstName = @recieverFirstName
		  ,RecieverLastName = @recieverLastName
		  ,DestinationPrincipalAmount = @destinationPrincipalAmount
		  ,OriginatorsPrincipalAmount = @originatorsPrincipalAmount
		  ,DestinationCountryCode = @destinationCountryCode
		  ,DestinationCurrencyCode = @destinationCurrencyCode
		  ,recordingCountryCode = @recordingCountryCode
		  ,recordingCurrencyCode = @recordingCurrencyCode
		  ,IsFixedOnSend = @isFixedOnSend
		  ,DeliveryOption = @deliveryOption
		  ,PersonalMessage = @personalMessage
		  ,PromotionsCode = @promotionsCode
		  ,GCNumber = @gcNumber
		  ,GrossTotalAmount = @grossTotalAmount
		  ,TaxAmount = @taxAmount
		  ,[DTTerminalLastModified] = @dtTerminalLastModified
		  ,[DTServerLastModified] = @dtServerLastModified
	   WHERE WUTrxId = @WUTrxId
	
	
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
