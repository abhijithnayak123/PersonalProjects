
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_PickupDetails_tWUnion_Recipient_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_PickupDetails]'))
ALTER TABLE [dbo].[tWUnion_PickupDetails] DROP CONSTRAINT [FK_tWUnion_PickupDetails_tWUnion_Recipient_Account]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_RecipientProfiles]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] DROP CONSTRAINT [FK_tWUnion_Trx_tWUnion_RecipientProfiles]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_PickupDetails_tWUnion_Recipient_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_PickupDetails]'))
ALTER TABLE [dbo].[tWUnion_PickupDetails] DROP CONSTRAINT [FK_tWUnion_PickupDetails_tWUnion_Recipient_Account]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_PickupDetails]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_PickupDetails]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_RecipientProfiles]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] DROP CONSTRAINT [FK_tWUnion_Trx_tWUnion_RecipientProfiles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Trx]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_PickupMethods]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_PickupMethods]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tWUnion_RecipientProfiles_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tWUnion_Recipient_Account] DROP CONSTRAINT [DF_tWUnion_RecipientProfiles_DTCreate]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Recipient_Account]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Recipient_Account]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Relationships]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Relationships]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_States]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_States]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_AmountTypes]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_AmountTypes]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_t_WUnion_Cities_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tWUnion_Cities] DROP CONSTRAINT [DF_t_WUnion_Cities_DTCreate]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Cities]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Cities]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Countries]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Countries]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_CountryCurrencies]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_CountryCurrencies]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_CountryCurrencyDeliveryMethods]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_CountryCurrencyDeliveryMethods]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_DeliveryOptions]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_DeliveryOptions]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_PaymentMethods]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_PaymentMethods]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_PaymentMethods]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_PaymentMethods](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](250) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_PaymentMethods] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_DeliveryOptions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_DeliveryOptions](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Product] [varchar](50) NOT NULL,
	[Category] [varchar](20) NULL,
	[T_Index] [varchar](20) NULL,
	[Description] [varchar](1000) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_DeliveryOptions] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_CountryCurrencyDeliveryMethods]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_CountryCurrencyDeliveryMethods](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[CountryCode] [varchar](20) NULL,
	[CurrencyCode] [varchar](20) NULL,
	[SvcCode] [varchar](20) NULL,
	[SvcName] [varchar](20) NULL,
	[Route] [varchar](20) NULL,
	[Banner] [varchar](50) NULL,
	[Description] [varchar](1000) NULL,
	[Templt] [varchar](100) NULL,
	[CountryViewFilter] [varchar](200) NULL,
	[ExclFlags] [varchar](10) NULL,
	[SourceMinCurrency] [money] NULL,
	[SourceMaxCurrency] [money] NULL,
	[SourceCurrencyIncr] [money] NULL,
	[DestinationMinCurrency] [money] NULL,
	[DestinationMaxCurrency] [money] NULL,
	[DestinationCurrencyIncr] [money] NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_CountryCurrencyDeliveryMethods] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_CountryCurrencies]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_CountryCurrencies](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[CountryCode] [varchar](20) NOT NULL,
	[CurrencyCode] [varchar](20) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_CountryCurrencies] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Countries]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_Countries](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ISOCountryCode] [varchar](20) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWunion_Countries] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Cities]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_Cities](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[StateCode] [varchar](20) NOT NULL,
	[DTCreate] [datetime] NOT NULL CONSTRAINT [DF_t_WUnion_Cities_DTCreate]  DEFAULT (getdate()),
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_t_WUnion_Cities] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_AmountTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_AmountTypes](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[AmountType] [varchar](20) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_AmountTypes] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_States]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_States](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[StateCode] [varchar](20) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[ISOCountryCode] [varchar](20) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_TWUnion_States] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Relationships]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_Relationships](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[RelationshipName] [varchar](50) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_Relationships] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Recipient_Account]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_Recipient_Account](
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[CustomerId] [bigint] NOT NULL,
	[FName] [nvarchar](50) NULL,
	[LName] [nvarchar](50) NULL,
	[Active] [bit] NULL,
	[Relationship] [nvarchar](20) NULL,
	[Address] [nvarchar](200) NULL,
	[PickupMethodId] [int] NULL,
	[AccountNumber] [nvarchar](50) NULL,
	[DOB] [datetime] NULL,
	[Gender] [nvarchar](6) NULL,
	[PrimaryPhoneNumber] [nvarchar](20) NULL,
	[PhoneType] [varchar](50) NULL,
	[MobileProvider] [varchar](50) NULL,
	[PaymentMethodId] [int] NULL,
	[AmountType] [nvarchar](20) NULL,
	[PaymentType] [nvarchar](20) NULL,
	[DeliveryMethodId] [varchar](50) NULL,
	[DeliveryOptionId] [int] NULL,
	[DestinationCurrencyCode] [nvarchar](20) NULL,
	[DestinationCountryCode] [nvarchar](20) NULL,
	[DestinationStateCode] [nvarchar](20) NULL,
	[DestinationCity] [nvarchar](200) NULL,
	[DTCreate] [datetime] NOT NULL CONSTRAINT [DF_tWUnion_RecipientProfiles_DTCreate]  DEFAULT (getdate()),
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_RecipientProfiles] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_PickupMethods]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_PickupMethods](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DescriptionEN] [nvarchar](60) NULL,
	[DescriptionES] [nvarchar](60) NULL,
	[CurrencyConversionDescriptionEN] [nvarchar](20) NULL,
	[CurrencyConversionDescriptionES] [nvarchar](20) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_PickupMethods] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_Trx](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[WUnionAccountPK] [uniqueidentifier] NOT NULL,
	[OriginatorsPrincipalAmount] [bigint] NULL,
	[OriginatingCountryCode] [varchar](20) NULL,
	[OriginatingCurrencyCode] [varchar](20) NULL,
	[TranascationType] [varchar](20) NULL,
	[PromotionsCode] [varchar](50) NULL,
	[ExchangeRate] [float] NULL,
	[DestinationPrincipalAmount] [bigint] NULL,
	[GrossTotalAmount] [bigint] NULL,
	[Charges] [bigint] NULL,
	[TaxAmount] [bigint] NULL,
	[Mtcn] [varchar](50) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_Trx] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_PickupDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_PickupDetails](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[WUnionAccountPK] [uniqueidentifier] NOT NULL,
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
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_t_WUnion_PickupDetails] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_PickupDetails_tWUnion_Recipient_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_PickupDetails]'))
ALTER TABLE [dbo].[tWUnion_PickupDetails]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_PickupDetails_tWUnion_Recipient_Account] FOREIGN KEY([WUnionAccountPK])
REFERENCES [dbo].[tWUnion_Recipient_Account] ([rowguid])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_PickupDetails_tWUnion_Recipient_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_PickupDetails]'))
ALTER TABLE [dbo].[tWUnion_PickupDetails] CHECK CONSTRAINT [FK_tWUnion_PickupDetails_tWUnion_Recipient_Account]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_RecipientProfiles]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Trx_tWUnion_RecipientProfiles] FOREIGN KEY([WUnionAccountPK])
REFERENCES [dbo].[tWUnion_Recipient_Account] ([rowguid])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_RecipientProfiles]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] CHECK CONSTRAINT [FK_tWUnion_Trx_tWUnion_RecipientProfiles]
GO
