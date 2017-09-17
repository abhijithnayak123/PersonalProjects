--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Update the check transaction state
-- Jira ID:		<AL-7582>
-- ================================================================================


IF OBJECT_ID(N'usp_UpdateCheckTransactionState', N'P') IS NOT NULL
DROP PROC usp_UpdateCheckTransactionState
GO

CREATE PROCEDURE usp_UpdateCheckTransactionState
(
	@transactionId BIGINT,
	@state INT,
	@dTServerLastModified DATETIME,
	@dTTerminalLastModified DATETIME
)

AS
BEGIN
	BEGIN TRY
	
		UPDATE
		  tTxn_Check
		SET
		    State = @state,
			DTServerLastModified =  @dTServerLastModified,
			DTTerminalLastModified = @dTTerminalLastModified
		WHERE 
		  TransactionId = @transactionId

       -- Date should be updated, once check is declined from the CP monitor
	    
		UPDATE 
		  tMessageCenter
		SET
		  DTTerminalLastModified = @dTTerminalLastModified,
		  DTServerLastModified   = @dTServerLastModified
		WHERE 
		  TransactionId = @transactionId

	END TRY
	BEGIN CATCH

       EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END




