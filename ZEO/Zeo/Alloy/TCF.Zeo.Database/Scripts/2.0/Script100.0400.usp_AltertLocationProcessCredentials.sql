--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <07-03-2017>
-- Description:	Alter tLocationProcessCredentials add Identifier2
-- Jira ID:		<2317: As Zeo, I want to share with Visa the dynamic location of the prepaid card transaction(s)>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLocationProcessorCredentials' AND COLUMN_NAME = 'Identifier2')
BEGIN
	ALTER TABLE tLocationProcessorCredentials 
	ADD Identifier2 nvarchar(100) NULL
END
GO