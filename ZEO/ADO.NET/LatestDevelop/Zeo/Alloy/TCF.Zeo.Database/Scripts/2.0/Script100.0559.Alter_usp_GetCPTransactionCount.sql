--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Get Check processing transactions count except Parked, Canclled, Declined, and Failed
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
	@customerId BIGINT,
	@transactionId BIGINT

AS
BEGIN
	BEGIN TRY
		IF @transactionId = 0
		BEGIN
			SELECT 
				COUNT(DISTINCT(tc.TransactionID)) AS TransactionCount
			From 
				tTxn_Check tc
			INNER JOIN 
				tCustomerSessions cs ON cs.CustomerSessionID = tc.CustomerSessionId
			INNER JOIN 
				tCustomers c ON c.CustomerID = cs.CustomerId		
			INNER JOIN 
				tShoppingCarts sc ON sc.CustomerSessionId = cs.CustomerSessionId
			WHERE 
				c.CustomerID = @customerID AND tc.State NOT IN (5,6,8) AND sc.State != 2
			---Get Check processing transactions count except Parked, Canclled, Declined, and Failed
		END
		ELSE
		BEGIN
			SELECT 
				COUNT(DISTINCT(tc.TransactionID)) AS TransactionCount
			From 
				tTxn_Check tc
			INNER JOIN 
				tCustomerSessions cs ON cs.CustomerSessionID = tc.CustomerSessionId
			INNER JOIN 
				tCustomers c ON c.CustomerID = cs.CustomerId		
			INNER JOIN 
				tShoppingCarts sc ON sc.CustomerSessionId = cs.CustomerSessionId
			WHERE 
				c.CustomerID = @customerID AND tc.State NOT IN (5,6,8) AND sc.State != 2 AND tc.TransactionID < @transactionId
			---Get Check processing transactions count except Parked, Canclled, Declined, and Failed
		END

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END