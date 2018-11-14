--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <03-02-2018>
-- Description:	 Alter the table tPromotions and tPromotions_Aud to add the status column
-- Jira ID:		<B-13218>
-- ================================================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromotions' AND [COLUMN_NAME] = 'IsActive')
BEGIN
	ALTER TABLE tPromotions
	DROP COLUMN IsActive
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromotions' AND [COLUMN_NAME] = 'Status')
BEGIN
	ALTER TABLE tPromotions
	ADD Status INT NULL
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromotions_Aud' AND [COLUMN_NAME] = 'IsActive')
BEGIN
	ALTER TABLE tPromotions_Aud
	DROP COLUMN IsActive
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromotions_Aud' AND [COLUMN_NAME] = 'Status')
BEGIN
	ALTER TABLE tPromotions_Aud
	ADD Status INT NULL
END