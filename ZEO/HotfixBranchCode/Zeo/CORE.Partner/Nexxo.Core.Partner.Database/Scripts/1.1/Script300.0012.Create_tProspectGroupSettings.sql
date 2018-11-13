CREATE TABLE [dbo].[tProspectGroupSettings](
	[rowguid] [uniqueidentifier] NOT NULL,
	[ProspectId] [uniqueidentifier] NOT NULL,
	[ChannelPartnerGroupId] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tProspectGroupSettings] PRIMARY KEY NONCLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tProspectGroupSettings]  WITH CHECK ADD CONSTRAINT [FK_tProspectGroupSettings_tChannelPartnerGroups] FOREIGN KEY([ChannelPartnerGroupId])
REFERENCES [dbo].[tChannelPartnerGroups] ([Id])
GO

ALTER TABLE [dbo].[tProspectGroupSettings] CHECK CONSTRAINT [FK_tProspectGroupSettings_tChannelPartnerGroups]
GO

ALTER TABLE [dbo].[tProspectGroupSettings]  WITH CHECK ADD CONSTRAINT [FK_tProspectGroupSettings_tProspects] FOREIGN KEY([ProspectId])
REFERENCES [dbo].[tProspects] ([id])
GO

ALTER TABLE [dbo].[tProspectGroupSettings] CHECK CONSTRAINT [FK_tProspectGroupSettings_tProspects]
GO


