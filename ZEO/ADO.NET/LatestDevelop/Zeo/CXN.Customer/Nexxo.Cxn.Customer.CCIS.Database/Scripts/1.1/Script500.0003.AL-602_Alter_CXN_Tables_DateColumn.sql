-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter views date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

-- tCCIS_Account
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCIS_Account'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tCCIS_Account.DTCreate'
		,@newname = 'DTTerminalCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCIS_Account'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tCCIS_Account.DTLastMod'
		,@newname = 'DTTerminalLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tCCISConnectsDb
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCISConnectsDb'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tCCISConnectsDb.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCISConnectsDb'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tCCISConnectsDb.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- 