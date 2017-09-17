IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Card'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Card.rowguid'
		,@newname = 'FViewCardPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Card'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Card.id'
		,@newname = 'FViewCardID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Card_AUD'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Card_AUD.rowguid'
		,@newname = 'FViewCardPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Card_AUD'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Card_AUD.id'
		,@newname = 'FViewCardID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Credential'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Credential.rowguid'
		,@newname = 'FViewCredentialPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Credential'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Credential.ID'
		,@newname = 'FViewCredentialID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_IdTypes'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_IdTypes.rowguid'
		,@newname = 'FViewIdTypesPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_IdTypes'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_IdTypes.ID'
		,@newname = 'FViewIdTypesID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Trx'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Trx.rowguid'
		,@newname = 'FViewTrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Trx'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Trx.ID'
		,@newname = 'FViewTrxID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Trx'
			AND COLUMN_NAME = 'AccountPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Trx.AccountPK'
		,@newname = 'FViewCardPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Trx_AUD'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Trx_AUD.rowguid'
		,@newname = 'FViewTrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Trx_AUD'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Trx_AUD.ID'
		,@newname = 'FViewTrxID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFView_Trx_AUD'
			AND COLUMN_NAME = 'AccountPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tFView_Trx_AUD.AccountPK'
		,@newname = 'FViewCardPK'
		,@objtype = 'COLUMN';
END
GO
