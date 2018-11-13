--===========================================================================================
-- Auther:			Ratheesh
-- Date Created:	8/19/2014
-- Description:		Script for alter mastercatalog
--===========================================================================================
--Alter mastercatalog
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMasterCatalog' AND COLUMN_NAME = 'BillerCode')
BEGIN
	ALTER TABLE dbo.tMasterCatalog
	ADD BillerCode varchar(20) NULL
END
GO