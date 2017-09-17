--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to update customer fee adjustments
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_UpdateCustomerFeeAdjustment'
)
BEGIN
	DROP PROCEDURE usp_UpdateCustomerFeeAdjustment
END
GO

CREATE PROCEDURE usp_UpdateCustomerFeeAdjustment
	@customerID BIGINT,
	@transactionId BIGINT,
	@isAvailed BIT,
	@transactionType INT,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME
AS
BEGIN
	BEGIN TRY

		DECLARE @FeeAdjustmentId BIGINT
		
		SELECT 
			@FeeAdjustmentId = tf.FeeAdjustmentId
		FROM
			tTxn_FeeAdjustments tf
		INNER JOIN
			tChannelPartnerFeeAdjustments cpf ON cpf.FeeAdjustmentId = tf.FeeAdjustmentId
		WHERE
			tf.TransactionId = @transactionId AND cpf.TransactionType = @transactionType
		
		;With customerFeeAdj as
		(
			SELECT TOP 1 CustomerFeeAdjustmentsID, CustomerID, IsAvailed, DTServerCreate, DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, FeeAdjustmentId FROM tCustomerFeeAdjustments
			WHERE
			CustomerID = @CustomerID AND FeeAdjustmentId = @FeeAdjustmentId AND IsAvailed = 0
		)
		
		Update customerFeeAdj 
		SET
			IsAvailed = @IsAvailed,
			DTServerLastModified = @DTServerLastModified,
			DTTerminalLastModified = @DTTerminalLastModified

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END


