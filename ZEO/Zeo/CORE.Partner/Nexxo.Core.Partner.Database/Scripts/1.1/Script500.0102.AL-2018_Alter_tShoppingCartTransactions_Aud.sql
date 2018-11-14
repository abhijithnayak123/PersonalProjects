--===========================================================================================
-- Author:		<Rogy Eapen>
-- Created date: <09/24/2015>
-- Description:	<Script to add Column in tShoppingCartTransactions_Aud>           
-- Jira ID:	<AL-2018>
--===========================================================================================

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' 
		AND COLUMN_NAME = 'CartTxnPK'
		)
BEGIN
	ALTER TABLE tShoppingCartTransactions_Aud
	ADD CartTxnPK UNIQUEIDENTIFIER NULL;
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud'
		AND COLUMN_NAME = 'CartItemStatus'
		)
BEGIN
	ALTER TABLE tShoppingCartTransactions_Aud
	ADD  CartItemStatus varchar(50) NULL;
END
GO