--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <10-19-2016>
-- Description:	As an engineer, I want to implement ADO.Net for GPR - Database changes
-- Jira ID:		<AL-8323>
-- ================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_Trx'
		AND CONSTRAINT_NAME = 'FK_tVisa_Trx_tVisa_Account'
)
BEGIN
	ALTER TABLE tVisa_Trx 
	DROP CONSTRAINT FK_tVisa_Trx_tVisa_Account
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_Account'
		AND CONSTRAINT_NAME = 'PK_tVisa_Account'
)
BEGIN
	ALTER TABLE tVisa_Account 
	DROP CONSTRAINT PK_tVisa_Account
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTxn_Funds'
		AND CONSTRAINT_NAME = 'PK_tTxn_Funds'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	DROP CONSTRAINT PK_tTxn_Funds
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE TABLE_NAME = 'tVisa_Trx'
		AND CONSTRAINT_NAME = 'PK_tVisa_Trx'
)
BEGIN
	ALTER TABLE tVisa_Trx 
	DROP CONSTRAINT PK_tVisa_Trx
END
GO



IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Account'
		AND COLUMN_NAME = 'FirstName'
)
BEGIN
	ALTER TABLE tVisa_Account 
	ALTER COLUMN FirstName VARCHAR(50) NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Account'
		AND COLUMN_NAME = 'LastName'
)
BEGIN
	ALTER TABLE tVisa_Account 
	ALTER COLUMN LastName VARCHAR(50) NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Account'
		AND COLUMN_NAME = 'DateOfBirth'
)
BEGIN
	ALTER TABLE tVisa_Account 
	ALTER COLUMN DateOfBirth DATETIME NULL
END
GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'FirstName'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN FirstName
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'MiddleName'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN MiddleName
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'FirstName'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN FirstName
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'LastName'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN LastName
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'DateOfBirth'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN DateOfBirth
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'SSN'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN SSN
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'SSN'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN SSN
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'Phone'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN Phone
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'Address1'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN Address1
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'Address2'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN Address2
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'City'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN City
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'State'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN State
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'ZipCode'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN ZipCode
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'Country'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN Country
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'MothersMaidenName'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN MothersMaidenName
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'IDCode'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN IDCode
--END
--GO


--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Account'
--		AND COLUMN_NAME = 'Email'
--)
--BEGIN
--	ALTER TABLE tVisa_Account 
--	DROP COLUMN Email
--END
--GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Account' 
		AND COLUMN_NAME = 'CustomerId'
)
BEGIN
	ALTER TABLE tVisa_Account 
	ADD CustomerId BIGINT NULL
END
GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Account' 
		AND COLUMN_NAME = 'CustomerSessionId'
)
BEGIN
	ALTER TABLE tVisa_Account 
	ADD CustomerSessionId BIGINT NULL
END
GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Account' 
		AND COLUMN_NAME = 'CustomerRevisionNo'
)
BEGIN
	ALTER TABLE tVisa_Account 
	ADD CustomerRevisionNo BIGINT NULL
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_Account'
		AND CONSTRAINT_NAME = 'PK_tVisa_Account'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_Account 
	ADD CONSTRAINT PK_tVisa_Account PRIMARY KEY CLUSTERED (VisaAccountId)
END
GO


IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Trx' 
		AND COLUMN_NAME = 'VisaAccountId'
)
BEGIN
	ALTER TABLE tVisa_Trx 
	ADD VisaAccountId BIGINT NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Trx' 
		AND COLUMN_NAME = 'AccountPK'
)
BEGIN
	ALTER TABLE tVisa_Trx 
	ALTER COLUMN AccountPK UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Trx' 
		AND COLUMN_NAME = 'ChannelPartnerID'
)
BEGIN
	ALTER TABLE tVisa_Trx 
	ALTER COLUMN ChannelPartnerID BIGINT NULL
END
GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tVisa_Trx' 
--		AND COLUMN_NAME = 'ChannelPartnerID'
--)
--BEGIN
--	ALTER TABLE tVisa_Trx 
--	DROP COLUMN ChannelPartnerID
--END
--GO


IF NOT EXISTS(
	SELECT 1
	FROM SYS.TABLES 
	WHERE NAME =  'tVisa_Account_Aud'
)
BEGIN
	CREATE TABLE [dbo].[tVisa_Account_Aud]
	(
		[VisaAccountID] [bigint] NOT NULL,
		[ProxyId] [varchar](25) NULL,
		[PseudoDDA] [varchar](50) NULL,
		[CardNumber] [varchar](50) NULL,
		[CardAliasId] [varchar](50) NULL,
		[Activated] [bit] NOT NULL,
		[ExpirationMonth] [int] NULL,
		[ExpirationYear] [int] NULL,
		[SubClientNodeId] [bigint] NULL,
		[DTTerminalCreate] [datetime] NOT NULL,
		[DTTerminalLastModified] [datetime] NULL,
		[DTServerCreate] [datetime] NOT NULL,
		[DTServerLastModified] [datetime] NULL,
		[DTAccountClosed] [datetime] NULL,
		[PrimaryCardAliasId] [varchar](50) NULL,
		[ActivatedLocationNodeId] [bigint] NULL,
		[CustomerID] [bigint] NULL,
		[CustomerSessionID] [bigint] NULL,
		[CustomerRevisionNo] [bigint] NULL,
		[RevisionNo] [bigint] NOT NULL,
		[AuditEvent] [smallint] not null,
		[DTAudit] [datetime] NOT NULL
   )
END
GO
IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTxn_Funds' 
		AND COLUMN_NAME = 'CXEId'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	ALTER COLUMN CXEId BIGINT NULL
END
GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tTxn_Funds' 
--		AND COLUMN_NAME = 'CXEState'
--)
--BEGIN
--	ALTER TABLE tTxn_Funds 
--	DROP COLUMN CXEState
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tTxn_Funds' 
--		AND COLUMN_NAME = 'CXEId'
--)
--BEGIN
--	ALTER TABLE tTxn_Funds 
--	DROP COLUMN CXEId
--END
--GO

--IF EXISTS (
--		SELECT 1 
--		FROM INFORMATION_SCHEMA.COLUMNS 
--		WHERE TABLE_NAME = 'tTxn_Funds' 
--		AND COLUMN_NAME = 'CustomerSessionPK'
--)
--BEGIN
--	ALTER TABLE tTxn_Funds 
--	DROP COLUMN CustomerSessionPK
--END
--GO


IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTxn_Funds' 
		AND COLUMN_NAME = 'CustomerSessionPK'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	ALTER COLUMN CustomerSessionPK UNIQUEIDENTIFIER NULL
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTxn_Funds'
		AND CONSTRAINT_NAME = 'FK_tTxn_Funds_tAccounts'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	DROP CONSTRAINT FK_tTxn_Funds_tAccounts
END
GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTxn_Funds' 
		AND COLUMN_NAME = 'CustomerSessionId'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	ADD CustomerSessionId BIGINT NULL
END
GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTxn_Funds' 
		AND COLUMN_NAME = 'ProviderId'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	ADD ProviderId INT
END
GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTxn_Funds' 
		AND COLUMN_NAME = 'ProviderAccountId'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	ADD ProviderAccountId BIGINT
END
GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTxn_Funds' 
		AND COLUMN_NAME = 'CustomerRevisionNo'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	ADD CustomerRevisionNo INT
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTxn_Funds' 
		AND COLUMN_NAME = 'AccountPK'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	ALTER COLUMN AccountPK UNIQUEIDENTIFIER NULL
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTxn_Funds'
		AND CONSTRAINT_NAME = 'PK_tTxn_Funds'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	ADD CONSTRAINT PK_tTxn_Funds PRIMARY KEY CLUSTERED (TransactionId)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_Trx'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_Trx 
	ADD CONSTRAINT PK_tVisa_Trx PRIMARY KEY CLUSTERED (VisaTrxID)
END
GO



IF NOT EXISTS(
	SELECT 1
	FROM SYS.TABLES 
	WHERE NAME =  'tTxn_Funds_Aud'
)
BEGIN
	CREATE TABLE [dbo].[tTxn_Funds_Aud]
	(
		[TransactionID] [bigint] NOT NULL,
		[CXNId] [bigint] NULL,
		[CustomerSessionId] [BIGINT] NULL,
		[Amount] [money] NULL,
		[Fee] [money] NULL,
		[Description] [nvarchar](255) NULL,
		[State] [int] NULL,
		[DTTerminalCreate] [datetime] NOT NULL,
		[DTTerminalLastModified] [datetime] NULL,
		[ConfirmationNumber] [varchar](50) NULL,
		[FundType] [int] NULL,
		[DTServerCreate] [datetime] NULL,
		[DTServerLastModified] [datetime] NULL,
		[BaseFee] [money] NULL,
		[DiscountApplied] [money] NULL,
		[AdditionalFee] [money] NULL,
		[DiscountName] [varchar](50) NULL,
		[DiscountDescription] [varchar](100) NULL,
		[IsSystemApplied] [bit] NOT NULL,
		[AddOnCustomerId] [bigint] NULL,
		[ProviderId] [int] NULL,
		[CustomerRevisionNo] [int] NULL,
		[ProviderAccountId] [BIGINT] NULL,
		[RevisionNo] [bigint] NOT NULL,
		[AuditEvent] [smallint] NOT null,
		[DTAudit] [datetime] NOT NULL
	)
END
GO

---------------------------------------------------------Changes Related to Visa FeeTypes----------------------------------------------------------

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_ChannelPartnerFeeTypeMapping' 
		AND CONSTRAINT_NAME = 'FK_tVisa_ChannelPartnerFeeTypeMapping_tVisa_FeeTypes'
)
BEGIN
	ALTER TABLE tVisa_ChannelPartnerFeeTypeMapping 
	DROP CONSTRAINT FK_tVisa_ChannelPartnerFeeTypeMapping_tVisa_FeeTypes
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_Fee' 
		AND CONSTRAINT_NAME = 'FK_tVisa_Fee_tVisa_ChannelPartnerFeeTypeMapping'
)
BEGIN
	ALTER TABLE tVisa_Fee 
	DROP CONSTRAINT FK_tVisa_Fee_tVisa_ChannelPartnerFeeTypeMapping
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_FeeTypes'
		AND CONSTRAINT_NAME = 'PK_tVisa_FeeTypes'
)
BEGIN
	ALTER TABLE tVisa_FeeTypes 
	DROP CONSTRAINT PK_tVisa_FeeTypes
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_ChannelPartnerFeeTypeMapping' 
		AND CONSTRAINT_NAME = 'PK_tVisa_ChannelPartnerFeeTypeMapping'
)
BEGIN
	ALTER TABLE tVisa_ChannelPartnerFeeTypeMapping 
	DROP CONSTRAINT PK_tVisa_ChannelPartnerFeeTypeMapping
END
GO


IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_Fee' 
		AND COLUMN_NAME = 'ChannelPartnerFeeTypeId'
)
BEGIN
	ALTER TABLE tVisa_Fee 
	ADD ChannelPartnerFeeTypeId BIGINT NULL
END
GO


IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_ChannelPartnerFeeTypeMapping' 
		AND COLUMN_NAME = 'VisaFeeTypeId'
)
BEGIN
	ALTER TABLE tVisa_ChannelPartnerFeeTypeMapping 
	ADD VisaFeeTypeId BIGINT NULL
END
GO

--------------------------------------------------------------Changes Related to Visa Shipping Types-----------------------------------------------

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_ChannelPartnerShippingTypeMapping' 
		AND CONSTRAINT_NAME = 'FK_tVisa_ChannelPartnerShippingTypeMapping_tVisa_CardShippingTypes'
)
BEGIN
	ALTER TABLE tVisa_ChannelPartnerShippingTypeMapping 
	DROP CONSTRAINT FK_tVisa_ChannelPartnerShippingTypeMapping_tVisa_CardShippingTypes
END
GO


IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_ShippingFee' 
		AND CONSTRAINT_NAME = 'FK_tVisa_Fee_tVisa_ChannelPartnerShippingTypeMapping'
)
BEGIN
	ALTER TABLE tVisa_ShippingFee 
	DROP CONSTRAINT FK_tVisa_Fee_tVisa_ChannelPartnerShippingTypeMapping
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_ShippingFee' 
		AND CONSTRAINT_NAME = 'FK_tVisa_ShippingFee_tVisa_ChannelPartnerShippingTypeMapping'
)
BEGIN
	ALTER TABLE tVisa_ShippingFee 
	DROP CONSTRAINT FK_tVisa_ShippingFee_tVisa_ChannelPartnerShippingTypeMapping
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_ChannelPartnerShippingTypeMapping'
		AND CONSTRAINT_NAME = 'PK_tVisa_ChannelPartnerShippingTypeMapping'
)
BEGIN
	ALTER TABLE tVisa_ChannelPartnerShippingTypeMapping 
	DROP CONSTRAINT PK_tVisa_ChannelPartnerShippingTypeMapping
END
GO

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_CardShippingTypes' 
		AND CONSTRAINT_NAME = 'PK_tCardShippingType'
)
BEGIN
	ALTER TABLE tVisa_CardShippingTypes 
	DROP CONSTRAINT PK_tCardShippingType
END
GO


IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_ChannelPartnerShippingTypeMapping' 
		AND COLUMN_NAME = 'ChannelPartnerShippingTypeId'
)
BEGIN
	ALTER TABLE tVisa_ChannelPartnerShippingTypeMapping 
	ADD ChannelPartnerShippingTypeId BIGINT Identity(1000000000, 1)  NOT NULL
END
GO


IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_ShippingFee' 
		AND COLUMN_NAME = 'ChannelPartnerShippingTypeId'
)
BEGIN
	ALTER TABLE tVisa_ShippingFee 
	ADD ChannelPartnerShippingTypeId BIGINT NULL
END
GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tVisa_ChannelPartnerShippingTypeMapping' 
		AND COLUMN_NAME = 'CardShippingTypeId'
)
BEGIN
	ALTER TABLE tVisa_ChannelPartnerShippingTypeMapping 
	ADD CardShippingTypeId BIGINT NULL
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_CardClass'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_CardClass 
	ADD CONSTRAINT PK_tVisa_CardClass PRIMARY KEY CLUSTERED (VisaCardClassId)
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_CardShippingTypes'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_CardShippingTypes 
	ADD CONSTRAINT PK_tVisa_CardShippingTypes PRIMARY KEY CLUSTERED (CardShippingTypeId)
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_ChannelPartnerShippingTypeMapping'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_ChannelPartnerShippingTypeMapping 
	ADD CONSTRAINT PK_tVisa_ChannelPartnerShippingTypeMapping PRIMARY KEY CLUSTERED (ChannelPartnerShippingTypeId)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_Fee'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_Fee 
	ADD CONSTRAINT PK_tVisa_Fee PRIMARY KEY CLUSTERED (VisaFeeId)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_FeeTypes'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_FeeTypes 
	ADD CONSTRAINT PK_tVisa_FeeTypes PRIMARY KEY CLUSTERED (VisaFeeTypeId)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_ShippingFee'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_ShippingFee 
	ADD CONSTRAINT PK_tVisa_ShippingFee PRIMARY KEY CLUSTERED (VisaShippingFeeId)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_ChannelPartnerFeeTypeMapping'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_ChannelPartnerFeeTypeMapping 
	ADD CONSTRAINT PK_tVisa_ChannelPartnerFeeTypeMapping PRIMARY KEY CLUSTERED (ChannelPartnerFeeTypeId)
END
GO

--

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_Credential'
		AND CONSTRAINT_NAME = 'PK_tVisa_Credential'
)
BEGIN
	ALTER TABLE tVisa_Credential 
	DROP CONSTRAINT PK_tVisa_Credential
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tVisa_Credential'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tVisa_Credential 
	ADD CONSTRAINT PK_tVisa_Credential PRIMARY KEY CLUSTERED (VisaCredentialId)
END
GO

-----------------------------------Altering the PK Columns with default constraints-------------------------------------------------------

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tVisa_Trx_PK' )
BEGIN
	ALTER TABLE tVisa_Trx ADD CONSTRAINT DF_tVisa_Trx_PK DEFAULT NEWID() FOR VisaTrxPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTxn_Funds_PK' )
BEGIN
	ALTER TABLE tTxn_Funds ADD CONSTRAINT DF_tTxn_Funds_PK DEFAULT NEWID() FOR TxnPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tVisa_Account_PK' )
BEGIN
	ALTER TABLE tVisa_Account ADD CONSTRAINT DF_tVisa_Account_PK DEFAULT NEWID() FOR VisaAccountPK
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Funds' AND COLUMN_NAME IN ('CXEState'))
BEGIN
	 EXEC sp_RENAME 'tTxn_Funds.CXEState' , 'State' , 'COLUMN'	 
END
GO


IF NOT EXISTS ( SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Trx' AND COLUMN_NAME = 'AccountPK')
BEGIN
	ALTER TABLE tVisa_Trx ALTER COLUMN AccountPK UNIQUEIDENTIFIER NULL
END
GO



