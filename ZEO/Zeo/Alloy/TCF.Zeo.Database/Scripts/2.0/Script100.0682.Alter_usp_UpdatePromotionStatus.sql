--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <01-31-2018>
-- Description:	 SP to update promotion status
-- Version One:		<B-12321>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_UpdatePromotionStatus')
BEGIN
	DROP PROCEDURE usp_UpdatePromotionStatus
END 
GO

CREATE PROCEDURE usp_UpdatePromotionStatus
(
	 @promotionId BIGINT,
	 @isActive BIT,
	 @serverLastModify DATETIME,
	 @terminalLastModify DATETIME,
	 @isValidPromotion BIT OUTPUT
)
AS
BEGIN

	IF @isActive = 1
	BEGIN
		
		SELECT @isValidPromotion = (
			CASE 
				WHEN Name IS NULL OR ProductId IS NULL OR ProviderId IS NULL OR StartDate IS NULL OR EndDate IS NULL THEN 0 
				ELSE 1
			END
		)
		FROM tPromotions WHERE PromotionId = @promotionId

		IF @isValidPromotion = 1
		BEGIN
			IF(dbo.ufn_VerifyQualifiers(@promotionId) = 1)
			BEGIN	
				IF(dbo.ufn_VerifyProvisions(@promotionId) = 0)
				BEGIN
					SET @isValidPromotion = 0
				END
			END
			ELSE
			BEGIN
				SET @isValidPromotion = 0
			END
		END

	END

	--If promotion, Qualifiers and provisions details are correct
	--Then allow user to changes the status else send the invalid promotion details
	IF (@isValidPromotion = 1 OR @isActive = 0)
	
	BEGIN
		UPDATE 
			tPromotions
		SET 
			IsActive = @isActive,
			DTServerLastModified = @serverLastModify,
			DTTerminalLastModified = @terminalLastModify
		WHERE 
			PromotionId = @promotionId


		SET @isValidPromotion = 1
	END

END 