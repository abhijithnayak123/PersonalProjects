-- Auther:			Rahul
-- Date Created:	9/18/2014
-- Description:		Script for alter tCustomerPreferedProducts
--===========================================================================================
--Alter tCustomerPreferedProducts
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomerPreferedProducts' AND COLUMN_NAME = 'BillerCode')
BEGIN
	ALTER TABLE dbo.tCustomerPreferedProducts
	ADD BillerCode varchar(20) NULL
END
GO