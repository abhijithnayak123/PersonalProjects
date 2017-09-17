-- ================================================================================
-- Author:		Nitish Biradar
-- Create date: 20/02/2017
-- Description: Changes in all the modules to add the constraints of common modules
-- Jira ID:		
-- ================================================================================

--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx' AND COLUMN_NAME = 'ChannelPartnerID')
--BEGIN
--	ALTER TABLE tChxr_Trx 
--	ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL 
--END
--GO

--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx' AND COLUMN_NAME = 'ChannelPartnerID' and DATA_TYPE = 'SMALLINT') 
--BEGIN
--	 EXEC sp_RENAME '[tChxr_Trx].[ChannelPartnerID]' , 'ChannelPartnerId' , 'COLUMN'	 
--END
--GO

--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx_Aud' AND COLUMN_NAME = 'ChannelPartnerID')
--BEGIN
--	ALTER TABLE tChxr_Trx_Aud 
--	ALTER COLUMN ChannelPartnerID SMALLINT NULL 
--END
--GO

--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx_Aud' AND COLUMN_NAME = 'ChannelPartnerID' and DATA_TYPE = 'SMALLINT') 
--BEGIN
--	 EXEC sp_RENAME '[tChxr_Trx_Aud].[ChannelPartnerID]' , 'ChannelPartnerId' , 'COLUMN'	 
--END
--GO

--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Trx' AND COLUMN_NAME = 'ChannelPartnerID')
--BEGIN
--	ALTER TABLE tVisa_Trx 
--	ALTER COLUMN ChannelPartnerID SMALLINT NULL 
--END
--GO

--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Trx' AND COLUMN_NAME = 'ChannelPartnerID' and DATA_TYPE = 'SMALLINT') 
--BEGIN
--	 EXEC sp_RENAME '[tVisa_Trx].[ChannelPartnerID]' , 'ChannelPartnerId' , 'COLUMN'	 
--END
--GO

--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Trx' AND COLUMN_NAME = 'ChannelParterId') 
--BEGIN
--	 EXEC sp_RENAME '[tWUnion_BillPay_Trx].[ChannelParterId]' , 'ChannelPartnerId' , 'COLUMN'	 
--END
--GO

--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Trx' AND COLUMN_NAME = 'ChannelPartnerId')
--BEGIN
--	ALTER TABLE tWUnion_BillPay_Trx 
--	ALTER COLUMN ChannelPartnerId SMALLINT NULL 
--END
--GO


--IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Trx_Aud' AND COLUMN_NAME = 'ChannelPartnerId')
--BEGIN
--	ALTER TABLE tWUnion_BillPay_Trx_Aud 
--	ADD ChannelPartnerId SMALLINT NULL 
--END
--GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tWUnion_Trx 
	ALTER COLUMN ChannelPartnerId SMALLINT NOT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tWUnion_Trx_Aud 
	ALTER COLUMN ChannelPartnerId SMALLINT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentDetails' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tAgentDetails 
	ALTER COLUMN ChannelPartnerId SMALLINT NOT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tCustomers 
	ALTER COLUMN ChannelPartnerId SMALLINT NOT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tCustomers_Aud 
	ALTER COLUMN ChannelPartnerId SMALLINT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLocations' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tLocations 
	ALTER COLUMN ChannelPartnerId SMALLINT NOT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMasterCatalog' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tMasterCatalog 
	ALTER COLUMN ChannelPartnerId SMALLINT NOT NULL 
END
GO



--- Adding the FK reference
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tLocations]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
    ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD CONSTRAINT [FK_tChannelPartnerPricing_tLocations] FOREIGN KEY(LocationId)
	REFERENCES [dbo].[tLocations] ([LocationID])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tProducts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
    ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerPricing_tProducts] FOREIGN KEY(ProductId)
	REFERENCES [dbo].[tProducts] ([ProductsID])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tFeeAdjustmentConditions_tChannelPartnerFeeAdjustments]') AND parent_object_id = OBJECT_ID(N'[dbo].[tFeeAdjustmentConditions]'))
BEGIN
    ALTER TABLE [dbo].[tFeeAdjustmentConditions]  WITH CHECK ADD  CONSTRAINT [FK_tFeeAdjustmentConditions_tChannelPartnerFeeAdjustments] FOREIGN KEY(FeeAdjustmentId)
	REFERENCES [dbo].[tChannelPartnerFeeAdjustments] ([FeeAdjustmentId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLocations_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLocations]'))
BEGIN
    ALTER TABLE [dbo].[tLocations]  WITH CHECK ADD  CONSTRAINT [FK_tLocations_tChannelPartners] FOREIGN KEY(ChannelPartnerId)
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Trx_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Trx]'))
--BEGIN
--    ALTER TABLE [dbo].[tChxr_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Trx_tChannelPartners] FOREIGN KEY(ChannelPartnerId)
--	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
--END
--GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_BillPay_Trx_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Trx]'))
--BEGIN
--    ALTER TABLE [dbo].[tWUnion_BillPay_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_BillPay_Trx_tChannelPartners] FOREIGN KEY(ChannelPartnerId)
--	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
--END
--GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_Trx_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_Trx]'))
--BEGIN
--    ALTER TABLE [dbo].[tVisa_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tVisa_Trx_tChannelPartners] FOREIGN KEY(ChannelPartnerId)
--	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
--END
--GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
--BEGIN
--    ALTER TABLE [dbo].[tWUnion_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Trx_tChannelPartners] FOREIGN KEY(ChannelPartnerId)
--	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
--END
--GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentDetails_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tAgentDetails]'))
BEGIN
    ALTER TABLE [dbo].[tAgentDetails]  WITH CHECK ADD  CONSTRAINT [FK_tAgentDetails_tChannelPartners] FOREIGN KEY(ChannelPartnerId)
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomers_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCustomers]'))
BEGIN
    ALTER TABLE [dbo].[tCustomers]  WITH CHECK ADD  CONSTRAINT [FK_tCustomers_tChannelPartners] FOREIGN KEY(ChannelPartnerId)
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLocations_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLocations]'))
BEGIN
    ALTER TABLE [dbo].[tLocations]  WITH CHECK ADD  CONSTRAINT [FK_tLocations_tChannelPartners] FOREIGN KEY(ChannelPartnerId)
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tMasterCatalog_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tMasterCatalog]'))
BEGIN
    ALTER TABLE [dbo].[tMasterCatalog]  WITH CHECK ADD  CONSTRAINT [FK_tMasterCatalog_tChannelPartners] FOREIGN KEY(ChannelPartnerId)
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPartnerCatalog' AND COLUMN_NAME = 'MasterCatalogPK')
BEGIN
	ALTER TABLE tPartnerCatalog 
	ALTER COLUMN MasterCatalogPK UNIQUEIDENTIFIER NULL 
END
GO