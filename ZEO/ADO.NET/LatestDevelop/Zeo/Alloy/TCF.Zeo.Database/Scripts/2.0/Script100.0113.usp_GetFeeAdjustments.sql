--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to fetch fee adjustments
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
		SELECT 
			cpf.FeeAdjustmentId, cpf.TransactionType, cpf.Name, cpf.Description, cpf.SystemApplied, cpf.AdjustmentRate, cpf.AdjustmentAmount, cpf.PromotionType, cpf.ChannelPartnerId, 
			fac.ConditionTypeId, fac.CompareTypeId, fac.ConditionValue
		FROM 
			tChannelPartnerFeeAdjustments cpf
		INNER JOIN 
			tFeeAdjustmentConditions fac
		ON 
			cpf.FeeAdjustmentId = fac.FeeAdjustmentId
		WHERE 
			cpf.TransactionType = @transactionType AND cpf.ChannelPartnerId = @channelPartnerId AND DTStart <= @dTStart AND (DTEnd IS NULL OR DTEnd >= @dTEnd) 

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END