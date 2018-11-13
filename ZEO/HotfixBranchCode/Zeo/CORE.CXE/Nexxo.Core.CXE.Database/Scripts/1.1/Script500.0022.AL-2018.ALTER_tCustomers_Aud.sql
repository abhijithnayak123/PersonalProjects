-- ============================================================
-- Author:		Manikandan Govindraj
-- Create date: <09/24/2015>
-- Description:	<Script for Altering tCustomers_aud table>
-- Jira ID:		<AL-2018>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'ReceiptLanguage')
BEGIN
	ALTER TABLE dbo.tCustomers_Aud
	ADD ReceiptLanguage VARCHAR(50) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'ProfileStatus')
BEGIN
	ALTER TABLE dbo.tCustomers_Aud
	ADD ProfileStatus SMALLINT NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'CountryOfBirth')
BEGIN
	ALTER TABLE dbo.tCustomers_Aud
	ADD CountryOfBirth VARCHAR(5) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'Notes')
BEGIN
	ALTER TABLE dbo.tCustomers_Aud
	ADD Notes VARCHAR(250) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'ClientID')
BEGIN
	ALTER TABLE dbo.tCustomers_Aud
	ADD ClientID VARCHAR(15) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'LegalCode')
BEGIN
	ALTER TABLE dbo.tCustomers_Aud
	ADD LegalCode CHAR(1) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'PrimaryCountryCitizenShip')
BEGIN
	ALTER TABLE dbo.tCustomers_Aud
	ADD PrimaryCountryCitizenShip VARCHAR(5) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'SecondaryCountryCitizenShip')
BEGIN
	ALTER TABLE dbo.tCustomers_Aud
	ADD SecondaryCountryCitizenShip VARCHAR(5) NULL 
END
GO