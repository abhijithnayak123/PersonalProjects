-- ================================================================================
-- Author:		<Rogy Eapen>
-- Create date: <18/01/2016>
-- Description:	<As Engineering, I need to have consistent naming convention for Id
--				and PK columns>
-- Jira ID:		<AL-4485>
-- ================================================================================

-- Changing of column names from rowguid to TCISAccountPK and id to TCISAccountID
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTCIS_Account'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTCIS_Account.rowguid'
		,@newname = 'TCISAccountPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTCIS_Account'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTCIS_Account.id'
		,@newname = 'TCISAccountID'
		,@objtype = 'COLUMN';
END
GO