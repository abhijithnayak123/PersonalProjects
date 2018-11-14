--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to fetch customer fee adjustments
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetCPTransactionCount'
)
BEGIN
	DROP PROCEDURE usp_GetCPTransactionCount
END
GO

CREATE PROCEDURE usp_GetCPTransactionCount
	@customerId BIGINT

AS
BEGIN
	BEGIN TRY
		SELECT 
			COUNT(1) AS TransactionCount
		From 
			tTxn_Check tc
		INNER JOIN 
			tCustomerSessions cs ON cs.CustomerSessionID = tc.CustomerSessionId
		INNER JOIN 
			tCustomers c ON c.CustomerID = cs.CustomerId		
		WHERE 
			c.CustomerID = @customerId AND tc.State = 4
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END