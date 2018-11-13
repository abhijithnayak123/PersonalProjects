--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update column names and foreign key relationships>           
-- Jira ID:	<AL-244>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Account'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Account.rowguid'
		,@newname = 'WUBillPayAccountPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Account'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Account.ID'
		,@newname = 'WUBillPayAccountID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Trx'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Trx.rowguid'
		,@newname = 'WUBillPayTrxPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Trx'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Trx.ID'
		,@newname = 'WUBillPayTrxID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Trx'
			AND COLUMN_NAME = 'AccountPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_BillPay_Trx.AccountPK'
		,@newname = 'WUBillPayAccountPK'
		,@objtype = 'COLUMN';
END
GO

--non pk foreign keys
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Trx'
			AND COLUMN_NAME = 'PaymentDetails_Destination_CountryCode'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_BillPay_Trx]

	ALTER COLUMN [PaymentDetails_Destination_CountryCode] VARCHAR(20);
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Trx'
			AND COLUMN_NAME = 'PaymentDetails_Destination_CountryCurrency'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_BillPay_Trx]

	ALTER COLUMN [PaymentDetails_Destination_CountryCurrency] VARCHAR(20);
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Trx'
			AND COLUMN_NAME = 'PaymentDetails_Originating_CountryCode'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_BillPay_Trx]

	ALTER COLUMN [PaymentDetails_Originating_CountryCode] VARCHAR(20);
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_BillPay_Trx'
			AND COLUMN_NAME = 'PaymentDetails_Originating_CountryCurrency'
		)
BEGIN
	ALTER TABLE [dbo].[tWUnion_BillPay_Trx]

	ALTER COLUMN [PaymentDetails_Originating_CountryCurrency] VARCHAR(20);
END
GO
--ALTER TABLE [dbo].tWUnion_Cities  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Cities_tWUnion_States] FOREIGN KEY([StateCode])
--REFERENCES [dbo].[tWUnion_States] ([StateCode])
--GO
--IF EXISTS (
--		SELECT 1
--		FROM INFORMATION_SCHEMA.COLUMNS
--		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
--			AND COLUMN_NAME = 'CountryCode'
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_CountryCurrencies]

--	ALTER COLUMN [CountryCode] VARCHAR(20);
--END
--GO

--IF EXISTS (
--		SELECT 1
--		FROM INFORMATION_SCHEMA.COLUMNS
--		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
--			AND COLUMN_NAME = 'CurrencyCode'
--		)
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_CountryCurrencies]

--	ALTER COLUMN [CurrencyCode] VARCHAR(20);
--END
--GO

----ALTER TABLE [dbo].[tWUnion_CountryCurrencies]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_CountryCurrencies_tWUnion_Countries] FOREIGN KEY([CountryCode])
----REFERENCES [dbo].[tWUnion_Countries] ([ISOCountryCode])
----GO
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

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_ImportBillers'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_ImportBillers.rowguid'
		,@newname = 'WUBillersPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_ImportBillers'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_ImportBillers.ID'
		,@newname = 'WUBillersID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_ImportBillers'
			AND COLUMN_NAME = 'WUAccount'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_ImportBillers.WUAccount'
		,@newname = 'WUBillPayAccountPK'
		,@objtype = 'COLUMN';
END
GO