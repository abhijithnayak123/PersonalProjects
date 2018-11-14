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
	@feeAdjustmentId BIGINT,
	@transactionId BIGINT,
	@isActive BIT,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@transactionType INT

AS
BEGIN
	BEGIN TRY

		IF EXISTS(SELECT 1 FROM tTxn_FeeAdjustments tf WITH (NOLOCK) INNER JOIN tChannelPartnerFeeAdjustments cpf WITH (NOLOCK) ON TransactionId = @transactionId AND (tf.FeeAdjustmentId = cpf.FeeAdjustmentId or tf.FeeAdjustmentId = 0)) 
			BEGIN
				UPDATE tTxn_FeeAdjustments 
				SET
					@isActive = @isActive,
					FeeAdjustmentId = @feeAdjustmentId,
					DTServerLastModified = @DTServerLastModified,
					DTTerminalLastModified = @DTTerminalLastModified
				FROM 
					tTxn_FeeAdjustments tf WITH (NOLOCK)
				INNER JOIN 
					tChannelPartnerFeeAdjustments cpf WITH (NOLOCK) 
				ON  
				    tf.TransactionId = @transactionId AND ((tf.FeeAdjustmentId = cpf.FeeAdjustmentId AND cpf.TransactionType = @transactionType) OR tf.FeeAdjustmentId = 0)
	
			END		
		ELSE IF (@feeAdjustmentId > 0)
		BEGIN
			EXEC usp_CreateTrxFeeAdjustment @feeAdjustmentId, @transactionId, @isActive, @dTTerminalLastModified, @dTServerLastModified
		END

	END TRY

	BEGIN CATCH	       
	 
        EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END


