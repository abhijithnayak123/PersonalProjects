--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to fetch customer fee adjustments
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetCutomerFeeAdjustments'
)
BEGIN
	DROP PROCEDURE usp_GetCutomerFeeAdjustments
END
GO

CREATE PROCEDURE usp_GetCutomerFeeAdjustments
	@CustomerID BIGINT,
	@TransactionType INT
AS
BEGIN
	BEGIN TRY
		SELECT 
			cpf.FeeAdjustmentId, cpf.TransactionType, cpf.Name, cpf.Description, cpf.SystemApplied, cpf.AdjustmentRate, cpf.AdjustmentAmount, cpf.PromotionType, cpf.ChannelPartnerId, 
			fac.ConditionTypeId, fac.CompareTypeId, fac.ConditionValue
		FROM 
			tCustomerFeeAdjustments cusf
		INNER JOIN 
			tChannelPartnerFeeAdjustments cpf
		ON
			cusf.FeeAdjustmentId = cpf.FeeAdjustmentId
		INNER JOIN 
			tFeeAdjustmentConditions fac
		ON 
			cpf.FeeAdjustmentId = fac.FeeAdjustmentId
		WHERE 
			cusf.CustomerID = @CustomerID AND IsAvailed = 0 AND cpf.TransactionType = @TransactionType

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END