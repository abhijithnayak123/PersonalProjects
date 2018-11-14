--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Update check details by transaction Id
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateCheckTransactionById', N'P') IS NOT NULL
DROP PROC usp_UpdateCheckTransactionById
GO

CREATE PROCEDURE usp_UpdateCheckTransactionById
	@transactionId                    BIGINT,
	@cxnTransactionId                 BIGINT,	
	@amount                           MONEY,
	@fee                              MONEY,
	@description                      NVARCHAR(250),
	@state                            INT ,
	@confirmationNumber               VARCHAR(50),
	@baseFee                          MONEY,
	@discountApplied                  MONEY,
	@additionalfee                    MONEY,
	@discountName                     VARCHAR(50),
	@discountDescription              NVARCHAR(100),
	@isSystemApplied                  BIT,	
	@checkTypeId                      INT,
	@isPendingCheckApprovedOrDeclined BIT,
	@dTTerminalLastModified           DATETIME,
	@dTServerLastModified             DATETIME
AS
BEGIN	
 
    BEGIN TRY
	  
		DECLARE @pendingCheck INT = 1 -- Pending Check status		

		UPDATE 
			tTxn_Check
		SET
			Amount = @amount,
			Fee = @fee,
			AdditionalFee= @additionalFee,
			Description = @description,
			ConfirmationNumber = @confirmationNumber,
			BaseFee = @baseFee,
			DiscountName = @discountName,
			DiscountDescription = @discountDescription,
			CheckType = @checkTypeId,
			State = @state,
			CXNId = @cxnTransactionId,
			DiscountApplied = @discountApplied,
			IsSystemApplied = @isSystemApplied,
			DTServerLastModified = @dTServerLastModified,
			DTTerminalLastModified = @dTTerminalLastModified

		WHERE 
			TransactionId = @transactionId

	    -- Create message center if the check is pending state

		IF (@state = @pendingCheck)
		BEGIN
		     EXEC usp_CreateMessageCenter @transactionId, @dTTerminalLastModified, @dTServerLastModified
		END

		IF(@isPendingCheckApprovedOrDeclined = 1)
		BEGIN
		     UPDATE 
			   tMessageCenter
			 SET
			   DTTerminalLastModified = @dTTerminalLastModified,
			   DTServerLastModified   = @dTServerLastModified
			 WHERE 
			   TransactionId = @transactionId			   
		END

	END TRY
	BEGIN CATCH

       EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError	
	   	
    END CATCH
	
END
GO

