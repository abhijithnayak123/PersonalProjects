--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to fetch customer fee adjustments
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetAuthorizedMOTransaction'
)
BEGIN
	DROP PROCEDURE usp_GetAuthorizedMOTransaction
END
GO

CREATE PROCEDURE usp_GetAuthorizedMOTransaction
	@customerID BIGINT

AS
BEGIN
	BEGIN TRY
		SELECT 
			tf.FeeAdjustmentId AS FeeAdjustmentId, tf.TransactionId AS TransactionId From tTxn_MoneyOrder tm
		INNER JOIN 
			tCustomerSessions cs ON cs.CustomerSessionID = tm.CustomerSessionId
		INNER JOIN 
			tCustomers c ON c.CustomerID = cs.CustomerId
		INNER JOIN 
			tTxn_FeeAdjustments tf ON tm.TransactionID = tf.TransactionId
		WHERE 
			c.CustomerID = @customerID AND tm.State = 2
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END