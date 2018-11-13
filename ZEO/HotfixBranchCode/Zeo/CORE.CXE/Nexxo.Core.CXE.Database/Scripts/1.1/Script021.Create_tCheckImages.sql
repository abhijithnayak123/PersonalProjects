
CREATE TABLE [dbo].[tCheckImages](
	[CheckPK] [uniqueidentifier] NOT NULL,
	[Front] [varbinary](max) NULL,
	[Back] [varbinary](max) NULL,
	[Format] [varchar](10) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tCheckImages] PRIMARY KEY CLUSTERED 
(
	[CheckPK] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tCheckImages]  WITH CHECK ADD  CONSTRAINT [FK_tCheckImages_tTxn_Check_Stage] FOREIGN KEY([CheckPK])
REFERENCES [dbo].[tTxn_Check_Stage] ([rowguid])
GO

ALTER TABLE [dbo].[tCheckImages] CHECK CONSTRAINT [FK_tCheckImages_tTxn_Check_Stage]
GO


