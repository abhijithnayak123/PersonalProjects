--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-244>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tGPRCards'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tGPRCards.id'
		,@newname = 'GPRCardsPK'
		,@objtype = 'COLUMN';--pk column was named as ID
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tRelationships'
			AND COLUMN_NAME = 'rowid'
		)
BEGIN
	EXEC sp_rename @objname = 'tRelationships.rowid'
		,@newname = 'RelationshipPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tBillPayProcessorLogin'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tBillPayProcessorLogin.id'
		,@newname = 'BillPayProcessorPK'
		,@objtype = 'COLUMN';--pk column was names as ID
END
GO

