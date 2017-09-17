
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tSkins]') AND type in (N'U'))
DROP TABLE [dbo].[tSkins]
GO

CREATE TABLE [dbo].[tSkins](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ChannelPartnerId] [int] NOT NULL,
	[ValueType] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ValueEn] [nvarchar](max) NOT NULL,
	[ValueEs] [nvarchar](max) NOT NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tSkins_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Default [DF_tSkins_Id_1]    Script Date: 05/20/2013 17:23:35 ******/
ALTER TABLE [dbo].[tSkins] ADD  CONSTRAINT [DF_tSkins_Id_1]  DEFAULT (newid()) FOR [Id]
GO
/****** Object:  Default [DF_tSkins_DTCreate]    Script Date: 05/20/2013 17:23:35 ******/
ALTER TABLE [dbo].[tSkins] ADD  CONSTRAINT [DF_tSkins_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO