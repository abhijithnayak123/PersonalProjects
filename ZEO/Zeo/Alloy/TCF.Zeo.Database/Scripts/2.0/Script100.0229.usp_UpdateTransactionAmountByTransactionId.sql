-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 12/2/2016	
-- Description:	To Update Transaction Amount 
-- =============================================


IF OBJECT_ID(N'usp_UpdateTransactionAmountByTransactionId', N'P') IS NOT NULL
DROP PROC usp_UpdateTransactionAmountByTransactionId
GO

CREATE PROCEDURE usp_UpdateTransactionAmountByTransactionId
	@amount MONEY,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@trxId BIGINT,
	@PromoCode VARCHAR(50)

AS
BEGIN
BEGIN TRY
	UPDATE
		tVisa_Trx
	SET
		 Amount = @amount,
		 PromoCode=@PromoCode,
		 DTServerLastModified = @dTTerminalLastModified,
		 DTTerminalLastModified = @dTTerminalLastModified
	WHERE
		VisaTrxID = @trxId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO