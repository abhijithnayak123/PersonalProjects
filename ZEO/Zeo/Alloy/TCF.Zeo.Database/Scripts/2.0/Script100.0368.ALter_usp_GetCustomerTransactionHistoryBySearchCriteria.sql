-- =========================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Transaction History module>
-- Jira ID:		<AL-8869>
-- ==========================================================================================


IF EXISTS(
	SELECT 1
	FROM SYS.objects
	WHERE NAME = 'usp_GetCustomerTransactionHistoryBySearchCriteria'
)

BEGIN
	DROP PROCEDURE usp_GetCustomerTransactionHistoryBySearchCriteria
END
GO


CREATE PROCEDURE usp_GetCustomerTransactionHistoryBySearchCriteria
	@customerId BIGINT,
	@transactionType NVARCHAR(255) = NULL,
	@location VARCHAR(200) = NULL,
	@dateRange DATETIME = NULL	
AS
BEGIN
DECLARE @transactionStates VARCHAR(200) = 'Committed'
	BEGIN TRY
		SELECT 
			CustomerId,
			TransactionDate,
			Teller,
			TellerId,
			SessionID,
			TransactionId,
			Location,
			TransactionType,
			TotalAmount,
			TransactionDetail,
			CustomerName,
			TransactionStatus
		FROM
			vTransactionHistory
		WHERE 
			CustomerId = @customerId
			AND  TransactionStatus = @transactionStates
			AND (@transactionType IS NULL OR LOWER(TransactionType) = LOWER(@transactionType)) 
			AND (@location IS NULL OR LOWER(Location) = LOWER(@location)) 
			AND (@dateRange IS NULL OR CAST( TransactionDate AS DATE) >= CAST( @dateRange AS DATE)) 
		ORDER BY
			TransactionDate DESC
	END TRY
	BEGIN CATCH 
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
