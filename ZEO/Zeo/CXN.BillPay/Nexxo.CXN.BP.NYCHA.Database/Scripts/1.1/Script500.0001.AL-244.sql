--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update column names and foreign key relationships>           
-- Jira ID:	<AL-244>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHA_BillPay_Trx'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHA_BillPay_Trx.rowguid'
		,@newname = 'NYCHATrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHA_BillPay_Trx'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHA_BillPay_Trx.ID'
		,@newname = 'NYCHATrxID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHA_BillPay_Account'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHA_BillPay_Account.rowguid'
		,@newname = 'NYCHAAccountPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHA_BillPay_Account'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHA_BillPay_Account.ID'
		,@newname = 'NYCHAAccountID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHA_BillPay_Trx'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHA_BillPay_Trx.rowguid'
		,@newname = 'NYCHATrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHA_BillPay_Trx'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHA_BillPay_Trx.ID'
		,@newname = 'NYCHATrxID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHA_BillPay_Trx_Aud'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHA_BillPay_Trx_Aud.rowguid'
		,@newname = 'NYCHATrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHA_BillPay_Trx_Aud'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHA_BillPay_Trx_Aud.ID'
		,@newname = 'NYCHATrxID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHAFiles'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHAFiles.ID'
		,@newname = 'NYCHAFilePK'
		,@objtype = 'COLUMN';----pk column was named as ID and not unique identifier
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHAPayments'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHAPayments.ID'
		,@newname = 'NYCHAPaymentsPK'
		,@objtype = 'COLUMN';----pk column was named as ID
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHATenant'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHATenant.ID'
		,@newname = 'NYCHATenantPK'
		,@objtype = 'COLUMN';----pk column was named as ID
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNYCHATenant'
			AND COLUMN_NAME = 'NychaFileID'
		)
BEGIN
	EXEC sp_rename @objname = 'tNYCHATenant.NychaFileID'
		,@newname = 'NYCHAFilePK'
		,@objtype = 'COLUMN';
END
GO

--non PK Foreign keys
IF NOT EXISTS (
		SELECT 1
		FROM sys.objects
		WHERE upper(type_desc) LIKE 'UNIQUE_CONSTRAINT'
			AND upper(OBJECT_NAME(parent_object_id)) = 'TNYCHATENANT'
			AND upper(OBJECT_NAME(OBJECT_ID)) = 'U_TNYCHATENANT'
		)
BEGIN
	ALTER TABLE [dbo].[tNYCHATenant] ADD CONSTRAINT U_tNYCHATenant UNIQUE ([TenantID]);
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM sys.foreign_keys AS f
		INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
		INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
		WHERE OBJECT_NAME(f.parent_object_id) = 'tNYCHA_BillPay_Account'
			AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'TenantID'
			AND OBJECT_NAME(f.referenced_object_id) = 'tNYCHATenant'
			AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'TenantID'
		)
BEGIN
	ALTER TABLE [dbo].[tNYCHA_BillPay_Account]
		WITH CHECK ADD CONSTRAINT [FK_tNYCHA_BillPay_Account_tNYCHATenant] FOREIGN KEY ([TenantID]) REFERENCES [dbo].[tNYCHATenant]([TenantID]);
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM sys.foreign_keys AS f
		INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
		INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
		WHERE OBJECT_NAME(f.parent_object_id) = 'tNYCHA_BillPay_Trx'
			AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'TenantID'
			AND OBJECT_NAME(f.referenced_object_id) = 'tNYCHATenant'
			AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'TenantID'
		)
BEGIN
	ALTER TABLE [dbo].[tNYCHA_BillPay_Trx]
		WITH CHECK ADD CONSTRAINT [FK_tNYCHA_BillPay_Trx_tNYCHATenant] FOREIGN KEY ([TenantID]) REFERENCES [dbo].[tNYCHATenant]([TenantID]);
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM sys.foreign_keys AS f
		INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
		INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
		WHERE OBJECT_NAME(f.parent_object_id) = 'tNYCHAPayments'
			AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'TenantID'
			AND OBJECT_NAME(f.referenced_object_id) = 'tNYCHATenant'
			AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'TenantID'
		)
BEGIN
	ALTER TABLE [dbo].[tNYCHAPayments]
		WITH CHECK ADD CONSTRAINT [FK_tNYCHAPayments_tNYCHATenant] FOREIGN KEY ([TenantID]) REFERENCES [dbo].[tNYCHATenant]([TenantID])
END
GO