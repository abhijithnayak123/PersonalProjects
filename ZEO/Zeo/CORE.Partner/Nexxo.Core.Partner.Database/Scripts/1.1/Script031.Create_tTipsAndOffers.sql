
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTipsAndOffers]') AND type in (N'U'))
DROP TABLE [dbo].[tTipsAndOffers]
GO


CREATE TABLE [dbo].[tTipsAndOffers](
	[Id] [int] NOT NULL,
	[ViewName] [varchar](50) NOT NULL,
	[ChannelPartnerName] [varchar](50) NOT NULL,
	[TipsAndOffersEn] [nvarchar](300) NULL,
	[TipsAndOffersEs] [nvarchar](300) NULL,
	[OptionalFilter] [varchar](50) NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTipsAndOffers1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO