-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter table date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================


-- tWUnion_Account
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Account'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Account.DTServerCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Account'
			AND COLUMN_NAME = 'DTServerLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Account.DTServerLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Account'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Account.DTCreate'
		,@newname = 'DTTerminalCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Account'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Account.DTLastMod'
		,@newname = 'DTTerminalLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_AmountTypes
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_AmountTypes'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_AmountTypes.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_AmountTypes'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_AmountTypes.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_Catalog
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Catalog'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Catalog.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Catalog'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Catalog.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_Cities
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Cities'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Cities.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Cities'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Cities.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_Countries
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Countries'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Countries.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Countries'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Countries.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_CountryCurrencies
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencies.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencies.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_CountryCurrencyDeliveryMethods
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencyDeliveryMethods'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencyDeliveryMethods.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencyDeliveryMethods'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencyDeliveryMethods.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_Credential
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Credential'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Credential.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Credential'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Credential.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_DeliveryOptions
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_DeliveryOptions'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_DeliveryOptions.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_DeliveryOptions'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_DeliveryOptions.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_ErrorMessages
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_ErrorMessages'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_ErrorMessages.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_ErrorMessages'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_ErrorMessages.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_NameTypeMapping
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_NameTypeMapping'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_NameTypeMapping.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_NameTypeMapping'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_NameTypeMapping.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_Receiver
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Receiver'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Receiver.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Receiver'
			AND COLUMN_NAME = 'DTLastModified'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Receiver.DTLastModified'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_Relationships
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Relationships'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Relationships.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Relationships'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Relationships.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_States
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_States'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_States.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_States'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_States.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_Trx
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx.DTServerCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx'
			AND COLUMN_NAME = 'DTServerLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx.DTServerLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx.DTCreate'
		,@newname = 'DTTerminalCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx.DTLastMod'
		,@newname = 'DTTerminalLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tWUnion_Trx_Aud
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx_Aud'
			AND COLUMN_NAME = 'DTServerCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx_Aud.DTServerCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx_Aud'
			AND COLUMN_NAME = 'DTServerLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx_Aud.DTServerLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx_Aud'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx_Aud.DTCreate'
		,@newname = 'DTTerminalCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx_Aud'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx_Aud.DTLastMod'
		,@newname = 'DTTerminalLastModified'
		,@objtype = 'COLUMN'
END
GO
