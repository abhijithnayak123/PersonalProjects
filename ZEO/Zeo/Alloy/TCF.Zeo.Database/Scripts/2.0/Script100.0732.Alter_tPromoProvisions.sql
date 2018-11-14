--- ===============================================================================
-- Author     :	 Nitish Biradar
-- Description:  Contact information displayed in messages
-- Creatd Date:  03-04-2018
-- Story Id   :  B-13192
-- ================================================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromoProvisions' AND COLUMN_NAME = 'locationIds')
BEGIN
	ALTER TABLE tPromoProvisions 
	ALTER COLUMN locationIds NVARCHAR(MAX)
END
GO