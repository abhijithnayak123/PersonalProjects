-- ================================================================================
-- Author				: Nishad Varghese
-- Modified By			: Manikandan Govindraj
-- Modified By Again	: Nitish Biradar 
-- Create date			: 12/09/2017
-- Description			: If TCF has referral, then update the customer fee adjustments
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
	@customerId             BIGINT,
	@transactionId          BIGINT,
	@state					INT,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified   DATETIME,
	@channelPartnerId       BIGINT
)
AS
BEGIN
	BEGIN TRY

    --Updating the MoneyOrder Status
		UPDATE tTxn_MoneyOrder
		SET
			State = @state,
			Description = CASE 
							WHEN @state = 4    -- Commit
							THEN 'MoneyOrder '+ CheckNumber 
							ELSE NULL
							END,
			DTTerminalLastModified = @dTTerminalLastModified,
			DTServerLastModified = @dTServerLastModified
		WHERE
			TransactionId = @TransactionId

    DECLARE @isReferralSectionEnable BIT = 0
	SELECT @isReferralSectionEnable = IsReferralSectionEnable FROM tChannelPartnerConfig WITH (NOLOCK) WHERE ChannelPartnerID = @channelPartnerId


	IF (@state = 6)
	BEGIN
		UPDATE 
			tTxn_FeeAdjustments
		SET IsActive = 0,
			DTTerminalLastModified = @dTTerminalLastModified,
			DTServerLastModified = @dTServerLastModified
		WHERE
			TransactionId = @TransactionId AND ProductId = 5
	END


	IF (@state = 4 AND @isReferralSectionEnable = 1)
	BEGIN
	    -- Update the customer fee adjustment. Its only for referral promotion
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