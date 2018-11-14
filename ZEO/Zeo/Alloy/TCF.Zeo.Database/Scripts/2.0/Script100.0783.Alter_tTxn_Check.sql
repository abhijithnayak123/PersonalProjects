--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <07-12-2018>
-- Description:	 Adding new column - ShoppingCartDescription to display the Checktype in the ShoppingCart.
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME = 'ShoppingCartDescription')
BEGIN
    ALTER TABLE tTxn_Check 
	ADD ShoppingCartDescription NVARCHAR(255) NULL 
END
GO