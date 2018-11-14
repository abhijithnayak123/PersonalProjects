-- ================================================================================
-- Author:		Nishad Varghese
-- Create date: 01/Sep/2016
-- Description:	Update MO status
-- Jira ID:		AL-7706
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE TYPE = 'P' and NAME = 'usp_UpdateMoneyOrderStatus'
)
BEGIN
	DROP PROCEDURE usp_UpdateMoneyOrderStatus
END
GO


CREATE PROCEDURE usp_UpdateMoneyOrderStatus
(
	@customerId      BIGINT,
	@transactionId          BIGINT,
	@state					INT,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified   DATETIME
)
AS
BEGIN
	BEGIN TRY

    --Updating the MoneyOrder Status
		UPDATE tTxn_MoneyOrder
		SET
			State = @state,
			Description = CASE 
							WHEN @state = 4 --Commit
							THEN 'MoneyOrder '+ CheckNumber 
							ELSE NULL
							END,
			DTTerminalLastModified = @dTTerminalLastModified,
			DTServerLastModified = @dTServerLastModified
		WHERE
			TransactionId = @TransactionId

	IF @state = 4
	BEGIN
	    -- Update the customer fee adjustment.
		 EXEC usp_UpdateCustomerFeeAdjustment 
			  @customerId, 
			  @transactionId, 
			  1, --Is Availed
			  5,--Transaction Type for MoneyOrder
			  @dTServerLastModified, 
			  @dTTerminalLastModified
	END

	
	END TRY

	BEGIN CATCH	        
	
       EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END