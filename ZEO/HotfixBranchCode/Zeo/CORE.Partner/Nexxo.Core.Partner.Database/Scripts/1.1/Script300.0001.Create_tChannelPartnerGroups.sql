CREATE TABLE [dbo].[tChannelPartnerGroups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ChannelPartnerPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChannelPartnerGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChannelPartnerGroups]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerGroups_tChannelPartners] FOREIGN KEY([ChannelPartnerPK])
REFERENCES [dbo].[tChannelPartners] ([rowguid])
GO

ALTER TABLE [dbo].[tChannelPartnerGroups] CHECK CONSTRAINT [FK_tChannelPartnerGroups_tChannelPartners]
GO


