--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-14-2017>
-- Description:	 Stored procedure to update transaction fee adjustments
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
	@transactionType INT,
	@channelPartnerId INT

AS
BEGIN
	BEGIN TRY

		IF EXISTS(SELECT 1 FROM tTxn_FeeAdjustments WHERE TransactionId = @transactionId AND IsActive = 1) 
			BEGIN
				UPDATE tTxn_FeeAdjustments
				SET
					IsActive = 0,
					DTServerLastModified = @DTServerLastModified,
					DTTerminalLastModified = @DTTerminalLastModified
				FROM 
					tTxn_FeeAdjustments tf 
				INNER JOIN 
					tChannelPartnerFeeAdjustments cpf ON tf.FeeAdjustmentId = cpf.FeeAdjustmentId
				WHERE
					TransactionId = @transactionId AND IsActive = 1 AND cpf.TransactionType = @transactionType AND cpf.ChannelPartnerId = @channelPartnerId
			END
		
		IF @feeAdjustmentId > 0
		BEGIN
			EXEC usp_CreateTrxFeeAdjustment @feeAdjustmentId, @transactionId, @isActive, @dTTerminalLastModified, @dTServerLastModified
		END

	END TRY

	BEGIN CATCH	       
	 
        EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END


