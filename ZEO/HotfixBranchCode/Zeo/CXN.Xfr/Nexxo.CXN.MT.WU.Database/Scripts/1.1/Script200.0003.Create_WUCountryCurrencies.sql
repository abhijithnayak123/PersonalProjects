IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_CountryCurrencies]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_CountryCurrencies]
GO

/****** Object:  Table [dbo].[tWUnion_CountryCurrencies]    Script Date: 10/10/2013 12:55:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tWUnion_CountryCurrencies](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[CountryName] nvarchar(200) NULL,
	[CountryCode] nvarchar(50) NULL,
	[CountryNumCode] nvarchar(max) NULL, 
	[CurrencyCode] nvarchar(50) NULL, 
	[CurrencyNumCode] nvarchar(max) NULL,
	[CurrencyName] nvarchar(200) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_CountryCurrencies] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO