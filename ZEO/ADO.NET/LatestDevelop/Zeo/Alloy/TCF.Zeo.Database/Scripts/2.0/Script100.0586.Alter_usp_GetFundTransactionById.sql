-- ========================================================
-- Author:		<Kaushik Sakala>
-- Modified By : Abhijith
-- Create date: <11/15/2016>
-- Modified Date : 07/24/2017
-- Description:	<Stored Procedure to get fund transaction>
-- Modification : Removing the CXN State as it this column dropped and replacing it with State.
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
		[State],
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
