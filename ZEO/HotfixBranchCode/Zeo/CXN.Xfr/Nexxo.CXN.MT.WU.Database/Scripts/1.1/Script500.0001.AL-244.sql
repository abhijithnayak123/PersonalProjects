--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update column names and foreign key relationships>           
-- Jira ID:	<AL-244>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Account'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Account.rowguid'
		,@newname = 'WUAccountPK'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Account'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Account.ID'
		,@newname = 'WUAccountID'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_AmountTypes'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_AmountTypes.rowguid'
		,@newname = 'WUAmountTypePK'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_AmountTypes'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_AmountTypes.id'
		,@newname = 'WUAmountTypeID'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx.rowguid'
		,@newname = 'WUTrxPK'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx.ID'
		,@newname = 'WUTrxID'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx'
			AND COLUMN_NAME = 'WUnionAccountPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx.WUnionAccountPK'
		,@newname = 'WUAccountPK'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx_AUD'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx_AUD.rowguid'
		,@newname = 'WUTrxPK'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx_AUD'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx_AUD.ID'
		,@newname = 'WUTrxID'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx_AUD'
			AND COLUMN_NAME = 'WUnionAccountPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Trx_AUD.WUnionAccountPK'
		,@newname = 'WUAccountPK'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Cities'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Cities.rowguid'
		,@newname = 'WUCityPK'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Cities'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Cities.ID'
		,@newname = 'WUCityID'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Countries'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Countries.rowguid'
		,@newname = 'WUCountryPK'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Countries'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Countries.ID'
		,@newname = 'WUCountryID'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_States'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_States.rowguid'
		,@newname = 'WUStatePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_States'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_States.ID'
		,@newname = 'WUStateID'
		,@objtype = 'COLUMN';
END
GO

-- send money
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencies.rowguid'
		,@newname = 'WUCurrencyPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencies.ID'
		,@newname = 'WUCurrencyID'
		,@objtype = 'COLUMN';
END
GO

/*non pk but should be FK*/
--IF NOT EXISTS (
--		SELECT 1
--		FROM sys.objects
--		WHERE upper(type_desc) LIKE 'UNIQUE_CONSTRAINT'
--			AND upper(OBJECT_NAME(parent_object_id)) = 'TWUNION_COUNTRIES'
--			AND upper(OBJECT_NAME(OBJECT_ID)) = 'U_TWUNION_CURRENCIES'
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_Countries] ADD CONSTRAINT U_tWUnion_Currencies UNIQUE ([ISOCountryCode]);
--END
--GO

--IF NOT EXISTS (
--		SELECT 1
--		FROM sys.foreign_keys AS f
--		INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
--		INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
--		WHERE OBJECT_NAME(f.parent_object_id) = 'tWUnion_States'
--			AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'ISOCountryCode'
--			AND OBJECT_NAME(f.referenced_object_id) = 'tWUnion_Countries'
--			AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'ISOCountryCode'
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_States]
--		WITH CHECK ADD CONSTRAINT [FK_tWUnion_States_tWUnion_Countries] FOREIGN KEY ([ISOCountryCode]) REFERENCES [dbo].[tWUnion_Countries]([ISOCountryCode])
--END
--GO

-- send money
--IF NOT EXISTS (
--		SELECT 1
--		FROM sys.objects
--		WHERE upper(type_desc) LIKE 'UNIQUE_CONSTRAINT'
--			AND upper(OBJECT_NAME(parent_object_id)) = 'TWUNION_STATES'
--			AND upper(OBJECT_NAME(OBJECT_ID)) = 'U_TWUNION_STATES'
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_States] ADD CONSTRAINT U_tWUnion_States UNIQUE (
--		[StateCode]
--		,[ISOCountryCode]
--		);
--END
--GO

--ALTER TABLE [dbo].tWUnion_Cities  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Cities_tWUnion_States] FOREIGN KEY([StateCode])
--REFERENCES [dbo].[tWUnion_States] ([StateCode])
--GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'CountryCode'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_CountryCurrencies]

	ALTER COLUMN [CountryCode] VARCHAR(20);
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'CurrencyCode'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_CountryCurrencies]

	ALTER COLUMN [CurrencyCode] VARCHAR(20);
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx'
			AND COLUMN_NAME = 'OriginalDestinationCountryCode'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_Trx]

	ALTER COLUMN [OriginalDestinationCountryCode] VARCHAR(20);
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Trx'
			AND COLUMN_NAME = 'OriginalDestinationCurrencyCode'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_Trx]

	ALTER COLUMN [OriginalDestinationCurrencyCode] VARCHAR(20);
END
GO

--ALTER TABLE [dbo].[tWUnion_CountryCurrencies]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_CountryCurrencies_tWUnion_Countries] FOREIGN KEY([CountryCode])
--REFERENCES [dbo].[tWUnion_Countries] ([ISOCountryCode])
--GO
--IF NOT EXISTS (
--		SELECT 1
--		FROM sys.objects
--		WHERE upper(type_desc) LIKE 'UNIQUE_CONSTRAINT'
--			AND upper(OBJECT_NAME(parent_object_id)) = 'TWUNION_COUNTRYCURRENCIES'
--			AND upper(OBJECT_NAME(OBJECT_ID)) = 'U_TWUNION_COUNTRYCURRENCIES'
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_CountryCurrencies] ADD CONSTRAINT U_tWUnion_CountryCurrencies UNIQUE (
--		[CountryCode]
--		,[CurrencyCode]
--		);
--END
--GO

--IF NOT EXISTS (
--		SELECT 1
--		FROM sys.foreign_keys AS f
--		INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
--		INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
--		WHERE OBJECT_NAME(f.parent_object_id) = 'tWUnion_Trx'
--			AND COL_NAME(fc.parent_object_id, fc.parent_column_id) IN (
--				'OriginatingCountryCode'
--				,'OriginatingCurrencyCode'
--				)
--			AND OBJECT_NAME(f.referenced_object_id) = 'tWUnion_CountryCurrencies'
--			AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) IN (
--				'CountryCode'
--				,'CurrencyCode'
--				)
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_Trx]
--		WITH CHECK ADD CONSTRAINT [FK_tWUnion_Trx_tWUnion_Countries_1] FOREIGN KEY (
--				[OriginatingCountryCode]
--				,[OriginatingCurrencyCode]
--				) REFERENCES [dbo].[tWUnion_CountryCurrencies]([CountryCode], [CurrencyCode])
--END
--GO

--IF NOT EXISTS (
--		SELECT 1
--		FROM sys.foreign_keys AS f
--		INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
--		INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
--		WHERE OBJECT_NAME(f.parent_object_id) = 'tWUnion_Trx'
--			AND COL_NAME(fc.parent_object_id, fc.parent_column_id) IN (
--				'OriginalDestinationCountryCode'
--				,'OriginalDestinationCurrencyCode'
--				)
--			AND OBJECT_NAME(f.referenced_object_id) = 'tWUnion_CountryCurrencies'
--			AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) IN (
--				'CountryCode'
--				,'CurrencyCode'
--				)
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_Trx]
--		WITH CHECK ADD CONSTRAINT [FK_tWUnion_Trx_tWUnion_Countries_2] FOREIGN KEY (
--				[OriginalDestinationCountryCode]
--				,[OriginalDestinationCurrencyCode]
--				) REFERENCES [dbo].[tWUnion_CountryCurrencies]([CountryCode], [CurrencyCode])
--END
--GO

/*/non pk but should be FK*/
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencyDeliveryMethods'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencyDeliveryMethods.rowguid'
		,@newname = 'WUDeliveryMethodPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencyDeliveryMethods'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencyDeliveryMethods.ID'
		,@newname = 'WUDeliveryMethodID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Receiver'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Receiver.rowguid'
		,@newname = 'WUReceiverPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Receiver'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Receiver.ID'
		,@newname = 'WUReceiverID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_DeliveryOptions'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_DeliveryOptions.rowguid'
		,@newname = 'WUDeliveryOptionPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_DeliveryOptions'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_DeliveryOptions.ID'
		,@newname = 'WUDeliveryOptionID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Credential'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Credential.rowguid'
		,@newname = 'WUCredentialPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Credential'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Credential.ID'
		,@newname = 'WUCredentialID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_ErrorMessages'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_ErrorMessages.rowguid'
		,@newname = 'WUErrorMessagePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_ErrorMessages'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_ErrorMessages.ID'
		,@newname = 'WUErrorMessageID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'CountryCode'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_CountryCurrencies]

	ALTER COLUMN [CountryCode] VARCHAR(20);
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'CurrencyCode'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_CountryCurrencies]

	ALTER COLUMN [CurrencyCode] VARCHAR(20);
END
GO

--ALTER TABLE [dbo].[tWUnion_CountryCurrencies]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_CountryCurrencies_tWUnion_Countries] FOREIGN KEY([CountryCode])
--REFERENCES [dbo].[tWUnion_Countries] ([ISOCountryCode])
--GO
IF NOT EXISTS (
		SELECT 1
		FROM sys.objects
		WHERE upper(type_desc) LIKE 'UNIQUE_CONSTRAINT'
			AND upper(OBJECT_NAME(parent_object_id)) = 'TWUNION_COUNTRYCURRENCIES'
			AND upper(OBJECT_NAME(OBJECT_ID)) = 'U_TWUNION_COUNTRYCURRENCIES'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_CountryCurrencies] ADD CONSTRAINT U_tWUnion_CountryCurrencies UNIQUE (
		[CountryCode]
		,[CurrencyCode]
		);
END
GO
--IF NOT EXISTS (
--		SELECT 1
--		FROM sys.foreign_keys AS f
--		INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
--		INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
--		WHERE OBJECT_NAME(f.parent_object_id) = 'tWUnion_BillPay_Trx'
--			AND COL_NAME(fc.parent_object_id, fc.parent_column_id) IN (
--				'PaymentDetails_Originating_CountryCode'
--				,'PaymentDetails_Originating_CountryCurrency'
--				)
--			AND OBJECT_NAME(f.referenced_object_id) = 'tWUnion_CountryCurrencies'
--			AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) IN (
--				'CountryCode'
--				,'CurrencyCode'
--				)
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_BillPay_Trx]
--		WITH CHECK ADD CONSTRAINT [FK_tWUnion_BillPay_Trx_tWUnion_Countries_1] FOREIGN KEY (
--				[PaymentDetails_Originating_CountryCode]
--				,[PaymentDetails_Originating_CountryCurrency]
--				) REFERENCES [dbo].[tWUnion_CountryCurrencies]([CountryCode], [CurrencyCode])
--END
--GO

--IF NOT EXISTS (
--		SELECT 1
--		FROM sys.foreign_keys AS f
--		INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
--		INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
--		WHERE OBJECT_NAME(f.parent_object_id) = 'tWUnion_BillPay_Trx'
--			AND COL_NAME(fc.parent_object_id, fc.parent_column_id) IN (
--				'PaymentDetails_Destination_CountryCode'
--				,'PaymentDetails_Destination_CountryCurrency'
--				)
--			AND OBJECT_NAME(f.referenced_object_id) = 'tWUnion_CountryCurrencies'
--			AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) IN (
--				'CountryCode'
--				,'CurrencyCode'
--				)
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_BillPay_Trx]
--		WITH CHECK ADD CONSTRAINT [FK_tWUnion_BillPay_Trx_tWUnion_Countries_2] FOREIGN KEY (
--				[PaymentDetails_Destination_CountryCode]
--				,[PaymentDetails_Destination_CountryCurrency]
--				) REFERENCES [dbo].[tWUnion_CountryCurrencies]([CountryCode], [CurrencyCode])
--END
--GO