CREATE TABLE [dbo].[tTSys_Account](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
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
 CONSTRAINT [PK_tTSys_Account] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Index [IX_tTSys_Account_Id]    Script Date: 09/19/2013 20:38:12 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tTSys_Account_Id] ON [dbo].[tTSys_Account] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

