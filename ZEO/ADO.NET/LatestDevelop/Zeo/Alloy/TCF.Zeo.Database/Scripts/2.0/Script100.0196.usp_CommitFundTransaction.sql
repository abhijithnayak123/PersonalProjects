-- =============================================
-- Author:		<Kaushik Sakala>
-- Create date: <16/11/2016>
-- Description:	<Commit the fund transaction>
-- Jira ID:		<Al-8323>
-- =============================================

IF OBJECT_ID(N'usp_CommitFundTransaction', N'P') IS NOT NULL
	DROP PROC usp_CommitFundTransaction
GO


CREATE PROCEDURE usp_CommitFundTransaction 
(
	@transactionId BIGINT,
	@status INT,
	@CustomerSessionId BIGINT,
	@DTServerLastModified DATETIME,
	@DTTerminalLastModified DATETIME
)
AS
BEGIN
	BEGIN TRY
	UPDATE 
	   tTxn_Funds
	SET 
	   State = @status,
	   CustomerSessionId = @CustomerSessionId,
	   DTTerminalLastModified = @DTTerminalLastModified,
	   DTServerLastModified = @dTServerLastModified
	WHERE 
	   TransactionId = @transactionId
END TRY

BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
