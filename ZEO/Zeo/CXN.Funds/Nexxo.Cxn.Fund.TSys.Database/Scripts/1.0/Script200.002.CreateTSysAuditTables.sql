CREATE TABLE [dbo].[tTSys_Account_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[ProgramId] [bigint] NOT NULL,
	[ExternalKey] [nvarchar](50) NOT NULL,
	[CardNumber] [nvarchar](200) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[MiddleName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[DOB] [datetime] NULL,
	[SSN] [nvarchar](50) NULL,
	[Phone] [nvarchar](20) NULL,
	[PhoneType] [nvarchar](20) NULL,
	[Address1] [nvarchar](100) NULL,
	[Address2] [nvarchar](100) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[ZipCode] [nvarchar](20) NULL,
	[Country] [nvarchar](50) NULL,
	[Activated] [bit] NOT NULL,
	[FraudScore] [int] NULL,
	[FraudResolution] [nvarchar](200) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL,
	[RevisionNo] [bigint] NULL
) ON [PRIMARY]

GO

CREATE CLUSTERED INDEX [IX_tTSys_Account_Aud] ON [dbo].[tTSys_Account_Aud]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tTSys_Trx_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NULL,
	[Description] [nvarchar](200) NULL,
	[DTLocalTransaction] [datetime] NULL,
	[DTTransmission] [datetime] NULL,
	[Status] [int] NOT NULL,
	[ErrorCode] [nvarchar](50) NULL,
	[ErrorMsg] [nvarchar](100) NULL,
	[ConfirmationId] [nvarchar](50) NULL,
	[Balance] [money] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[ChannelPartnerID] [bigint] NOT NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL,
	[RevisionNo] [bigint] NULL
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_tTSys_Trx_Aud] ON [dbo].[tTSys_Trx_Aud]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

