-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Add shopping cart transactions
-- Jira ID:		AL-8047
-- ================================================================================


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_AddShoppingCartTransaction')
BEGIN
    DROP PROCEDURE usp_AddShoppingCartTransaction
END
GO


CREATE PROCEDURE usp_AddShoppingCartTransaction
(
	@customerSessionId   BIGINT,
	@transactionId       BIGINT,
	@productId           INT,
	@dtServerCreate      DATETIME,
	@dtTerminalCreate    DATETIME
)
AS
BEGIN
   BEGIN TRY

	    DECLARE @state  INT = 1  -- Active state
        DECLARE @cartId BIGINT = dbo.ufn_GetShoppingCartId(@customerSessionId, @state)          

		-- Create the shopping cart If shopping cart not exists.

        IF (@cartId IS NULL)
        BEGIN
        
        	EXEC usp_AddShoppingCart 
        	                   @cartId OUTPUT,
        	                   @state,                   
        				       @customerSessionId,
        				       @dtServerCreate,
        				       @dtTerminalCreate
        END


		-- Create the shopping cart transactions

		INSERT INTO tShoppingCartTransactions
		               (
					    CartId,
						TransactionId,
						CartItemStatus,
						ProductId
					    )
                  VALUES
				     	(
						 @cartId,
						 @transactionId,
						 0,				 -- Added
						 @productId
						)

  END TRY

  BEGIN CATCH
   
    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

  END CATCH

END
GO
