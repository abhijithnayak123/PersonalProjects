-- ================================================================================
-- Author:		Rizwana shaik
-- Create date: 02/23/2017
-- Description: Get Shopping Cart for receipt
-- Jira ID:		<>
-- ================================================================================

-- exec usp_GetShoppingCart 10000000 1

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_GetShoppingCartForReceipts')
BEGIN
	DROP PROCEDURE usp_GetShoppingCartForReceipts
END
GO
CREATE PROCEDURE usp_GetShoppingCartForReceipts
(
	@CustomerSessionId BIGINT
)
AS
BEGIN	
	BEGIN TRY	
	         DECLARE @ShoppingCartId BIGINT
					 SELECT TOP 1 @ShoppingCartId = CartID FROM tshoppingcarts WHERE  CustomerSessionId = @CustomerSessionId ORDER BY CartID DESC
        -- Get Check transaction
                     SELECT 
                           tc.TransactionId
	                 FROM  tShoppingCartTransactions sct 
	                       INNER JOIN tTxn_Check tc ON sct.TransactionId = tc.TransactionId 	
	                 WHERE
						   sct.CartId = @ShoppingCartId AND sct.ProductId = 1 AND sct.CartItemStatus = 0 AND tc.State=4	    
	  -- Get Bill Pay transaction
                    SELECT 
			              bp.TransactionId
                    FROM  tTxn_BillPay bp 
	                      INNER JOIN tShoppingCartTransactions sct ON sct.TransactionId = bp.TransactionId
	                WHERE sct.CartId = @ShoppingCartId AND sct.ProductId = 2 AND sct.CartItemStatus = 0 AND bp.State=4
	 -- Get Money Order Transaction        
	                SELECT 
	                      mo.TransactionId as TransactionId
	                FROM  tTxn_MoneyOrder mo			
	                      INNER JOIN tShoppingCartTransactions sct ON mo.TransactionId = sct.TransactionId
	                WHERE sct.CartId = @ShoppingCartId AND sct.ProductId = 5 AND sct.CartItemStatus = 0 AND mo.state=4

	  -- Get Fund transactions
                   SELECT 		 
		                 f.TransactionId,			
		                 f.FundType
		           FROM  tTxn_Funds f			
	                     INNER JOIN tShoppingCartTransactions sct ON f.TransactionId = sct.TransactionId
	               WHERE sct.CartId = @ShoppingCartId AND sct.ProductId = 6 AND sct.CartItemStatus = 0 AND f.State=4


	-- Get Money Transfer transaction

                  SELECT 
			            mt.TransactionId,
                        wmt.TranascationType,
			            wmt.TransactionSubType
                  FROM  tTxn_MoneyTransfer mt 	
	                    INNER JOIN tShoppingCartTransactions sct ON sct.TransactionId = mt.TransactionId
	                    INNER JOIN tWUnion_Trx wmt               ON mt.CXNId = wmt.WUTrxId 
	              WHERE sct.CartId = @ShoppingCartId AND sct.ProductId = 3 AND sct.CartItemStatus = 0 AND mt.State=4
	-- Get Cash In transaction

                  SELECT 
		               c.TransactionId		
				  FROM tTxn_Cash c			
	                   INNER JOIN tShoppingCartTransactions sct ON c.TransactionId = sct.TransactionId
	              WHERE sct.CartId = @ShoppingCartId AND sct.ProductId = 7 AND sct.CartItemStatus = 0 AND c.State=4
	 END TRY
	 BEGIN CATCH
	     EXEC usp_CreateErrorInfo  
	 END CATCH
END
GO
