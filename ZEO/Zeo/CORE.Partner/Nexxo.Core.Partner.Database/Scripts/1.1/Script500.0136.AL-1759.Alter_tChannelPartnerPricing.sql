	-- ================================================================================
-- Author:		<Abhijith Nayak>
-- Create date: <11/25/2015>
-- Description:	<As Alloy, a configurable pricing engine should be built to apply specific pricing at the right level for each product and apply to that level>
-- Jira ID:		<AL-1759>
-- ================================================================================

IF EXISTS(SELECT * 
			FROM sys.indexes 
			WHERE name='UQ_tChannelPartnerPricing_PricingGroup_ChannelPartner_Location_Product_ProductType' 
			AND object_id = OBJECT_ID('tChannelPartnerPricing'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing] 
	DROP CONSTRAINT UQ_tChannelPartnerPricing_PricingGroup_ChannelPartner_Location_Product_ProductType
END


ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD CONSTRAINT [IX_tChannelPartnerPricing_ChannelPartner_Location_Product_ProductType] 
UNIQUE (ChannelPartnerPK, LocationPK, ProductPK, ProductType)
GO

