--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <03-11-2017>
-- Description:	Get MO check transaction count.
-- ================================================================================

-- exec usp_GetCommittedMOTrxCountByPromoCode 2685104044137065,'BOGO',1000000024,1,1000000014,1

IF OBJECT_ID(N'usp_GetCommittedMOTrxCountByPromoCode', N'P') IS NOT NULL
DROP PROC usp_GetCommittedMOTrxCountByPromoCode
GO




CREATE PROCEDURE usp_GetCommittedMOTrxCountByPromoCode
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
			tTxn_MoneyOrder mo WITH (NOLOCK) 
		INNER JOIN 
			tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerID = @customerId AND mo.DiscountName = @promoCode AND cs.CustomerSessionID = mo.CustomerSessionId 	
		WHERE 
	    mo.State = 4 AND mo.CustomerSessionId != @customerSessionId 

		    
	
	IF @transactionId = 0
	BEGIN	
		    SELECT @currentSessionTrxCount = COUNT(1) 
			FROM 
				tTxn_MoneyOrder mo WITH (NOLOCK)			 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON mo.CustomerSessionId = @customerSessionId AND mo.DiscountName = @promoCode AND sct.TransactionId = mo.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   mo.State NOT IN (5,6,8) AND sc.State != 2 AND mo.IsSystemApplied = @isSystemApplied 

	END 	
	ELSE	
	BEGIN
	
			SELECT @currentSessionTrxCount = COUNT(1) 
			FROM 
				tTxn_MoneyOrder mo WITH (NOLOCK)			 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON mo.CustomerSessionId = @customerSessionId AND mo.DiscountName = @promoCode AND sct.TransactionId = mo.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   mo.State NOT IN (5,6,8) AND sc.State != 2 AND mo.IsSystemApplied = @isSystemApplied AND mo.TransactionID != @transactionId
    END
	
	SELECT (@committedTrxCount + @currentSessionTrxCount) AS CommittedTrxCount

	END TRY
	BEGIN CATCH
	   EXECUTE usp_CreateErrorInfo
	END CATCH
END



