--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <30-12-2016>
-- Description:	 Get cash transaction
-- Jira ID:		<AL-7582>
-- ================================================================================

IF OBJECT_ID(N'usp_GetCashTransactionById', N'P') IS NOT NULL
DROP PROC usp_GetCashTransactionById
GO


CREATE PROCEDURE usp_GetCashTransactionById
	@transactionId BIGINT
AS
BEGIN
	
	SELECT
		Amount,
		CASE State   
			WHEN 1 THEN 'Pending'     
			WHEN 2 THEN 'Authorized'     
			WHEN 3 THEN 'AuthorizationFailed'     
			WHEN 4 THEN 'Committed'     
			WHEN 5 THEN 'Failed' 
			WHEN 6 THEN 'Cancelled'
			WHEN 7 THEN 'Expired'
			WHEN 8 THEN 'Declined'
			WHEN 9 THEN 'Initiated'
			WHEN 10 THEN 'Hold'
			WHEN 11 THEN 'Priced'
			WHEN 12 THEN 'Processing'    
			ELSE 'Unknown' 
	    END TransactionStatus,
		CASE CashType   
			WHEN 1 THEN 'Cash In'   
			WHEN 2 THEN 'Cash Out'   
		END AS TransactionType
	 FROM 
		tTxn_Cash
	WHERE 
		TransactionID = @transactionId		
END
