IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFView_Card_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tFView_Card_Aud]
GO

CREATE TABLE [dbo].[tFView_Card_Aud](
	[LogId] [uniqueidentifier] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CardNumber] [nvarchar](30) NULL,
	[AccountNumber] [nvarchar](30) NULL,
	[BSAccountNumber] [nvarchar](30) NULL,
	[NameAsOnCard] [nvarchar](50) NULL,
	[FirstName] [nvarchar](50) NULL,
	[MiddleName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[DateOfBirth] [datetime] NULL,
	[SSNNumber] [nvarchar](12) NULL,
	[GovernmentID] [nvarchar](50) NULL,
	[IDNumber] [nvarchar](50) NULL,
	[GovtIdExpirationDate] [datetime] NULL,
	[GovtIDIssueCountry] [nvarchar](50) NULL,
	[GovtIDIssueDate] [datetime] NULL,
	[GovtIDIssueState] [nvarchar](50) NULL,
	[AddressLine1] [nvarchar](100) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[PostalCode] [nvarchar](20) NULL,
	[HomePhoneNumber] [nvarchar](20) NULL,
	[ShippingContactName] [nvarchar](200) NULL,
	[ShippingAddressLine1] [nvarchar](100) NULL,
	[ShippingAddressLine2] [nvarchar](100) NULL,
	[ShippingCity] [nvarchar](50) NULL,
	[ShippingState] [nvarchar](50) NULL,
	[ShippingZipCode] [nvarchar](20) NULL,
	[ExpiryDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[DTActivated] [datetime] NULL,
	[ActivatedBy] [int] NULL,
	[DTDeactivated] [datetime] NULL,
	[DeactivatedBy] [int] NULL,
	[DeactivatedReason] [nvarchar](100) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFView_Trx_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tFView_Trx_Aud]
GO

CREATE TABLE [dbo].[tFView_Trx_Aud](
	[LogId] [uniqueidentifier] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[ProcessorId] [int] NULL,
	[PrimaryAccountNumber] [bigint] NULL,
	[TransactionType] [varchar](20) NULL,
	[TransactionAmount] [money] NOT NULL,
	[CardAcceptorIdCode] [varchar](20) NULL,
	[CardAcceptorTerminalID] [varchar](100) NULL,
	[CardAcceptorBusinessCode] [int] NULL,
	[TransactionDescription] [varchar](50) NULL,
	[MessageTypeIdentifier] [varchar](10) NULL,
	[TransactionCurrencyCode] [varchar](10) NULL,
	[DTLocalTransaction] [datetime] NULL,
	[DTTransmission] [datetime] NULL,
	[CreditPlanMaster] [int] NULL,
	[AccountNumber] [varchar](20) NULL,
	[TransactionID] [varchar](20) NULL,
	[CardBalance] [money] NOT NULL,
	[ErrorCode] [varchar](10) NULL,
	[ErrorMsg] [varchar](200) NULL,
	[CardStatus] [varchar](10) NULL,
	[ActivationRequired] [varchar](5) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) ON [PRIMARY]

GO
