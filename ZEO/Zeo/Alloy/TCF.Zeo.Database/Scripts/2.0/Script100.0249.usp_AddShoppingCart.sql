-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Add Shopping Cart
-- Jira ID:		AL-8047
-- ================================================================================


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_AddShoppingCart')
BEGIN
    DROP PROCEDURE usp_AddShoppingCart
END
GO


CREATE PROCEDURE usp_AddShoppingCart
(
	@cartId               BIGINT OUTPUT,
	@state                INT,
	@customerSessionId    BIGINT,
	@dTServerCreate       DATETIME,
	@dTTerminalCreate     DATETIME
)
AS
BEGIN
    BEGIN TRY

          INSERT INTO tShoppingCarts 
            		   (
            			State,
						Status,
            			CustomerSessionId,
            			DTServerCreate,
            			DTTerminalCreate
            		   )
                      VALUES
            		   (
            			@State,
						1, -- Initial Checkout
            			@CustomerSessionId,
            			@dTServerCreate,
            			@dTTerminalCreate
            		   )

        SET @cartId = CAST(SCOPE_IDENTITY() AS BIGINT)


    END TRY
	
	BEGIN CATCH	    
		EXECUTE usp_CreateErrorInfo;  
	END CATCH

END
GO

