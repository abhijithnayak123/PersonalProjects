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
	@customerId BIGINT

AS
BEGIN
	BEGIN TRY

		IF EXISTS(SELECT 1 FROM tTxn_FeeAdjustments tf WITH (NOLOCK) INNER JOIN tPromotions cpf WITH (NOLOCK) ON TransactionId = @transactionId AND (tf.PromotionId = cpf.PromotionId or tf.PromotionId = 0)) 
			BEGIN
				UPDATE tTxn_FeeAdjustments 
				SET
					IsActive = @isActive,
					PromotionId = @promotionId,
					DTServerLastModified = @DTServerLastModified,
					DTTerminalLastModified = @DTTerminalLastModified
				FROM 
					tTxn_FeeAdjustments tf WITH (NOLOCK)
				INNER JOIN 
					tPromotions cpf WITH (NOLOCK) 
				ON  
				    tf.TransactionId = @transactionId AND ((tf.PromotionId = cpf.PromotionId AND cpf.PromotionId= @transactionType) OR tf.PromotionId = 0)
	
			END		
		ELSE IF (@promotionId > 0)
		BEGIN
			EXEC usp_CreateTrxFeeAdjustment @promotionId, @transactionId, @isActive, @dTTerminalLastModified, @dTServerLastModified, @customerId
		END

	END TRY

	BEGIN CATCH	       
	 
        EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END


