--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <03-11-2017>
-- Description:	Get committed check transaction count.
-- ================================================================================

-- exec usp_GetCommittedCheckTrxCountByPromoCode 2685104044137065,'BOGO',1000000024,1,1000000014,1

IF OBJECT_ID(N'usp_GetCommittedCheckTrxCountByPromoCode', N'P') IS NOT NULL
DROP PROC usp_GetCommittedCheckTrxCountByPromoCode
GO




CREATE PROCEDURE usp_GetCommittedCheckTrxCountByPromoCode
(
	@customerId BIGINT,
	@promoCode  NVARCHAR(100),
	@transactionId BIGINT,
	@productId INT,
	@customerSessionId BIGINT,
	@isSystemApplied BIT
)
AS
BEGIN	
    BEGIN TRY
	
		DECLARE @committedTrxCount INT = 0, @currentSessionTrxCount INT = 0

	    SELECT @committedTrxCount = COUNT(1) 
		FROM 
			tTxn_Check tc WITH (NOLOCK) 
		INNER JOIN 
			tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerID = @customerId AND tc.DiscountName = @promoCode AND cs.CustomerSessionID = tc.CustomerSessionId 	
		WHERE 
	    tc.State = 4 AND tc.CustomerSessionId != @customerSessionId 

		    
	
	IF @transactionId = 0
	BEGIN	
		    SELECT @currentSessionTrxCount = COUNT(1) 
			FROM 
				tTxn_Check tc WITH (NOLOCK)			 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON tc.CustomerSessionId = @customerSessionId AND tc.DiscountName = @promoCode AND sct.TransactionId = tc.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   tc.State NOT IN (5,6,8) AND sc.State != 2 AND tc.IsSystemApplied = @isSystemApplied 

	END 	
	ELSE	
	BEGIN
	
			SELECT @currentSessionTrxCount = COUNT(1) 
			FROM 
				tTxn_Check tc WITH (NOLOCK)			 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON tc.CustomerSessionId = @customerSessionId AND tc.DiscountName = @promoCode AND sct.TransactionId = tc.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   tc.State NOT IN (5,6,8) AND sc.State != 2 AND tc.IsSystemApplied = @isSystemApplied AND tc.TransactionID != @transactionId
    END
	
	SELECT (@committedTrxCount + @currentSessionTrxCount) AS CommittedTrxCount

	END TRY
	BEGIN CATCH
	   EXECUTE usp_CreateErrorInfo
	END CATCH
END


