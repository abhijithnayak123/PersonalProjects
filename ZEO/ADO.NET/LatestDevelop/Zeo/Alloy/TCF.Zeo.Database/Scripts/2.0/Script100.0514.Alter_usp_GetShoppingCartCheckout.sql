-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Get Active Shopping Cart checkout details
-- Jira ID:		AL-8047
-- ================================================================================

-- exec usp_GetShoppingCartCheckout 1000000009,34,1


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_GetShoppingCartCheckout')
BEGIN
	DROP PROCEDURE usp_GetShoppingCartCheckout
END
GO


CREATE PROCEDURE usp_GetShoppingCartCheckout
(
	@customerSessionId BIGINT,
	@channelPartnerId  BIGINT,
	@isReferral        BIT
)
AS
BEGIN
	
	BEGIN TRY


        DECLARE @cartId BIGINT = dbo.ufn_GetShoppingCartId(@customerSessionId, 1)

		-- Update the shoppingcart referral status.

		UPDATE tShoppingCarts 
		SET 
		  IsReferral = @isReferral
	    WHERE
		  CartId = @cartId
		
		SELECT 
		   @cartId as CartId,
		   dbo.ufn_IsCashOverCounter(@channelPartnerId) as IsCashOverCounter

        SELECT 
           tc.TransactionId,
           tc.State AS State,
	       sct.ProductId,
			 '' AS CheckNumber,	
		   tc.Amount,				
		   0 AS TransactionType,
			0 AS TransactionSubType
	    FROM
	       tShoppingCartTransactions sct WITH (NOLOCK)
	    INNER JOIN
	       tTxn_Check tc WITH (NOLOCK)
	       ON sct.TransactionId = tc.TransactionId 		 
	    WHERE
	       sct.CartId = @cartId 
			 AND 
			 sct.ProductId = 1    -- Check Processing
			 AND
			 sct.CartItemStatus = 0 -- Added
	    
		UNION ALL
	    
        SELECT
		     bp.TransactionId,			  
			 bp.State as State,
			 sct.ProductId,
			 '' AS CheckNumber,
			 bp.Amount,		
			 0 as TransactionType,
			 0 AS TransactionSubType			 
        FROM 
		     tTxn_BillPay bp WITH (NOLOCK)
		INNER JOIN
		     tShoppingCartTransactions sct WITH (NOLOCK)
			 ON sct.TransactionId = bp.TransactionId
		WHERE 
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 2     -- Bill Pay
			AND
			sct.CartItemStatus = 0 --Added


        UNION ALL

		SELECT 
			mo.TransactionId,
			mo.State AS State,		
			sct.ProductId,	
			mo.CheckNumber AS CheckNumber,
			mo.Amount,	
			0 as TransactionType,
			0 AS TransactionSubType	
		FROM
		    tTxn_MoneyOrder mo	WITH (NOLOCK)		
		INNER JOIN 
		    tShoppingCartTransactions sct WITH (NOLOCK)
		    ON mo.TransactionId = sct.TransactionId
		WHERE 
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 5    -- Money Order
			AND
  		   sct.CartItemStatus = 0 --Added
        
       
	    UNION ALL
	    
        SELECT 		 
			f.TransactionId,			
			f.State as State,
			sct.ProductId,
			'' AS CheckNumber,
			f.Amount,
			f.FundType TransactionType,
			0 AS TransactionSubType		
		FROM
		    tTxn_Funds f WITH (NOLOCK)			
		INNER JOIN
		    tShoppingCartTransactions sct WITH (NOLOCK)
		    ON f.TransactionId = sct.TransactionId
		WHERE 
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 6       -- Fund
			AND
			sct.CartItemStatus = 0	 --Added

		UNION ALL
	    
        SELECT 		 
			mt.TransactionId,			
			mt.State as State,
			sct.ProductId,
			'' AS CheckNumber,
			mt.Amount,
			mt.TransferType,
			mt.TransactionSubType AS TransactionSubType	
		FROM
		    tTxn_MoneyTransfer mt WITH (NOLOCK)		
		INNER JOIN
		    tShoppingCartTransactions sct WITH (NOLOCK)
		    ON mt.TransactionId = sct.TransactionId
		WHERE 
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 3      -- Money Transfer
			AND
			sct.CartItemStatus = 0 -- Added


	END TRY
	BEGIN CATCH
	      EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH
END
GO


