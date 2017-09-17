-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 12/2/2016	
-- Description:	To Update Transaction Amount 
-- =============================================


IF OBJECT_ID(N'usp_UpdateFundsCXNTransactionId', N'P') IS NOT NULL
DROP PROC usp_UpdateFundsCXNTransactionId
GO

CREATE PROCEDURE usp_UpdateFundsCXNTransactionId
	@cxnId BIGINT,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@transactionId BIGINT

AS
BEGIN
BEGIN TRY
	UPDATE
		tTxn_Funds
	SET
		 CXNId = @cxnId,
		 DTServerLastModified = @dTTerminalLastModified,
		 DTTerminalLastModified = @dTTerminalLastModified
	WHERE
		TransactionID = @transactionId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO