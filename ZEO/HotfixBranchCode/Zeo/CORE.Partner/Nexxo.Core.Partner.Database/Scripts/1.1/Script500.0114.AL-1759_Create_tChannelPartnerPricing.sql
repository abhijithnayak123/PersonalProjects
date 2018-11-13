--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <10/28/2015>
-- Description:	<As Alloy, a configurable pricing engine should be built to apply specific pricing at the right level for each product and apply to that level>
-- Jira ID:		<AL-1759>
-- ================================================================================

CREATE TABLE [dbo].[tChannelPartnerPricing](
    [ChannelPartnerPricingPK] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY (1000000000, 1),
	[PricingGroupPK] [uniqueidentifier] NOT NULL,
	[ChannelPartnerPK] [uniqueidentifier] NOT NULL,
	[LocationPK] [uniqueidentifier] NULL,
	[ProductPK] [uniqueidentifier] NOT NULL,
	[ProductType] [int] NULL,
    [DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	[DTTerminalCreate] [datetime] NOT NULL,
	[DTTerminalLastModified] [datetime] NULL,

	CONSTRAINT [PK_tChannelPartnerPricing] PRIMARY KEY CLUSTERED 
	(
		[ChannelPartnerPricingPK] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD CONSTRAINT [FK_tChannelPartnerPricing_tPricingGroups] FOREIGN KEY([PricingGroupPK])
REFERENCES [dbo].[tPricingGroups] ([PricingGroupPK])
GO

ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD CONSTRAINT [FK_tChannelPartnerPricing_tChannelPartner] FOREIGN KEY([ChannelPartnerPK])
REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerPK])
GO

ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD CONSTRAINT [FK_tChannelPartnerPricing_tLocations] FOREIGN KEY([LocationPK])
REFERENCES [dbo].[tLocations] ([LocationPK])
GO

ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD CONSTRAINT [FK_tChannelPartnerPricing_tProducts] FOREIGN KEY([ProductPK])
REFERENCES [dbo].[tProducts] ([rowguid])
GO

ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD CONSTRAINT [UQ_tChannelPartnerPricing_PricingGroup_ChannelPartner_Location_Product_ProductType] 
UNIQUE (PricingGroupPK, ChannelPartnerPK, LocationPK, ProductPK, ProductType)
GO

