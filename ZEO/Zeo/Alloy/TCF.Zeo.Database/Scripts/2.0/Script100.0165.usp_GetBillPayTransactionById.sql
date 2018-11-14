--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-11-2016>
-- Description:	Get bill pay transactions
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_GetBillPayTransactionById') IS NOT NULL
BEGIN
	DROP PROCEDURE usp_GetBillPayTransactionById
END
GO

CREATE PROCEDURE usp_GetBillPayTransactionById
(
	@transactionID BIGINT
)
AS
BEGIN
	BEGIN TRY
		SELECT 
			Amount,
			Fee,
			Description,
			State,
			ConfirmationNumber,
			ProductId,
			AccountNumber,
			ProviderAccountID,
			ProviderID
		FROM 
			dbo.tTxn_BillPay WITH (NOLOCK)
		WHERE 
			TransactionID = @transactionID
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END
GO
