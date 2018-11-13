-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <10/08/2015>
-- Description:	<As engineering team member, I want to have consistent format for date time values across all tables.>
-- Jira ID:		<AL-617>
-- ================================================================================

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCertegy_Account'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCertegy_Account
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCertegy_Account'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCertegy_Account
	ADD DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCertegy_CheckImages'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCertegy_CheckImages
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCertegy_CheckImages'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCertegy_CheckImages
	ADD DTTerminalLastModified DATETIME 
END
GO

-- to make the columns not null
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCertegy_Account'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCertegy_Account
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCertegy_Account'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCertegy_Account
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCertegy_CheckImages'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCertegy_CheckImages
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCertegy_CheckImages'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCertegy_CheckImages
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO