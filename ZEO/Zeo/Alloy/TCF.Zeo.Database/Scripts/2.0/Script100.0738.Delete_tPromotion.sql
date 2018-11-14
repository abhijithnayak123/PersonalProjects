--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-13-2018>
-- Description:	 Script for deleting the 'COURTESYFREE' promotion from the database.
-- ================================================================================

IF EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'COURTESYFREE')
BEGIN
	DECLARE @promotionId BIGINT = (SELECT PromotionId FROM tPromotions WHERE Name = 'COURTESYFREE')

	DELETE FROM tPromoQualifiers
	WHERE PromotionId = @promotionId

	DELETE FROM tPromoProvisions
	WHERE PromotionId = @promotionId

	DELETE FROM tPromotions
	WHERE PromotionId = @promotionId
END
GO
