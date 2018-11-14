--- ===============================================================================
-- Author     :	 Abhijith
-- Description:  Adding a new field - "IsPromotionHidden"
-- Created Date:  04-18-2018
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromotions' AND COLUMN_NAME = 'IsPromotionHidden')
BEGIN
	ALTER TABLE tPromotions 
	ADD IsPromotionHidden BIT DEFAULT 0 NOT NULL
END
GO