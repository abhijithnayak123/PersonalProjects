
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMasterCatalog]') AND type in (N'U'))
DROP TABLE [dbo].[tMasterCatalog]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tMasterCatalog](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(100000000,1) NOT NULL,
	[ProviderCatalogId] [bigint] NOT NULL,
	[BillerName] [varchar](255) NOT NULL,
	[ChannelPartnerId] [int] NOT NULL,
	[ProviderId] [int] NOT NULL,
	[ProductType] [int] NULL,
	[LogoURL] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[DtCreate] [datetime] NOT NULL,
	[DtModified] [datetime] NULL,
 CONSTRAINT [PK_MasterCatalog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


