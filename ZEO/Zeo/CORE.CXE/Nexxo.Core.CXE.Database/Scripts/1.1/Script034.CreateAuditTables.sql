IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCustomerEmploymentDetails_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tCustomerEmploymentDetails_Aud]
GO

CREATE TABLE [dbo].[tCustomerEmploymentDetails_Aud](
	[CustomerPK] [uniqueidentifier] NOT NULL,
	[Occupation] [nvarchar](255) NULL,
	[Employer] [nvarchar](255) NULL,
	[EmployerPhone] [nvarchar](255) NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCustomerGovernmentIdDetails_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tCustomerGovernmentIdDetails_Aud]
GO

CREATE TABLE [dbo].[tCustomerGovernmentIdDetails_Aud](
	[CustomerPK] [uniqueidentifier] NOT NULL,
	[IdTypeId] [bigint] NULL,
	[Identification] [nvarchar](255) NULL,
	[ExpirationDate] [datetime] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[IssueDate] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCustomerProfiles_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tCustomerProfiles_Aud]
GO

CREATE TABLE [dbo].[tCustomerProfiles_Aud](
	[CustomerPK] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[LastName2] [nvarchar](255) NULL,
	[MothersMaidenName] [nvarchar](255) NULL,
	[DOB] [datetime] NULL,
	[Address1] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[ZipCode] [nvarchar](255) NULL,
	[Phone1] [nvarchar](255) NULL,
	[Phone1Type] [nvarchar](255) NULL,
	[Phone1Provider] [nvarchar](255) NULL,
	[Phone2] [nvarchar](255) NULL,
	[Phone2Type] [nvarchar](255) NULL,
	[Phone2Provider] [nvarchar](255) NULL,
	[SSN] [nvarchar](255) NULL,
	[TaxpayerId] [nvarchar](255) NULL,
	[DoNotCall] [bit] NULL,
	[SMSEnabled] [bit] NULL,
	[MarketingSMSEnabled] [bit] NULL,
	[ChannelPartnerId] [bigint] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[Gender] [nvarchar](6) NULL,
	[Email] [nvarchar](320) NULL,
	[PIN] [nvarchar](4) NULL,
	[IsMailingAddressDifferent] [bit] NULL,
	[MailingAddress1] [nvarchar](255) NULL,
	[MailingAddress2] [nvarchar](255) NULL,
	[MailingCity] [nvarchar](255) NULL,
	[MailingState] [nvarchar](255) NULL,
	[MailingZipCode] [nvarchar](255) NULL,
	[AuditEvent] [smallint] NULL,
	[DTAudit] [datetime] NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_Check_Stage_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tTxn_Check_Stage_Aud]
GO

CREATE TABLE [dbo].[tTxn_Check_Stage_Aud](
	[LogId] [uniqueidentifier] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[MICR] [nvarchar](100) NOT NULL,
	[CheckType] [int] NULL,
	[Status] [int] NOT NULL,
	[IssueDate] [datetime] NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_Funds_Stage_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tTxn_Funds_Stage_Aud]
GO

CREATE TABLE [dbo].[tTxn_Funds_Stage_Aud](
	[LogId] [uniqueidentifier] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) ON [PRIMARY]

GO