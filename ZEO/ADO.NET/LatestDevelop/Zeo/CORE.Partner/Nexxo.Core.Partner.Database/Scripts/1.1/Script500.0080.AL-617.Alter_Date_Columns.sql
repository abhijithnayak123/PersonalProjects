-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03/08/2015>
-- Description:	<Alter Table to Adding values for DTTerminalCreate, DTTerminalLastModified.>
-- Jira ID:		<AL-617>
-- ================================================================================

DISABLE TRIGGER trShoppingCartsAudit ON tShoppingCarts
GO

DISABLE TRIGGER tPartnerCustomers_Audit ON tPartnerCustomers
GO

DISABLE TRIGGER tPartnerCustomerGroupSettings_Insert_Update ON tPartnerCustomerGroupSettings
GO

DISABLE TRIGGER tPartnerCustomerGroupSettings_Delete ON tPartnerCustomerGroupSettings
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCustomerPreferedProducts
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerPreferedProducts
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	UPDATE tShoppingCarts
	SET DTServerCreate = '1753-01-01 12:00:00'
	WHERE DTServerCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	ALTER TABLE tShoppingCarts
	ALTER COLUMN DTServerCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerFeeAdjustments'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCustomerFeeAdjustments
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerFeeAdjustments'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerFeeAdjustments
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessionCounterIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tCustomerSessionCounterIdDetails
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessionCounterIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tCustomerSessionCounterIdDetails
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationCounterIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tLocationCounterIdDetails
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationCounterIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tLocationCounterIdDetails
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationProcessorCredentials'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tLocationProcessorCredentials
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationProcessorCredentials'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tLocationProcessorCredentials
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocations'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tLocations
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocations'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tLocations
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMoneyOrderImage'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tMoneyOrderImage
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMoneyOrderImage'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tMoneyOrderImage
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNpsTerminals'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tNpsTerminals
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNpsTerminals'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tNpsTerminals
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tPartnerCustomerGroupSettings
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tPartnerCustomerGroupSettings
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tPartnerCustomerGroupSettings_Aud
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tPartnerCustomerGroupSettings_Aud
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tPartnerCustomers
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tPartnerCustomers
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tPartnerCustomers_Aud
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers_Aud'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tPartnerCustomers_Aud
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectEmploymentDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tProspectEmploymentDetails
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectEmploymentDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tProspectEmploymentDetails
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGovernmentIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tProspectGovernmentIdDetails
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGovernmentIdDetails'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tProspectGovernmentIdDetails
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGroupSettings'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tProspectGroupSettings
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGroupSettings'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tProspectGroupSettings
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspects'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tProspects
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspects'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tProspects
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts_Aud'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	UPDATE tShoppingCarts_Aud
	SET DTServerCreate = '1753-01-01 12:00:00'
	WHERE DTServerCreate is NULL
END
GO

IF  EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts_Aud'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	ALTER TABLE tShoppingCarts_Aud
	ALTER COLUMN DTServerCreate DATETIME NOT NULL;
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTerminals'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	UPDATE tTerminals
	SET DTTerminalCreate = '1753-01-01 12:00:00'
	WHERE DTTerminalCreate is NULL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTerminals'
			AND COLUMN_NAME = 'DTTerminalCreate'
		)
BEGIN
	ALTER TABLE tTerminals
	ALTER COLUMN DTTerminalCreate DATETIME NOT NULL;
END
GO

