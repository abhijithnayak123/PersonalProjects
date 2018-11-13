-- Author:		Swarnalakshmi S
-- Create date: Feb 10th 2015
-- Description:	<Altered  DiscountDesc, IsSystemApplied column in tTxn_MoneyOrder>
-- Rally ID:	US1799
-- ============================================================



IF EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'DiscountDescription' AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder'))
BEGIN
	ALTER TABLE tTxn_MoneyOrder 
	ALTER COLUMN DiscountDescription varchar(100) NULL
END
GO

IF  EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'IsSystemApplied' AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder'))
BEGIN
	ALTER TABLE tTxn_MoneyOrder 
	DROP COLUMN IsSystemApplied
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'IsSystemApplied' AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder'))
BEGIN
	ALTER TABLE tTxn_MoneyOrder 
	ADD IsSystemApplied BIT DEFAULT (0)
END
GO

