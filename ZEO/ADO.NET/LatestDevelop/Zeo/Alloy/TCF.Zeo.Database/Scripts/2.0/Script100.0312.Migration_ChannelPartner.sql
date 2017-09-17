--- ===============================================================================
-- Author:		<Nishad Varghese>
-- Create date: <07-25-2016>
-- Description:	Update the foreign key for ChannelPartner related tables
-- Jira ID:		<AL-7580>
-- ================================================================================

BEGIN TRY
	BEGIN TRAN;

	---- Update the ChannelPartnerId in tChannelPartnerCertificate
	UPDATE CP SET CP.ChannelPartnerId = C.ChannelPartnerId
	FROM tChannelPartnerCertificate AS CP
	INNER JOIN tChannelPartners AS C
	ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	---- Update the ChannelPartnerId in tChannelPartnerConfig
	UPDATE CP SET CP.ChannelPartnerId = C.ChannelPartnerId
	FROM tChannelPartnerConfig AS CP
	INNER JOIN tChannelPartners AS C
	ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	---- Update the ChannelPartnerId in tChannelPartnerFeeAdjustments
	UPDATE CP SET CP.ChannelPartnerId = C.ChannelPartnerId
	FROM tChannelPartnerFeeAdjustments AS CP
	INNER JOIN tChannelPartners AS C
	ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	---- Update the ChannelPartnerId in tChannelPartnerGroups
	UPDATE CP SET CP.ChannelPartnerId = C.ChannelPartnerId
	FROM tChannelPartnerGroups AS CP
	INNER JOIN tChannelPartners AS C
	ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	---- Update the ChannelPartnerId in tChannelPartnerMasterCountryMapping
	UPDATE CP SET CP.ChannelPartnerId = C.ChannelPartnerId
	FROM tChannelPartnerMasterCountryMapping AS CP
	INNER JOIN tChannelPartners AS C
	ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	---- Update the MasterCountriesId in tChannelPartnerMasterCountryMapping
	UPDATE CP SET CP.MasterCountriesId = MC.MasterCountriesID
	FROM tChannelPartnerMasterCountryMapping AS CP
	INNER JOIN tMasterCountries AS MC
	ON CP.MasterCountryId = MC.MasterCountriesPK

	---- Update the ChannelPartnerId in tChannelPartnerPricing
	UPDATE CP SET CP.ChannelPartnerId = C.ChannelPartnerId
	FROM tChannelPartnerPricing AS CP
	INNER JOIN tChannelPartners AS C
	ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	---- Update the ChannelPartnerId in tChannelPartnerPricing_Aud
	UPDATE CP SET CP.ChannelPartnerId = C.ChannelPartnerId
	FROM tChannelPartnerPricing_Aud AS CP
	INNER JOIN tChannelPartners AS C
	ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	---- Update the PricingGroupId in tChannelPartnerPricing
	UPDATE CP SET CP.PricingGroupId = P.PricingGroupsID
	FROM tChannelPartnerPricing AS CP
	INNER JOIN tPricingGroups AS P
	ON CP.PricingGroupPK = P.PricingGroupPK

	---- Update the PricingGroupId in tChannelPartnerPricing_Aud
	UPDATE CP SET CP.PricingGroupId = P.PricingGroupsID
	FROM tChannelPartnerPricing_Aud AS CP
	INNER JOIN tPricingGroups AS P
	ON CP.PricingGroupPK = P.PricingGroupPK

	---- Update the LocationId in tChannelPartnerPricing
	UPDATE CP SET CP.LocationID = L.LocationID
	FROM tChannelPartnerPricing AS CP
	INNER JOIN tLocations AS L
	ON CP.LocationPK = L.LocationPK

	---- Update the LocationId in tChannelPartnerPricing_Aud
	UPDATE CP SET CP.LocationID = L.LocationID
	FROM tChannelPartnerPricing_Aud AS CP
	INNER JOIN tLocations AS L
	ON CP.LocationPK = L.LocationPK

	---- Update the ProductId in tChannelPartnerPricing
	UPDATE CP SET CP.ProductId = P.ProductsID
	FROM tChannelPartnerPricing AS CP
	INNER JOIN tProducts AS P
	ON CP.ProductPK = P.ProductsPK

	---- Update the ProductId in tChannelPartnerPricing_Aud
	UPDATE CP SET CP.ProductId = P.ProductsID
	FROM tChannelPartnerPricing_Aud AS CP
	INNER JOIN tProducts AS P
	ON CP.ProductPK = P.ProductsPK

	---- Update the ProductProcessorId in tChannelPartnerProductProcessorsMapping
	UPDATE tcpp SET tcpp.ProductProcessorId = tpp.ProductProcessorsMappingID
	FROM tChannelPartnerProductProcessorsMapping AS tcpp
	INNER JOIN tProductProcessorsMapping AS tpp
	ON tcpp.ProductProcessorPK = tpp.ProductProcessorsMappingPK


	---- Update the ChannelPartnerId in tChannelPartnerProductProcessorsMapping
	UPDATE CP SET CP.ChannelPartnerId = C.ChannelPartnerId
	FROM tChannelPartnerProductProcessorsMapping AS CP
	INNER JOIN tChannelPartners AS C
	ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	---- Update the ChannelPartnerId in tChannelPartnerIDTypeMapping
	UPDATE CP SET CP.ChannelPartnerID = C.ChannelPartnerId
	FROM tChannelPartnerIDTypeMapping AS CP
	INNER JOIN tChannelPartners AS C
	ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	---- Update the NexxoIdTypeID in tChannelPartnerIDTypeMapping
	UPDATE CP SET CP.NexxoIdTypeID = nt.NexxoIdTypeID
	FROM tChannelPartnerIDTypeMapping AS CP
	INNER JOIN tNexxoIdTypes AS nt
	ON CP.NexxoIdTypePK = nt.NexxoIdTypePK

	---- Update the ChannelPartnerId in tChannelPartnerSMTPDetails
	--UPDATE CP SET CP.ChannelPartnerId = C.ChannelPartnerId
	--FROM tChannelPartnerSMTPDetails AS CP
	--INNER JOIN tChannelPartners AS C
	--ON CP.ChannelPartnerPK = C.ChannelPartnerPK

	--======================================================================================


	COMMIT TRAN
END TRY
BEGIN CATCH
	 IF(@@TRANCOUNT > 0)
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage,
		XACT_STATE()as state;
		ROLLBACK TRAN
END CATCH;
--==========================================================================================
GO


BEGIN TRY
	BEGIN TRAN;

	-- ALTER ChannelPartnerId(bigint) as not nullable for FK reference
	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerConfig' AND COLUMN_NAME = 'ChannelPartnerID')
	BEGIN
		ALTER TABLE tChannelPartnerConfig 
		ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
	END

	-- ALTER LocationId(bigint) as not nullable for FK reference
	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerCertificate' AND COLUMN_NAME = 'ChannelPartnerID')
	BEGIN
		ALTER TABLE tChannelPartnerCertificate 
		ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
	END

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerConfig' AND COLUMN_NAME = 'ChannelPartnerID')
	BEGIN
		ALTER TABLE tChannelPartnerConfig 
		ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
	END

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerFeeAdjustments' AND COLUMN_NAME = 'ChannelPartnerID')
	BEGIN
		ALTER TABLE tChannelPartnerFeeAdjustments 
		ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
	END

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerGroups' AND COLUMN_NAME = 'ChannelPartnerID')
	BEGIN
		ALTER TABLE tChannelPartnerGroups 
		ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
	END

	--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping' 
	--	AND COLUMN_NAME = 'ChannelPartnerId')
	--BEGIN
	--	ALTER TABLE tChannelPartnerIDTypeMapping 
	--	ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
	--END
	--GO

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' 
		AND COLUMN_NAME = 'ChannelPartnerId')
	BEGIN
		ALTER TABLE tChannelPartnerMasterCountryMapping 
		ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
	END

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' 
		AND COLUMN_NAME = 'ChannelPartnerId')
	BEGIN
		ALTER TABLE tChannelPartnerPricing 
		ALTER COLUMN ChannelPartnerId SMALLINT NOT NULL
	END

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping' 
		AND COLUMN_NAME = 'ChannelPartnerID')
	BEGIN
		ALTER TABLE tChannelPartnerProductProcessorsMapping 
		ALTER COLUMN ChannelPartnerID SMALLINT NOT NULL
	END

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerSMTPDetails' 
		AND COLUMN_NAME = 'ChannelPartnerId')
	BEGIN
		ALTER TABLE tChannelPartnerSMTPDetails 
		ALTER COLUMN ChannelPartnerId SMALLINT NOT NULL
	END

	COMMIT TRAN
END TRY

BEGIN CATCH
	 IF(@@TRANCOUNT > 0)
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage,
		XACT_STATE()as state;
		ROLLBACK TRAN
END CATCH;

GO

BEGIN TRY
	BEGIN TRAN;


	-- Add PK constraint to tChannelPartnerConfig table. 
	IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerConfig' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerConfig')
	BEGIN
		ALTER TABLE [dbo].[tChannelPartnerConfig] 
		ADD  CONSTRAINT [PK_tChannelPartnerConfig] PRIMARY KEY CLUSTERED (ChannelPartnerID)
	END
	
	COMMIT TRAN
END TRY

BEGIN CATCH
	 IF(@@TRANCOUNT > 0)
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage,
		XACT_STATE()as state;
		ROLLBACK TRAN
END CATCH;

GO
