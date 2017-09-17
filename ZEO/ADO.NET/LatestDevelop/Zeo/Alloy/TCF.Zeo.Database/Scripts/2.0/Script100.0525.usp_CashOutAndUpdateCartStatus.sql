-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Create cash Out transactions and Update cart status)
-- Jira ID:		AL-8047
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_CashOutAndUpdateCartStatus')
BEGIN
	DROP PROCEDURE usp_CashOutAndUpdateCartStatus
END
GO

CREATE PROCEDURE usp_CashOutAndUpdateCartStatus
(
	@customerSessionId BIGINT,
	@cartId            BIGINT,
	@cartStatus        INT,
	@amount            MONEY,
	@dTServerCreate    DATETIME,
	@dTTerminalCreate  DATETIME
)
AS
BEGIN
	
	BEGIN TRY

		DECLARE @transactionId BIGINT
        
		-- Create cash out transaction

		EXEC usp_AddCash		
		    @transactionId OUTPUT,
			 @customerSessionId,
			 @amount,
			 4,                  -- Committed
			 2,                  -- CashOut 
			 @dTTerminalCreate,
			 @dTServerCreate


		EXEC  usp_AddShoppingCartTransaction 
			   @customerSessionId, 
				@transactionId,
				7,                    -- product Id
				@dTServerCreate,
				@dTTerminalCreate

        -- Update Shopping cart Status as completed.

		UPDATE 
	        tShoppingCarts
		SET 
		    Status = @cartStatus
		WHERE 
		    CartId = @cartId
	
	END TRY
	BEGIN CATCH

		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END
GO


