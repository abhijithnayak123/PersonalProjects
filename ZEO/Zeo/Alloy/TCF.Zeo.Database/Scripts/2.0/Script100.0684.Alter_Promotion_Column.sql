--- ===============================================================================
-- Author     :	 Nitish Biradar
-- Description:  Alter the column to allow null value for auto save
-- Creatd Date:  02-26-2018
-- Story Id   :  B-13218 
-- ================================================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromotions' AND COLUMN_NAME = 'Name')
BEGIN
    ALTER TABLE tPromotions 
	ALTER COLUMN Name NVARCHAR(100) NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromotions' AND COLUMN_NAME = 'ProductId')
BEGIN
    ALTER TABLE tPromotions 
	ALTER COLUMN ProductId INT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromotions' AND COLUMN_NAME = 'ProviderId')
BEGIN
    ALTER TABLE tPromotions 
	ALTER COLUMN ProviderId INT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromotions' AND COLUMN_NAME = 'StartDate')
BEGIN
    ALTER TABLE tPromotions 
	ALTER COLUMN StartDate DATE NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromotions' AND COLUMN_NAME = 'EndDate')
BEGIN
    ALTER TABLE tPromotions 
	ALTER COLUMN EndDate DATE NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromotions' AND COLUMN_NAME = 'Priority')
BEGIN
    ALTER TABLE tPromotions 
	ALTER COLUMN Priority INT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromoQualifiers' AND COLUMN_NAME = 'StartDate')
BEGIN
    ALTER TABLE tPromoQualifiers 
	ALTER COLUMN StartDate DATE NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPromoQualifiers' AND COLUMN_NAME = 'ProductId')
BEGIN
    ALTER TABLE tPromoQualifiers 
	ALTER COLUMN ProductId INT NULL 
END
GO

GO