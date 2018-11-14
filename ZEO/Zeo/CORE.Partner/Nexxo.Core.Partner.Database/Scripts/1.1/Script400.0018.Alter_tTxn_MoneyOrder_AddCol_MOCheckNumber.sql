-- ============================================================
-- Author:		Lokesh
-- Create date: <12/01/2014>
-- Description:	<Added column CheckNumber for tTxn_MoneyOrder>
-- Rally ID:	<US2291>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
  name = N'CheckNumber' 
	AND object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyOrder]')      
)
BEGIN         
	ALTER TABLE tTxn_MoneyOrder 
	ADD CheckNumber VARCHAR(50) NULL
END
GO