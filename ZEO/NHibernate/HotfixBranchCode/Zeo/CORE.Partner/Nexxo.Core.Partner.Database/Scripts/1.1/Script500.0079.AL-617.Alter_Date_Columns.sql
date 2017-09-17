-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03/08/2015>
-- Description:	<Alter Table to Add DTTerminalCreate, DTTerminalLastModified.>
-- Jira ID:		<AL-617>
-- ================================================================================

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerPreferedProducts
	ADD DTTerminalCreate DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCustomerPreferedProducts
	ADD DTTerminalLastModified DATETIME 
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	ALTER TABLE tShoppingCarts
	ADD DTServerCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts'
			AND COLUMN_NAME = 'DTServerLastModified'
		)
BEGIN
	ALTER TABLE tShoppingCarts
	ADD  DTServerLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerFeeAdjustments'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerFeeAdjustments
	ADD DTTerminalCreate DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerFeeAdjustments'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCustomerFeeAdjustments
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessionCounterIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerSessionCounterIdDetails
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessionCounterIdDetails'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCustomerSessionCounterIdDetails
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessions'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerSessions
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessions'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tCustomerSessions
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationCounterIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tLocationCounterIdDetails
	ADD DTTerminalCreate DATETIME	
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationCounterIdDetails'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tLocationCounterIdDetails
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationProcessorCredentials'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tLocationProcessorCredentials
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationProcessorCredentials'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tLocationProcessorCredentials
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocations'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tLocations
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocations'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tLocations
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMoneyOrderImage'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tMoneyOrderImage
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMoneyOrderImage'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tMoneyOrderImage
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNpsTerminals'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tNpsTerminals
	ADD DTTerminalCreate DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNpsTerminals'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tNpsTerminals
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tPartnerCustomerGroupSettings
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tPartnerCustomerGroupSettings
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tPartnerCustomerGroupSettings_Aud
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings_Aud'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tPartnerCustomerGroupSettings_Aud
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tPartnerCustomers
	ADD DTTerminalCreate DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tPartnerCustomers
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tPartnerCustomers_Aud
	ADD DTTerminalCreate DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers_Aud'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tPartnerCustomers_Aud
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectEmploymentDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tProspectEmploymentDetails
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectEmploymentDetails'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tProspectEmploymentDetails
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGovernmentIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tProspectGovernmentIdDetails
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGovernmentIdDetails'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tProspectGovernmentIdDetails
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGroupSettings'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tProspectGroupSettings
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGroupSettings'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tProspectGroupSettings
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspects'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tProspects
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspects'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tProspects
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts_Aud'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	ALTER TABLE tShoppingCarts_Aud
	ADD DTServerCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts_Aud'
			AND COLUMN_NAME = 'DTServerLastModified'
		)
BEGIN
	ALTER TABLE tShoppingCarts_Aud
	ADD  DTServerLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTerminals'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tTerminals
	ADD DTTerminalCreate DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTerminals'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tTerminals
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_FeeAdjustments'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tTxn_FeeAdjustments
	ADD DTTerminalCreate DATETIME
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_FeeAdjustments'
			AND COLUMN_NAME = 'DTTerminalLastModified'
		)
BEGIN
	ALTER TABLE tTxn_FeeAdjustments
	ADD  DTTerminalLastModified DATETIME 
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_FeeAdjustments'
			AND COLUMN_NAME = 'DTServerLastModified'
		)
BEGIN
	ALTER TABLE tTxn_FeeAdjustments
	ADD  DTServerLastModified DATETIME 
END
GO
GO