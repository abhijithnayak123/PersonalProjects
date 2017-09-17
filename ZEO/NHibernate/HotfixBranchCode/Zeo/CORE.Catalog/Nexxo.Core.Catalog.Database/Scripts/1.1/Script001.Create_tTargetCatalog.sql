IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTargetCatalog]') AND type in (N'U'))
DROP TABLE [dbo].[tTargetCatalog]
GO

/****** Object:  Table [dbo].[tTargetCatalog]    Script Date: 05/21/2013 23:36:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTargetCatalog](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[CatalogIndex] [int] NOT NULL,
	[ProcessorID] [int] NOT NULL,
	[ChannelPartnerID] [smallint] NOT NULL,
	[BillerName] [nvarchar](60) NOT NULL,
	[ProductKey] [nvarchar](50) NULL,
	[ProductType] [int] NOT NULL,
	[Fee] [money] NULL,
	[MinimumLoad] [money] NULL,
	[MaximumLoad] [money] NULL,
	[LogoURL] [nvarchar](50) NULL,
	[BillerNotes] [nvarchar](1000) NULL,
	[BillerDisclosure] [nvarchar](max) NULL,
	[LocationRegionId] [uniqueidentifier] NULL,
	[ExtraDataRequired] [bit] NULL,
	[DataPrompt] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [pk_Products] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO