--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <01-17-2017>
-- Description:	This SP is used to update the transaction state.
-- Jira ID:		<AL-8325>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateStateTransaction', N'P') IS NOT NULL
DROP PROC usp_UpdateStateTransaction
GO

CREATE PROCEDURE usp_UpdateStateTransaction
(

	@transactionId BIGINT,
	@dtTerminalLastModified DATETIME,
	@dtServerLastModified DATETIME,
	@state INT
)
AS 
BEGIN
BEGIN TRY

	DECLARE @isSendMoneyOrReceiveMoney BIT = 0

	IF EXISTS ( 
			SELECT TOP 1 * FROM tTxn_MoneyTransfer WHERE 
			TransactionID = @transactionId AND 
			TransactionSubType IS NULL AND (TransferType = 1 OR TransferType = 2))
		BEGIN
	
			UPDATE tTxn_MoneyTransfer
			SET 
				[State] = @state,
				DTServerLastModified = @dtServerLastModified,
				DTTerminalLastModified = @dtTerminalLastModified
			WHERE 
				TransactionID = @transactionId

			SET @isSendMoneyOrReceiveMoney = 1

		END
	ELSE
		BEGIN
			DECLARE @originalTransactionId BIGINT = (SELECT OriginalTransactionID FROM tTxn_MoneyTransfer WHERE TransactionID = @transactionId)

			UPDATE tTxn_MoneyTransfer
			SET 
				[State] = @state,
				DTServerLastModified = @dtServerLastModified,
				DTTerminalLastModified = @dtTerminalLastModified
			WHERE 
				OriginalTransactionID = @originalTransactionId AND (TransactionSubType = 1 OR TransactionSubType = 2) AND [State] = 2
		END

	SELECT @isSendMoneyOrReceiveMoney AS isSendMoneyOrReceiveMoney

END TRY

BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO


