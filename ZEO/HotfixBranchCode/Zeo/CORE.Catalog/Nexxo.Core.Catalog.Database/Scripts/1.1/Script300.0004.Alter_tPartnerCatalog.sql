--===========================================================================================
-- Auther:			Rahul
-- Date Created:	9/19/2014
-- Description:		Script for alter tPartnerCatalog
--===========================================================================================
--Alter tPartnerCatalog

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tPartnerCatalog' AND COLUMN_NAME = 'BillerCode')
BEGIN
	ALTER TABLE dbo.tPartnerCatalog
	ADD BillerCode varchar(20) NULL
END
GO