-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 11/03/2016	
-- Description:	Get Visa Transaction by trxId
-- =============================================

IF OBJECT_ID(N'usp_GetVisaTransactionByTransactionId', N'P') IS NOT NULL
DROP PROC usp_GetVisaTransactionByTransactionId
GO

CREATE PROCEDURE usp_GetVisaTransactionByTransactionId
	@transactionId BIGINT

AS
BEGIN
BEGIN TRY
	SELECT 
		VisaTrxID,
		TransactionType,
		Amount,
		Fee,
		Description,
		Status,
		Balance,
		PromoCode,
		VisaAccountID		
	FROM 
		tVisa_Trx
	Where
		VisaTrxID = @transactionId
END TRY
BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO