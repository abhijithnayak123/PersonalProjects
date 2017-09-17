--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillerDenomination'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillerDenomination.rowguid'
		,@newname = 'MGBIllerDenominationPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillerLimit'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillerLimit.rowguid'
		,@newname = 'MGBillerLimitPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillerLimit'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillerLimit.id'
		,@newname = 'MGBillerLimitID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Account'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Account.rowguid'
		,@newname = 'MGBillPayAccountPK'
		,@objtype = 'COLUMN';
END
GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Account'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Account.id'
		,@newname = 'MGBillPayAccountID'
		,@objtype = 'COLUMN';
END
GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx.rowguid'
		,@newname = 'MGBillPayTrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx.id'
		,@newname = 'MGBillPayTrxID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx_Aud'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx_Aud.rowguid'
		,@newname = 'MGBillPayTrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_BillPay_Trx_Aud'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_BillPay_Trx_Aud.id'
		,@newname = 'MGBillPayTrxID'
		,@objtype = 'COLUMN';
END
GO

