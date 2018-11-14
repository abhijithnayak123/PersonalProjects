--- ===============================================================================
-- Author:		<Nishad Varghese>
-- Create date: <07-22-2016>
-- Description:	Alter PK and FK constraints for ChannelPartner related tables
-- Jira ID:		<AL-7580>
-- ================================================================================

--BEGIN TRY
--BEGIN TRANSACTION


-- DROP FK constraints in ChannelPartner related tables
--========================================================================================

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[CK_tChannelPartner_X9_Audit_Header_ChannelPartnerID_tChannelPartner_RowGuid]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Audit_Header]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartner_X9_Audit_Header] 
	DROP CONSTRAINT [CK_tChannelPartner_X9_Audit_Header_ChannelPartnerID_tChannelPartner_RowGuid]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[CK_tChannelPartner_X9_Parameters_ChannelPartnerID_tChannelPartner_RowGuid]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Parameters]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartner_X9_Parameters] 
	DROP CONSTRAINT [CK_tChannelPartner_X9_Parameters_ChannelPartnerID_tChannelPartner_RowGuid]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerCertificate_tChannelPartners_ChannelPartnerPK]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerCertificate]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerCertificate] 
	DROP CONSTRAINT [FK_tChannelPartnerCertificate_tChannelPartners_ChannelPartnerPK]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerConfig_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerConfig] 
	DROP CONSTRAINT [FK_tChannelPartnerConfig_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerFeeAdjustments_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerFeeAdjustments]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerFeeAdjustments] 
	DROP CONSTRAINT [FK_tChannelPartnerFeeAdjustments_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerGroups_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerGroups]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerGroups] 
	DROP CONSTRAINT [FK_tChannelPartnerGroups_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ChannelPartner_tChannelPartnerMasterCountryMapping]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerMasterCountryMapping]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerMasterCountryMapping] 
	DROP CONSTRAINT [FK_ChannelPartner_tChannelPartnerMasterCountryMapping]
END
GO

--IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tPricingGroups]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerPricing] 
--	DROP CONSTRAINT [FK_tChannelPartnerPricing_tPricingGroups]
--END
--GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_tChannelPartnerPricing_ChannelPartner_Location_Product_ProductType' AND object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
    ALTER TABLE [dbo].[tChannelPartnerPricing] 
	DROP CONSTRAINT [IX_tChannelPartnerPricing_ChannelPartner_Location_Product_ProductType]
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tChannelPartnerProductProcessorsMapping')
BEGIN
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tChannelPartnerProductProcessorsMapping' ;

	EXEC('ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tProductProcessorsMapping' 
	AND OBJECT_NAME(parent_object_id)='tChannelPartnerProductProcessorsMapping')
BEGIN
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tProductProcessorsMapping' 
	AND OBJECT_NAME(parent_object_id)='tChannelPartnerProductProcessorsMapping' ;

	EXEC('ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping] DROP CONSTRAINT ' + @FKName)
END
GO


IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tProductProcessorsMapping' 
	AND OBJECT_NAME(parent_object_id)='tChannelPartnerProductProcessorsMapping')
BEGIN
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tProductProcessorsMapping' 
	AND OBJECT_NAME(parent_object_id)='tChannelPartnerProductProcessorsMapping' ;

	EXEC('ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerGroups' 
	AND OBJECT_NAME(parent_object_id)='tProspectGroupSettings')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerGroups' 
	AND OBJECT_NAME(parent_object_id)='tProspectGroupSettings' ;

	EXEC('ALTER TABLE [dbo].[tProspectGroupSettings] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerGroups' 
	AND OBJECT_NAME(parent_object_id)='tPartnerCustomerGroupSettings')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerGroups' 
	AND OBJECT_NAME(parent_object_id)='tPartnerCustomerGroupSettings' ;

	EXEC('ALTER TABLE [dbo].[tPartnerCustomerGroupSettings] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerFeeAdjustments' 
	AND OBJECT_NAME(parent_object_id)='tCustomerFeeAdjustments')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerFeeAdjustments' 
	AND OBJECT_NAME(parent_object_id)='tCustomerFeeAdjustments' ;
	
	EXEC('ALTER TABLE [dbo].[tCustomerFeeAdjustments] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerFeeAdjustments' 
	AND OBJECT_NAME(parent_object_id)='tTxn_FeeAdjustments')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerFeeAdjustments' 
	AND OBJECT_NAME(parent_object_id)='tTxn_FeeAdjustments' ;

	EXEC('ALTER TABLE [dbo].[tTxn_FeeAdjustments] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerFeeAdjustments' 
	AND OBJECT_NAME(parent_object_id)='tFeeAdjustmentConditions')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartnerFeeAdjustments' 
	AND OBJECT_NAME(parent_object_id)='tFeeAdjustmentConditions' ;

	EXEC('ALTER TABLE [dbo].[tFeeAdjustmentConditions] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tChannelPartnerProductProcessorsMapping')
BEGIN
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tChannelPartnerProductProcessorsMapping' ;

	EXEC('ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tPartnerCustomers')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tPartnerCustomers' ;

	EXEC('ALTER TABLE [dbo].[tPartnerCustomers] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tProspects')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tProspects' ;

	EXEC('ALTER TABLE [dbo].[tProspects] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tUserRolesPermissionsMapping')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tUserRolesPermissionsMapping' ;

	EXEC('ALTER TABLE [dbo].[tUserRolesPermissionsMapping] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tNpsTerminals')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tNpsTerminals' ;

	EXEC('ALTER TABLE [dbo].[tNpsTerminals] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tTerminals')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @FKName nvarchar(300)
	SELECT @FKName=name FROM sys.foreign_keys WHERE type_desc = 'FOREIGN_KEY_CONSTRAINT' 
	AND OBJECT_NAME(referenced_object_id)='tChannelPartners' 
	AND OBJECT_NAME(parent_object_id)='tTerminals' ;

	EXEC('ALTER TABLE [dbo].[tTerminals] DROP CONSTRAINT ' + @FKName)
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartner_X9_Audit_Header_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Audit_Header]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartner_X9_Audit_Header] 
	DROP CONSTRAINT [FK_tChannelPartner_X9_Audit_Header_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartner_X9_Parameters_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Parameters]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartner_X9_Parameters] 
	DROP CONSTRAINT [FK_tChannelPartner_X9_Parameters_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerCertificate_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerCertificate]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerCertificate] 
	DROP CONSTRAINT [FK_tChannelPartnerCertificate_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerConfig_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerConfig] 
	DROP CONSTRAINT [FK_tChannelPartnerConfig_tChannelPartners]
END
GO


IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerFeeAdjustments_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerFeeAdjustments]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerFeeAdjustments] 
	DROP CONSTRAINT [FK_tChannelPartnerFeeAdjustments_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerGroups_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerGroups]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerGroups] 
	DROP CONSTRAINT [FK_tChannelPartnerGroups_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerIDTypeMapping_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerIDTypeMapping]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerIDTypeMapping] 
	DROP CONSTRAINT [FK_tChannelPartnerIDTypeMapping_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerMasterCountryMapping_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerMasterCountryMapping]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerMasterCountryMapping] 
	DROP CONSTRAINT [FK_tChannelPartnerMasterCountryMapping_tChannelPartners]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing] 
	DROP CONSTRAINT [FK_tChannelPartnerPricing_tChannelPartners]
END
GO

--IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tLocations]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerPricing] 
--	DROP CONSTRAINT [FK_tChannelPartnerPricing_tLocations]
--END
--GO

--IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tProducts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerPricing] 
--	DROP CONSTRAINT [FK_tChannelPartnerPricing_tProducts]
--END
--GO

--IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tPricingGroups]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerPricing] 
--	DROP CONSTRAINT [FK_tChannelPartnerPricing_tPricingGroups]
--END
--GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerProductProcessorsMapping_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerProductProcessorsMapping]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping] 
	DROP CONSTRAINT [FK_tChannelPartnerProductProcessorsMapping_tChannelPartners]
END
GO

--IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerProductProcessorsMapping_tProductProcessorsMapping]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerProductProcessorsMapping]'))
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping] 
--	DROP CONSTRAINT [FK_tChannelPartnerProductProcessorsMapping_tProductProcessorsMapping]
--END
--GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerSMTPDetails_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerSMTPDetails]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerSMTPDetails] 
	DROP CONSTRAINT [FK_tChannelPartnerSMTPDetails_tChannelPartners]
END
GO


--========================================================================================


-- DROP PK constraints in ChannelPartner related tables
--========================================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' 
			AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerCertificate')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @PKName nvarchar(300)
	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerCertificate'
	EXEC('ALTER TABLE [dbo].[tChannelPartnerCertificate] DROP CONSTRAINT ' + @PKName)
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerConfig')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @PKName nvarchar(300)
	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerConfig'
	EXEC('ALTER TABLE [dbo].[tChannelPartnerConfig] DROP CONSTRAINT ' + @PKName)
END
GO



--IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND 
--	OBJECT_NAME(parent_object_id) = 'tChannelPartnerGroups')
--BEGIN
--	 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
--	DECLARE @PKName nvarchar(300)
--	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND 
--	OBJECT_NAME(parent_object_id) = 'tChannelPartnerGroups'
--	EXEC('ALTER TABLE [dbo].[tChannelPartnerGroups] DROP CONSTRAINT ' + @PKName)
--END
--GO

IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerIDTypeMapping')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @PKName nvarchar(300)
	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerIDTypeMapping'
	EXEC('ALTER TABLE [dbo].[tChannelPartnerIDTypeMapping] DROP CONSTRAINT ' + @PKName)
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerMasterCountryMapping')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @PKName nvarchar(300)
	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerMasterCountryMapping'
	EXEC('ALTER TABLE [dbo].[tChannelPartnerMasterCountryMapping] DROP CONSTRAINT ' + @PKName)
END
GO

--IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerPricing' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerPricing')
--BEGIN
--	ALTER TABLE [dbo].tChannelPartnerPricing 
--	DROP CONSTRAINT [PK_tChannelPartnerPricing]
--END
--GO

IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerGroups')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @PKName nvarchar(300)
	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerGroups'
	EXEC('ALTER TABLE [dbo].[tChannelPartnerGroups] DROP CONSTRAINT ' + @PKName)
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerProductProcessorsMapping')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @PKName nvarchar(300)
	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerProductProcessorsMapping'
	EXEC('ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping] DROP CONSTRAINT ' + @PKName)
END
GO


IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND 
OBJECT_NAME(parent_object_id) = 'tChannelPartners')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @PKName nvarchar(300)
	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND 
	OBJECT_NAME(parent_object_id) = 'tChannelPartners'

	EXEC('ALTER TABLE [dbo].[tChannelPartners] DROP CONSTRAINT ' + @PKName)
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerSMTPDetails')
BEGIN
 -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
	DECLARE @PKName nvarchar(300)
	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND 
	OBJECT_NAME(parent_object_id) = 'tChannelPartnerSMTPDetails'

	EXEC('ALTER TABLE [dbo].[tChannelPartnerSMTPDetails] DROP CONSTRAINT ' + @PKName)
END
GO

---- Adding new column in Channel Partner related tables. --------------------------
--========================================================================================

-- ReName ChannelPartnerId to ChannelPartnerPK
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Audit_Header' AND COLUMN_NAME = 'ChannelPartnerId' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
	 EXEC sp_RENAME '[tChannelPartner_X9_Audit_Header].[ChannelPartnerId]' , 'ChannelPartnerPK' , 'COLUMN'	 
END
GO

-- ADD ChannelPartnerId(bigint) for FK reference
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Audit_Header' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartner_X9_Audit_Header 
	ADD ChannelPartnerID SMALLINT NULL
END
GO

-- ReName ChannelPartnerId to ChannelPartnerPK
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Parameters' AND COLUMN_NAME = 'ChannelPartnerId' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
	 EXEC sp_RENAME '[tChannelPartner_X9_Parameters].[ChannelPartnerId]' , 'ChannelPartnerPK' , 'COLUMN'	 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Parameters' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartner_X9_Parameters 
	ADD ChannelPartnerID SMALLINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerCertificate' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerCertificate 
	ADD ChannelPartnerID SMALLINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerConfig' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerConfig 
	ADD ChannelPartnerID SMALLINT NULL
END
GO

---- Since this column is PK in the table we need to add a NOT NULL constraint on this.
--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerConfig' AND COLUMN_NAME = 'ChannelPartnerID')
--BEGIN
--	ALTER TABLE tChannelPartnerConfig 
--	ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
--END
--GO

-- This column is being added as part of FeeAdjustment module. So datatype has to be changed to SmallInt.
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerFeeAdjustments' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerFeeAdjustments 
	ALTER COLUMN ChannelPartnerID SMALLINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerGroups' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerGroups 
	ADD ChannelPartnerID SMALLINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping' AND COLUMN_NAME = 'ChannelPartnerIDTypeMappingId')
BEGIN
	ALTER TABLE tChannelPartnerIDTypeMapping 
	ADD ChannelPartnerIDTypeMappingID BIGINT IDENTITY(1,1) NOT NULL
END
GO

-- Rename ChannelPartnerId to ChannelPartnerPK
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping' AND COLUMN_NAME = 'ChannelPartnerId' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
	 EXEC sp_RENAME '[tChannelPartnerIDTypeMapping].[ChannelPartnerId]' , 'ChannelPartnerPK' , 'COLUMN'	 
END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerIDTypeMapping 
	ADD ChannelPartnerID SMALLINT NULL
END
GO
-- Rename NexxoIdTypeId to NexxoIdTypePK
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping' AND COLUMN_NAME = 'NexxoIdTypeId' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
	 EXEC sp_RENAME '[tChannelPartnerIDTypeMapping].[NexxoIdTypeId]' , 'NexxoIdTypePK' , 'COLUMN'	 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping' AND COLUMN_NAME = 'NexxoIdTypeID')
BEGIN
	ALTER TABLE tChannelPartnerIDTypeMapping 
	ADD NexxoIdTypeID BIGINT NULL
END
GO

-- ADD new ChannelPartnerMasterCountryMappingId(bigint) for PK
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME = 'ChannelPartnerMasterCountryMappingId')
BEGIN
	ALTER TABLE tChannelPartnerMasterCountryMapping 
	ADD ChannelPartnerMasterCountryMappingID BIGINT IDENTITY(1,1) NOT NULL
END
GO

-- ReName ChannelPartnerId to ChannelPartnerPK
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME = 'ChannelPartnerId' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
	 EXEC sp_RENAME '[tChannelPartnerMasterCountryMapping].[ChannelPartnerId]' , 'ChannelPartnerPK' , 'COLUMN'	 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerMasterCountryMapping 
	ADD ChannelPartnerID SMALLINT NULL
END
GO

-- ReName MasterCountryID to MasterCountryPK
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME = 'ChannelPartnerId' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
	 EXEC sp_RENAME '[tChannelPartnerMasterCountryMapping].[MasterCountryId]' , 'MasterCountryPK' , 'COLUMN'	 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME = 'MasterCountriesID')
BEGIN
	ALTER TABLE tChannelPartnerMasterCountryMapping 
	ADD MasterCountriesID BIGINT NULL
END
GO

--IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'PricingGroupsID')
--BEGIN
--	ALTER TABLE tChannelPartnerPricing 
--	ADD PricingGroupsID BIGINT NULL
--END
--GO

-- This column is added as part of Fee Adjustment.So changing the datatype if the column exists.
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerPricing 
	ALTER COLUMN ChannelPartnerID SMALLINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'LocationID')
BEGIN
	ALTER TABLE tChannelPartnerPricing 
	ADD LocationID BIGINT NULL
END
GO

-- ADD ChannelPartnerId(bigint)
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerPricing_Aud 
	ADD ChannelPartnerID SMALLINT NULL
END
GO

--IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'PricingGroupsID')
--BEGIN
--	ALTER TABLE tChannelPartnerPricing_Aud 
--	ADD PricingGroupsID BIGINT NULL
--END
--GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'LocationID')
BEGIN
	ALTER TABLE tChannelPartnerPricing_Aud 
	ADD LocationID BIGINT NULL
END
GO

-- ReName ChannelPartnerId to ChannelPartnerPK
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping' AND COLUMN_NAME = 'ProductProcessorId' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
	 EXEC sp_RENAME '[tChannelPartnerProductProcessorsMapping].[ProductProcessorId]' , 'ProductProcessorPK' , 'COLUMN'	 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping' AND COLUMN_NAME = 'ProductProcessorId')
BEGIN
	ALTER TABLE tChannelPartnerProductProcessorsMapping 
	ADD ProductProcessorId BIGINT NULL
END
GO

-- ReName ChannelPartnerId to ChannelPartnerPK
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping' AND COLUMN_NAME = 'ChannelPartnerId' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
	 EXEC sp_RENAME '[tChannelPartnerProductProcessorsMapping].[ChannelPartnerId]' , 'ChannelPartnerPK' , 'COLUMN'	 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerProductProcessorsMapping 
	ADD ChannelPartnerID SMALLINT NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerSMTPDetails' AND COLUMN_NAME = 'ChannelPartnerID')
BEGIN
	ALTER TABLE tChannelPartnerSMTPDetails 
	ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
END
GO


------ Adding PK reference to a new columns ------------------------------------------
--========================================================================================

-- Add PK constraint to tChannelPartners table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartners' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartners')
BEGIN
	ALTER TABLE [dbo].[tChannelPartners] 
	ADD CONSTRAINT [PK_tChannelPartners] PRIMARY KEY CLUSTERED (ChannelPartnerId)
END
GO

-- Add PK constraint to tChannelPartnerCertificate table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerCertificate' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerCertificate')
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerCertificate] 
	ADD  CONSTRAINT [PK_tChannelPartnerCertificate] PRIMARY KEY CLUSTERED (ChannelPartnerCertificateId)
END
GO

-- Add PK constraint to tChannelPartnerConfig table
--IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerConfig' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerConfig')
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerConfig] 
--	ADD  CONSTRAINT [PK_tChannelPartnerConfig] PRIMARY KEY CLUSTERED (ChannelPartnerID)
--END
--GO

-- Add PK constraint to tChannelPartnerGroups table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerGroups' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerGroups')
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerGroups] 
	ADD  CONSTRAINT [PK_tChannelPartnerGroups] PRIMARY KEY CLUSTERED (ChannelPartnerGroupID)
END
GO

-- Add PK constraint to tChannelPartnerIDTypeMapping table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerIDTypeMapping' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerIDTypeMapping')
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerIDTypeMapping] 
	ADD  CONSTRAINT [PK_tChannelPartnerIDTypeMapping] PRIMARY KEY CLUSTERED (ChannelPartnerIDTypeMappingID) 
END
GO

-- Add PK constraint to tChannelPartnerMasterCountryMapping table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerMasterCountryMapping' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerMasterCountryMapping')
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerMasterCountryMapping] 
	ADD  CONSTRAINT [PK_tChannelPartnerMasterCountryMapping] PRIMARY KEY CLUSTERED (ChannelPartnerMasterCountryMappingID) 
END
GO

-- Add PK constraint to tChannelPartnerPricing table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerPricing' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerPricing')
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing] 
	ADD  CONSTRAINT [PK_tChannelPartnerPricing] PRIMARY KEY CLUSTERED (ChannelPartnerPricingID)
END
GO

-- Add PK constraint to tChannelPartnerProductProcessorsMapping table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerProductProcessorsMapping' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerProductProcessorsMapping')
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping] 
	ADD  CONSTRAINT [PK_tChannelPartnerProductProcessorsMapping] PRIMARY KEY CLUSTERED (ChannelPartnerProductProcessorsMappingID)
END
GO

-- Add PK constraint to tChannelPartnerSMTPDetails table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerSMTPDetails' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerSMTPDetails')
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerSMTPDetails] 
	ADD  CONSTRAINT [PK_tChannelPartnerSMTPDetails] PRIMARY KEY CLUSTERED (ChannelPartnerSMTPID)
END
GO

--========================================================================================


-- ADD FK constraints in ChannelPartner related tables
--========================================================================================
IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartner_X9_Audit_Header_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Audit_Header]'))
BEGIN
    ALTER TABLE [dbo].[tChannelPartner_X9_Audit_Header]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartner_X9_Audit_Header_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartner_X9_Parameters_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Parameters]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartner_X9_Parameters]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartner_X9_Parameters_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerCertificate_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerCertificate]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerCertificate]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerCertificate_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerConfig_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerConfig]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerConfig_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerFeeAdjustments_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerFeeAdjustments]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerFeeAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerFeeAdjustments_tChannelPartners] FOREIGN KEY([ChannelPartnerId])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerGroups_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerGroups]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerGroups]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerGroups_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerIDTypeMapping_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerIDTypeMapping]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerIDTypeMapping]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerIDTypeMapping_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerMasterCountryMapping_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerMasterCountryMapping]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerMasterCountryMapping]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerMasterCountryMapping_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerPricing_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tLocations]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerPricing_tLocations] FOREIGN KEY([LocationID])
--	REFERENCES [dbo].[tLocations] ([LocationID])
--END
--GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tProducts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerPricing_tProducts] FOREIGN KEY([ProductsID])
--	REFERENCES [dbo].[tProducts] ([ProductsID])
--END
--GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerPricing_tPricingGroups]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerPricing]'))
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerPricing]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerPricing_tPricingGroups] FOREIGN KEY([PricingGroupsID])
--	REFERENCES [dbo].[tPricingGroups] ([PricingGroupsID])
--END
--GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerProductProcessorsMapping_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerProductProcessorsMapping]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerProductProcessorsMapping_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO


--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerProductProcessorsMapping_tProductProcessorsMapping]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerProductProcessorsMapping]'))
--BEGIN
--	ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerProductProcessorsMapping_tProductProcessorsMapping] FOREIGN KEY([ProductProcessorID])
--	REFERENCES [dbo].[tProductProcessorsMapping] ([ProductProcessorsMappingID])
--END
--GO


IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerSMTPDetails_tChannelPartners]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerSMTPDetails]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartnerSMTPDetails]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerSMTPDetails_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO




--========================================================================================

--	COMMIT TRANSACTION

--END TRY
--BEGIN CATCH
--	ROLLBACK TRANSACTION
--END CATCH
--GO

--Making PK column NULL
IF EXISTS(
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME= 'tChannelPartnerConfig'
		AND COLUMN_NAME = 'ChannelPartnerPK'
)
BEGIN 
	ALTER TABLE tChannelPartnerConfig
	ALTER COLUMN ChannelPartnerPK UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS(
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME= 'tChannelPartners'
		AND COLUMN_NAME = 'ChannelPartnerPK'
)
BEGIN 
	ALTER TABLE tChannelPartners
	ALTER COLUMN ChannelPartnerPK UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS(
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME= 'tChannelPartnerProductProcessorsMapping'
		AND COLUMN_NAME = 'ChannelPartnerProductProcessorsMappingPK'
)
BEGIN 
	ALTER TABLE tChannelPartnerProductProcessorsMapping
	ALTER COLUMN ChannelPartnerProductProcessorsMappingPK UNIQUEIDENTIFIER NULL
END
GO

----IF EXISTS(
----		SELECT 1
----		FROM INFORMATION_SCHEMA.COLUMNS
----		WHERE TABLE_NAME= 'tProductProcessorsMapping'
----		AND COLUMN_NAME = 'ProductProcessorsMappingPK'
----)
----BEGIN 
----	ALTER TABLE tProductProcessorsMapping
----	ALTER COLUMN ProductProcessorsMappingPK UNIQUEIDENTIFIER NULL
----END
----GO


----IF EXISTS(
----		SELECT 1
----		FROM INFORMATION_SCHEMA.COLUMNS
----		WHERE TABLE_NAME= 'tProcessors'
----		AND COLUMN_NAME = 'ProcessorsPK'
----)
----BEGIN 
----	ALTER TABLE tProcessors
----	ALTER COLUMN ProcessorsPK UNIQUEIDENTIFIER NULL
----END
----GO