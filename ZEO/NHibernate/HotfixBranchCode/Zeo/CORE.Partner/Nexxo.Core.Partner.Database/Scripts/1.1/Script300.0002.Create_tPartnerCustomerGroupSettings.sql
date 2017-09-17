CREATE TABLE [dbo].[tPartnerCustomerGroupSettings](
	[rowguid] [uniqueidentifier] NOT NULL,
	[PartnerCustomerPK] [uniqueidentifier] NOT NULL,
	[ChannelPartnerGroupId] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tPartnerCustomerGroupSettings] PRIMARY KEY NONCLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tPartnerCustomerGroupSettings]  WITH CHECK ADD  CONSTRAINT [FK_tPartnerCustomerGroupSettings_tChannelPartnerGroups] FOREIGN KEY([ChannelPartnerGroupId])
REFERENCES [dbo].[tChannelPartnerGroups] ([Id])
GO

ALTER TABLE [dbo].[tPartnerCustomerGroupSettings] CHECK CONSTRAINT [FK_tPartnerCustomerGroupSettings_tChannelPartnerGroups]
GO

ALTER TABLE [dbo].[tPartnerCustomerGroupSettings]  WITH CHECK ADD  CONSTRAINT [FK_tPartnerCustomerGroupSettings_tPartnerCustomers] FOREIGN KEY([PartnerCustomerPK])
REFERENCES [dbo].[tPartnerCustomers] ([rowguid])
GO

ALTER TABLE [dbo].[tPartnerCustomerGroupSettings] CHECK CONSTRAINT [FK_tPartnerCustomerGroupSettings_tPartnerCustomers]
GO