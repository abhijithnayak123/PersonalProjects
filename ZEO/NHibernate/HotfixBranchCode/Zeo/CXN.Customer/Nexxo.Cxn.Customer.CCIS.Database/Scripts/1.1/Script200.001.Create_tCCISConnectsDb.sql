
GO

/****** Object:  Table [dbo].[tCCISConnectsDb]    Script Date: 04/22/2014 15:55:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCCISConnectsDb]') AND type in (N'U'))
DROP TABLE [dbo].[tCCISConnectsDb]
GO



/****** Object:  Table [dbo].[tCCISConnectsDb]    Script Date: 04/22/2014 15:55:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tCCISConnectsDb](
	[CustomerNumber] [varchar](20) NULL,
	[CustomerTaxNumber] [nvarchar](255) NULL,
	[PrimaryPhoneNumber] [nvarchar](50) NULL,
	[SecondaryPhoneNumber] [nvarchar](50) NULL,
	[LastName] [nvarchar](255) NULL,
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[MiddleName2] [nvarchar](255) NULL,
	[AddressStreet] [nvarchar](255) NULL,
	[AddressCity] [nvarchar](255) NULL,
	[AddressState] [nvarchar](255) NULL,
	[ZipCode] [nvarchar](30) NULL,
	[DOB] [datetime] NULL,
	[MothersMaidenName] [nvarchar](255) NULL,
	[IdNumber] [nvarchar](255) NULL,
	[ExternalKey] [varchar](10) NULL,
	[MetBankNumber] [varchar](3) NULL,
	[ProgramId] [varchar](10) NULL,
	[DTCreate] [datetime] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tCCISConnectsDb] ADD  CONSTRAINT [DF_tCCISConnectsDb_CreateDate_GETDATE]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tCCISConnectsDb] ADD  DEFAULT (newid()) FOR [rowguid]
GO


