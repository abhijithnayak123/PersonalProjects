-- ================================================================================
-- Author:		Ashok Kumar G
-- Create date: 01/20/2017
-- Description: Get Shopping Cart
-- Jira ID:		<>
-- ================================================================================

-- exec usp_GetShoppingCart 10000000 1

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_GetShoppingCart')
BEGIN
	DROP PROCEDURE usp_GetShoppingCart
END
GO

CREATE PROCEDURE usp_GetShoppingCart
(
	@ShoppingCartId BIGINT
)
AS
BEGIN	
	BEGIN TRY	
	IF(@ShoppingCartId IS NOT NULL)
    BEGIN
        -- Get Check transaction
    SELECT 
        tc.TransactionId,
        tc.State AS State
	FROM
	    tShoppingCartTransactions sct 
	INNER JOIN
	    tTxn_Check tc
	    ON sct.TransactionId = tc.TransactionId 	
	INNER JOIN
	    tChxr_Trx ct 
	    ON tc.CXNId = ct.ChxrTrxID 
	WHERE
		sct.CartId = @ShoppingCartId 
		AND 
		sct.ProductId = 1
		AND
		sct.CartItemStatus = 0	    

	-- Get Bill Pay transaction
    SELECT 
			bp.TransactionId,
            bp.[State] as State
    FROM tTxn_BillPay bp 
	INNER JOIN tMasterCatalog mc
			ON bp.ProductId = mc.MasterCatalogID	
	INNER JOIN
		    tShoppingCartTransactions sct 
			ON sct.TransactionId = bp.TransactionId
	WHERE 
		sct.CartId = @ShoppingCartId 
		AND 
		sct.ProductId = 2
		AND
		sct.CartItemStatus = 0

	-- Get Money Order Transaction        
	SELECT 
		mo.TransactionId as TransactionId,
	
		mo.[State] AS State		
				
	FROM
		tTxn_MoneyOrder mo			
	INNER JOIN 
		tShoppingCartTransactions sct
		ON mo.TransactionId = sct.TransactionId
	WHERE 
		sct.CartId = @ShoppingCartId 
		AND 
		sct.ProductId = 5 
		AND
		sct.CartItemStatus = 0

	    -- Get Fund transactions
        SELECT 		 
		f.TransactionId,			
		f.Amount,
		f.State as State,
		f.BaseFee,
		f.Fee,
		f.DiscountApplied,
		f.DiscountName,
		f.Description,
		f.FundType,
		va.CardNumber
		FROM
		tTxn_Funds f			
	INNER JOIN 
		tVisa_Account va
		ON f.ProviderAccountId = va.VisaAccountID
	INNER JOIN
		tShoppingCartTransactions sct
		ON f.TransactionId = sct.TransactionId
	WHERE 
		sct.CartId = @ShoppingCartId 
		AND 
		sct.ProductId = 6
		AND
		sct.CartItemStatus = 0


	-- Get Money Transfer transaction

        SELECT 
			mt.TransactionId,
            mt.[State] as State,
            wmt.TranascationType,
			wmt.TransactionSubType
    FROM 
		    tTxn_MoneyTransfer mt 	
	INNER JOIN
		    tShoppingCartTransactions sct 
			ON sct.TransactionId = mt.TransactionId
	INNER JOIN
		    tWUnion_Trx wmt
			ON mt.CXNId = wmt.WUTrxId 
	WHERE 
			sct.CartId = @ShoppingCartId 
			AND 
			sct.ProductId = 3     
			AND
			sct.CartItemStatus = 0


	    -- Get Cash In transaction

        SELECT 
		c.TransactionId	,		
        c.[State] as State		FROM
		tTxn_Cash c			
	inner join 
		tShoppingCartTransactions sct
		ON c.TransactionId = sct.TransactionId
	WHERE 
		sct.CartId = @ShoppingCartId 
		AND 
		sct.ProductId = 7
		AND
		sct.CartItemStatus = 0
    END

	END TRY
	BEGIN CATCH
	      EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH
END
GO
