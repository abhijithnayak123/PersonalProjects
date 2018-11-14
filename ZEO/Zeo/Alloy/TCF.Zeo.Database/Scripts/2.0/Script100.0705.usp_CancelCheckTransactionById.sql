--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Cancel the check transaction
-- Jira ID:		<AL-7582>
-- ================================================================================


IF OBJECT_ID(N'usp_CancelCheckTransactionById', N'P') IS NOT NULL
DROP PROC usp_CancelCheckTransactionById
GO

CREATE PROCEDURE usp_CancelCheckTransactionById
(
	@transactionId BIGINT,
	@state INT,
	@dTServerLastModified DATETIME,
	@dTTerminalLastModified DATETIME
)

AS
BEGIN
	BEGIN TRY

		-- Update the Check transaction state
		EXEC usp_UpdateCheckTransactionState @transactionId, @state, @dTServerLastModified, @dTTerminalLastModified

		-- In active the message center
		UPDATE
		    tMessageCenter
		SET
		    IsActive = 0,
			DTServerLastModified =  @dTServerLastModified,
			DTTerminalLastModified = @dTTerminalLastModified
		WHERE 
		  TransactionId = @transactionId

		-- Update transaction fee adjustments.
		UPDATE 
			tTxn_FeeAdjustments 
		SET
			IsActive = 0,
			DTServerLastModified = @DTServerLastModified,
			DTTerminalLastModified = @DTTerminalLastModified
		WHERE 
			TransactionId = @transactionId and ProductId = 1 

        
	END TRY
	
	BEGIN CATCH
       EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH
END



