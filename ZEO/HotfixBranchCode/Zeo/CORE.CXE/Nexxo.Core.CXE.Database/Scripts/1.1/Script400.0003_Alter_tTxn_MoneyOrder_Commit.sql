-- ============================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/22/2015>
-- Description:	<Added additional column 'AccountNumber''RoutingNumber' 
--				 and MICR Number to tTxn_MoneyOrder_Commit
-- Rally ID:	<US2406 - TA6855>
-- ============================================================

IF NOT EXISTS
(
	SELECT 1 FROM
		SYS.COLUMNS 
	WHERE
		Name = N'AccountNumber' 
		AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder_Commit')
)
BEGIN
	ALTER TABLE dbo.tTxn_MoneyOrder_Commit
	ADD AccountNumber VARCHAR(20) NULL
END
GO 

IF NOT EXISTS
(
	SELECT 1 FROM
		SYS.COLUMNS 
	WHERE
		Name = N'RoutingNumber' 
		AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder_Commit')
)
BEGIN
	ALTER TABLE dbo.tTxn_MoneyOrder_Commit
	ADD RoutingNumber VARCHAR(20) NULL
END
GO 

IF NOT EXISTS
(
	SELECT 1 FROM
		SYS.COLUMNS 
	WHERE
	    Name = N'MICR' 
	    AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder_Commit')
)
BEGIN
	ALTER TABLE dbo.tTxn_MoneyOrder_Commit 
	ADD MICR VARCHAR(50) NULL
END
GO 