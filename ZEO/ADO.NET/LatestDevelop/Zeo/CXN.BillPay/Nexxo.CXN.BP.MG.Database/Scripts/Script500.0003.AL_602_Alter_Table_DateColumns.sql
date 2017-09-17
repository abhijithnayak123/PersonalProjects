-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter views date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

-- tMGram_BillerDenomination does not have date columns

-- tMGram_BillerLimit does not have date columns

-- tMGram_BillPay_Account
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Account'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Account.DTServerCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Account'
			AND COLUMN_NAME = 'DTServerLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Account.DTServerLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Account'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Account.DTCreate'
		,@newname = 'DTTerminalCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Account'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Account.DTLastMod'
		,@newname = 'DTTerminalLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tMGram_BillPay_Trx
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx.DTServerCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx'
			AND COLUMN_NAME = 'DTServerLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx.DTServerLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx.DTCreate'
		,@newname = 'DTTerminalCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx.DTLastMod'
		,@newname = 'DTTerminalLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tMGram_BillPay_Trx_Aud
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx_Aud'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx_Aud.DTServerCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx_Aud'
			AND COLUMN_NAME = 'DTServerLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx_Aud.DTServerLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx_Aud'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx_Aud.DTCreate'
		,@newname = 'DTTerminalCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx_Aud'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx_Aud.DTLastMod'
		,@newname = 'DTTerminalLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tMGram_Catalog
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_Catalog'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_Catalog.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_Catalog'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_Catalog.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO