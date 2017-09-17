-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Create cash In transaction and add to shoppint cart
-- Jira ID:		AL-8047
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_CashIn')
BEGIN
	DROP PROCEDURE usp_CashIn
END
GO


CREATE PROCEDURE usp_CashIn
(
	@customerSessionId   BIGINT,
	@amount              MONEY,
	@dTTerminalCreate    DATETIME,
	@dTServerCreate      DATETIME	
)
AS
BEGIN
	
	BEGIN TRY
	    SET NOCOUNT ON

		     DECLARE @transactionId BIGINT
			 DECLARE @productId INT = 7   -- Cash


			 -- Add cash
	         EXEC usp_AddCash
			         @transactionId OUTPUT,
					 @customerSessionId,
					 @amount,
					 2,                  -- Authorized
					 1,                  -- CashIn 
					 @dTTerminalCreate,
					 @dTServerCreate


             -- Add cash to cart
			 EXEC  usp_AddShoppingCartTransaction 
			                    @customerSessionId, 
								@transactionId,
								@productId,
								@dTServerCreate,
								@dTTerminalCreate


    END TRY

BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH
END
GO

