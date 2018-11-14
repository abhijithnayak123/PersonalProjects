--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <03-08-2018>
-- Description:	 Alter the table tPromotions and tPromotions_Aud to add the 'FreeTransactionCount' column
-- Jira ID:		<B-13858>
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromotions' AND [COLUMN_NAME] = 'FreeTransactionCount')
BEGIN
	ALTER TABLE tPromotions
	ADD FreeTransactionCount INT NULL
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromotions' AND [COLUMN_NAME] = 'Stackable')
BEGIN
	ALTER TABLE tPromotions
	ADD Stackable BIT NULL
	
	ALTER TABLE tPromotions_Aud
	ADD Stackable BIT NULL
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromotions_Aud' AND [COLUMN_NAME] = 'FreeTransactionCount')
BEGIN
	ALTER TABLE tPromotions_Aud
	ADD FreeTransactionCount INT NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromoQualifiers' AND [COLUMN_NAME] = 'MaxTransactionCount')
BEGIN
	ALTER TABLE tPromoQualifiers
	DROP COLUMN MaxTransactionCount
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromoQualifiers_Aud' AND [COLUMN_NAME] = 'MaxTransactionCount')
BEGIN
	ALTER TABLE tPromoQualifiers_Aud
	DROP COLUMN MaxTransactionCount 
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromoProvisions' AND [COLUMN_NAME] = 'IsPercentage')
BEGIN
	ALTER TABLE tPromoProvisions
	DROP COLUMN IsPercentage 

	
	ALTER TABLE tPromoProvisions_Aud
	DROP COLUMN IsPercentage 
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tPromoProvisions' AND [COLUMN_NAME] = 'DiscountType')
BEGIN
	ALTER TABLE tPromoProvisions
	ADD DiscountType INT

	
	ALTER TABLE tPromoProvisions_Aud
	ADD DiscountType INT
END
GO



IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'tTxn_FeeAdjustments' AND [COLUMN_NAME] = 'ProductId')
BEGIN
	ALTER TABLE tTxn_FeeAdjustments
	ADD ProductId INT NULL
END
GO


SP_RENAME 'tPromoProvisions.DiscountValue', 'Value', 'COLUMN'
GO

SP_RENAME 'tPromoProvisions_Aud.DiscountValue', 'Value', 'COLUMN'
GO





