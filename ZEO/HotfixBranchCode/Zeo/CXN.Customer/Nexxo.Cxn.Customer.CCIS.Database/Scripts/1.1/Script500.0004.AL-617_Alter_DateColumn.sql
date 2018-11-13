﻿-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <10/08/2015>
-- Description:	<As engineering team member, I want to have consistent format for date time values across all tables.>
-- Jira ID:		<AL-617>
-- ================================================================================

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCIS_Account'
			AND COLUMN_NAME = 'DTServerCreate'

		)
BEGIN
	ALTER TABLE tCCIS_Account
	ADD DTServerCreate DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCIS_Account'
			AND COLUMN_NAME = 'DTServerLastModified'
		)
BEGIN
	ALTER TABLE tCCIS_Account
	ADD DTServerLastModified DATETIME 
END
GO

-- To make the columns not null

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCIS_Account'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	UPDATE tCCIS_Account
	SET DTServerCreate = '1753-01-01 12:00:00'
	WHERE DTServerCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCIS_Account'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	ALTER TABLE tCCIS_Account
	ALTER COLUMN DTServerCreate DATETIME NOT NULL;
END
GO