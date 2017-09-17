IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFISConnectsDb]') AND type in (N'U'))
DROP TABLE [dbo].[tFISConnectsDb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tFISConnectsDb](
	[CustomerNumber] [varchar](20) NULL,
	[PrimaryPhoneNumber] [varchar](10) NULL,
	[Secondaryphone] [varchar](10) NULL,
	[CustomerTaxNumber] [varchar](9) NULL,
	[LastName] [varchar](10) NULL,
	[FirstName] [varchar](10) NULL,
	[MiddleName] [varchar](5) NULL,
	[MiddleName2] [varchar](16) NULL,
	[AddressStreet] [varchar](40) NULL,
	[AddressCity] [varchar](25) NULL,
	[AddressState] [varchar](2) NULL,
	[ZipCode] [varchar](9) NULL,
	[DOB] [datetime] NULL,
	[DriversLicenseNumber] [varchar](20) NULL,
	[MothersMaidenName] [varchar](20) NULL,
	[Gender] [varchar](1) NULL,
	[ExternalKey] [varchar](10) NULL,
	[MetBankNumber] [varchar](3) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO