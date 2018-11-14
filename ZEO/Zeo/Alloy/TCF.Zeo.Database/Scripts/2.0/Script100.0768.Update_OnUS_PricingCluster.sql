--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <06/19/2018>
-- Description:	Updating the Pricing Cluster for OnUS.
-- ================================================================================

BEGIN TRY
BEGIN TRAN

	DECLARE @pricingGroupsID BIGINT
	DECLARE @checkTypeId BIGINT
	DECLARE @providerId BIGINT = 202  ---Pricing Cluster for OnUS

	DECLARE @productId BIGINT =
	(
		SELECT ProductsID
		FROM tProducts
		WHERE Name = 'ProcessCheck'
	)


	----------------------------  For OnUS ---------------------------------------
	IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-OnUS-NoFee')
	BEGIN

		INSERT INTO tPricingGroups
		([PricingGroupName],[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
		VALUES
		('CC-OnUS-NoFee', GETDATE(), NULL, GETDATE(), NULL)

		SET @pricingGroupsID = CAST(SCOPE_IDENTITY() AS BIGINT)

		INSERT INTO tPricing
		([PricingGroupId],[CompareTypeId],[MinimumAmount],[MaximumAmount],
			[MinimumFee],[MaximumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
		VALUES
		(@pricingGroupsID, NULL,NULL,NULL, 0, 999999.99, 0, 0, GETDATE(), NULL, GETDATE(), NULL)

		SET @checkTypeId =
		(
			SELECT CheckTypeId
			FROM tCheckTypes
			WHERE Name = 'OnUsOCMO'
		)

		 INSERT INTO tChannelPartnerPricing
			([PricingGroupId],[ChannelPartnerId],[LocationId],[ProductId],[ProductType],
			[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified],[ProductProviderCode])
		 VALUES
			(@pricingGroupsID, 34, NULL, @productId, @checkTypeId, GETDATE(), NULL, GETDATE(), NULL,@providerId)

		SET @checkTypeId =
		(
			SELECT CheckTypeId
			FROM tCheckTypes
			WHERE Name = 'OnUsOTHER'
		)

		INSERT INTO tChannelPartnerPricing
			([PricingGroupId],[ChannelPartnerId],[LocationId],[ProductId],[ProductType],
			[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified],[ProductProviderCode])
		 VALUES
			(@pricingGroupsID, 34, NULL, @productId, @checkTypeId, GETDATE(), NULL, GETDATE(), NULL,@providerId)

	END


	----------------------------  For OnUS ---------------------------------------
	IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-OnUS-Fee')
	BEGIN

		INSERT INTO tPricingGroups
		([PricingGroupName],[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
		VALUES
		('CC-OnUS-Fee', GETDATE(), NULL, GETDATE(), NULL)

		SET @pricingGroupsID = CAST(SCOPE_IDENTITY() AS BIGINT)

		INSERT INTO tPricing
		([PricingGroupId],[CompareTypeId],[MinimumAmount],[MaximumAmount],
			[MinimumFee],[MaximumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
		VALUES
		(@pricingGroupsID, NULL,NULL,NULL, 0, 999999.99, 1, 1, GETDATE(), NULL, GETDATE(), NULL)

		SET @checkTypeId =
		(
			SELECT CheckTypeId
			FROM tCheckTypes
			WHERE Name = 'OnUsTRUE'
		)

		 INSERT INTO tChannelPartnerPricing
			([PricingGroupId],[ChannelPartnerId],[LocationId],[ProductId],[ProductType],
			[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified],[ProductProviderCode])
		 VALUES
			(@pricingGroupsID, 34, NULL, @productId, @checkTypeId, GETDATE(), NULL, GETDATE(), NULL,@providerId)

	END
	COMMIT TRAN
END TRY
BEGIN CATCH
    ROLLBACK TRAN
END CATCH
----------------------------  For OnUS ---------------------------------------


