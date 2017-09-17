--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to create trxn fee adjustment
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (SELECT  1 FROM sys.objects WHERE NAME = 'usp_CreateTrxFeeAdjustment')
BEGIN
	DROP PROCEDURE usp_CreateTrxFeeAdjustment
END
GO

CREATE PROCEDURE usp_CreateTrxFeeAdjustment
	@FeeAdjustmentId BIGINT,
	@TransactionId BIGINT,
	@IsActive BIT,
	@DTTerminalCreate DATETIME,
	@DTServerCreate DATETIME

AS
BEGIN
	BEGIN TRY
	INSERT INTO tTxn_FeeAdjustments
		(FeeAdjustmentId, TransactionId, IsActive, DTServerCreate ,DTTerminalCreate ,DTTerminalLastModified ,DTServerLastModified)
	VALUES
		(@FeeAdjustmentId, @TransactionId, @IsActive, @DTServerCreate, @DTTerminalCreate, NULL, NULL)

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
GO