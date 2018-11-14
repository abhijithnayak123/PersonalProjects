--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-04-2016>
-- Description:	This SP is used to update a money transfer transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateMoneyTransferTransaction', N'P') IS NOT NULL
DROP PROC usp_UpdateMoneyTransferTransaction
GO


CREATE PROCEDURE usp_UpdateMoneyTransferTransaction
(
	@transactionId BIGINT
	,@fee MONEY
	,@amount MONEY = 0
	,@cxnId BIGINT
	,@dtServerLastModified DATETIME
	,@dtTerminalLastModified DATETIME
)
AS
BEGIN
	
BEGIN TRY

	UPDATE tTxn_MoneyTransfer
	SET CXNId = @cxnId
		,Fee = @fee
		,Amount = 
			(
				-- This condition added as some of the calls are updating the amount field and whichever is not updating the 
				-- amount field retain the amount value eg: GetFee() call.
				CASE 
					WHEN @amount = 0 THEN Amount
					ELSE @amount
				END
			)
		,DTServerLastModified = @dtServerLastModified
		,DTTerminalLastModified = @dtTerminalLastModified
	WHERE TransactionID = @transactionId

END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
