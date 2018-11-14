-- =============================================
-- Author:		<Kaushik Sakala>
-- Create date: <17/11/2016>
-- Description:	<Update Fund Transaction Status>
-- JIRA ID :	<AL-8232>
-- =============================================
IF OBJECT_ID(N'usp_UpdateFundTransactionState', N'P') IS NOT NULL
	DROP PROC usp_UpdateFundTransactionState
GO

CREATE PROCEDURE usp_UpdateFundTransactionState 
(
	@transactionId BIGINT,
	@State INT,
	@DTServerLastModified DATETIME,
	@DTTerminalLastModified DATETIME
)
	AS
BEGIN
	BEGIN TRY
		UPDATE tTxn_Funds 
		SET
			[State] = @State,
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
