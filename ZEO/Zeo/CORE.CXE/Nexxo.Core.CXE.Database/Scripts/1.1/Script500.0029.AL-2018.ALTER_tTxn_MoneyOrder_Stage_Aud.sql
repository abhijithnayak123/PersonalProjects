-- ============================================================
-- Author:		Manikandan Govindraj
-- Create date: <09/24/2015>
-- Description:	<Script for Altering tTxn_MoneyOrder_Stage_Aud table>
-- Jira ID:		<AL-2018>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tTxn_MoneyOrder_Stage_Aud' AND COLUMN_NAME = 'AccountNumber')
BEGIN
	ALTER TABLE dbo.tTxn_MoneyOrder_Stage_Aud
	ADD AccountNumber VARCHAR(20) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tTxn_MoneyOrder_Stage_Aud' AND COLUMN_NAME = 'RoutingNumber')
BEGIN
	ALTER TABLE dbo.tTxn_MoneyOrder_Stage_Aud
	ADD RoutingNumber VARCHAR(20) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tTxn_MoneyOrder_Stage_Aud' AND COLUMN_NAME = 'MICR')
BEGIN
	ALTER TABLE dbo.tTxn_MoneyOrder_Stage_Aud
	ADD MICR VARCHAR(50) NULL 
END
GO