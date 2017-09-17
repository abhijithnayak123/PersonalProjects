/****** Object:  Table [dbo].[tFirstViewTransactionMapping]    Script Date: 05/14/2013 17:32:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tFirstViewTransactionMapping') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tFirstViewTransactionMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FundId] [bigint] NULL,
	[ProcessorReferenceId] [varchar](50) NULL,
	[ProcessorId] [int] NULL,
	[PrimaryAccountNumber] [bigint] NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[DateOfBirth] [datetime] NULL,
	[SSNNumber] [bigint] NULL,
	[GovernmentID] [varchar](20) NULL,
	[IDNumber] [varchar](50) NULL,
	[AddressLine1] [varchar](100) NULL,
	[AddressLine2] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[PostalCode] [varchar](20) NULL,
	[HomePhoneNumber] [varchar](20) NULL,
	[TranType] [varchar](20) NULL,
	[TransactionAmount] [decimal](18, 0) NULL,
	[CardAcceptorIdCode] [varchar](20) NULL,
	[CardAcceptorTerminalID] [varchar](20) NULL,
	[CardAcceptorBusinessCode] [int] NULL,
	[TransactionDescription] [varchar](50) NULL,
	[MessageTypeIdentifier] [varchar](10) NULL,
	[TransactionCurrencyCode] [varchar](10) NULL,
	[DTLocalTransaction] [datetime] NULL,
	[DTTransmission] [datetime] NULL,
	[CreditPlanMaster] [int] NULL,
	[AccountNumber] [varchar](20) NULL,
	[TransactionID] [varchar](20) NULL,
	[CardBalance] [decimal](18, 0) NULL,
	[ErrorCode] [varchar](10) NULL,
	[ErrorMsg] [varchar](200) NULL,
	[DTCreate] [datetime] NOT NULL,
	[CardStatus] [varchar](10) NULL,
	[ActivationRequired] [varchar](5) NULL,
 CONSTRAINT [PK_tFirstViewTransactionMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

ALTER TABLE [dbo].[tFirstViewTransactionMapping] ADD  CONSTRAINT [DF_tFirstViewTransactionMapping_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]

END
