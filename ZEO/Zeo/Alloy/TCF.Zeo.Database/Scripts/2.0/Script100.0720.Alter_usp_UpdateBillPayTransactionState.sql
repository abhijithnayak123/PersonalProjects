--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Modify By: <Nitish Biradar>
-- Modify date: <14-03-2018>
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

		IF @newState = 6
		BEGIN
			
			UPDATE 
				tTxn_FeeAdjustments 
			SET
				IsActive = 0,
				DTServerLastModified = @DTServerLastModified,
				DTTerminalLastModified = @DTTerminalLastModified
			WHERE 
				TransactionId = @transactionID and ProductId = 2 
		END
	
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END
GO
