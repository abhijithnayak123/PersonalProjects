--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to validate promotion code
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_ValidatePromoCode'
)
BEGIN
	DROP PROCEDURE usp_ValidatePromoCode
END
GO

CREATE PROCEDURE usp_ValidatePromoCode
	@transactionType INT,
	@dTStart DATETIME,
	@dTEnd DATETIME,
	@channelPartnerId SMALLINT,
	@promoCode NVARCHAR(1000)
AS
BEGIN
	BEGIN TRY
		--SELECT CASE WHEN (
		--SELECT 
		--	COUNT(1) 
		--FROM 
		--	tChannelPartnerFeeAdjustments cpf
		--INNER JOIN 
		--	tFeeAdjustmentConditions fac
		--ON 
		--	cpf.FeeAdjustmentId = fac.FeeAdjustmentId
		--WHERE 
		--	cpf.TransactionType = @transactionType AND fac.ConditionValue = @promoCode AND cpf.ChannelPartnerId = @channelPartnerId AND DTStart <= @dTStart AND (DTEnd IS NULL OR DTEnd >= @dTEnd) 
		--	) = 0 THEN @false ELSE @true END AS IsValid
		
		SELECT CASE WHEN 
			COUNT(1) > 0
			THEN CAST(1 AS BIT)
			ELSE CAST(0 AS BIT) 
			END AS IsValid
		FROM 
			tChannelPartnerFeeAdjustments cpf
		INNER JOIN 
			tFeeAdjustmentConditions fac
		ON 
			cpf.FeeAdjustmentId = fac.FeeAdjustmentId
		WHERE 
			cpf.TransactionType = @transactionType AND fac.ConditionValue = @promoCode AND cpf.ChannelPartnerId = @channelPartnerId AND DTStart <= @dTStart AND (DTEnd IS NULL OR DTEnd >= @dTEnd) 



	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END