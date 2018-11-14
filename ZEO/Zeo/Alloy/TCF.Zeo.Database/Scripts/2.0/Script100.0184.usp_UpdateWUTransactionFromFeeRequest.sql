--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to update the WU transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateWUTransactionFromFeeRequest', N'P') IS NOT NULL
DROP PROC usp_UpdateWUTransactionFromFeeRequest
GO

CREATE PROCEDURE usp_UpdateWUTransactionFromFeeRequest
(
     @wuTrxId BIGINT
	,@grossTotalAmount DECIMAL(18,2)
	,@destinationPrincipalAmount DECIMAL(18,2)
	,@originatorsPrincipalAmount DECIMAL(18,2)
	,@message_charge DECIMAL(18,2)
	,@charges DECIMAL(18,2)
	,@exchangeRate DECIMAL(18,4)
	,@plus_charges_amount DECIMAL(18,2)
	,@municipal_tax DECIMAL(18,2)
    ,@state_tax DECIMAL(18,2)
    ,@county_tax DECIMAL(18,2)
	,@taxAmount DECIMAL(18,2)
	,@destinationState VARCHAR(100)
	,@testQuestionAvaliable VARCHAR(5)
	,@dtServerLastModified DATETIME
	,@dtTerminalLastModified DATETIME
)
AS
BEGIN
	
BEGIN TRY
		UPDATE [dbo].[tWUnion_Trx]
		SET 
			GrossTotalAmount = @grossTotalAmount
			,DestinationPrincipalAmount = @destinationPrincipalAmount
			,OriginatorsPrincipalAmount = @originatorsPrincipalAmount
			,message_charge = @message_charge
			,Charges = @charges
			,ExchangeRate = @exchangeRate
			,plus_charges_amount = @plus_charges_amount
			,municipal_tax = @municipal_tax
			,state_tax = @state_tax
            ,county_tax = @county_tax
			,TaxAmount = @taxAmount
			,DestinationState = @destinationState
			,TestQuestionAvaliable = @testQuestionAvaliable
			,DTTerminalLastModified = @dtTerminalLastModified
			,DTServerLastModified = @dtServerLastModified
	   WHERE WUTrxId = @wuTrxId
	
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
