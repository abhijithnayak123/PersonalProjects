--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to fetch customer fee adjustments
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetMOTransactionCount'
)
BEGIN
	DROP PROCEDURE usp_GetMOTransactionCount
END
GO

CREATE PROCEDURE usp_GetMOTransactionCount
	@customerId BIGINT

AS
BEGIN
	BEGIN TRY
		SELECT 
			COUNT(1) AS TransactionCount
		From 
			tTxn_MoneyOrder tm
		INNER JOIN 
			tCustomerSessions cs ON cs.CustomerSessionID = tm.CustomerSessionId
		INNER JOIN 
			tCustomers c ON c.CustomerID = cs.CustomerId		
		WHERE 
			c.CustomerID = @customerId AND tm.State = 4
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END