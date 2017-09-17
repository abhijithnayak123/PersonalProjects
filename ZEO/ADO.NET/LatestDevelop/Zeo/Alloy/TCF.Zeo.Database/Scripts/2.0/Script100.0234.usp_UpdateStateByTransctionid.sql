--- ===============================================================================
-- Author:		<Kiranmaie Munagapati>
-- Create date: <07-12-2016>
-- Description:	 Update state
-- Jira ID:		<AL-8325>
-- ================================================================================


IF OBJECT_ID(N'usp_UpdateStateByTransactionId', N'P') IS NOT NULL
DROP PROC usp_UpdateStateByTransactionId
GO

CREATE PROCEDURE usp_UpdateStateByTransactionId
(
	@transactionId BIGINT,
	@state INT,
	@dtTerminalLastModified DATETIME,
	@dtServerLastModified DATETIME
)

AS
BEGIN
	BEGIN TRY
	   

		UPDATE
		    tTxn_MoneyTransfer
		SET
			[State] = @state,
			DTServerLastModified = @dtServerLastModified,
			DTTerminalLastModified = @dtTerminalLastModified
		WHERE 
		  TransactionId = @transactionId

      
	END TRY
	
	BEGIN CATCH
	  	EXECUTE usp_CreateErrorInfo
	END CATCH
END



