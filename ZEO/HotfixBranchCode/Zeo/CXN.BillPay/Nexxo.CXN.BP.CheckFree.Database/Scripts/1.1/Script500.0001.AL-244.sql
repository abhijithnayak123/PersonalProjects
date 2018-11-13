--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update column names and foreign key relationships>           
-- Jira ID:	<AL-244>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCheckFree_Account'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tCheckFree_Account.rowguid'
		,@newname = 'CheckFreeAccountPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCheckFree_Account'
			AND COLUMN_NAME = 'Id'
		)
BEGIN
	EXEC sp_rename @objname = 'tCheckFree_Account.Id'
		,@newname = 'CheckFreeAccountID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCheckFree_Trx'
			AND COLUMN_NAME = 'Id'
		)
BEGIN
	EXEC sp_rename @objname = 'tCheckFree_Trx.id'
		,@newname = 'CheckFreeTrxPK'
		,@objtype = 'COLUMN';--pk column was named as ID
END
GO