--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <12-14-2016>
-- Description:	This SP is used to update the Money Transfer transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateModifyTransactions', N'P') IS NOT NULL
DROP PROC usp_UpdateModifyTransactions
GO


CREATE PROCEDURE usp_UpdateModifyTransactions
(
	@cancelTransactionId BIGINT
	,@modifyTransactionId BIGINT
	,@cancelWUTransactionId BIGINT
	,@modifyWUTransactionId BIGINT
	,@dtTerminalDate DATETIME
	,@dtServerDate DATETIME
)
AS
BEGIN
BEGIN TRY
		
		UPDATE tTxn_MoneyTransfer
		SET CXNId = @cancelWUTransactionId
			,DTServerLastModified = @dtServerDate
			,DTTerminalLastModified = @dtTerminalDate
		WHERE TransactionID = @cancelTransactionId

		UPDATE tTxn_MoneyTransfer
		SET CXNId = @modifyWUTransactionId
			,DTServerLastModified = @dtServerDate
			,DTTerminalLastModified = @dtTerminalDate
		WHERE TransactionID = @modifyTransactionId

END TRY
BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH
END
GO


