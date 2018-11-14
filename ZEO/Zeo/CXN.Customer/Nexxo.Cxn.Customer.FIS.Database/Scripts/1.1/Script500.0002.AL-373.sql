--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFIS_Credential'
			AND COLUMN_NAME = 'FISCredentialPK'
		)
BEGIN
ALTER TABLE [dbo].tFIS_Credential ADD PRIMARY KEY (FISCredentialPK);
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFISConnectsDb'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tFISConnectsDb.rowguid'
		,@newname = 'FISConnectsPK'
		,@objtype = 'COLUMN';
END
GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFISConnectsDb'
			AND COLUMN_NAME = 'FISConnectsPK'
		)
BEGIN
	ALTER TABLE [dbo].tFISConnectsDb ADD PRIMARY KEY (FISConnectsPK);
END
GO
