-- ========================================================
-- Author:		<Kaushik Sakala>
-- Create date: <11/15/2016>
-- Description:	<Stored Procedure to get fund transaction>
-- ========================================================

IF OBJECT_ID(N'usp_GetFundTransactionById', N'P') IS NOT NULL
DROP PROC usp_GetFundTransactionById
GO

CREATE PROCEDURE usp_GetFundTransactionById
@transactionId	BIGINT

AS
BEGIN
BEGIN TRY
	SELECT 
		ProviderId,
		ProviderAccountId,
		Amount,
		Fee,
		[Description],
		CXNState AS [STATE],
		ConfirmationNumber,
		FundType,
		BaseFee,
		AdditionalFee,
		DiscountName,
		IsSystemApplied,
		AddOnCustomerId
	FROM 
		tTxn_Funds
	WHERE
		TransactionID = @transactionId
END TRY

	BEGIN CATCH
	  EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO
