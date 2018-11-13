--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCISConnectsDb'
			AND COLUMN_NAME = 'CISConnectstPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tCCISConnectsDb.CISConnectstPK'
		,@newname = 'CCISConnectsPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCCISConnectsDb'
			AND COLUMN_NAME = 'CCISConnectsPK'
		)
BEGIN
ALTER TABLE [dbo].tCCISConnectsDb ADD PRIMARY KEY (CCISConnectsPK);
END
GO