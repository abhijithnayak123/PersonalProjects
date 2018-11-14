CREATE TABLE [dbo].[tChxr_CheckImages](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Front] [varbinary](max) NOT NULL,
	[Back] [varbinary](max) NOT NULL,
	[Format] [nvarchar](10) NOT NULL,
	[FrontTIF] [varbinary](max) NOT NULL,
	[BackTIF] [varbinary](max) NOT NULL,
	[ChxrTrxPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChxr_CheckImages] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tChxr_CheckImages]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_CheckImages_tChxr_Trx] FOREIGN KEY([ChxrTrxPK])
REFERENCES [dbo].[tChxr_Trx] ([rowguid])
GO

ALTER TABLE [dbo].[tChxr_CheckImages] CHECK CONSTRAINT [FK_tChxr_CheckImages_tChxr_Trx]
GO
