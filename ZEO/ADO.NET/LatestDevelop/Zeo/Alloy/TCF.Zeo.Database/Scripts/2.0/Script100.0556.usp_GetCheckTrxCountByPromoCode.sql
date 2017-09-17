--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <13-06-2017>
-- Description:	Get transactions count for the promo code
-- Jira ID:		<AL-7705>
-- ================================================================================

-- exec usp_GetTransactionsCountByPromoCode 2146506813372844,'THREETHENFREE',1000000003

IF OBJECT_ID(N'usp_GetCheckTrxCountByPromoCode', N'P') IS NOT NULL
DROP PROC usp_GetCheckTrxCountByPromoCode
GO

CREATE PROCEDURE usp_GetCheckTrxCountByPromoCode
(
	@customerId BIGINT,
	@promoCode  NVARCHAR(100),
	@transactionId BIGINT,
	@isSystemApplied BIT,
	@productId INT,
	@dTStart DATETIME
)
AS
BEGIN	
    BEGIN TRY
	
	
	IF @transactionId = 0
	BEGIN
		
		SELECT COUNT(1) as count
			FROM 
				tTxn_Check tc WITH (NOLOCK)
			INNER JOIN 
				tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerID = @customerId AND tc.DiscountName = @promoCode AND cs.CustomerSessionID = tc.CustomerSessionId 
			INNER JOIN 
			    tShoppingCartTransactions sct ON sct.TransactionId = tc.TransactionID and sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId
			WHERE 
			   tc.State NOT IN (5,6,8) AND sc.State != 2 AND tc.IsSystemApplied = @isSystemApplied --AND tc.DTServerCreate >= @dTStart 
	END 
	
	ELSE
	
	BEGIN
		
		SELECT COUNT(1) as count
			FROM 
				tTxn_Check tc WITH (NOLOCK)
			INNER JOIN 
				tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerID = @customerId AND tc.DiscountName = @promoCode AND cs.CustomerSessionID = tc.CustomerSessionId 
			INNER JOIN 
			    tShoppingCartTransactions sct ON sct.TransactionId = tc.TransactionID and sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId
			WHERE 			
              tc.State NOT IN (5,6,8) AND sc.State != 2 AND tc.IsSystemApplied = @isSystemApplied --AND tc.DTServerCreate >= @dTStart 
			  AND (tc.TransactionID < @transactionId OR (tc.TransactionID > @transactionId AND tc.State = 4))
    END

	END TRY
	BEGIN CATCH
	   EXECUTE usp_CreateErrorInfo
	END CATCH
END


