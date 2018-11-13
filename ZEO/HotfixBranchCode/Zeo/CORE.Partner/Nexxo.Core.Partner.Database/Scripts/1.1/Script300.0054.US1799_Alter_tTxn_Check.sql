-- Author:		<SwarnaLakshmi>
-- Create date: Feb 4th 2015
-- Description:	<Added DiscountName, DiscountDesc, IsSystemApplied column in tTxn_Check>
-- Rally ID:	US1799
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'DiscountName' AND OBJECT_ID = OBJECT_ID(N'tTxn_Check'))
BEGIN
	ALTER TABLE tTxn_Check ADD
	DiscountName varchar(50) NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'DiscountDescription' AND OBJECT_ID = OBJECT_ID(N'tTxn_Check'))
BEGIN
	ALTER TABLE tTxn_Check ADD
	DiscountDescription varchar(100) NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'IsSystemApplied' AND OBJECT_ID = OBJECT_ID(N'tTxn_Check'))
BEGIN
	ALTER TABLE tTxn_Check ADD
	IsSystemApplied bit NOT NULL DEFAULT(0)
END
GO