--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-11-2016>
-- Description:	 Update transaction state
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_UpdateBillPayTransactionState') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_UpdateBillPayTransactionState
END
GO

CREATE PROCEDURE usp_UpdateBillPayTransactionState 
(
	 @newState               INT,
	 @transactionID          BIGINT,
	 @dtTerminalLastModified DATETIME,
	 @dtServerLastModified   DATETIME
)
AS
BEGIN
	BEGIN TRY
		UPDATE 
			dbo.tTxn_BillPay
		SET
			State = @newState,
			DTTerminalLastModified = @dtTerminalLastModified,
			DTServerLastModified = @dtServerLastModified
		WHERE 
			TransactionID = @transactionID
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END
GO
