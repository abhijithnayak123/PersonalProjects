--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <13-06-2017>
-- Description:	Get transactions count for the promo code-- 
-- ================================================================================

-- exec usp_GetMOTrxCountByPromoCode 2146506813372844,'THREETHENFREE',1000000003

IF OBJECT_ID(N'usp_GetMOTrxCountByPromoCode', N'P') IS NOT NULL
DROP PROC usp_GetMOTrxCountByPromoCode
GO

CREATE PROCEDURE usp_GetMOTrxCountByPromoCode
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
	
		DECLARE @count INT = 0
	
	IF @transactionId = 0
	BEGIN
		
		SELECT @count = COUNT(1) 
			FROM 
				tTxn_MoneyOrder mo WITH (NOLOCK)
			INNER JOIN 
				tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerID = @customerId AND mo.DiscountName = @promoCode AND cs.CustomerSessionID = mo.CustomerSessionId 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON sct.TransactionId = mo.TransactionID and sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId
			WHERE 
			   mo.State NOT IN (5,6,8) AND sc.State != 2 AND mo.IsSystemApplied = @isSystemApplied --AND mo.DTServerCreate >= @dTStart 
	END 
	
	ELSE
	
	BEGIN
		
		SELECT @count = COUNT(1) 
			FROM 
				tTxn_MoneyOrder mo WITH (NOLOCK)
			INNER JOIN 
				tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerID = @customerId AND mo.DiscountName = @promoCode AND cs.CustomerSessionID = mo.CustomerSessionId 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON sct.TransactionId = mo.TransactionID and sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId
			WHERE 			
              mo.State NOT IN (5,6,8) AND sc.State != 2 AND mo.IsSystemApplied = @isSystemApplied --AND mo.DTServerCreate >= @dTStart 
			  AND (mo.TransactionID < @transactionId OR (mo.TransactionID > @transactionId AND mo.State = 4))
    END

	
	SELECT @count AS Count, ISNULL(Description,'') + ' check# :' + CONVERT(VARCHAR(5),@count + 1) as PromoDescrition FROM tChannelPartnerFeeAdjustments WHERE Name = @promoCode AND TransactionType = @productId 

	END TRY
	BEGIN CATCH
	   EXECUTE usp_CreateErrorInfo
	END CATCH
END


