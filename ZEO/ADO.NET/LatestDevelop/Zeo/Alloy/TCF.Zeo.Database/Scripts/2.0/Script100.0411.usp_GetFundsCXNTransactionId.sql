-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 12/2/2016	
-- Description:	To Update Transaction Amount 
-- =============================================


IF OBJECT_ID(N'usp_GetFundsCXNTransactionId', N'P') IS NOT NULL
DROP PROC usp_GetFundsCXNTransactionId
GO

CREATE PROCEDURE usp_GetFundsCXNTransactionId
	@transactionId BIGINT

AS
BEGIN
BEGIN TRY
	SELECT 
		CXNId
	FROM
		 dbo.tTxn_Funds ttf
	WHERE
		TransactionID = @transactionId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO