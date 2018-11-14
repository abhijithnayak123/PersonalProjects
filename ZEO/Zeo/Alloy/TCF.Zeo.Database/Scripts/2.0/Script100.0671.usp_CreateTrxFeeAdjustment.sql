--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Modified By : <Manikandan Govindraj>
-- Create date: <08-02-2016>
-- Modified Date: <01-23-2018>
-- Modified reason : added customerId in transaction fee adjustments table.
-- Description:	 Stored procedure to create trxn fee adjustment
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (SELECT  1 FROM sys.objects WHERE NAME = 'usp_CreateTrxFeeAdjustment')
BEGIN
	DROP PROCEDURE usp_CreateTrxFeeAdjustment
END
GO

CREATE PROCEDURE usp_CreateTrxFeeAdjustment
	@PromotionId BIGINT,
	@TransactionId BIGINT,
	@IsActive BIT,
	@DTTerminalCreate DATETIME,
	@DTServerCreate DATETIME,
	@CustomerId BIGINT

AS
BEGIN
	BEGIN TRY
	INSERT INTO tTxn_FeeAdjustments
		(PromotionId, TransactionId, IsActive, DTServerCreate ,DTTerminalCreate ,DTTerminalLastModified ,DTServerLastModified, CustomerId)
	VALUES
		(@PromotionId, @TransactionId, @IsActive, @DTServerCreate, @DTTerminalCreate, NULL, NULL, @CustomerId)

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
GO