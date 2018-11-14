-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <12-01-2017>
-- Description:	Adding ProductProviderCode in tChannelPartnerPricing table
-- Jira ID:		<B-08674>
-- ================================================================================


-- add new column ProductProviderCode as 'tChannelPartnerPricing' table.

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'ProductProviderCode')
BEGIN
    ALTER TABLE tChannelPartnerPricing
	ADD ProductProviderCode BIGINT 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'ProductProviderCode')
BEGIN
    ALTER TABLE tChannelPartnerPricing_Aud
	ADD ProductProviderCode BIGINT 
END
GO

--Updating the Product Provider Code in all CheckTypes as INGO.
UPDATE tChannelPartnerPricing
SET ProductProviderCode = 200
WHERE ProductProviderCode IS NULL AND ProductId = 1
GO 

-- Add MaximumFee in tPricing table.

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPricing' AND COLUMN_NAME = 'MaximumFee')
BEGIN
    ALTER TABLE tPricing
	ADD MaximumFee DECIMAL(18,2) 
END
GO

