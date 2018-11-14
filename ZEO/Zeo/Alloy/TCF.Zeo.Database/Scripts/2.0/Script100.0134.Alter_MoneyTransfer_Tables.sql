--- ===============================================================================
-- Author:		<Abhijith Nayak>
-- Create date: <10-21-2016>
-- Description:	 Alter PK and FK constraints for MoneyTransfer related tables
-- Jira ID:		<AL-8324>
-- ================================================================================

------------- DROP FK constraints in Money Transfer transaction related tables------------------------
------------- Starts Here ----------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tWUnion_Trx_tWUnion_Account') AND parent_object_id = OBJECT_ID(N'tWUnion_Trx'))
BEGIN
	ALTER TABLE tWUnion_Trx 
	DROP CONSTRAINT FK_tWUnion_Trx_tWUnion_Account
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tWUnion_PickupDetails_tWUnion_Recipient_Account') AND parent_object_id = OBJECT_ID(N'tWUnion_PickupDetails'))
BEGIN
	ALTER TABLE tWUnion_PickupDetails 
	DROP CONSTRAINT FK_tWUnion_PickupDetails_tWUnion_Recipient_Account
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tTxn_MoneyTransfer_tAccounts') AND parent_object_id = OBJECT_ID(N'tTxn_MoneyTransfer'))
BEGIN
	ALTER TABLE tTxn_MoneyTransfer 
	DROP CONSTRAINT FK_tTxn_MoneyTransfer_tAccounts
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tWunion_CountryTranslation_tWUnion_Countries') AND parent_object_id = OBJECT_ID(N'tWunion_CountryTranslation'))
BEGIN
	ALTER TABLE tWunion_CountryTranslation 
	DROP CONSTRAINT FK_tWunion_CountryTranslation_tWUnion_Countries
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tWUnion_Account_tCustomerSessions') AND parent_object_id = OBJECT_ID(N'tWUnion_Account'))
BEGIN
	ALTER TABLE tWUnion_Account 
	DROP CONSTRAINT FK_tWUnion_Account_tCustomerSessions
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tWUnion_Account_tCustomers') AND parent_object_id = OBJECT_ID(N'tWUnion_Account'))
BEGIN
	ALTER TABLE tWUnion_Account 
	DROP CONSTRAINT FK_tWUnion_Account_tCustomers
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tTxn_MoneyTransfer_tCustomerSessions') AND parent_object_id = OBJECT_ID(N'tTxn_MoneyTransfer'))
BEGIN
	ALTER TABLE tTxn_MoneyTransfer 
	DROP CONSTRAINT FK_tTxn_MoneyTransfer_tCustomerSessions
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tWUnion_Trx_tTxn_MoneyTransfer') AND parent_object_id = OBJECT_ID(N'tWUnion_Trx'))
BEGIN
	ALTER TABLE tWUnion_Trx 
	DROP CONSTRAINT FK_tWUnion_Trx_tTxn_MoneyTransfer
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tWUnion_Trx_tWUnion_Account') AND parent_object_id = OBJECT_ID(N'tWUnion_Trx'))
BEGIN
	ALTER TABLE tWUnion_Trx 
	DROP CONSTRAINT FK_tWUnion_Trx_tWUnion_Account
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tWUnion_Trx_tWUnion_Receiver') AND parent_object_id = OBJECT_ID(N'tWUnion_Trx'))
BEGIN
	ALTER TABLE tWUnion_Trx 
	DROP CONSTRAINT FK_tWUnion_Trx_tWUnion_Receiver
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWUnion_Countries')
BEGIN
	DROP INDEX IX_tWUnion_Countries ON tWUnion_Countries
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWUnion_States')
BEGIN
	DROP INDEX IX_tWUnion_States ON tWUnion_States
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWUnion_CountryCurrencies')
BEGIN
	DROP INDEX IX_tWUnion_CountryCurrencies ON tWUnion_CountryCurrencies
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWunion_CountryTranslation')
BEGIN
	DROP INDEX IX_tWunion_CountryTranslation ON tWUnion_CountryTranslation
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWUnion_CountryCurrencyDeliveryMethods')
BEGIN
	DROP INDEX IX_tWUnion_CountryCurrencyDeliveryMethods ON tWUnion_CountryCurrencyDeliveryMethods
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWUnion_NameTypeMapping')
BEGIN
	DROP INDEX IX_tWUnion_NameTypeMapping ON tWUnion_NameTypeMapping
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tWUnion_PickupDetails_tWUnion_Account') AND parent_object_id = OBJECT_ID(N'tWUnion_PickupDetails'))
BEGIN
	ALTER TABLE tWUnion_PickupDetails 
	DROP CONSTRAINT FK_tWUnion_PickupDetails_tWUnion_Account
END
GO

--====================================== Ends Here ============================================================================================

--============ DROP PK constraints in Money Transfer transaction related tables=======================
--================ Starts Here =========================================================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Trx' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Trx')
BEGIN
	ALTER TABLE tWUnion_Trx 
	DROP CONSTRAINT PK_tWUnion_Trx
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_States' AND OBJECT_NAME(OBJECT_ID) = 'PK_TWUnion_States')
BEGIN
	ALTER TABLE tWUnion_States 
	DROP CONSTRAINT PK_TWUnion_States
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Relationships' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Relationships')
BEGIN
	ALTER TABLE tWUnion_Relationships 
	DROP CONSTRAINT PK_tWUnion_Relationships
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Recipient_Account' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_RecipientProfiles')
BEGIN
	ALTER TABLE tWUnion_Recipient_Account 
	DROP CONSTRAINT PK_tWUnion_RecipientProfiles
END
GO

DECLARE @PKName VARCHAR(500)
SELECT @PKName = Col.Constraint_Name from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY'
    AND Col.Table_Name = 'tWUnion_Receiver'

IF (@PKName != '')
BEGIN
   EXEC('ALTER TABLE tWUnion_Receiver DROP CONSTRAINT '+ @PKName)     
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_PickupMethods' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_PickupMethods')
BEGIN
	ALTER TABLE tWUnion_PickupMethods 
	DROP CONSTRAINT PK_tWUnion_PickupMethods
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_PickupDetails' AND OBJECT_NAME(OBJECT_ID) = 'PK_t_WUnion_PickupDetails')
BEGIN
	ALTER TABLE tWUnion_PickupDetails 
	DROP CONSTRAINT PK_t_WUnion_PickupDetails
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_PaymentMethods' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_PaymentMethods')
BEGIN
	ALTER TABLE tWUnion_PaymentMethods 
	DROP CONSTRAINT PK_tWUnion_PaymentMethods
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_NameTypeMapping' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_NameTypeMapping')
BEGIN
	ALTER TABLE tWUnion_NameTypeMapping 
	DROP CONSTRAINT PK_tWUnion_NameTypeMapping
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_ErrorMessages' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_ErrorMessages')
BEGIN
	ALTER TABLE tWUnion_ErrorMessages 
	DROP CONSTRAINT PK_tWUnion_ErrorMessages
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_ErrorMessages' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_ErrorMessages')
BEGIN
	ALTER TABLE tWUnion_ErrorMessages 
	DROP CONSTRAINT PK_tWUnion_ErrorMessages
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_DeliveryOptions' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_DeliveryOptions')
BEGIN
	ALTER TABLE tWUnion_DeliveryOptions 
	DROP CONSTRAINT PK_tWUnion_DeliveryOptions
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Credential' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWU_Credential')
BEGIN
	ALTER TABLE tWUnion_Credential 
	DROP CONSTRAINT PK_tWU_Credential
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_CountryCurrencyDeliveryMethods' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_CountryCurrencyDeliveryMethods')
BEGIN
	ALTER TABLE tWUnion_CountryCurrencyDeliveryMethods 
	DROP CONSTRAINT PK_tWUnion_CountryCurrencyDeliveryMethods
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_CountryCurrencies' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_CountryCurrencies')
BEGIN
	ALTER TABLE tWUnion_CountryCurrencies 
	DROP CONSTRAINT PK_tWUnion_CountryCurrencies
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Countries' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWunion_Countries')
BEGIN
	ALTER TABLE tWUnion_Countries 
	DROP CONSTRAINT PK_tWunion_Countries
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Cities' AND OBJECT_NAME(OBJECT_ID) = 'PK_t_WUnion_Cities')
BEGIN
	ALTER TABLE tWUnion_Cities 
	DROP CONSTRAINT PK_t_WUnion_Cities
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Catalog' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Catalog')
BEGIN
	ALTER TABLE tWUnion_Catalog 
	DROP CONSTRAINT PK_tWUnion_Catalog
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_AmountTypes' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_AmountTypes')
BEGIN
	ALTER TABLE tWUnion_AmountTypes 
	DROP CONSTRAINT PK_tWUnion_AmountTypes
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Account' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Account')
BEGIN
	ALTER TABLE tWUnion_Account 
	DROP CONSTRAINT PK_tWUnion_Account
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_MoneyTransfer' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_MoneyTransfer')
BEGIN
	ALTER TABLE tTxn_MoneyTransfer 
	DROP CONSTRAINT PK_tTxn_MoneyTransfer
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTipsAndOffers' AND COLUMN_NAME = 'TipAndOfferPK' )
BEGIN
	 EXEC SP_RENAME 'tTipsAndOffers.TipAndOfferPK','TipAndOfferId','COLUMN'
END
GO

--============================== Ends Here =========================================================================

--============ Add the columns in Money Transfer transaction related tables=======================
--================ Starts Here =========================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME IN ('CustomerId', 'CustomerSessionId', 'CustomerRevisionNo'))
BEGIN
	ALTER TABLE tWUnion_Account 
	ADD CustomerId BIGINT NULL

	ALTER TABLE tWUnion_Account 
	ADD CustomerRevisionNo BIGINT NULL 
		
	ALTER TABLE tWUnion_Account 
	ADD CustomerSessionId BIGINT NULL 
END
GO

--Commented as this will be executed in the future to remove the old columns.
------------------Starts Here -----------------------------------------------
--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'FirstName'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN FirstName
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'LastName'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN LastName
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'Address'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN Address
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'City'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN City
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'State'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN State
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'PostalCode'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN PostalCode
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'Email'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN Email
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'ContactPhone'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN ContactPhone
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'MobilePhone'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN MobilePhone
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'MiddleName'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN MiddleName
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account'
--		AND COLUMN_NAME = 'SecondLastName'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN SecondLastName
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Account' 
--		AND COLUMN_NAME = 'WUAccountPK'
--)
--BEGIN
--	ALTER TABLE tWUnion_Account 
--	DROP COLUMN WUAccountPK
--END
--GO
------------------Ends Here -----------------------------------------------

IF NOT EXISTS(
	SELECT 1
	FROM SYS.TABLES 
	WHERE NAME = 'tWUnion_Account_Aud'
)
BEGIN
	CREATE TABLE [tWUnion_Account_Aud]
	(
		[WUAccountID] [BigInt], 
		[NameType] [varchar](200) NULL,
		[CustomerId] [bigint] NULL,
		[CustomerSessionId] [bigint] NULL,
		[CustomerRevisionNo] [bigint] NULL,
		[PreferredCustomerAccountNumber] [varchar](250) NULL,
		[PreferredCustomerLevelCode] [varchar](250) NULL,
		[SmsNotificationFlag] [varchar](250) NULL,
		[DTAudit] [datetime] NOT NULL,
		[AuditEvent] [smallint] NOT NULL,
		[RevisionNo] [bigint] NOT NULL,
		[DTTerminalCreate] [datetime] NOT NULL,
		[DTTerminalLastModified] [datetime] NULL,
		[DTServerCreate] [datetime] NOT NULL,
		[DTServerLastModified] [datetime] NULL
   )
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME IN ('CustomerSessionId', 'CustomerRevisionNo', 'ProviderId', 'ProviderAccountId'))
BEGIN
	ALTER TABLE tTxn_MoneyTransfer 
	ADD CustomerSessionId BIGINT NULL	

	ALTER TABLE tTxn_MoneyTransfer 
	ADD CustomerRevisionNo BIGINT NULL	

	ALTER TABLE tTxn_MoneyTransfer 
	ADD ProviderId INT NULL	

	ALTER TABLE tTxn_MoneyTransfer 
	ADD ProviderAccountId BIGINT NULL	

END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME IN ('CustomerSessionPK', 'AccountPK', 'CXEId'))
BEGIN

	ALTER TABLE tTxn_MoneyTransfer 
	ALTER COLUMN CustomerSessionPK UNIQUEIDENTIFIER NULL

	ALTER TABLE tTxn_MoneyTransfer 
	ALTER COLUMN AccountPK UNIQUEIDENTIFIER NULL	
	
	ALTER TABLE tTxn_MoneyTransfer 
	ALTER COLUMN CXEId BIGINT NULL			

END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'Destination')
BEGIN
	ALTER TABLE tTxn_MoneyTransfer 
	ADD [Destination] NVARCHAR(200) NULL
END
GO


--Making the WUReceiverPK column of tWUnion_Receiver nullable
IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWUnion_Receiver' 
		AND COLUMN_NAME = 'WUReceiverPK'
)
BEGIN
	ALTER TABLE tWUnion_Receiver 
	ALTER COLUMN WUReceiverPK UNIQUEIDENTIFIER NULL
END
GO

--Making the WUAccountPK column of tWUnion_Account nullable
IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWUnion_Account' 
		AND COLUMN_NAME = 'WUAccountPK'
)
BEGIN
	ALTER TABLE tWUnion_Account 
	ALTER COLUMN WUAccountPK UNIQUEIDENTIFIER NULL
END
GO


--IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'State')
--BEGIN
--	ALTER TABLE tTxn_MoneyTransfer 
--	ADD [State] INT NULL
--END
--GO

--Commented as this will be executed in the future to remove the old columns.
------------------Starts Here -----------------------------------------------

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tTxn_MoneyTransfer' 
--		AND COLUMN_NAME = 'CXEId'
--)
--BEGIN
--	ALTER TABLE tTxn_MoneyTransfer 
--	DROP COLUMN CXEId
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tTxn_MoneyTransfer' 
--		AND COLUMN_NAME = 'CXNId'
--)
--BEGIN
--	ALTER TABLE tTxn_MoneyTransfer 
--	DROP COLUMN CXNId
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tTxn_MoneyTransfer' 
--		AND COLUMN_NAME = 'CustomerSessionPK'
--)
--BEGIN
--	ALTER TABLE tTxn_MoneyTransfer 
--	DROP COLUMN CustomerSessionPK
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tTxn_MoneyTransfer' 
--		AND COLUMN_NAME = 'AccountPK'
--)
--BEGIN
--	ALTER TABLE tTxn_MoneyTransfer 
--	DROP COLUMN AccountPK
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tTxn_MoneyTransfer' 
--		AND COLUMN_NAME = 'CXEState'
--)
--BEGIN
--	ALTER TABLE tTxn_MoneyTransfer 
--	DROP COLUMN CXEState
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tTxn_MoneyTransfer' 
--		AND COLUMN_NAME = 'CXNState'
--)
--BEGIN
--	ALTER TABLE tTxn_MoneyTransfer 
--	DROP COLUMN CXNState
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Receiver' 
--		AND COLUMN_NAME = 'WUReceiverPK'
--)
--BEGIN
--	ALTER TABLE tWUnion_Receiver 
--	DROP COLUMN WUReceiverPK
--END
--GO
------------------Ends Here -----------------------------------------------


IF NOT EXISTS(
	SELECT 1
	FROM SYS.TABLES 
	WHERE NAME = 'tTxn_MoneyTransfer_Aud'
)
BEGIN
	CREATE TABLE [tTxn_MoneyTransfer_Aud]
	(
		[TransactionID] [bigint] NULL,
		[CustomerSessionId] [bigint] NULL,
		[CustomerRevisionNo] [bigint] NULL,
		[ProviderId] [int] NULL,
		[ProviderAccountId] [bigint] NULL,
		[Amount] [money] NULL,
		[Fee] [money] NULL,
		[Description] [nvarchar](255) NULL,
		[State] [int] NULL,
		[DTTerminalCreate] [datetime] NOT NULL,
		[DTTerminalLastModified] [datetime] NULL,
		[ConfirmationNumber] [varchar](50) NULL,
		[RecipientId] [bigint] NULL,
		[ExchangeRate] [money] NULL,
		[DTServerCreate] [datetime] NULL,
		[DTServerLastModified] [datetime] NULL,
		[TransferType] [int] NULL,
		[Destination] [nvarchar](200) NULL,
		[TransactionSubType] [varchar](20) NULL,
		[OriginalTransactionID] [bigint] NULL,
		[DTAudit] [datetime] NOT NULL,
		[AuditEvent] [smallint] NOT NULL,
		[RevisionNo] [bigint] NOT NULL,
   )
END
GO


--IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'TransactionID')
--BEGIN

--	ALTER TABLE tWUnion_Trx 
--	ADD [TransactionID] BIGINT NULL
--END
--GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'WUAccountID')
BEGIN

	ALTER TABLE tWUnion_Trx 
	ADD [WUAccountID] BIGINT NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'WUReceiverID')
BEGIN

	ALTER TABLE tWUnion_Trx 
	ADD [WUReceiverID] BIGINT NULL
END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'WUReceiverID')
BEGIN

	ALTER TABLE tWUnion_Trx_Aud 
	ADD [WUReceiverID] BIGINT NULL
END
GO

--IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'TransactionID')
--BEGIN

--	ALTER TABLE tWUnion_Trx_Aud 
--	ADD [TransactionID] BIGINT NULL
--END
--GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'WUAccountID')
BEGIN

	ALTER TABLE tWUnion_Trx_Aud 
	ADD [WUAccountID] BIGINT NULL
END
GO

-- Making the columns nullable in audit table and main table.
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'WUTrxPK')
BEGIN
	ALTER TABLE tWUnion_Trx 
	ALTER COLUMN WUTrxPK UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'WUTrxPK')
BEGIN
	ALTER TABLE tWUnion_Trx_Aud 
	ALTER COLUMN WUTrxPK UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'WUAccountPK')
BEGIN
	ALTER TABLE tWUnion_Trx_Aud 
	ALTER COLUMN WUAccountPK UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'DTTerminalCreate')
BEGIN
	ALTER TABLE tWUnion_Trx_Aud 
	ALTER COLUMN DTTerminalCreate DATETIME NULL
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'TxnPK')
BEGIN
	ALTER TABLE tTxn_MoneyTransfer 
	ALTER COLUMN TxnPK UNIQUEIDENTIFIER NULL
END
GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Trx' 
--		AND COLUMN_NAME = 'WUnionRecipientAccountPK'
--)
--BEGIN
--	ALTER TABLE tWUnion_Trx 
--	DROP COLUMN WUnionRecipientAccountPK
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Trx' 
--		AND COLUMN_NAME = 'WUAccountPK'
--)
--BEGIN
--	ALTER TABLE tWUnion_Trx 
--	DROP COLUMN WUAccountPK
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Trx' 
--		AND COLUMN_NAME = 'WUTrxPK'
--)
--BEGIN
--	ALTER TABLE tWUnion_Trx 
--	DROP COLUMN WUTrxPK
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Trx_Aud' 
--		AND COLUMN_NAME = 'WUnionRecipientAccountPK'
--)
--BEGIN
--	ALTER TABLE tWUnion_Trx 
--	DROP COLUMN WUnionRecipientAccountPK
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Trx_Aud' 
--		AND COLUMN_NAME = 'WUAccountPK'
--)
--BEGIN
--	ALTER TABLE tWUnion_Trx 
--	DROP COLUMN WUAccountPK
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tWUnion_Trx_Aud' 
--		AND COLUMN_NAME = 'WUTrxPK'
--)
--BEGIN
--	ALTER TABLE tWUnion_Trx 
--	DROP COLUMN WUTrxPK
--END
--GO


--- Adding Primary Key constraints for the tables -------------------------------------
---------Starts Here ------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Receiver' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Receiver')
BEGIN
	ALTER TABLE [dbo].[tWUnion_Receiver] ADD CONSTRAINT [PK_tWUnion_Receiver] PRIMARY KEY CLUSTERED (WUReceiverID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Account' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Account')
BEGIN
	ALTER TABLE [dbo].[tWUnion_Account] ADD CONSTRAINT [PK_tWUnion_Account] PRIMARY KEY CLUSTERED (WUAccountID)
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Account_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Account]'))
BEGIN
	ALTER TABLE [dbo].[tWUnion_Account]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Account_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
	REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionId])
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Account_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Account]'))
BEGIN
	ALTER TABLE [dbo].[tWUnion_Account]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Account_tCustomers] FOREIGN KEY([CustomerId])
	REFERENCES [dbo].[tCustomers] ([CustomerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_MoneyTransfer' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_MoneyTransfer')
BEGIN
	ALTER TABLE [dbo].[tTxn_MoneyTransfer] ADD CONSTRAINT [PK_tTxn_MoneyTransfer] PRIMARY KEY CLUSTERED (TransactionID)
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_MoneyTransfer_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyTransfer]'))
BEGIN
	ALTER TABLE [dbo].[tTxn_MoneyTransfer]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_MoneyTransfer_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
	REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Trx' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Trx')
BEGIN
	ALTER TABLE [dbo].tWUnion_Trx ADD CONSTRAINT [PK_tWUnion_Trx] PRIMARY KEY CLUSTERED (WUTrxID)
END
GO

--IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tTxn_MoneyTransfer]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
--BEGIN
--	ALTER TABLE [dbo].[tWUnion_Trx]  WITH CHECK ADD CONSTRAINT [FK_tWUnion_Trx_tTxn_MoneyTransfer] FOREIGN KEY([TransactionID])
--	REFERENCES [dbo].[tTxn_MoneyTransfer] ([TransactionID])
--END
--GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
BEGIN
	ALTER TABLE [dbo].[tWUnion_Trx]  WITH CHECK ADD CONSTRAINT [FK_tWUnion_Trx_tWUnion_Account] FOREIGN KEY([WUAccountID])
	REFERENCES [dbo].[tWUnion_Account] ([WUAccountID])
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Receiver]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
BEGIN
	ALTER TABLE [dbo].[tWUnion_Trx]  WITH CHECK ADD CONSTRAINT [FK_tWUnion_Trx_tWUnion_Receiver] FOREIGN KEY([WUReceiverID])
	REFERENCES [dbo].[tWUnion_Receiver] ([WUReceiverID])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Countries' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Countries')
BEGIN
	ALTER TABLE [dbo].tWUnion_Countries ADD CONSTRAINT [PK_tWUnion_Countries] PRIMARY KEY CLUSTERED (WUCountryID)
END
GO

---

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWUnion_Countries')
BEGIN
	CREATE NONCLUSTERED INDEX IX_tWUnion_Countries   
		ON tWUnion_Countries (ISOCountryCode);
END
GO


IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_States' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_States')
BEGIN
	ALTER TABLE [dbo].tWUnion_States ADD CONSTRAINT [PK_tWUnion_States] PRIMARY KEY CLUSTERED (WUStateID)
END
GO

---

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWUnion_CountryCurrencies')
BEGIN
	CREATE NONCLUSTERED INDEX IX_tWUnion_CountryCurrencies   
		ON tWUnion_CountryCurrencies (CountryCode);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Cities' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Cities')
BEGIN
	ALTER TABLE [dbo].tWUnion_Cities ADD CONSTRAINT [PK_tWUnion_Cities] PRIMARY KEY CLUSTERED (WUCityID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_CountryCurrencies' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_CountryCurrencies')
BEGIN
	ALTER TABLE [dbo].tWUnion_CountryCurrencies ADD CONSTRAINT [PK_tWUnion_CountryCurrencies] PRIMARY KEY CLUSTERED (WUCountryCurrencyID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWunion_CountryTranslation' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWunion_CountryTranslation')
BEGIN
	ALTER TABLE [dbo].tWunion_CountryTranslation ADD CONSTRAINT [PK_tWunion_CountryTranslation] PRIMARY KEY CLUSTERED (WUCountryID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Credential' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Credential')
BEGIN
	ALTER TABLE [dbo].tWUnion_Credential ADD CONSTRAINT [PK_tWUnion_Credential] PRIMARY KEY CLUSTERED (WUCredentialID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_CountryCurrencyDeliveryMethods' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_CountryCurrencyDeliveryMethods')
BEGIN
	ALTER TABLE [dbo].tWUnion_CountryCurrencyDeliveryMethods ADD CONSTRAINT [PK_tWUnion_CountryCurrencyDeliveryMethods] PRIMARY KEY CLUSTERED (WUDeliveryMethodID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_ErrorMessages' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_ErrorMessages')
BEGIN
	ALTER TABLE [dbo].tWUnion_ErrorMessages ADD CONSTRAINT [PK_tWUnion_ErrorMessages] PRIMARY KEY CLUSTERED (WUErrorMessageID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_DeliveryOptions' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_DeliveryOptions')
BEGIN
	ALTER TABLE [dbo].tWUnion_DeliveryOptions ADD CONSTRAINT [PK_tWUnion_DeliveryOptions] PRIMARY KEY CLUSTERED (WUDeliveryOptionID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Catalog' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Catalog')
BEGIN
	ALTER TABLE [dbo].tWUnion_Catalog ADD CONSTRAINT [PK_tWUnion_Catalog] PRIMARY KEY CLUSTERED (WUCatalogID)
END
GO


IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_Trx_WUTrxPK' )
BEGIN
	ALTER TABLE tWUnion_Trx ADD CONSTRAINT DF_tWUnion_Trx_WUTrxPK DEFAULT NEWID() FOR WUTrxPK
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWUnion_States')
BEGIN
	
	CREATE NONCLUSTERED INDEX IX_tWUnion_States   
		ON tWUnion_States (ISOCountryCode);
END
GO


IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWunion_CountryTranslation')
BEGIN
	
	CREATE NONCLUSTERED INDEX IX_tWunion_CountryTranslation   
		ON [tWunion_CountryTranslation] (ISOCountryCode);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tWUnion_CountryCurrencyDeliveryMethods')
BEGIN
	
	CREATE NONCLUSTERED INDEX IX_tWUnion_CountryCurrencyDeliveryMethods   
		ON [tWUnion_CountryCurrencyDeliveryMethods] (CountryCode);
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWUnion_States' 
		AND COLUMN_NAME = 'WUStatePK'
)
BEGIN
	ALTER TABLE tWUnion_States 
	ALTER COLUMN WUStatePK uniqueidentifier NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWunion_CountryTranslation' 
		AND COLUMN_NAME = 'WUCountryTranslationPK'
)
BEGIN
	ALTER TABLE tWunion_CountryTranslation 
	ALTER COLUMN WUCountryTranslationPK uniqueidentifier NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWUnion_DeliveryOptions' 
		AND COLUMN_NAME = 'WUDeliveryOptionPK'
)
BEGIN
	ALTER TABLE tWUnion_DeliveryOptions 
	ALTER COLUMN WUDeliveryOptionPK uniqueidentifier NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWUnion_Cities' 
		AND COLUMN_NAME = 'WUCityPK'
)
BEGIN
	ALTER TABLE tWUnion_Cities 
	ALTER COLUMN WUCityPK uniqueidentifier NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWUnion_Countries' 
		AND COLUMN_NAME = 'WUCountryPK'
)
BEGIN
	ALTER TABLE tWUnion_Countries 
	ALTER COLUMN WUCountryPK uniqueidentifier NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies' 
		AND COLUMN_NAME = 'WUCountryCurrencyPK'
)
BEGIN
	ALTER TABLE tWUnion_CountryCurrencies 
	ALTER COLUMN WUCountryCurrencyPK uniqueidentifier NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWunion_DeliveryTranslations' 
		AND COLUMN_NAME = 'WUDeliveryTranslationsPK'
)
BEGIN
	ALTER TABLE tWunion_DeliveryTranslations 
	ALTER COLUMN WUDeliveryTranslationsPK uniqueidentifier NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWUnion_ErrorMessages' 
		AND COLUMN_NAME = 'WUErrorMessagePK'
)
BEGIN
	ALTER TABLE tWUnion_ErrorMessages 
	ALTER COLUMN WUErrorMessagePK uniqueidentifier NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tWUnion_Catalog' 
		AND COLUMN_NAME = 'WUCatalogPK'
)
BEGIN
	ALTER TABLE tWUnion_Catalog 
	ALTER COLUMN WUCatalogPK uniqueidentifier NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tMasterCatalog' 
		AND COLUMN_NAME = 'MasterCatalogPK'
)
BEGIN
	ALTER TABLE tMasterCatalog 
	ALTER COLUMN MasterCatalogPK uniqueidentifier NULL
END
GO


IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tPartnerCatalog' 
		AND COLUMN_NAME = 'tPartnerCatalogPK'
)
BEGIN
	ALTER TABLE tPartnerCatalog 
	ALTER COLUMN tPartnerCatalogPK uniqueidentifier NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME IN ('CXEState'))
BEGIN
	 EXEC sp_RENAME 'tTxn_MoneyTransfer.CXEState' , 'State' , 'COLUMN'	 
END
GO


IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Receiver' AND COLUMN_NAME = 'WUReceiverPK')
BEGIN
	ALTER TABLE tWUnion_Receiver 
	ALTER COLUMN WUReceiverPK UNIQUEIDENTIFIER NULL
END
GO
--============================== Ends Here =========================================================================