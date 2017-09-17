-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: park the shopping cart transactions
-- Jira ID:		AL-8047
-- ================================================================================


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_ParkShoppingCartTransaction')
BEGIN
    DROP PROCEDURE usp_ParkShoppingCartTransaction
END
GO

CREATE PROCEDURE usp_ParkShoppingCartTransaction
(
	@customerSessionId       BIGINT,
	@transactionId           BIGINT,
	@productId               INT,
	@serverDate              DATETIME,
	@terminalDate            DATETIME
)
AS
BEGIN
   BEGIN TRY

	    DECLARE @state  INT = 2 -- Parked state
		DECLARE @cartId BIGINT = dbo.ufn_GetShoppingCartId(@customerSessionId, @state)		


        -- Create the park shopping cart if parked shopping not exists.

        IF (@cartId IS NULL)
        BEGIN
        
        	EXEC usp_AddShoppingCart 
        	                   @cartId OUTPUT,
        	                   @state,                   
        				       @customerSessionId,
        				       @serverDate,
        				       @terminalDate
        END
		
		

		-- Update CartId into shopping cart transactions

        UPDATE
		  tShoppingCartTransactions 
		SET
		  CartId = @cartId
		WHERE
		  TransactionId = @transactionId
		  AND
		  ProductId = @productId
     

   END TRY

   BEGIN CATCH
    
    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

   END CATCH

END
GO


