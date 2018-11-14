-- Author:		Swarnalakshmi S
-- Create date: Feb 4th 2015
-- Description:	<Added DiscountName, DiscountDesc, IsSystemApplied column in tTxn_MoneyOrder>
-- Rally ID:	US1799
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'DiscountName' AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder'))
BEGIN
	ALTER TABLE tTxn_MoneyOrder ADD
	DiscountName varchar(50) NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'DiscountDescription' AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder'))
BEGIN
	ALTER TABLE tTxn_MoneyOrder ADD
	DiscountDescription money NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'IsSystemApplied' AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder'))
BEGIN
	ALTER TABLE tTxn_MoneyOrder ADD
	IsSystemApplied money NULL
END
GO

