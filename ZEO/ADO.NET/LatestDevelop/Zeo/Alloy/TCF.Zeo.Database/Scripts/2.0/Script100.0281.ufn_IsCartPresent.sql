-- ================================================================================
-- Author      : Manikandan Govindraj
-- Create date : 12/07/2016
-- Description : Create the function to verify the shopping cart item
-- Jira ID     : AL-8047
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'FN' AND NAME = 'ufn_IsCartItemPresent')
BEGIN
    DROP FUNCTION dbo.ufn_IsCartItemPresent
END
GO

CREATE FUNCTION ufn_IsCartItemPresent
(
    @customerSessionId  BIGINT
)
RETURNS BIT
BEGIN
	
	DECLARE @isCartItemPresent BIT = 0
	
	IF EXISTS
	( 
	    SELECT 1
	    FROM 
	    	tShoppingCarts sc WITH (NOLOCK) 
	        INNER JOIN
	    	tShoppingCartTransactions sct WITH (NOLOCK) 
	    	ON sc.CartId = sct.CartId
	    WHERE 
	    	sct.ProductId != 7  -- Cash
	    	AND
	        sc.CustomerSessionId = @customerSessionId
	        AND
			sc.State = 1        -- Active
			AND
			sct.CartItemStatus = 0  --added
	)
	BEGIN

	    SET @isCartItemPresent = 1

	END

	RETURN @isCartItemPresent

END
GO

