--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <03-21-2017>
-- Description:	Get the Customer SessionId Id based on TransactionId. 
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_GetCustomerSessionIdByTransactionId', N'P') IS NOT NULL
BEGIN
     DROP PROCEDURE usp_GetCustomerSessionIdByTransactionId
END 
GO


CREATE PROCEDURE usp_GetCustomerSessionIdByTransactionId
(
    @transactionId BIGINT
	,@transactionType int
)
AS
BEGIN
     BEGIN TRY
	         SELECT  tsc.CustomerSessionId
	         FROM    dbo.tShoppingCarts tsc 
		             INNER JOIN dbo.tShoppingCartTransactions tsct  ON tsct.CartId = tsc.CartID
	         WHERE   tsct.TransactionId = @transactionId AND tsct.ProductId = @transactionType 
    END TRY
BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
