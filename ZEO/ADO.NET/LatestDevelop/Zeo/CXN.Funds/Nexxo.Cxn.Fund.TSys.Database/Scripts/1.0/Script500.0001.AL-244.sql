--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-244>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Account'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Account.rowguid'
		,@newname = 'TSysAccountPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Account'
			AND COLUMN_NAME = 'ID'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Account.ID'
		,@newname = 'TSysAccountID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Account_AUD'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Account_AUD.rowguid'
		,@newname = 'TSysAccountPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Account_AUD'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Account_AUD.ID'
		,@newname = 'TSysAccountID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Trx'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Trx.rowguid'
		,@newname = 'TSysTrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Trx'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Trx.ID'
		,@newname = 'TSysTrxID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Trx'
			AND COLUMN_NAME = 'AccountPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Trx.AccountPK'
		,@newname = 'TSysAccountPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Trx_AUD'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Trx_AUD.rowguid'
		,@newname = 'TSysTrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Trx_AUD'
			AND COLUMN_NAME = 'ID'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Trx_AUD.ID'
		,@newname = 'TSysTrxID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTSys_Trx_AUD'
			AND COLUMN_NAME = 'AccountPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tTSys_Trx_AUD.AccountPK'
		,@newname = 'TSysAccountPK'
		,@objtype = 'COLUMN';
END
GO
