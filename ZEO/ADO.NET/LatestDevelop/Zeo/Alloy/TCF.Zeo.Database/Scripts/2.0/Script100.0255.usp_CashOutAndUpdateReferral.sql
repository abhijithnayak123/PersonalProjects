-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Create cash Out transactions and update/add customer fee adjustments (Referral Referee Promotions)
-- Jira ID:		AL-8047
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_CashOutAndUpdateReferral')
BEGIN
	DROP PROCEDURE usp_CashOutAndUpdateReferral
END
GO

CREATE PROCEDURE usp_CashOutAndUpdateReferral
(
	@customerSessionId BIGINT,
	@cartId            BIGINT,
	@cartStatus        INT,
	@cashToCustomer    MONEY,
	@dTServerCreate    DATETIME,
	@dTTerminalCreate  DATETIME,
	@isReferral        BIT
)
AS
BEGIN
	
	BEGIN TRY
		
	    DECLARE @transactionId BIGINT

	    -- Update the cash in transaction state to Completed

	    UPDATE c
		SET
		    c.State = 4		-- Committed 
		FROM
		    tTxn_Cash c			
		INNER JOIN 
		    tShoppingCartTransactions sct
		    ON c.TransactionId = sct.TransactionId
		WHERE 
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 7
			AND
			c.State = 2       -- Authorized
 

        
		-- Create cash out transaction

		IF( @cashToCustomer > 0 )
		BEGIN

				EXEC usp_AddCash
				     @transactionId OUTPUT,                 
					 @customerSessionId,
					 @cashToCustomer,
					 4,                  -- Committed
					 2,                  -- CashOut 
					 @dTTerminalCreate,
					 @dTServerCreate


                -- Add cash to cart
			    EXEC  usp_AddShoppingCartTransaction 
			                    @customerSessionId, 
								@transactionId,
								7,                    -- product Id
								@dTServerCreate,
								@dTTerminalCreate

		END



        -- Update Shopping cart Status as completed.

		UPDATE 
	        tShoppingCarts
		SET 
		    Status = @cartStatus,
			State  = 3              --- Closed 
		WHERE 
		    CartId = @cartId


		IF @isReferral = 1
		BEGIN
			EXEC usp_CreateCustomerFeeAdjustment 0, @dTTerminalCreate, @dTServerCreate,@customerSessionId
		END
	
	END TRY
	BEGIN CATCH

		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END
GO


