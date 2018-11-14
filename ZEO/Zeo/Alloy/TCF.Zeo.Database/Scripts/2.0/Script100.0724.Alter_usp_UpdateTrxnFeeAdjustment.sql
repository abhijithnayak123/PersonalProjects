--- ===============================================================================
-- Author:		<Manikandan G>
-- Create date: <09-12-2017>
-- Description:	 Added NOLOCK in select 
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_UpdateTrxnFeeAdjustment'
)
BEGIN
	DROP PROCEDURE usp_UpdateTrxnFeeAdjustment
END
GO

CREATE PROCEDURE usp_UpdateTrxnFeeAdjustment
	@promotionId BIGINT,
	@transactionId BIGINT,
	@isActive BIT,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@transactionType INT,
	@customerId BIGINT,
	@provisionID BIGINT

AS
BEGIN
	BEGIN TRY

		IF EXISTS(SELECT 1 FROM tTxn_FeeAdjustments  WITH (NOLOCK) WHERE TransactionId = @transactionId and ProductId = @transactionType ) 
			BEGIN
				UPDATE tTxn_FeeAdjustments 
				SET
					IsActive = @isActive,
					PromotionId = @promotionId,
				    ProvisionId = @provisionID,
					DTServerLastModified = @DTServerLastModified,
					DTTerminalLastModified = @DTTerminalLastModified
				WHERE 
				TransactionId = @transactionId and ProductId = @transactionType 
			END		
		ELSE IF (@promotionId > 0)
		BEGIN
			EXEC usp_CreateTrxFeeAdjustment @promotionId, @transactionId, @isActive, @dTTerminalLastModified, @dTServerLastModified, @customerId, @transactionType, @provisionID
		END

	END TRY

	BEGIN CATCH	       
	 
        EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END


