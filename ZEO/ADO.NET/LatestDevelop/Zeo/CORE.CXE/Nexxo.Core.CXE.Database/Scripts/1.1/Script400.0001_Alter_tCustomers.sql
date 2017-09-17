-- ============================================================
-- Author:		Abhijith
-- Create date: <11/19/2014>
-- Description:	<Script for Altering tCustomers table>
-- Rally ID:	<US2169>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'ClientID')
BEGIN
	ALTER TABLE dbo.tCustomers
	ADD ClientID VARCHAR(15) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'LegalCode')
BEGIN
	ALTER TABLE dbo.tCustomers
	ADD LegalCode CHAR(1) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'PrimaryCountryCitizenShip')
BEGIN
	ALTER TABLE dbo.tCustomers
	ADD PrimaryCountryCitizenShip VARCHAR(5) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'SecondaryCountryCitizenShip')
BEGIN
	ALTER TABLE dbo.tCustomers
	ADD SecondaryCountryCitizenShip VARCHAR(5) NULL 
END
GO