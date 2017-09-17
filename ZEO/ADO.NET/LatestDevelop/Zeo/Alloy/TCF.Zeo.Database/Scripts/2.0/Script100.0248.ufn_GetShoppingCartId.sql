-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Create the function to get shopping cart Id
-- Jira ID:		AL-8047
-- ================================================================================


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'FN' AND NAME = 'ufn_GetShoppingCartId')
BEGIN
    DROP FUNCTION dbo.ufn_GetShoppingCartId
END
GO

CREATE FUNCTION ufn_GetShoppingCartId
(
    @customerSessionId  BIGINT,
	@state              INT
)
RETURNS BIGINT
BEGIN
	
	DECLARE @shoppingCartId BIGINT
	
	SELECT 
		@shoppingCartId = CartId 
	FROM 
		tShoppingCarts WITH (NOLOCK)
	WHERE 
		State = @state
		AND
	    CustomerSessionId = @customerSessionId
	
	RETURN @shoppingCartId

END
GO

