--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <09-15-2016>
-- Description:	 Alter script for pricing cluster
-- Jira ID:		<AL-7927>
-- ================================================================================


IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tLocations]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing] DROP CONSTRAINT [FK_tChannelPartnerPricing_tLocations]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tPricingGroups]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing] DROP CONSTRAINT [FK_tChannelPartnerPricing_tPricingGroups]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tChannelPartner]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing] DROP CONSTRAINT [FK_tChannelPartnerPricing_tChannelPartner]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tProducts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing] DROP CONSTRAINT [FK_tChannelPartnerPricing_tProducts]
END
GO

--IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tPricing_tFeeAdjustmentCompareTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[tPricing]'))
--BEGIN
--	ALTER TABLE [dbo].[tPricing] DROP CONSTRAINT [FK_tPricing_tFeeAdjustmentCompareTypes]
--END
--GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tPricing_tPricingGroups]') AND parent_object_id = OBJECT_ID(N'[dbo].[tPricing]'))
BEGIN
	ALTER TABLE [dbo].[tPricing] DROP CONSTRAINT [FK_tPricing_tPricingGroups]
END
GO

IF NOT EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'PricingGroupId')
BEGIN
	ALTER TABLE tChannelPartnerPricing ADD PricingGroupId BIGINT 
END
GO

IF NOT EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tChannelPartnerPricing ADD ChannelPartnerId INT 
END
GO

IF NOT EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'PricingGroupId')
BEGIN
	ALTER TABLE tChannelPartnerPricing_Aud ADD PricingGroupId BIGINT 
END
GO

IF NOT EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'ProductId')
BEGIN
	ALTER TABLE tChannelPartnerPricing_Aud ADD ProductId BIGINT 
END
GO


IF NOT EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'ProductId')
BEGIN
	ALTER TABLE tChannelPartnerPricing ADD ProductId BIGINT 
END
GO

IF NOT EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'LocationId')
BEGIN
	ALTER TABLE tChannelPartnerPricing ADD LocationId BIGINT 
END
GO

IF NOT EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPricing' AND COLUMN_NAME = 'PricingGroupId')
BEGIN
	ALTER TABLE tPricing ADD PricingGroupId BIGINT 
END
GO

--Drop Primary Keys
IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerPricing' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerPricing')
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing] DROP CONSTRAINT [PK_tChannelPartnerPricing]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tPricing' AND OBJECT_NAME(OBJECT_ID) = 'PK_tPricing')
BEGIN
	ALTER TABLE [dbo].[tPricing] DROP CONSTRAINT [PK_tPricing]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tPricingGroups' AND OBJECT_NAME(OBJECT_ID) = 'PK_tPricingGroups')
BEGIN
	ALTER TABLE [dbo].[tPricingGroups] DROP CONSTRAINT [PK_tPricingGroups]
END
GO

---Adding a Primary Key
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerPricing' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerPricing')
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing] ADD CONSTRAINT [PK_tChannelPartnerPricing] PRIMARY KEY CLUSTERED (ChannelPartnerPricingID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tPricing' AND OBJECT_NAME(OBJECT_ID) = 'PK_tPricing')
BEGIN
	ALTER TABLE [dbo].[tPricing] ADD CONSTRAINT [PK_tPricing] PRIMARY KEY CLUSTERED (PricingID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tPricingGroups' AND OBJECT_NAME(OBJECT_ID) = 'PK_tPricingGroups')
BEGIN
	ALTER TABLE [dbo].[tPricingGroups] ADD CONSTRAINT [PK_tPricingGroups] PRIMARY KEY CLUSTERED (PricingGroupsID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tPricingGroups]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
    ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerPricing_tPricingGroups] FOREIGN KEY(PricingGroupId)
	REFERENCES [dbo].[tPricingGroups] ([PricingGroupsID])
END
GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tLocations]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
--BEGIN
--    ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerPricing_tLocations] FOREIGN KEY(LocationId)
--	REFERENCES [dbo].[tLocations] ([LocationID])
--END
--GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tProducts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
--BEGIN
--    ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerPricing_tProducts] FOREIGN KEY(ProductId)
--	REFERENCES [dbo].[tProducts] ([ProductsID])
--END
--GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tPricing_tPricingGroups]') AND parent_object_id = OBJECT_ID(N'[dbo].[tPricing]'))
BEGIN
    ALTER TABLE [dbo].[tPricing]  WITH CHECK ADD  CONSTRAINT [FK_tPricing_tPricingGroups] FOREIGN KEY(PricingGroupId)
	REFERENCES [dbo].[tPricingGroups] ([PricingGroupsID])
END
GO

IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'ChannelPartnerPricingPK')
BEGIN
	ALTER TABLE tChannelPartnerPricing 
	ALTER COLUMN ChannelPartnerPricingPK UNIQUEIDENTIFIER NULL 
END
GO


----- ReName CompareTypePK to CompareTypeId
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPricing' AND COLUMN_NAME = 'CompareTypePK')
BEGIN
	 EXEC sp_RENAME '[tPricing].[CompareTypePK]' , 'CompareTypeId' , 'COLUMN'	 
END
GO


IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPricing' AND COLUMN_NAME = 'PricingGroupPK')
BEGIN
	ALTER TABLE tPricing ALTER COLUMN PricingGroupPK UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPricing' AND COLUMN_NAME = 'PricingPK')
BEGIN
	ALTER TABLE tPricing ALTER COLUMN PricingPK UNIQUEIDENTIFIER NULL
END
GO

