--===========================================================================================
-- Auther:			Bijo James
-- Date Created:	7/10/2014
-- Description:		Script for alter tPartnerCatalog and tMasterCatalog
--===========================================================================================
--Alter tPartnerCatalog

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tPartnerCatalog' AND COLUMN_NAME = 'Keywords')
BEGIN
	ALTER TABLE dbo.tPartnerCatalog
	ADD Keywords varchar(max) NULL
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMasterCatalog' AND COLUMN_NAME = 'Keywords')
BEGIN
	ALTER TABLE dbo.tMasterCatalog
	ADD Keywords varchar(max) NULL
END
GO