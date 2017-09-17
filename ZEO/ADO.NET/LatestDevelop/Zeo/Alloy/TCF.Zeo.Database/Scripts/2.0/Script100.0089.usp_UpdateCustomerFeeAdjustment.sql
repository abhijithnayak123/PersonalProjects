-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 create SP for update customer fee adjustment
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateCustomerFeeAdjustment', N'P') IS NOT NULL
DROP PROC usp_UpdateCustomerFeeAdjustment
GO


CREATE PROCEDURE usp_UpdateCustomerFeeAdjustment
(	
	@transactionId BIGINT,
	@customerId BIGINT,
	@promotionType VARCHAR(50),
	@channelPartnerId INT,
	@dTServerLastModified DATETIME,
	@dTTerminalLastMadified DATETIME 
)
AS
BEGIN
	
	BEGIN TRY

		UPDATE 
		   CF
		SET 
		   CF.IsAvailed = 1,
		   CF.DTTerminalLastModified = @dTTerminalLastMadified,
		   CF.DTServerLastModified = @dTServerLastModified   

		FROM
			tCustomerFeeAdjustments CF WITH (NOLOCK)

   --Alter Script required for Fee Adjustment----------------------------------------
		--INNER JOIN 
		--	tChannelPartnerFeeAdjustments CPF WITH (NOLOCK)
		--ON 
		--	CF.FeeAdjustmentId = CPF.FeeAdjustmentId
		--INNER JOIN 
		--	tTxn_FeeAdjustments as TF WITH (NOLOCK)
		--ON 
		--	TF.FeeAdjustmentId = CPF.FeeAdjustmentId
		--WHERE 
		--   TF.TransactionId = @transactionId 
		--   AND
		--   CF.CustomerID = @customerId
		--   AND
		--   CPF.PromotionType = @promotionType
		--   AND
		--   CPF.ChannelPartnerId = @channelPartnerId
   
END TRY

BEGIN CATCH
   
	EXECUTE usp_CreateErrorInfo
		
END CATCH

END
GO








