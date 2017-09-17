--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <13-06-2017>
-- Description:	Made changes to get the promo count from DB
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
	@dTStart DATETIME,
	@customerSessionId BIGINT
)
AS
BEGIN	
    BEGIN TRY
	
	DECLARE @count INT = 0, @committedTrxCount INT = 0

	SELECT @committedTrxCount = COUNT(1) 
	FROM 
		tTxn_Check tc WITH (NOLOCK) 
	INNER JOIN 
		tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerID = @customerId AND tc.DiscountName = @promoCode AND cs.CustomerSessionID = tc.CustomerSessionId 	
	WHERE 
	   tc.State = 4 AND tc.IsSystemApplied = @isSystemApplied 

	
	IF @transactionId = 0
	BEGIN
		
		SELECT @count = COUNT(1) 
			FROM 
				tTxn_Check tc WITH (NOLOCK)			 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON tc.CustomerSessionId = @customerSessionId AND tc.DiscountName = @promoCode AND sct.TransactionId = tc.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   tc.State NOT IN (4,5,6,8) AND sc.State != 2 AND tc.IsSystemApplied = @isSystemApplied 
	END 
	
	ELSE
	
	BEGIN
		
		SELECT @count = COUNT(1) 
			FROM 
				tTxn_Check tc WITH (NOLOCK)			 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON tc.CustomerSessionId = @customerSessionId AND tc.DiscountName = @promoCode AND sct.TransactionId = tc.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId  
			WHERE 			
              tc.State NOT IN (4,5,6,8) AND sc.State != 2 AND tc.IsSystemApplied = @isSystemApplied 
			  AND (tc.TransactionID < @transactionId) -- OR (tc.TransactionID > @transactionId AND tc.State = 4))
    END

	 SELECT (@count + @committedTrxCount) AS Count, ISNULL(Description + CHAR(13) + CHAR(10),'') + 'Check# :' + CONVERT(VARCHAR(5),@count + @committedTrxCount + 1) as PromoDescrition FROM tChannelPartnerFeeAdjustments WHERE Name = @promoCode AND TransactionType = @productId


	END TRY
	BEGIN CATCH
	   EXECUTE usp_CreateErrorInfo
	END CATCH
END


