
/****** Object:  Table [dbo].[tRelationships]    Script Date: 05/28/2013 17:59:27 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[rowid_default]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tRelationships] DROP CONSTRAINT [rowid_default]
END

GO

/****** Object:  Table [dbo].[tRelationships]    Script Date: 05/29/2013 16:43:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tRelationships]') AND type in (N'U'))
DROP TABLE [dbo].[tRelationships]
GO

/****** Object:  Table [dbo].[tRelationships]    Script Date: 05/29/2013 16:43:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tRelationships](
	[Name] [nvarchar](20) NOT NULL,
	[rowid] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[rowid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tRelationships] ADD  CONSTRAINT [rowid_default]  DEFAULT (newid()) FOR [rowid]
GO





IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tWesternUnio__id__793DFFAF]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tWesternUnionPickupDetails] DROP CONSTRAINT [DF__tWesternUnio__id__793DFFAF]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tWesternU__DTCre__7A3223E8]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tWesternUnionPickupDetails] DROP CONSTRAINT [DF__tWesternU__DTCre__7A3223E8]
END

GO
/****** Object:  Table [dbo].[tWesternUnionPickupDetails]    Script Date: 05/29/2013 19:24:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWesternUnionPickupDetails]') AND type in (N'U'))
DROP TABLE [dbo].[tWesternUnionPickupDetails]
GO

/****** Object:  Table [dbo].[tWesternUnionPickupDetails]    Script Date: 05/29/2013 19:24:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tWesternUnionPickupDetails](
	[id] [uniqueidentifier] NOT NULL,
	[xid] [uniqueidentifier] NOT NULL,
	[PickupPoPName] [nvarchar](100) NULL,
	[PickupPoPRegion] [nvarchar](100) NULL,
	[PickupPoPBranch] [nvarchar](100) NULL,
	[PickupPoPBranchOperator] [nvarchar](100) NULL,
	[PickupPoPReferenceNumber] [nvarchar](100) NULL,
	[BeneficiaryGovernmentIDType] [nvarchar](50) NULL,
	[BeneficiaryGovernmentIDNumber] [nvarchar](50) NULL,
	[BeneficiaryGovernmentIDIssuer] [nvarchar](50) NULL,
	[BeneficiaryGovernmentIDIssuerState] [nvarchar](50) NULL,
	[BeneficiaryGovernmentIDIssuerCountry] [nvarchar](50) NULL,
	[BeneficiaryGovernmentIDExpirationDate] [datetime] NULL,
	[BeneficiaryName] [nvarchar](100) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tWesternUnionPickupDetails] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[tWesternUnionPickupDetails] ADD  DEFAULT (getdate()) FOR [DTCreate]
GO




/****** Object:  Table [dbo].[tWesternUnionCountryCurrencies]    Script Date: 05/29/2013 16:46:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWesternUnionCountryCurrencies]') AND type in (N'U'))
DROP TABLE [dbo].[tWesternUnionCountryCurrencies]
GO
/****** Object:  Table [dbo].[tWesternUnionCountryCurrencies]    Script Date: 05/29/2013 16:46:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tWesternUnionCountryCurrencies](
	[CountryCode] [varchar](20) NOT NULL,
	[CurrencyCode] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


