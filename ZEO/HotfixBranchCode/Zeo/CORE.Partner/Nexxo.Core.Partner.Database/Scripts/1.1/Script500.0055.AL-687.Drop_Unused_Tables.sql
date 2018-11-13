--===========================================================================================
-- Author:		<Rogy Eapen>
-- Created date: <July 08 2015>
-- Description:	<Scripts for dropping unused tables, reference key and column>           
-- Jira ID:	<AL-687>
--===========================================================================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'FK_tLedgerEntries_tAccounts'))
BEGIN
	ALTER TABLE tLedgerEntries DROP CONSTRAINT FK_tLedgerEntries_tAccounts
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'FK_tLedgerEntries_tLedgerTransactions'))
BEGIN
	ALTER TABLE tLedgerEntries DROP CONSTRAINT FK_tLedgerEntries_tLedgerTransactions
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'FK_tTxn_MoneyOrder_tLedgerEntries'))
BEGIN
	ALTER TABLE tTxn_MoneyOrder DROP CONSTRAINT FK_tTxn_MoneyOrder_tLedgerEntries
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'LedgerEntryPK')
BEGIN
	ALTER TABLE tTxn_MoneyOrder DROP COLUMN LedgerEntryPK
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'FK_tTxn_MoneyTransfer_tLedgerEntries'))
BEGIN
	ALTER TABLE tTxn_MoneyTransfer DROP CONSTRAINT FK_tTxn_MoneyTransfer_tLedgerEntries
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'LedgerEntryPK')
BEGIN
	ALTER TABLE tTxn_MoneyTransfer DROP COLUMN LedgerEntryPK
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'FK_tTxn_Funds_tLedgerEntries'))
BEGIN
	ALTER TABLE tTxn_Funds DROP CONSTRAINT FK_tTxn_Funds_tLedgerEntries
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Funds' AND COLUMN_NAME = 'LedgerEntryPK')
BEGIN
	ALTER TABLE tTxn_Funds DROP COLUMN LedgerEntryPK
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'FK_tTxn_Check_tLedgerEntries'))
BEGIN
	ALTER TABLE tTxn_Check DROP CONSTRAINT FK_tTxn_Check_tLedgerEntries
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME = 'LedgerEntryPK')
BEGIN
	ALTER TABLE tTxn_Check DROP COLUMN LedgerEntryPK
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'FK_tTxn_Cash_tLedgerEntries'))
BEGIN
	ALTER TABLE tTxn_Cash DROP CONSTRAINT FK_tTxn_Cash_tLedgerEntries
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME = 'LedgerEntryPK')
BEGIN
	ALTER TABLE tTxn_Cash DROP COLUMN LedgerEntryPK
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'FK_tTxn_BillPay_tLedgerEntries'))
BEGIN
	ALTER TABLE tTxn_BillPay DROP CONSTRAINT FK_tTxn_BillPay_tLedgerEntries
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'LedgerEntryPK')
BEGIN
	ALTER TABLE tTxn_BillPay DROP COLUMN LedgerEntryPK
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tLedgerEntries')
BEGIN
   DROP TABLE tLedgerEntries
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tLedgerTransactions')
BEGIN
   DROP TABLE tLedgerTransactions
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tSkins')
BEGIN
   DROP TABLE tSkins
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tAcceptedIdentifications')
BEGIN
   DROP TABLE tAcceptedIdentifications
END
GO