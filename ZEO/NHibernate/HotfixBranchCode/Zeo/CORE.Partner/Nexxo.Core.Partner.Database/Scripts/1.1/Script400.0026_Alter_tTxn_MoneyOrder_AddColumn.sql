-- ============================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/22/2015>
-- Description:	<Added additional column 'AccountNumber' 
--				 and 'RoutingNumber' to tTxn_MoneyOrder
-- Rally ID:	<US2406 - TA6858>
-- ============================================================

IF NOT EXISTS
(
	SELECT 1 FROM
		SYS.COLUMNS 
	WHERE
		Name = N'AccountNumber' 
		AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder')
)
BEGIN
	ALTER TABLE dbo.tTxn_MoneyOrder 
	ADD AccountNumber VARCHAR(20) NULL
END
GO 

IF NOT EXISTS
(
	SELECT 1 FROM
		SYS.COLUMNS 
	WHERE
		Name = N'RoutingNumber' 
		AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder')
)
BEGIN
	ALTER TABLE dbo.tTxn_MoneyOrder 
	ADD RoutingNumber VARCHAR(20) NULL
END
GO 