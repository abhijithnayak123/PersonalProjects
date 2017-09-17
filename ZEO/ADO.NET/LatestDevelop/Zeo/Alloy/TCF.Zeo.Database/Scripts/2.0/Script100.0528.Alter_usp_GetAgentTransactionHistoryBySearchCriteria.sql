-- =========================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Transaction History module>
-- Jira ID:		<AL-8869>
-- ==========================================================================================

IF EXISTS(
	SELECT 1
	FROM SYS.objects
	WHERE NAME = 'usp_GetAgentTransactionHistoryBySearchCriteria'
)

BEGIN
	DROP PROCEDURE usp_GetAgentTransactionHistoryBySearchCriteria
END
GO

CREATE PROCEDURE usp_GetAgentTransactionHistoryBySearchCriteria
	@agentId BIGINT,
	@transactionType NVARCHAR(255) = NULL,
	@location VARCHAR(200) = NULL,
	@dateRange DATETIME = NULL,
	@showAll BIT = 0,
	@transactionId BIGINT
AS
BEGIN
	
	DECLARE @transactionState VARCHAR(200) = 'Committed'
	DECLARE @transactionStates TABLE (tstate VARCHAR(100))

	INSERT INTO @transactionStates 
		VALUES ('Pending'),('Authorized'),('AuthorizationFailed'),('Committed'),('Failed'),('Cancelled'),('Expired'),
				('Declined'),('Initiated'),('Hold'),('Priced'),('Processing'),('Approved')


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
			vTransactionHistory vth
		WHERE 
			((@agentId = 0 OR TellerId = @agentId)
			AND (@transactionType IS NULL OR LOWER(TransactionType) = LOWER(@transactionType)) 
			AND (@location IS NULL OR LOWER(Location) = LOWER(@location))
			AND(@transactionId = 0)
			AND (@dateRange IS NULL OR CAST( TransactionDate AS DATE) >= CAST( @dateRange AS DATE))
			AND ((@showAll = 0 AND TransactionStatus = @transactionState)
			OR (@showAll = 1 AND TransactionStatus IN (SELECT tstate FROM @transactionStates)))) 
			OR (TransactionId = @transactionId)
		ORDER BY
			TransactionDate DESC
	END TRY
	BEGIN CATCH 
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
