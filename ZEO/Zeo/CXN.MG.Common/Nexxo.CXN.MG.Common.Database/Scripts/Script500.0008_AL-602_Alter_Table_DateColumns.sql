-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter views date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

-- tMGram_Countries
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_Countries'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_Countries.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_Countries'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_Countries.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tMGram_CountryCurrencies
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_CountryCurrencies'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_CountryCurrencies.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_CountryCurrencies'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_CountryCurrencies.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tMGram_Currencies
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_Currencies'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_Currencies.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_Currencies'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_Currencies.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tMGram_DeliveryOptions
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_DeliveryOptions'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_DeliveryOptions.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_DeliveryOptions'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_DeliveryOptions.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tMGram_StateRegulators
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_StateRegulators'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_StateRegulators.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_StateRegulators'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_StateRegulators.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO

-- tMGram_States
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_States'
			AND COLUMN_NAME = 'DTCreate'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_States.DTCreate'
		,@newname = 'DTServerCreate'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMGram_States'
			AND COLUMN_NAME = 'DTLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tMGram_States.DTLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO
