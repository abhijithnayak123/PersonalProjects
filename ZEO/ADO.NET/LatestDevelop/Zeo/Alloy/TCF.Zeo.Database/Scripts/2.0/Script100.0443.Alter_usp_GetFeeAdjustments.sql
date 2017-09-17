--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <27-03-2016>
-- Description:	 Get Applicable fee adjustments for transaction type(Check/MoneyOrder)
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetFeeAdjustments'
)
BEGIN
	DROP PROCEDURE usp_GetFeeAdjustments
END
GO

CREATE PROCEDURE usp_GetFeeAdjustments
	@transactionType INT,
	@dTStart DATETIME,
	@dTEnd DATETIME,
	@channelPartnerId SMALLINT

AS
BEGIN
	BEGIN TRY

		--- Get FeeAdjustment conditions 
		SELECT FeeAdjustmentId, ConditionTypeId, CompareTypeId, ConditionValue FROM tFeeAdjustmentConditions 

		--- Get Applicable fee adjustments for transaction type(Check/MoneyOrder)
		SELECT 
			cpf.DTStart, cpf.FeeAdjustmentId, cpf.TransactionType, cpf.Name, cpf.Description, cpf.SystemApplied, cpf.AdjustmentRate, cpf.AdjustmentAmount, cpf.PromotionType, cpf.ChannelPartnerId
		FROM 
			tChannelPartnerFeeAdjustments cpf
		
		WHERE 
			cpf.TransactionType = @transactionType AND cpf.ChannelPartnerId = @channelPartnerId AND DTStart <= @dTStart AND (DTEnd IS NULL OR DTEnd >= @dTEnd) 

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END