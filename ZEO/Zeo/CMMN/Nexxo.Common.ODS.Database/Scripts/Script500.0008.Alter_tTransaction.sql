--- ===============================================================================
-- Author:		<Pushkal>
-- Create date: <14/11/2017>
-- Description:	Increasing the size of the promotion in tTransatcion table
-- ================================================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTransaction' AND COLUMN_NAME = 'PromotionDescription')
BEGIN
	ALTER TABLE tTransaction
	ALTER COLUMN PromotionDescription NVARCHAR(500)
END