-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Update or cancel cash in transactions
-- Jira ID:		AL-8047
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_UpdateOrCancelCash')
BEGIN
	DROP PROCEDURE usp_UpdateOrCancelCash
END
GO


CREATE PROCEDURE usp_UpdateOrCancelCash
(
	@customerSessionId   BIGINT,
	@cashCollected       MONEY,
	@dTTerminalCreate    DATETIME,
	@dTServerCreate      DATETIME	
)
AS
BEGIN
	
	BEGIN TRY

	    SET NOCOUNT ON

			 DECLARE @productId INT = 1   -- Cash
    
			 -- Cancel the cash and update the amount as '0'

			 EXEC usp_CancelCashIn
				    @customerSessionId,
					@dTTerminalCreate,
					@dTServerCreate
				 

             IF(@cashCollected > 0)
			 BEGIN

				EXEC usp_CashIn
				    @customerSessionId,
					@cashCollected, 
					@dTTerminalCreate,
					@dTServerCreate
				  
			 END

	       

    END TRY

BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH
END
GO

