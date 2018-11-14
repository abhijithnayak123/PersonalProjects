-- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-21-2017>
-- Description:	Altering the tCheckTypes in CheckType table. 
-- Jira ID:		<B-08674>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCheckTypes' AND COLUMN_NAME = 'ProductProviderCode')
BEGIN
    ALTER TABLE tCheckTypes
	ADD ProductProviderCode BIGINT
END
GO

--Updating the Product Provider Code in all CheckTypes as INGO.
UPDATE tCheckTypes
SET ProductProviderCode = 200
GO