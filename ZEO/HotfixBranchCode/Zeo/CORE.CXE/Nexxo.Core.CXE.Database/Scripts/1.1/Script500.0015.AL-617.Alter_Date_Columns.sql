-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03/08/2015>
-- Description:	<Alter Table to Add DTTerminalCreate, DTTerminalLastModified, 
--					DTServerCreate, DTServerLastModified.>
-- Jira ID:		<AL-617>
-- ================================================================================

DISABLE TRIGGER trCustomerGovernmentIdDetailsAudit ON tCustomerGovernmentIdDetails
GO

DISABLE TRIGGER trCustomerEmploymentDetailsAudit on tCustomerEmploymentDetails
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerAccounts'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCustomerAccounts
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerAccounts'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerAccounts
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerEmploymentDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCustomerEmploymentDetails
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerEmploymentDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerEmploymentDetails
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerEmploymentDetails_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCustomerEmploymentDetails_Aud
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerEmploymentDetails_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerEmploymentDetails_Aud
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCustomerGovernmentIdDetails
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerGovernmentIdDetails
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCustomerGovernmentIdDetails_Aud
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerGovernmentIdDetails_Aud
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	UPDATE tCustomerPreferedProducts
	SET DTServerCreate = '1753-01-01 12:00:00'
	WHERE DTServerCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	ALTER TABLE tCustomerPreferedProducts
	ALTER COLUMN DTServerCreate DATETIME NOT NULL;
END
GO