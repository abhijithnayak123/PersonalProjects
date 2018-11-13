-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <10/08/2015>
-- Description:	<As engineering team member, I want to have consistent format for date time values across all tables.>
-- Jira ID:		<AL-617>
-- ================================================================================

DISABLE TRIGGER trChxr_AccountAudit ON tChxr_Account
GO
IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_Account'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tChxr_Account
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_Account'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tChxr_Account
	ADD DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_Account_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tChxr_Account_Aud
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_Account_Aud'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tChxr_Account_Aud
	ADD DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_CheckImages'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	ALTER TABLE tChxr_CheckImages
	ADD DTServerCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_CheckImages'
			AND COLUMN_NAME = 'DTServerLastModified'
		)
BEGIN
	ALTER TABLE tChxr_CheckImages
	ADD  DTServerLastModified DATETIME 
END
GO
-- to make the columns not null

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_Account'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tChxr_Account
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_Account'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tChxr_Account
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_Account_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tChxr_Account_Aud
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_Account_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tChxr_Account_Aud
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_CheckImages'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	UPDATE tChxr_CheckImages
	SET DTServerCreate = '1753-01-01 12:00:00'
	WHERE DTServerCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChxr_CheckImages'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	ALTER TABLE tChxr_CheckImages
	ALTER COLUMN DTServerCreate DATETIME NOT NULL;
END
GO
