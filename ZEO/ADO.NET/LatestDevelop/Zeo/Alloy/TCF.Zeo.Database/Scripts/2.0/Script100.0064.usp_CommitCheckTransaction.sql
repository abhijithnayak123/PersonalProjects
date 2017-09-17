--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Commit the check transaction
-- Jira ID:		<AL-7582>
-- ================================================================================

IF OBJECT_ID(N'usp_CommitCheckTransaction', N'P') IS NOT NULL
DROP PROC usp_CommitCheckTransaction
GO


CREATE PROCEDURE usp_CommitCheckTransaction
(	
	@state INT,
	@transactionId BIGINT,
	@customerId BIGINT,
	@dTServerLastModified DATETIME,
	@dTTerminalLastModified DATETIME 
)
AS
BEGIN
			
	BEGIN TRY
	
	-- update the transaction status 

	UPDATE 
	   tTxn_Check
	SET 
	   State = @state,
	   DTTerminalLastModified = @dTTerminalLastModified,
	   DTServerLastModified = @dTServerLastModified
	WHERE 
	   TransactionId = @transactionId


	-- update the message center if the check is active

	UPDATE 
	   tMessageCenter
	SET 
	   IsActive = 0,
	   DTTerminalLastModified = @dTTerminalLastModified,
	   DTServerLastModified = @dTServerLastModified
	WHERE 
	   TransactionId = @transactionId


    -- Update the customer fee adjustment.

    EXEC usp_UpdateCustomerFeeAdjustment 
			@customerId, 
			@transactionId, 
			1, --Is Availed
			1,--Transaction Type for Check
			@dTServerLastModified, 
			@dTTerminalLastModified
    
END TRY

BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH

END
GO







