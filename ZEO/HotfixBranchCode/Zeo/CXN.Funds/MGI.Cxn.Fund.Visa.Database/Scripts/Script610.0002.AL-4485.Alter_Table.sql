-- ================================================================================
-- Author:		<Rogy Eapen>
-- Create date: <18/01/2016>
-- Description:	<As Engineering, I need to have consistent naming convention for Id
--				and PK columns>
-- Jira ID:		<AL-4485>
-- ================================================================================

-- Changing of column names from rowguid to VisaAccountPK and id to VisaAccountID
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_Account'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tVisa_Account.rowguid'
		,@newname = 'VisaAccountPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_Account'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tVisa_Account.id'
		,@newname = 'VisaAccountID'
		,@objtype = 'COLUMN';
END
GO

-- Changing of column names from rowguid to VisaCredentialPK and id to VisaCredentialID
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_Credential'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tVisa_Credential.rowguid'
		,@newname = 'VisaCredentialPK'
		,@objtype = 'COLUMN';
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_Credential'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tVisa_Credential.id'
		,@newname = 'VisaCredentialID'
		,@objtype = 'COLUMN';
END
GO

-- Changing of column names from rowguid to VisaTrxPK and id to VisaTrxID
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_Trx'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tVisa_Trx.rowguid'
		,@newname = 'VisaTrxPK'
		,@objtype = 'COLUMN';
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_Trx'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tVisa_Trx.id'
		,@newname = 'VisaTrxID'
		,@objtype = 'COLUMN';
END
GO