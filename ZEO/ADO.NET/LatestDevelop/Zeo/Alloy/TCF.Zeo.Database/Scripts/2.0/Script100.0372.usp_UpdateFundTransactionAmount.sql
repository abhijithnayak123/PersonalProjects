-- =============================================
-- Author:		<Kaushik Sakala>
-- Create date: <17/11/2016>
-- Description:	<Update Fund Transaction Amount>
-- JIRA ID :	<AL-8232>
-- =============================================
IF OBJECT_ID(N'usp_UpdateFundTransactionAmount', N'P') IS NOT NULL
	DROP PROC usp_UpdateFundTransactionAmount
GO

CREATE PROCEDURE usp_UpdateFundTransactionAmount 
(
	@transactionId BIGINT,
	@amount MONEY,
	@DTServerLastModified DATETIME,
	@DTTerminalLastModified DATETIME
)
	AS
BEGIN
	BEGIN TRY
		UPDATE tTxn_Funds 
		SET
			Amount = @amount,
			DTServerLastModified = @DTServerLastModified,
			DTTerminalLastModified = @DTTerminalLastModified
		WHERE
			TransactionID = @transactionId
	END TRY
	BEGIN CATCH
		EXEC usp_CreateErrorInfo
	END CATCH
END
GO
