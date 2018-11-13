--==================================================================================================
-- Author:		<KAUSHIK SAKALA>
-- Created date: <18/01/2016>
-- Description:	<As Engineering, I need to have consistent naming convention for Id and PK columns>           
-- Jira ID:	<AL-4485>
--===================================================================================================

-- Changing column name from rowguid to TableNamePK
IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tChannelPartnerIDTypeMapping')
)
EXEC sp_rename @objname='tChannelPartnerIDTypeMapping.rowguid', @newname='ChannelPartnerIDTypeMappingPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tChannelPartnerMasterCountryMapping')
)
EXEC sp_rename @objname='tChannelPartnerMasterCountryMapping.rowguid', @newname='ChannelPartnerMasterCountryMappingPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tChannelPartnerProductProcessorsMapping')
)
EXEC sp_rename @objname='tChannelPartnerProductProcessorsMapping.rowguid', @newname='ChannelPartnerProductProcessorsMappingPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tCustomerFeeAdjustments')
)
EXEC sp_rename @objname='tCustomerFeeAdjustments.rowguid', @newname='CustomerFeeAdjustmentsPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tLegalCodes')
)
EXEC sp_rename @objname='tLegalCodes.rowguid', @newname='LegalCodesPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tLocationProcessorCredentials')
)
EXEC sp_rename @objname='tLocationProcessorCredentials.rowguid', @newname='LocationProcessorCredentialsPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tMasterCountries')
)
EXEC sp_rename @objname='tMasterCountries.rowguid', @newname='MasterCountriesPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tMoneyOrderImage')
)
EXEC sp_rename @objname='tMoneyOrderImage.rowguid', @newname='MoneyOrderImagePK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tOccupations')
)
EXEC sp_rename @objname='tOccupations.rowguid', @newname='OccupationsPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tPermissions')
)
EXEC sp_rename @objname='tPermissions.rowguid', @newname='PermissionsPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tProcessors')
)
EXEC sp_rename @objname='tProcessors.rowguid', @newname='ProcessorsPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tProductProcessorsMapping')
)
EXEC sp_rename @objname='tProductProcessorsMapping.rowguid', @newname='ProductProcessorsMappingPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tProducts')
)
EXEC sp_rename @objname='tProducts.rowguid', @newname='ProductsPK', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'rowguid' AND OBJECT_ID = OBJECT_ID(N'tUserRolesPermissionsMapping')
)
EXEC sp_rename @objname='tUserRolesPermissionsMapping.rowguid', @newname='UserRolesPermissionsMappingPK', @objtype='COLUMN';
GO


-- Changing column name from ID to TableNameID
IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tChannelPartnerPricing')
)
EXEC sp_rename @objname='tChannelPartnerPricing.Id', @newname='ChannelPartnerPricingID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tChannelPartnerPricing_Aud')
)
EXEC sp_rename @objname='tChannelPartnerPricing_Aud.Id', @newname='ChannelPartnerPricingID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tChannelPartnerProductProcessorsMapping')
)
EXEC sp_rename @objname='tChannelPartnerProductProcessorsMapping.Id', @newname='ChannelPartnerProductProcessorsMappingID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tChannelPartnerIDTypeMapping')
)
EXEC sp_rename @objname='tChannelPartnerIDTypeMapping.Id', @newname='ChannelPartnerIDTypeMappingID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tCustomerFeeAdjustments')
)
EXEC sp_rename @objname='tCustomerFeeAdjustments.Id', @newname='CustomerFeeAdjustmentsID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tLegalCodes')
)
EXEC sp_rename @objname='tLegalCodes.Id', @newname='LegalCodesID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tLocationProcessorCredentials')
)
EXEC sp_rename @objname='tLocationProcessorCredentials.Id', @newname='LocationProcessorCredentialsID', @objtype='COLUMN';
GO


IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tMasterCountries')
)
EXEC sp_rename @objname='tMasterCountries.Id', @newname='MasterCountriesID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tMoneyOrderImage')
)
EXEC sp_rename @objname='tMoneyOrderImage.Id', @newname='MoneyOrderImageID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tOccupations')
)
EXEC sp_rename @objname='tOccupations.Id', @newname='OccupationsID', @objtype='COLUMN';
GO


IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tPermissions')
)
EXEC sp_rename @objname='tPermissions.Id', @newname='PermissionsID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tPricing')
)
EXEC sp_rename @objname='tPricing.Id', @newname='PricingID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tPricingGroups')
)
EXEC sp_rename @objname='tPricingGroups.Id', @newname='PricingGroupsID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tProcessors')
)
EXEC sp_rename @objname='tProcessors.Id', @newname='ProcessorsID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tProductProcessorsMapping')
)
EXEC sp_rename @objname='tProductProcessorsMapping.Id', @newname='ProductProcessorsMappingID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tProducts')
)
EXEC sp_rename @objname='tProducts.Id', @newname='ProductsID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tUserRoles')
)
EXEC sp_rename @objname='tUserRoles.Id', @newname='UserRolesID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tUserRolesPermissionsMapping')
)
EXEC sp_rename @objname='tUserRolesPermissionsMapping.Id', @newname='UserRolesPermissionsMappingID', @objtype='COLUMN';
GO

IF EXISTS
(
	SELECT 1 FROM SYS.COLUMNS 
	WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tUserStatuses')
)
EXEC sp_rename @objname='tUserStatuses.Id', @newname='UserStatusesID', @objtype='COLUMN';
GO