-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2017
-- Description: can close customer session
-- Jira ID:		AL-8047
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_IsCartPresent')
BEGIN
	DROP PROCEDURE usp_IsCartPresent
END
GO


CREATE PROCEDURE usp_IsCartPresent
(
	@customerSessionId     BIGINT
)
AS
BEGIN
	
	BEGIN TRY

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
					sc.CustomerSessionId = @customerSessionId
					AND
					sc.State = 1        -- Active
					AND
					sct.CartItemStatus = 0  --added
			)
			BEGIN

				SET @isCartItemPresent = 1

			END

			SELECT @isCartItemPresent AS IsCartPresent

    END TRY

BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO

