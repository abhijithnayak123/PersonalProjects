/****** Object:  Table [dbo].[tWesternUnionStates]    Script Date: 05/14/2013 17:19:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tWesternUnionStates') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tWesternUnionStates](
	[StateCode] [varchar](20) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[ISOCountryCode] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StateCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

END
/****** Object:  Table [dbo].[tWesternUnionPickupMethods]    Script Date: 05/14/2013 17:22:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tWesternUnionPickupMethods') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tWesternUnionPickupMethods](
	[id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DescriptionEN] [nvarchar](60) NULL,
	[DescriptionES] [nvarchar](60) NULL,
	[CurrencyConversionDescriptionEN] [nvarchar](20) NULL,
	[CurrencyConversionDescriptionES] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

END

/****** Object:  Table [dbo].[tWesternUnionPaymentMethods]    Script Date: 05/14/2013 17:22:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tWesternUnionPaymentMethods') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tWesternUnionPaymentMethods](
	[Id] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


SET ANSI_PADDING OFF
END

/****** Object:  Table [dbo].[tWesternUnionDeliveryOptions]    Script Date: 05/14/2013 17:23:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tWesternUnionDeliveryOptions') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tWesternUnionDeliveryOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PRODUCT] [varchar](50) NOT NULL,
	[CATEGORY] [varchar](20) NULL,
	[T_INDEX] [varchar](20) NULL,
	[DESCRIPTION] [varchar](1000) NULL,
 CONSTRAINT [PK_tWesternUnionDeliveryOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF
END

/****** Object:  Table [dbo].[tWesternUnionCountryCurrencyDeliveryMethods]    Script Date: 05/14/2013 17:23:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tWesternUnionCountryCurrencyDeliveryMethods') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tWesternUnionCountryCurrencyDeliveryMethods](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryCode] [varchar](20) NULL,
	[CurrencyCode] [varchar](20) NULL,
	[SVC_CODE] [varchar](20) NULL,
	[SVC_NAME] [varchar](20) NULL,
	[ROUTE] [varchar](20) NULL,
	[BANNER] [varchar](50) NULL,
	[DESCRIPTION] [varchar](1000) NULL,
	[TEMPLT] [varchar](100) NULL,
	[CTRY_VIEW_FILTER] [varchar](200) NULL,
	[EXCL_FLAGS] [varchar](10) NULL,
	[SOURCE_MIN_CURRENCY] [money] NULL,
	[SOURCE_MAX_CURRENCY] [money] NULL,
	[SOURCE_CURRENCY_INCR] [money] NULL,
	[DEST_MIN_CURRENCY] [money] NULL,
	[DEST_MAX_CURRENCY] [money] NULL,
	[DEST_CURRENCY_INCR] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF
END

/****** Object:  Table [dbo].[tWesternUnionCountries]    Script Date: 05/14/2013 17:23:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tWesternUnionCountries') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tWesternUnionCountries](
	[ISOCountryCode] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ISOCountryCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END

/****** Object:  Table [dbo].[tWesternUnionCities]    Script Date: 05/14/2013 17:24:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tWesternUnionCities') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tWesternUnionCities](
	[CityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[StateCode] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


SET ANSI_PADDING OFF

END

