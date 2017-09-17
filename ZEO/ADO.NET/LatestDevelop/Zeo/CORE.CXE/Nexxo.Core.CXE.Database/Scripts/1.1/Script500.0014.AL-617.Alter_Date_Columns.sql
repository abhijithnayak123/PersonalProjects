-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03/08/2015>
-- Description:	<Alter Table to Add DTTerminalCreate, DTTerminalLastModified, 
--					DTServerCreate, DTServerLastModified.>
-- Jira ID:		<AL-617>
-- ================================================================================

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerAccounts'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerAccounts
	ADD DTTerminalCreate DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerAccounts'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCustomerAccounts
	ADD DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerEmploymentDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerEmploymentDetails
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerEmploymentDetails'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCustomerEmploymentDetails
	ADD DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerEmploymentDetails_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerEmploymentDetails_Aud
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerEmploymentDetails_Aud'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCustomerEmploymentDetails_Aud
	ADD DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerGovernmentIdDetails
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCustomerGovernmentIdDetails
	ADD DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerGovernmentIdDetails_Aud
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails_Aud'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCustomerGovernmentIdDetails_Aud
	ADD DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	ALTER TABLE tCustomerPreferedProducts
	ADD DTServerCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = ' DTServerLastModified'
		)
BEGIN
	ALTER TABLE tCustomerPreferedProducts
	ADD  DTServerLastModified DATETIME 
END
GO