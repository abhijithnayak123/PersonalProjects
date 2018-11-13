IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChxr_Account_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tChxr_Account_Aud]
GO

CREATE TABLE [dbo].[tChxr_Account_Aud](
	[LogId] [uniqueidentifier] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Badge] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[ITIN] [nvarchar](20) NULL,
	[SSN] [nvarchar](20) NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Address1] [nvarchar](100) NOT NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](2) NOT NULL,
	[Zip] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Occupation] [nvarchar](100) NULL,
	[Employer] [nvarchar](100) NULL,
	[EmployerPhone] [nvarchar](100) NULL,
	[IDCardType] [nvarchar](100) NULL,
	[IDCardNumber] [nvarchar](50) NULL,
	[IDCardIssuedCountry] [nvarchar](50) NULL,
	[IDCardIssuedDate] [datetime] NULL,
	[IDCardImage] [varbinary](max) NULL,
	[IDCardExpireDate] [datetime] NULL,
	[CardNumber] [nvarchar](50) NULL,
	[CustomerScore] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChxr_Trx_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tChxr_Trx_Aud]
GO

CREATE TABLE [dbo].[tChxr_Trx_Aud](
	[LogId] [uniqueidentifier] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[ChexarAmount] [money] NULL,
	[ChexarFee] [money] NULL,
	[CheckDate] [datetime] NULL,
	[CheckNumber] [nvarchar](20) NULL,
	[RoutingNumber] [nvarchar](20) NULL,
	[AccountNumber] [nvarchar](20) NULL,
	[Micr] [nvarchar](50) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[InvoiceId] [int] NULL,
	[TicketId] [int] NULL,
	[WaitTime] [nvarchar](50) NULL,
	[Status] [int] NOT NULL,
	[ChexarStatus] [nvarchar](50) NOT NULL,
	[Type] [int] NULL,
	[ChexarType] [int] NULL,
	[DeclineCode] [int] NULL,
	[Message] [nvarchar](200) NULL,
	[Location] [nvarchar](50) NOT NULL,
	[ChxrAccountPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) ON [PRIMARY]

GO
