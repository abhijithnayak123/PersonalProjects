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
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-TCF-Government')
	BEGIN

		SET @pricingGroupsID = 
		(
			SELECT PricingGroupsID 
			FROM tPricingGroups 
			WHERE PricingGroupName='CC-TCF-Government'
		)

		SET @checkTypeId =
		(
			SELECT CheckTypeId
			FROM tCheckTypes
			WHERE Name = 'Printed Payroll' AND ProductProviderCode = @providerId
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
			WHERE Name = 'Government' AND ProductProviderCode = @providerId
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
			WHERE Name = 'US Treasury' AND ProductProviderCode = @providerId
		)

		INSERT INTO tChannelPartnerPricing
			([PricingGroupId],[ChannelPartnerId],[LocationId],[ProductId],[ProductType],
			[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified],[ProductProviderCode])
		 VALUES
			(@pricingGroupsID, 34, NULL, @productId, @checkTypeId, GETDATE(), NULL, GETDATE(), NULL,@providerId)
	END

	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-TCF-Payroll')
	BEGIN
		
		SET @pricingGroupsID = 
		(
			SELECT PricingGroupsID 
			FROM tPricingGroups 
			WHERE PricingGroupName='CC-TCF-Payroll'
		)

		SET @checkTypeId =
		(
			SELECT CheckTypeId
			FROM tCheckTypes
			WHERE Name = 'Ins/Attorney/Cashiers' AND ProductProviderCode = @providerId
		)

		 INSERT INTO tChannelPartnerPricing
			([PricingGroupId],[ChannelPartnerId],[LocationId],[ProductId],[ProductType],
			[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified],[ProductProviderCode])
		 VALUES
			(@pricingGroupsID, 34, NULL, @productId, @checkTypeId, GETDATE(), NULL, GETDATE(), NULL, @providerId)

		SET @checkTypeId =
		(
			SELECT CheckTypeId
			FROM tCheckTypes
			WHERE Name = 'Money Order' AND ProductProviderCode = @providerId
		)

		 INSERT INTO tChannelPartnerPricing
			([PricingGroupId],[ChannelPartnerId],[LocationId],[ProductId],[ProductType],
			[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified],[ProductProviderCode])
		 VALUES
			(@pricingGroupsID, 34, NULL, @productId, @checkTypeId, GETDATE(), NULL, GETDATE(), NULL, @providerId)

		SET @checkTypeId =
		(
			SELECT CheckTypeId
			FROM tCheckTypes
			WHERE Name = 'Handwritten Payroll' AND ProductProviderCode = @providerId
		)

		 INSERT INTO tChannelPartnerPricing
			([PricingGroupId],[ChannelPartnerId],[LocationId],[ProductId],[ProductType],
			[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified],[ProductProviderCode])
		 VALUES
			(@pricingGroupsID, 34, NULL, @productId, @checkTypeId, GETDATE(), NULL, GETDATE(), NULL, @providerId)


		SET @checkTypeId =
		(
			SELECT CheckTypeId
			FROM tCheckTypes
			WHERE Name = 'Two Party' AND ProductProviderCode = @providerId
		)

		 INSERT INTO tChannelPartnerPricing
			([PricingGroupId],[ChannelPartnerId],[LocationId],[ProductId],[ProductType],
			[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified],[ProductProviderCode])
		 VALUES
			(@pricingGroupsID, 34, NULL, @productId, @checkTypeId, GETDATE(), NULL, GETDATE(), NULL, @providerId)


		SET @checkTypeId =
		(
			SELECT CheckTypeId
			FROM tCheckTypes
			WHERE Name = 'Loan/RAL' AND ProductProviderCode = @providerId
		)

		 INSERT INTO tChannelPartnerPricing
			([PricingGroupId],[ChannelPartnerId],[LocationId],[ProductId],[ProductType],
			[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified],[ProductProviderCode])
		 VALUES
			(@pricingGroupsID, 34, NULL, @productId, @checkTypeId, GETDATE(), NULL, GETDATE(), NULL, @providerId)

	END
	COMMIT TRAN
END TRY
BEGIN CATCH
    ROLLBACK TRAN
END CATCH
----------------------------  For OnUS ---------------------------------------


