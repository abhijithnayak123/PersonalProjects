-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter table date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================


-- tWUnion_BillPay_Account
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Account'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Account.DTServerCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Account'
			AND COLUMN_NAME = 'DTServerLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Account.DTServerLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Account'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Account.DTCreate'
		,@newname = 'DTTerminalCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Account'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Account.DTLastMod'
		,@newname = 'DTTerminalLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_BillPay_Trx
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Trx'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Trx.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Trx'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Trx.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_ImportBillers
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_ImportBillers'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_ImportBillers.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_ImportBillers'
			AND COLUMN_NAME = 'DTLastModified'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_ImportBillers.DTLastModified'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO