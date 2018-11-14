--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-23-2018>
-- Description:	 Updating the existing promotions to hidden.
-- ================================================================================

UPDATE tPromotions
SET IsPromotionHidden = 1
WHERE Name IN ('TCFOCMO','FREECORPORATE','LOANCUST','FREECOMMAND')
GO


