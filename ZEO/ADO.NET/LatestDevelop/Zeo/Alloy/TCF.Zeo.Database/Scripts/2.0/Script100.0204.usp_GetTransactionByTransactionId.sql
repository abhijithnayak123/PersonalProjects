-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To get transaction by transaction id
-- =============================================

IF OBJECT_ID(N'usp_GetTransactionByTransactionId', N'P') IS NOT NULL
DROP PROC usp_GetTransactionByTransactionId
GO

CREATE PROCEDURE usp_GetTransactionByTransactionId
	(
		@transactionId BIGINT
	)
AS
BEGIN
BEGIN TRY
	SELECT 
		ProviderID,
		ProviderAccountId,
		Amount,
		Fee,
		[Description],
		[State] AS [State],
		ConfirmationNumber,
		FundType,
		BaseFee,
		AdditionalFee,
		DiscountName,
		IsSystemApplied,
		AddOnCustomerId
	FROM 
		tTxn_Funds
	Where
		TransactionID = @transactionId
END TRY
BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
